using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.Timing;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.Configuration;
using Zero.MultiTenancy;

namespace Zero.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            UserClaimsPrincipalFactory claimsPrincipalFactory)
            : base(
                userManager,
                multiTenancyConfig,
                tenantRepository,
                unitOfWorkManager,
                settingManager,
                userLoginAttemptRepository,
                userManagementConfig,
                iocResolver,
                passwordHasher,
                roleManager,
                claimsPrincipalFactory)
        {
        }

        protected override Task<AbpLoginResult<Tenant, User>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {
            // Check User Subscription
            var now = Clock.Now.ToUniversalTime();
            if (string.IsNullOrEmpty(tenancyName))
            {
                var useSubscriptionUser = AsyncHelper.RunSync(() => SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SubscriptionUser));
                if (useSubscriptionUser)
                {
                    var user = AsyncHelper.RunSync(() => UserManager.FindByNameOrEmailAsync(userNameOrEmailAddress));
                    if (user is { SubscriptionEndDateUtc: { } } && user.SubscriptionEndDateUtc.Value < now)
                    {
                        throw new Exception("UserIsExpiredSubscription");
                    }
                }
            }
            else
            {
                var tenant = AsyncHelper.RunSync(() => TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName));
                if (tenant != null)
                {
                    var useSubscriptionUser = AsyncHelper.RunSync(() => SettingManager.GetSettingValueForTenantAsync<bool>(AppSettings.UserManagement.SubscriptionUser, tenant.Id));
                    if (useSubscriptionUser)
                    {
                        using (UnitOfWorkManager.Current.SetTenantId(tenant.Id))
                        {
                            var user = AsyncHelper.RunSync(() => UserManager.FindByNameOrEmailAsync(userNameOrEmailAddress));
                            if (user is { SubscriptionEndDateUtc: { } } && user.SubscriptionEndDateUtc.Value < now)
                            {
                                throw new Exception("UserIsExpiredSubscription");
                            }
                        }
                    }
                }
            }

            return base.LoginAsyncInternal(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
        }

        protected override Task<AbpLoginResult<Tenant, User>> LoginAsyncInternal(UserLoginInfo login, string tenancyName)
        {
            // Check User Subscription
            var now = Clock.Now.ToUniversalTime();
            if (string.IsNullOrEmpty(tenancyName))
            {
                var useSubscriptionUser = AsyncHelper.RunSync(() => SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SubscriptionUser));
                if (useSubscriptionUser)
                {
                    var user = AsyncHelper.RunSync(() => UserManager.FindAsync(null, login));
                    if (user is { SubscriptionEndDateUtc: { } } && user.SubscriptionEndDateUtc.Value < now)
                    {
                        throw new Exception("UserIsExpiredSubscription");
                    }
                }
            }
            else
            {
                var tenant = AsyncHelper.RunSync(() => TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName));
                if (tenant != null)
                {
                    var useSubscriptionUser = AsyncHelper.RunSync(() => SettingManager.GetSettingValueForTenantAsync<bool>(AppSettings.UserManagement.SubscriptionUser, tenant.Id));
                    if (useSubscriptionUser)
                    {
                        using (UnitOfWorkManager.Current.SetTenantId(tenant.Id))
                        {
                            var user = AsyncHelper.RunSync(() => UserManager.FindAsync(tenant.Id, login));
                            if (user is { SubscriptionEndDateUtc: { } } && user.SubscriptionEndDateUtc.Value < now)
                            {
                                throw new Exception("UserIsExpiredSubscription");
                            }
                        }
                    }
                }
            }

            return base.LoginAsyncInternal(login, tenancyName);
        }
    }
}