using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Security;
using Microsoft.EntityFrameworkCore;
using Zero.Authorization;
using Zero.Authorization.Roles;
using Zero.Customize;
using Zero.Customize.Dashboard;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Dto;
using Zero.Url;

namespace Zero.MultiTenancy
{
    [AbpAuthorize(AppPermissions.Pages_Tenants)]
    public class TenantAppService : ZeroAppServiceBase, ITenantAppService
    {
        public IAppUrlService AppUrlService { get; set; }
        public IEventBus EventBus { get; set; }

        private readonly RoleManager _roleManager;
        
        private readonly IRepository<EditionPermission> _editionPermissionRepository;
        
        private readonly IRepository<EditionDashboardWidget> _editionDashboardWidgetRepository;

        private readonly IRepository<RoleDashboardWidget> _roleDashboardWidgetRepository;
        
        public TenantAppService(RoleManager roleManager, IRepository<EditionPermission> editionPermissionRepository, IRepository<EditionDashboardWidget> editionDashboardWidgetRepository, IRepository<RoleDashboardWidget> roleDashboardWidgetRepository)
        {
            _roleManager = roleManager;
            _editionPermissionRepository = editionPermissionRepository;
            _editionDashboardWidgetRepository = editionDashboardWidgetRepository;
            _roleDashboardWidgetRepository = roleDashboardWidgetRepository;
            AppUrlService = NullAppUrlService.Instance;
            EventBus = NullEventBus.Instance;
        }

        public async Task<PagedResultDto<TenantListDto>> GetTenants(GetTenantsInput input)
        {
            var query = TenantManager.Tenants
                .Include(t => t.Edition)
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), t => t.Name.Contains(input.Filter) || t.TenancyName.Contains(input.Filter))
                .WhereIf(input.CreationDateStart.HasValue, t => t.CreationTime >= input.CreationDateStart.Value)
                .WhereIf(input.CreationDateEnd.HasValue, t => t.CreationTime <= input.CreationDateEnd.Value)
                .WhereIf(input.SubscriptionEndDateStart.HasValue, t => t.SubscriptionEndDateUtc >= input.SubscriptionEndDateStart.Value.ToUniversalTime())
                .WhereIf(input.SubscriptionEndDateEnd.HasValue, t => t.SubscriptionEndDateUtc <= input.SubscriptionEndDateEnd.Value.ToUniversalTime())
                .WhereIf(input.EditionIdSpecified, t => t.EditionId == input.EditionId);

            var tenantCount = await query.CountAsync();
            var tenants = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<TenantListDto>(
                tenantCount,
                ObjectMapper.Map<List<TenantListDto>>(tenants)
                );
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Create)]
        [UnitOfWork(IsDisabled = true)]
        public async Task CreateTenant(CreateTenantInput input)
        {
            await TenantManager.CreateWithAdminUserAsync(
                input.ParentId,
                input.TenancyName,
                input.Name,
                input.AdminPassword,
                input.AdminEmailAddress,
                input.ConnectionString,
                input.IsActive,
                input.EditionId,
                input.ShouldChangePasswordOnNextLogin,
                input.SendActivationEmail,
                input.SubscriptionEndDateUtc?.ToUniversalTime(),
                input.IsInTrialPeriod,
                input.Domain,
                AppUrlService.CreateEmailActivationUrlFormat(input.TenancyName)
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task<TenantEditDto> GetTenantForEdit(EntityDto input)
        {
            var tenantEditDto = ObjectMapper.Map<TenantEditDto>(await TenantManager.GetByIdAsync(input.Id));
            tenantEditDto.ConnectionString = SimpleStringCipher.Instance.Decrypt(tenantEditDto.ConnectionString);
            return tenantEditDto;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task UpdateTenant(TenantEditDto input)
        {
            await TenantManager.CheckEditionAsync(input.EditionId, input.IsInTrialPeriod);

            input.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            var tenant = await TenantManager.GetByIdAsync(input.Id);

            if (tenant.EditionId != input.EditionId)
            {
                await EventBus.TriggerAsync(new TenantEditionChangedEventData
                {
                    TenantId = input.Id,
                    OldEditionId = tenant.EditionId,
                    NewEditionId = input.EditionId
                });
            }

            ObjectMapper.Map(input, tenant);
            tenant.SubscriptionEndDateUtc = tenant.SubscriptionEndDateUtc?.ToUniversalTime();

            await TenantManager.UpdateAsync(tenant);
            
            #region Customize
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                var roles = await _roleManager.Roles.ToListAsync();
                if (roles.Any())
                {
                    var systemPermissions = PermissionManager.GetAllPermissions();
                    var editionPermissions = await _editionPermissionRepository.GetAllListAsync(o => o.EditionId == tenant.EditionId);
                    var editionDashboardWidgetIds = await _editionDashboardWidgetRepository.GetAll().Where(o => o.EditionId == tenant.EditionId).Select(o=>o.DashboardWidgetId).ToListAsync();
                    var permissions = systemPermissions.Where(o => editionPermissions.Select(p=>p.PermissionName).Contains(o.Name)).ToList();
                    foreach (var role in roles)
                    {
                        var verifiedPermissions = StaticRolesHelper.AddRequiredPermissions(role, permissions);
                        
                        // Remove Dashboard Widget not in target editions
                        await _roleDashboardWidgetRepository.DeleteAsync(o => o.RoleId == role.Id && !editionDashboardWidgetIds.Contains(o.DashboardWidgetId));
                        
                        // Admin role
                        if (role.Name == StaticRoleNames.Tenants.Admin)
                        {
                            await _roleManager.ResetAllPermissionsAsync(role);
                            await _roleManager.HardGrantedPermissionsAsync(role, verifiedPermissions);
                        }
                        else
                        {
                            // Other role
                            if (role.Permissions == null || !role.Permissions.Any()) continue;
                            var listInTwo = verifiedPermissions.Select(o => o.Name).Intersect(role.Permissions.Select(o => o.Name)).ToList();
                            if (!listInTwo.Any()) continue;
                            {
                                var permissionLeft = verifiedPermissions.Where(o => listInTwo.Contains(o.Name)).ToList();
                                await _roleManager.ResetAllPermissionsAsync(role);
                                await _roleManager.HardGrantedPermissionsAsync(role, permissionLeft);
                            }
                        }
                    }
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
            #endregion
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Delete)]
        public async Task DeleteTenant(EntityDto input)
        {
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            await TenantManager.DeleteAsync(tenant);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Tenant));
            var featureValues = await TenantManager.GetFeatureValuesAsync(input.Id);

            return new GetTenantFeaturesEditOutput
            {
                Features = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList(),
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await TenantManager.SetFeatureValuesAsync(input.Id, input.FeatureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await TenantManager.ResetAllFeaturesAsync(input.Id);
        }

        public async Task UnlockTenantAdmin(EntityDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.Id))
            {
                var tenantAdmin = await UserManager.GetAdminAsync();
                tenantAdmin?.Unlock();
            }
        }
    }
}