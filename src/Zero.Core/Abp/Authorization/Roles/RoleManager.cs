using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Editions;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zero.Authorization.Users;
using Zero.Customize;
using Zero.MultiTenancy;

namespace Zero.Authorization.Roles
{
    /// <summary>
    /// Role manager.
    /// Used to implement domain logic for roles.
    /// </summary>
    public class RoleManager : AbpRoleManager<Role, User>
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly ICacheManager _cacheManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IPermissionManager _permissionManager;

        private readonly IRepository<Edition> _editionRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IRepository<EditionPermission> _editionPermissionRepository;
        
        private IRolePermissionStore<Role> RolePermissionStore
        {
            get
            {
                if (!(Store is IRolePermissionStore<Role>))
                {
                    throw new AbpException("Store is not IRolePermissionStore");
                }
                return Store as IRolePermissionStore<Role>;
            }
        }
        
        public RoleManager(
            RoleStore store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager> logger,
            IPermissionManager permissionManager,
            IRoleManagementConfig roleManagementConfig,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            ILocalizationManager localizationManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository, IRepository<EditionPermission> editionPermissionRepository, IMultiTenancyConfig multiTenancyConfig, IRepository<Tenant> tenantRepository, IRepository<Edition> editionRepository)
            : base(
                store,
                roleValidators,
                keyNormalizer,
                errors,
                logger,
                permissionManager,
                cacheManager,
                unitOfWorkManager,
                roleManagementConfig,
                organizationUnitRepository,
                organizationUnitRoleRepository)
        {
            _permissionManager = permissionManager;
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _localizationManager = localizationManager;
            _editionPermissionRepository = editionPermissionRepository;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantRepository = tenantRepository;
            _editionRepository = editionRepository;
        }

        public override Task SetGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            CheckPermissionsToUpdate(role, permissions);

            return base.SetGrantedPermissionsAsync(role, permissions);
        }
        
        public async Task HardGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            CheckPermissionsToUpdate(role, permissions);
            foreach (var permission in permissions)
            {
                await GrantPermissionAsync(role, permission);
            }
            var cacheKey = role.Id + "@" + (role.TenantId ?? 0);
            await _cacheManager.GetRolePermissionCache().RemoveAsync(cacheKey);
        }
        
        public virtual async Task<Role> GetRoleByIdAsync(long roleId)
        {
            var role = await FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                throw new ApplicationException("There is no role with id: " + roleId);
            }

            return role;
        }

        private void CheckPermissionsToUpdate(Role role, IEnumerable<Permission> permissions)
        {
            if (role.Name == StaticRoleNames.Host.Admin &&
                (!permissions.Any(p => p.Name == AppPermissions.Pages_Administration_Roles_Edit) ||
                 !permissions.Any(p => p.Name == AppPermissions.Pages_Administration_Users_ChangePermissions)))
            {
                throw new UserFriendlyException(L("YouCannotRemoveUserRolePermissionsFromAdminRole"));
            }
        }

        private new string L(string name)
        {
            return _localizationManager.GetString(ZeroConst.LocalizationSourceName, name);
        }

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// </summary>
        /// <param name="roleId">role id</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public override async Task<bool> IsGrantedAsync(int roleId, Permission permission)
        {
            //Get cached role permissions
            var cacheItem = await GetRolePermissionCacheItemAsync(roleId);

            //Check the permission
            return cacheItem.GrantedPermissions.Contains(permission.Name);
        }

        /// <summary>
        /// Checks if a role is granted for a permission.
        /// </summary>
        /// <param name="roleId">role id</param>
        /// <param name="permission">The permission</param>
        /// <returns>True, if the role has the permission</returns>
        public override bool IsGranted(int roleId, Permission permission)
        {
            //Get cached role permissions
            var cacheItem = GetRolePermissionCacheItem(roleId);

            //Check the permission
            return cacheItem.GrantedPermissions.Contains(permission.Name);
        }
        
        private async Task<RolePermissionCacheItem> GetRolePermissionCacheItemAsync(int roleId)
        {
            var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? 0);
            return await _cacheManager.GetRolePermissionCache().GetAsync(cacheKey, async () =>
            {
                var newCacheItem = new RolePermissionCacheItem(roleId);

                var role = await Store.FindByIdAsync(roleId.ToString(), CancellationToken);
                if (role == null)
                    throw new AbpException("There is no role with given id: " + roleId);

                var editionPermissionNames = new List<string>();
                if (_multiTenancyConfig.IsEnabled && role.TenantId.HasValue)
                {
                    var tenant = await _tenantRepository.GetAsync(role.TenantId.Value);
                    if (tenant.EditionId.HasValue)
                        editionPermissionNames = await _editionPermissionRepository.GetAll().Where(o => o.EditionId == tenant.EditionId.Value).Select(o => o.PermissionName).ToListAsync();
                }
                
                var staticRoleDefinition = RoleManagementConfig.StaticRoles.FirstOrDefault(r =>
                    r.RoleName == role.Name && r.Side == role.GetMultiTenancySide());

                if (staticRoleDefinition != null)
                {
                    foreach (var permission in _permissionManager.GetAllPermissions())
                    {
                        if (_multiTenancyConfig.IsEnabled && role.TenantId.HasValue)
                        {
                            if (editionPermissionNames.Contains(permission.Name) && staticRoleDefinition.IsGrantedByDefault(permission))
                                newCacheItem.GrantedPermissions.Add(permission.Name);    
                        }
                        else if (staticRoleDefinition.IsGrantedByDefault(permission))
                            newCacheItem.GrantedPermissions.Add(permission.Name);
                        
                    }
                }

                foreach (var permissionInfo in await RolePermissionStore.GetPermissionsAsync(roleId))
                {
                    if (_multiTenancyConfig.IsEnabled && role.TenantId.HasValue)
                    {
                        if (editionPermissionNames.Contains(permissionInfo.Name) && permissionInfo.IsGranted)
                            newCacheItem.GrantedPermissions.AddIfNotContains(permissionInfo.Name);
                        else
                            newCacheItem.GrantedPermissions.Remove(permissionInfo.Name);
                    }
                    else
                    {
                        if (permissionInfo.IsGranted)
                            newCacheItem.GrantedPermissions.AddIfNotContains(permissionInfo.Name);
                        else
                            newCacheItem.GrantedPermissions.Remove(permissionInfo.Name);
                    }
                }
                return newCacheItem;
            });
        }

        private RolePermissionCacheItem GetRolePermissionCacheItem(int roleId)
        {
            var cacheKey = roleId + "@" + (GetCurrentTenantId() ?? 0);
            return _cacheManager.GetRolePermissionCache().Get(cacheKey, () =>
            {
                var newCacheItem = new RolePermissionCacheItem(roleId);

                var role = AbpStore.FindById(roleId.ToString(), CancellationToken);
                if (role == null)
                    throw new AbpException("There is no role with given id: " + roleId);

                var editionPermissionNames = new List<string>();
                if (_multiTenancyConfig.IsEnabled && role.TenantId.HasValue)
                {
                    var tenant = _tenantRepository.Get(role.TenantId.Value);
                    if (tenant.EditionId.HasValue)
                        editionPermissionNames = _editionPermissionRepository.GetAll().Where(o => o.EditionId == tenant.EditionId.Value).Select(o => o.PermissionName).ToList();
                }
                
                var staticRoleDefinition = RoleManagementConfig.StaticRoles.FirstOrDefault(r =>
                    r.RoleName == role.Name && r.Side == role.GetMultiTenancySide());

                if (staticRoleDefinition != null)
                {
                    foreach (var permission in _permissionManager.GetAllPermissions())
                    {
                        if (_multiTenancyConfig.IsEnabled && role.TenantId.HasValue)
                        {
                            if (editionPermissionNames.Contains(permission.Name) && staticRoleDefinition.IsGrantedByDefault(permission))
                                newCacheItem.GrantedPermissions.Add(permission.Name);    
                        }
                        else if (staticRoleDefinition.IsGrantedByDefault(permission))
                            newCacheItem.GrantedPermissions.Add(permission.Name);
                    }
                }

                foreach (var permissionInfo in RolePermissionStore.GetPermissions(roleId))
                {
                    if (_multiTenancyConfig.IsEnabled && role.TenantId.HasValue)
                    {
                        if (editionPermissionNames.Contains(permissionInfo.Name) && permissionInfo.IsGranted)
                            newCacheItem.GrantedPermissions.AddIfNotContains(permissionInfo.Name);
                        else
                            newCacheItem.GrantedPermissions.Remove(permissionInfo.Name);
                    }
                    else
                    {
                        if (permissionInfo.IsGranted)
                            newCacheItem.GrantedPermissions.AddIfNotContains(permissionInfo.Name);
                        else
                            newCacheItem.GrantedPermissions.Remove(permissionInfo.Name);
                    }
                }

                return newCacheItem;
            });
        }
        
        private int? GetCurrentTenantId()
        {
            return _unitOfWorkManager.Current != null ? _unitOfWorkManager.Current.GetTenantId() : AbpSession.TenantId;
        }
    }
}
