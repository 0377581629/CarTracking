using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;
using Zero.Authorization.Permissions;
using Zero.Authorization.Permissions.Dto;
using Zero.Authorization.Roles.Dto;
using Zero.Customize.Dashboard;

namespace Zero.Authorization.Roles
{
    /// <summary>
    /// Application service that is used by 'role management' page.
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Administration_Roles)]
    public class RoleAppService : ZeroAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly IPermissionAppService _permissionAppService;
        private readonly IRepository<RoleDashboardWidget> _roleDashboardWidgetRepository;
        public RoleAppService(
            RoleManager roleManager,
            IRoleManagementConfig roleManagementConfig, IRepository<RoleDashboardWidget> roleDashboardWidgetRepository, IPermissionAppService permissionAppService)
        {
            _roleManager = roleManager;
            _roleManagementConfig = roleManagementConfig;
            _roleDashboardWidgetRepository = roleDashboardWidgetRepository;
            _permissionAppService = permissionAppService;
        }

        [HttpPost]
        public async Task<ListResultDto<RoleListDto>> GetRoles(GetRolesInput input)
        {
            var query = _roleManager.Roles;

            if (input.Permissions != null && input.Permissions.Any(p => !string.IsNullOrEmpty(p)))
            {
                input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                    r => r.GrantAllPermissionsByDefault &&
                         r.Side == AbpSession.MultiTenancySide
                ).Select(r => r.RoleName).ToList();

                foreach (var permission in input.Permissions)
                {
                    query = query.Where(r =>
                        r.Permissions.Any(rp => rp.Name == permission)
                            ? r.Permissions.Any(rp => rp.Name == permission && rp.IsGranted)
                            : staticRoleNames.Contains(r.Name)
                    );
                }
            }

            var roles = await query.ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        public async Task<GetRoleForEditOutput> GetRoleForEdit(NullableIdDto input)
        {
            // var permissions = PermissionManager.GetAllPermissions();
            // var permissionsByEdition = await _permissionAppService.GetAllPermissionsByCurrentTenant();
            // permissions = permissions.Where(o => permissionsByEdition.Contains(o.Name)).ToList();

            var permissions = _permissionAppService.GetAllPermissions().Items;
            
            var grantedPermissions = Array.Empty<Permission>();
            RoleEditDto roleEditDto;

            if (input.Id.HasValue) //Editing existing role?
            {
                var role = await _roleManager.GetRoleByIdAsync(input.Id.Value);
                grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
                roleEditDto = ObjectMapper.Map<RoleEditDto>(role);
            }
            else
            {
                roleEditDto = new RoleEditDto();
            }

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        public async Task CreateOrUpdateRole(CreateOrUpdateRoleInput input)
        {
            input.GrantedDashboardWidgets ??= new List<int>();
            if (input.Role.Id.HasValue)
            {
                await UpdateRoleAsync(input);
            }
            else
            {
                await CreateRoleAsync(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Delete)]
        public async Task DeleteRole(EntityDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            var users = await UserManager.GetUsersInRoleAsync(role.Name);
            foreach (var user in users)
            {
                CheckErrors(await UserManager.RemoveFromRoleAsync(user, role.Name));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Edit)]
        protected virtual async Task UpdateRoleAsync(CreateOrUpdateRoleInput input)
        {
            EntityFrameworkManager.ContextFactory = _ => _roleDashboardWidgetRepository.GetDbContext();
            Debug.Assert(input.Role.Id != null, "input.Role.Id should be set.");

            var role = await _roleManager.GetRoleByIdAsync(input.Role.Id.Value);
            role.DisplayName = input.Role.DisplayName;
            role.IsDefault = input.Role.IsDefault;

            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
            
            // Dashboard Widget
            var lstDetail = input.GrantedDashboardWidgets.Select(o => new RoleDashboardWidget
            {
                RoleId = role.Id,
                DashboardWidgetId = o
            }).ToList();
            if (lstDetail.Any())
                await _roleDashboardWidgetRepository.GetDbContext().BulkSynchronizeAsync(lstDetail,
                    options => { options.ColumnSynchronizeDeleteKeySubsetExpression = detail => detail.RoleId; });
            else
                await _roleDashboardWidgetRepository.DeleteAsync(o => o.RoleId == role.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Roles_Create)]
        protected virtual async Task CreateRoleAsync(CreateOrUpdateRoleInput input)
        {
            var role = new Role(AbpSession.TenantId, input.Role.DisplayName) { IsDefault = input.Role.IsDefault };
            CheckErrors(await _roleManager.CreateAsync(role));
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the role.
            await UpdateGrantedPermissionsAsync(role, input.GrantedPermissionNames);
            // Dashboard Widget
            var lstDetail = input.GrantedDashboardWidgets.Select(o => new RoleDashboardWidget
            {
                RoleId = role.Id,
                DashboardWidgetId = o
            }).ToList();
            if (lstDetail.Any())
                await _roleDashboardWidgetRepository.GetDbContext().BulkSynchronizeAsync(lstDetail,
                    options => { options.ColumnSynchronizeDeleteKeySubsetExpression = detail => detail.RoleId; });
            else
                await _roleDashboardWidgetRepository.DeleteAsync(o => o.RoleId == role.Id);
        }

        private async Task UpdateGrantedPermissionsAsync(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(grantedPermissionNames);
            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
    }
}
