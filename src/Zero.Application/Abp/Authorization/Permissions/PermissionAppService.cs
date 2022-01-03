using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Zero.Authorization.Permissions.Dto;
using Zero.Customize;
using Zero.MultiTenancy;

namespace Zero.Authorization.Permissions
{
    public class PermissionAppService : ZeroAppServiceBase, IPermissionAppService
    {
        private readonly IRepository<EditionPermission> _editionPermissionRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        public PermissionAppService(IRepository<EditionPermission> editionPermissionRepository, IRepository<Tenant> tenantRepository, IMultiTenancyConfig multiTenancyConfig)
        {
            _editionPermissionRepository = editionPermissionRepository;
            _tenantRepository = tenantRepository;
            _multiTenancyConfig = multiTenancyConfig;
        }

        public ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions(bool tenantSide = false)
        {
            var permissions = PermissionManager.GetAllPermissions();
            if (tenantSide)
                permissions = permissions.Where(o => o.MultiTenancySides != MultiTenancySides.Host).ToList();

            if (_multiTenancyConfig.IsEnabled && AbpSession.MultiTenancySide == MultiTenancySides.Tenant)
            {
                var tenant = _tenantRepository.FirstOrDefault(o => o.Id == AbpSession.TenantId.Value);
                if (tenant != null)
                {
                    var permissionsByEdition = _editionPermissionRepository.GetAllList(o => o.EditionId == tenant.EditionId).Select(o => o.PermissionName);
                    permissions = permissions.Where(o => permissionsByEdition.Contains(o.Name)).ToList();
                }
            }

            var rootPermissions = permissions.Where(p => p.Parent == null);
            var result = new List<FlatPermissionWithLevelDto>();

            foreach (var rootPermission in rootPermissions)
            {
                var level = 0;
                AddPermission(rootPermission, permissions, result, level);
            }

            return new ListResultDto<FlatPermissionWithLevelDto>
            {
                Items = result
            };
        }

        public async Task<List<string>> GetAllPermissionsByEdition(int editionId)
        {
            var lstPermissionId = await _editionPermissionRepository.GetAll().Where(o => o.EditionId == editionId)
                .Select(o => o.PermissionName).ToListAsync();
            var permissions = PermissionManager.GetAllPermissions();
            var result = permissions.Where(o => lstPermissionId.Contains(o.Name)).Select(o => o.Name).ToList();
            return result;
        }

        public async Task<List<string>> GetAllPermissionsByCurrentTenant()
        {
            var permissions = PermissionManager.GetAllPermissions();
            if (AbpSession.MultiTenancySide != MultiTenancySides.Tenant) return permissions.Select(o => o.Name).ToList();
            var tenant = await _tenantRepository.FirstOrDefaultAsync(o => o.Id == AbpSession.TenantId);
            var lstPermissionId = await _editionPermissionRepository.GetAll().Where(o => o.EditionId == tenant.EditionId)
                .Select(o => o.PermissionName).ToListAsync();
            return permissions.Where(o => lstPermissionId.Contains(o.Name)).Select(o => o.Name).ToList();
        }

        private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionWithLevelDto> result, int level)
        {
            var flatPermission = ObjectMapper.Map<FlatPermissionWithLevelDto>(permission);
            flatPermission.Level = level;
            result.Add(flatPermission);

            if (permission.Children == null)
            {
                return;
            }

            var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

            foreach (var childPermission in children)
            {
                AddPermission(childPermission, allPermissions, result, level + 1);
            }
        }
    }
}