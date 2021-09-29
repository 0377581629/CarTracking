using System;
using System.IO;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.AspNetZeroCore.Licensing;
using Abp.AspNetZeroCore.Web;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Timing;
using Abp.Zero.Configuration;
using Castle.Core.Internal;
using DPS.Reporting.Application;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Zero.Authentication.TwoFactor;
using Zero.Chat;
using Zero.Configuration;
using Zero.Customize.BackgroundJobs;
using Zero.EntityFrameworkCore;
using Zero.Startup;
using Zero.Web.Authentication.JwtBearer;
using Zero.Web.Authentication.TwoFactor;
using Zero.Web.Chat.SignalR;
using Zero.Web.Configuration;
using Zero.Web.DashboardCustomization;

namespace Zero.Web
{
    [DependsOn(
        typeof(ZeroApplicationModule),
        typeof(ZeroEntityFrameworkCoreModule),
        typeof(AbpAspNetZeroCoreWebModule),
        typeof(AbpAspNetCoreSignalRModule),
        typeof(ZeroGraphQLModule),
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency (and Abp.RedisCache nuget package) can be removed if not using Redis cache
        typeof(AbpHangfireAspNetCoreModule), //AbpHangfireModule dependency (and Abp.Hangfire.AspNetCore nuget package) can be removed if not using Hangfire
        typeof(ReportingApplicationModule)
    )]
    public class ZeroWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ZeroWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //Set default connection string
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ZeroConsts.ConnectionStringName
            );

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(ZeroApplicationModule).GetAssembly()
                );
            
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(ReportingApplicationModule).GetAssembly()
                );

            Configuration.Caching.Configure(TwoFactorCodeCacheItem.CacheName,
                cache =>
                {
                    cache.DefaultSlidingExpireTime = TwoFactorCodeCacheItem.DefaultSlidingExpireTime;
                });

            if (_appConfiguration["Authentication:JwtBearer:IsEnabled"] != null &&
                bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                ConfigureTokenAuth();
            }

            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

            Configuration.ReplaceService<IAppConfigurationWriter, AppConfigurationWriter>();

            //Uncomment this line to use Hangfire instead of default background job manager (remember also to uncomment related lines in Startup.cs file(s)).
            //Configuration.BackgroundJobs.UseHangfire();

            //Uncomment this line to use Redis cache instead of in-memory cache.
            //See app.config for Redis configuration and connection string
            //Configuration.Caching.UseRedis(options =>
            //{
            //    options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
            //    options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
            //});
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials =
                new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.AccessTokenExpiration = AppConsts.AccessTokenExpiration;
            tokenAuthConfig.RefreshTokenExpiration = AppConsts.RefreshTokenExpiration;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZeroWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFolders();

            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ZeroWebCoreModule).Assembly);
            
            var currencyRateBackgroundJobService = IocManager.Resolve<ICurrencyRateBackgroundJob>();
            RecurringJob.RemoveIfExists("SyncCurrencyRates");
            RecurringJob.AddOrUpdate("SyncCurrencyRates",() => currencyRateBackgroundJobService.UpdateRates(), Cron.Daily);
            
            var userSubscriptionBackgroundJobService = IocManager.Resolve<IUserSubscriptionBackgroundJob>();
            RecurringJob.RemoveIfExists("UserSubscription_ExpirationCheck");
            RecurringJob.AddOrUpdate("UserSubscription_ExpirationCheck",() => userSubscriptionBackgroundJobService.UserSubscriptionExpirationCheck(), Cron.Daily);
            RecurringJob.RemoveIfExists("UserSubscription_ExpireEmailNotifier");
            RecurringJob.AddOrUpdate("UserSubscription_ExpireEmailNotifier",() => userSubscriptionBackgroundJobService.UserSubscriptionExpireEmailNotifier(), Cron.Daily);
        }

        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath,
                $"Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}SampleProfilePics");
            appFolders.WebLogsFolder = Path.Combine(_env.ContentRootPath, $"App_Data{Path.DirectorySeparatorChar}Logs");
        }
    }
}
