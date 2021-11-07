using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.Text;
using Abp.Timing;
using Abp.UI;
using Abp.Zero;
using Microsoft.AspNetCore.Identity;
using Zero.Authorization.Roles;
using Zero.Configuration;
using Zero.Debugging;
using Zero.MultiTenancy;
using Zero.Notifications;

namespace Zero.Authorization.Users
{
    public class UserRegistrationManager : ZeroDomainServiceBase
    {
        #region Constructor
        public IAbpSession AbpSession { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserPolicy _userPolicy;
        
        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUserEmailer userEmailer,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IUserPolicy userPolicy)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _userPolicy = userPolicy;

            AbpSession = NullAbpSession.Instance;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
        #endregion
        
        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed, string emailActivationLink)
        {
            // CheckForTenant();
            
            CheckSelfRegistrationIsEnabled();

            var tenant = await GetActiveTenantAsync();
            var isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault);

            if (AbpSession.TenantId.HasValue)
                await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

            var user = new User
            {
                TenantId = tenant?.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = isNewRegisteredUserActiveByDefault,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

            if (UseSubscriptionUser())
            {
                user.IsInTrialPeriod = true;
                user.SubscriptionEndDateUtc = Clock.Now.ToUniversalTime().AddDays(UserSubscriptionTrialDays());
            }
            
            user.SetNormalizedNames();

            var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault));
            foreach (var defaultRole in defaultRoles)
            {
                user.Roles.Add(new UserRole(tenant?.Id, user.Id, defaultRole.Id));
            }

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            await CurrentUnitOfWork.SaveChangesAsync();

            if (!user.IsEmailConfirmed && !Debugger.IsAttached)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
            }

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool UseSubscriptionUser()
        {
            return AbpSession.TenantId.HasValue ? SettingManager.GetSettingValueForTenant<bool>(AppSettings.UserManagement.SubscriptionUser, AbpSession.GetTenantId()) : SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.SubscriptionUser);
        }
        
        private int UserSubscriptionTrialDays()
        {
            return AbpSession.TenantId.HasValue ? SettingManager.GetSettingValueForTenant<int>(AppSettings.UserManagement.SubscriptionTrialDays, AbpSession.GetTenantId()) : SettingManager.GetSettingValue<int>(AppSettings.UserManagement.SubscriptionTrialDays);
        }
        
        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
