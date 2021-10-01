using System;
using System.Globalization;
using System.Linq;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Threading;
using Abp.Timing;
using Fixer.dotnet;
using Fixer.dotnet.Abstractions;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;
using Zero.Authorization.Users;
using Zero.Configuration;
using Zero.MultiTenancy;

namespace Zero.Customize.BackgroundJobs
{
    public class UserSubscriptionBackgroundJob : ZeroDomainServiceBase, IUserSubscriptionBackgroundJob
    {
        #region Constructor

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly TenantManager _tenantManager;
        private readonly IRepository<User, long> _userRepository;

        private readonly UserManager _userManager;
        private readonly UserEmailer _userEmailer;

        public UserSubscriptionBackgroundJob(IUnitOfWorkManager unitOfWorkManager, TenantManager tenantManager, IRepository<User, long> userRepository, UserEmailer userEmailer, UserManager userManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _tenantManager = tenantManager;
            _userRepository = userRepository;
            _userEmailer = userEmailer;
            _userManager = userManager;
        }

        #endregion

        public void UserSubscriptionExpirationCheck()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            var utcNow = Clock.Now.ToUniversalTime();
            // Tenant Side
            var tenants = AsyncHelper.RunSync(()=>_tenantManager.Tenants.Where(o=>!o.IsDeleted && o.IsActive).ToListAsync());
            if (tenants.Any())
            {
                foreach (var tenant in tenants)
                {
                    var useSubscriptionUser = AsyncHelper.RunSync(() => SettingManager.GetSettingValueForTenantAsync<bool>(AppSettings.UserManagement.SubscriptionUser, tenant.Id));
                    if (!useSubscriptionUser) continue;
                    var userByTenants = AsyncHelper.RunSync(() => _userRepository.GetAllListAsync(o =>
                        o.TenantId == tenant.Id && o.UserName != AbpUserBase.AdminUserName && o.SubscriptionEndDateUtc != null && o.SubscriptionEndDateUtc < utcNow && o.IsActive));
                    if (!userByTenants.Any()) continue;
                    foreach (var user in userByTenants)
                    {
                        user.IsActive = false;
                        AsyncHelper.RunSync(() => _userManager.UpdateSecurityStampAsync(user));
                    }
                }
            }
            // Host side
            var useSubscriptionUserInHost = AsyncHelper.RunSync(() => SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SubscriptionUser));
            if (!useSubscriptionUserInHost) return;
            var userInHost = AsyncHelper.RunSync(() => _userRepository.GetAllListAsync(o =>
                o.TenantId == null && 
                o.UserName != AbpUserBase.AdminUserName && 
                o.SubscriptionEndDateUtc != null && 
                o.SubscriptionEndDateUtc < utcNow &&
                o.IsActive));
            if (!userInHost.Any()) return;
            foreach (var user in userInHost)
            {
                user.IsActive = false;
                AsyncHelper.RunSync(() => _userManager.UpdateSecurityStampAsync(user));
            }
            
            unitOfWork.Complete();
        }

        public void UserSubscriptionExpireEmailNotifier()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            var dateToCheckRemainingDayCount = Clock.Now.Date.AddDays(3).ToUniversalTime();
            // Tenants side
            var tenants = AsyncHelper.RunSync(()=>_tenantManager.Tenants.Where(o=>!o.IsDeleted && o.IsActive).ToListAsync());
            if (tenants.Any())
            {
                foreach (var tenant in tenants)
                {
                    var useSubscriptionUser = AsyncHelper.RunSync(() => SettingManager.GetSettingValueForTenantAsync<bool>(AppSettings.UserManagement.SubscriptionUser, tenant.Id));
                    if (!useSubscriptionUser) continue;
                    var userByTenants = AsyncHelper.RunSync(() => _userRepository.GetAllListAsync(o =>
                        o.TenantId == tenant.Id && 
                        o.UserName != AbpUserBase.AdminUserName && 
                        o.SubscriptionEndDateUtc != null && 
                        EF.Functions.DateDiffDay(o.SubscriptionEndDateUtc.Value.Date,dateToCheckRemainingDayCount) == 0 &&
                        o.IsActive));
                    if (!userByTenants.Any()) continue;
                    foreach (var user in userByTenants)
                    {
                        AsyncHelper.RunSync(() => _userEmailer.TryToSendUserSubscriptionExpiringSoonEmail(user.Id, dateToCheckRemainingDayCount));
                    }
                }
            }
            // Host side
            var useSubscriptionUserInHost = AsyncHelper.RunSync(() => SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SubscriptionUser));
            if (!useSubscriptionUserInHost) return;
            var userInHost = AsyncHelper.RunSync(() => _userRepository.GetAllListAsync(o =>
                o.TenantId == null && 
                o.UserName != AbpUserBase.AdminUserName && 
                o.SubscriptionEndDateUtc != null && 
                EF.Functions.DateDiffDay(o.SubscriptionEndDateUtc.Value.Date,dateToCheckRemainingDayCount) == 0 &&
                o.IsActive));
            if (!userInHost.Any()) return;
            foreach (var user in userInHost)
            {
                AsyncHelper.RunSync(() => _userEmailer.TryToSendUserSubscriptionExpiringSoonEmail(user.Id, dateToCheckRemainingDayCount));
            }
            unitOfWork.Complete();
        }
    }
}