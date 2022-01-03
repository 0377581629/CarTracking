using Abp.AspNetZeroCore;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Zero.Auditing;
using Zero.Configuration;
using Zero.Customize.BackgroundJobs;
using Zero.EntityFrameworkCore;
using Zero.MultiTenancy;
using Zero.Web.Areas.App.Startup;

namespace Zero.Web.Startup
{
    [DependsOn(
        typeof(ZeroWebCoreModule)
    )]
    public class ZeroWebMvcModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ZeroWebMvcModule(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "https://localhost:44302/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();
            
            IocManager.Register<DashboardViewConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroWebMvcModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                return;
            }

            var multiTenancyConfig = IocManager.Resolve<IMultiTenancyConfig>();
            multiTenancyConfig.TenantIdResolveKey = "abp.tenantid";
            
            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());

            if (Configuration.Auditing.IsEnabled && ExpiredAuditLogDeleterWorker.IsEnabled)
            {
                workManager.Add(IocManager.Resolve<ExpiredAuditLogDeleterWorker>());
            }
            
            var currencyRateBackgroundJobService = IocManager.Resolve<ICurrencyRateBackgroundJob>();
            RecurringJob.RemoveIfExists("SyncCurrencyRates");
            RecurringJob.AddOrUpdate("SyncCurrencyRates",() => currencyRateBackgroundJobService.UpdateRates(), Cron.Daily);
            
            var userSubscriptionBackgroundJobService = IocManager.Resolve<IUserSubscriptionBackgroundJob>();
            RecurringJob.RemoveIfExists("UserSubscription_ExpirationCheck");
            RecurringJob.AddOrUpdate("UserSubscription_ExpirationCheck",() => userSubscriptionBackgroundJobService.UserSubscriptionExpirationCheck(), Cron.Daily);
            RecurringJob.RemoveIfExists("UserSubscription_ExpireEmailNotifier");
            RecurringJob.AddOrUpdate("UserSubscription_ExpireEmailNotifier",() => userSubscriptionBackgroundJobService.UserSubscriptionExpireEmailNotifier(), Cron.Daily);
        }
    }
}