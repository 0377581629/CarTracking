using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;
using Zero.Authorization;
using Zero.Editions.Dto;
using Zero.MultiTenancy;
using Zero.Authorization.Roles;
using Zero.Customize;
using Zero.Customize.Dashboard;

namespace Zero.Editions
{
    public class EditionAppService : ZeroAppServiceBase, IEditionAppService
    {
        private readonly EditionManager _editionManager;
        private readonly IRepository<SubscribableEdition> _editionRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;

        private readonly RoleManager _roleManager;
        private readonly IRepository<EditionPermission> _editionPermissionRepository;
        private readonly IRepository<EditionDashboardWidget> _editionDashboardWidgetRepository;
        private readonly IRepository<RoleDashboardWidget> _roleDashboardWidgetRepository;
        
        public EditionAppService(
            EditionManager editionManager,
            IRepository<SubscribableEdition> editionRepository,
            IRepository<Tenant> tenantRepository,
            IBackgroundJobManager backgroundJobManager, 
            RoleManager roleManager, 
            IRepository<EditionPermission> editionPermissionRepository, 
            IRepository<EditionDashboardWidget> editionDashboardWidgetRepository, 
            IRepository<RoleDashboardWidget> roleDashboardWidgetRepository)
        {
            _editionManager = editionManager;
            _editionRepository = editionRepository;
            _tenantRepository = tenantRepository;
            _backgroundJobManager = backgroundJobManager;
            _roleManager = roleManager;
            _editionPermissionRepository = editionPermissionRepository;
            _editionDashboardWidgetRepository = editionDashboardWidgetRepository;
            _roleDashboardWidgetRepository = roleDashboardWidgetRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Editions)]
        public async Task<ListResultDto<EditionListDto>> GetEditions()
        {
            var editions = await (from edition in _editionRepository.GetAll()
                                  join expiringEdition in _editionRepository.GetAll() on edition.ExpiringEditionId equals expiringEdition.Id into expiringEditionJoined
                                  from expiringEdition in expiringEditionJoined.DefaultIfEmpty()
                                  select new
                                  {
                                      Edition = edition,
                                      expiringEditionDisplayName = expiringEdition.DisplayName
                                  }).ToListAsync();

            var result = new List<EditionListDto>();

            foreach (var edition in editions)
            {
                var resultEdition = ObjectMapper.Map<EditionListDto>(edition.Edition);
                resultEdition.ExpiringEditionDisplayName = edition.expiringEditionDisplayName;

                result.Add(resultEdition);
            }

            return new ListResultDto<EditionListDto>(result);
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Create, AppPermissions.Pages_Editions_Edit)]
        public async Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Edition));

            EditionEditDto editionEditDto;
            List<NameValue> featureValues;

            if (input.Id.HasValue) //Editing existing edition?
            {
                var edition = await _editionManager.FindByIdAsync(input.Id.Value);
                featureValues = (await _editionManager.GetFeatureValuesAsync(input.Id.Value)).ToList();
                editionEditDto = ObjectMapper.Map<EditionEditDto>(edition);
            }
            else
            {
                editionEditDto = new EditionEditDto();
                featureValues = features.Select(f => new NameValue(f.Name, f.DefaultValue)).ToList();
            }

            var featureDtos = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList();

            return new GetEditionEditOutput
            {
                Edition = editionEditDto,
                Features = featureDtos,
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        private async Task ValidateDataInput(CreateEditionDto input)
        {
            var obj = await _editionRepository.FirstOrDefaultAsync(o => o.DisplayName.ToLower().Contains(input.Edition.DisplayName.ToLower()));
            if (obj != null)
            {
                throw new UserFriendlyException(L("Error"), L("DisplayNameAlreadyExists"));
            }
        }

        private async Task ValidateDataInput(UpdateEditionDto input)
        {
            var obj = await _editionRepository.FirstOrDefaultAsync(o => o.Id != input.Edition.Id && o.DisplayName.ToLower().Contains(input.Edition.DisplayName.ToLower()));
            if (obj != null)
            {
                throw new UserFriendlyException(L("Error"), L("DisplayNameAlreadyExists"));
            }
        }
        
        [AbpAuthorize(AppPermissions.Pages_Editions_Create)]
        public async Task CreateEdition(CreateEditionDto input)
        {
            await ValidateDataInput(input);
            await CreateEditionAsync(input);
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Edit)]
        public async Task UpdateEdition(UpdateEditionDto input)
        {
            await ValidateDataInput(input);
            await UpdateEditionAsync(input);
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Delete)]
        public async Task DeleteEdition(EntityDto input)
        {
            var tenantCount = await _tenantRepository.CountAsync(t => t.EditionId == input.Id);
            if (tenantCount > 0)
            {
                throw new UserFriendlyException(L("ThereAreTenantsSubscribedToThisEdition"));
            }

            var edition = await _editionManager.GetByIdAsync(input.Id);
            await _editionManager.DeleteAsync(edition);
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition)]
        public async Task MoveTenantsToAnotherEdition(MoveTenantsToAnotherEditionDto input)
        {
            await _backgroundJobManager.EnqueueAsync<MoveTenantsToAnotherEditionJob, MoveTenantsToAnotherEditionJobArgs>(new MoveTenantsToAnotherEditionJobArgs
            {
                SourceEditionId = input.SourceEditionId,
                TargetEditionId = input.TargetEditionId,
                User = AbpSession.ToUserIdentifier()
            });
        }

        [AbpAuthorize(AppPermissions.Pages_Editions,AppPermissions.Pages_Tenants)]
        public async Task<List<SubscribableEditionComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null, bool addAllItem = false, bool onlyFreeItems = false)
        {
            var editions = await _editionManager.Editions.ToListAsync();
            var subscribableEditions = editions.Cast<SubscribableEdition>()
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            var editionItems =
                new ListResultDto<SubscribableEditionComboboxItemDto>(subscribableEditions
                    .Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()).Items.ToList();

            var defaultItem = new SubscribableEditionComboboxItemDto("", L("NotAssigned"), null);
            editionItems.Insert(0, defaultItem);

            if (addAllItem)
            {
                editionItems.Insert(0, new SubscribableEditionComboboxItemDto("-1", "- " + L("All") + " -", null));
            }

            if (selectedEditionId.HasValue)
            {
                var selectedEdition = editionItems.FirstOrDefault(e => e.Value == selectedEditionId.Value.ToString());
                if (selectedEdition != null)
                {
                    selectedEdition.IsSelected = true;
                }
            }
            else
            {
                editionItems[0].IsSelected = true;
            }

            return editionItems;
        }

        public async Task<int> GetTenantCount(int editionId)
        {
            return await _tenantRepository.CountAsync(t => t.EditionId == editionId);
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Create)]
        protected virtual async Task CreateEditionAsync(CreateEditionDto input)
        {
            EntityFrameworkManager.ContextFactory = _ => _editionPermissionRepository.GetDbContext();

            var edition = ObjectMapper.Map<SubscribableEdition>(input.Edition);

            if (edition.ExpiringEditionId.HasValue)
            {
                var expiringEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(edition.ExpiringEditionId.Value);
                if (!expiringEdition.IsFree)
                {
                    throw new UserFriendlyException(L("ExpiringEditionMustBeAFreeEdition"));
                }
            }

            await _editionManager.CreateAsync(edition);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.

            await SetFeatureValues(edition, input.FeatureValues);
            
            #region Customize
            // Set permission for edition
            if (input.GrantedPermissionNames != null)
            {
                var lstDetail = input.GrantedPermissionNames.Select(o=>new EditionPermission
                {
                    EditionId = edition.Id,
                    PermissionName = o
                });
                await _editionPermissionRepository.GetDbContext().BulkInsertAsync(lstDetail);
            }
            
            // Dashboard Widget
            if (input.GrantedDashboardWidgets != null)
            {
                var lstDetails = input.GrantedDashboardWidgets.Select(o=>new EditionDashboardWidget
                {
                    EditionId = edition.Id,
                    DashboardWidgetId = o
                });
                await _editionDashboardWidgetRepository.GetDbContext().BulkInsertAsync(lstDetails);
            }
            
            // Ensure system have only default edition
            if (input.Edition.IsDefault)
            {
                var lstSubEdition = await _editionRepository.GetAllListAsync();
                foreach (var itm in lstSubEdition)
                {
                    itm.IsDefault = itm.Id == edition.Id;
                    await _editionRepository.UpdateAsync(itm);
                }
            }
            
            #endregion
        }

        [AbpAuthorize(AppPermissions.Pages_Editions_Edit)]
        protected virtual async Task UpdateEditionAsync(UpdateEditionDto input)
        {
            if (input.Edition.Id != null)
            {
                EntityFrameworkManager.ContextFactory = _ => _editionPermissionRepository.GetDbContext();

                var edition = await _editionManager.GetByIdAsync(input.Edition.Id.Value);

                edition.DisplayName = input.Edition.DisplayName;

                await SetFeatureValues(edition, input.FeatureValues);
                
                #region Customize
                
                if (input.Edition.IsDefault)
                {
                    var lstSubEdition = await _editionRepository.GetAllListAsync();
                    foreach (var itm in lstSubEdition)
                    {
                        itm.IsDefault = itm.Id == input.Edition.Id;
                        await _editionRepository.UpdateAsync(itm);
                    }
                }
                
                await _editionPermissionRepository.DeleteAsync(o => o.EditionId == input.Edition.Id);

                if (input.GrantedPermissionNames != null && input.GrantedPermissionNames.Any())
                {
                    var systemPermissions = PermissionManager.GetAllPermissions();
                    var permissions = systemPermissions.Where(o => input.GrantedPermissionNames.Contains(o.Name)).ToList();

                    var editionPermissions = input.GrantedPermissionNames.Select(o => new EditionPermission
                    {
                        EditionId = edition.Id,
                        PermissionName = o
                    }).ToList();
                    await _editionPermissionRepository.GetDbContext().BulkInsertAsync(editionPermissions);
                    
                    // Dashboard Widget
                    await _editionDashboardWidgetRepository.DeleteAsync(o => o.EditionId == input.Edition.Id);
                    
                    var lstDetails = input.GrantedDashboardWidgets.Select(o=>new EditionDashboardWidget
                    {
                        EditionId = edition.Id,
                        DashboardWidgetId = o
                    }).ToList();
                    
                    await _editionDashboardWidgetRepository.DeleteAsync(o => o.EditionId == edition.Id);
                    
                    if (lstDetails.Any())
                        await _editionDashboardWidgetRepository.GetDbContext().BulkInsertAsync(lstDetails);
                    
                    var tenantIds = await _tenantRepository.GetAll().Where(o => o.EditionId == edition.Id && o.Name != "Default").Select(o=>o.Id).ToListAsync();
                    if (tenantIds.Any())
                    {
                        foreach (var tenantId in tenantIds)
                        {
                            using (CurrentUnitOfWork.SetTenantId(tenantId))
                            {
                                var roles = await _roleManager.Roles.ToListAsync();
                                if (!roles.Any()) continue;
                                var editionDashboardWidgetIds = lstDetails.Select(o => o.DashboardWidgetId).ToList();
                                
                                foreach (var role in roles)
                                {
                                    var verifiedPermissions = StaticRolesHelper.AddRequiredPermissions(role, editionPermissions.Select(o=>new Permission(o.PermissionName)).ToList());
                        
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
                                    
                                    // Admin role
                                    if (role.Name == StaticRoleNames.Tenants.Admin)
                                    {
                                        await _roleManager.ResetAllPermissionsAsync(role);
                                        await _roleManager.SetGrantedPermissionsAsync(role, permissions);
                                    }
                                    else
                                    {
                                        // Other role
                                        if (role.Permissions == null || !role.Permissions.Any()) continue;
                                        var listInTwo = permissions.Select(o => o.Name).Intersect(role.Permissions.Select(o => o.Name)).ToList();
                                        if (!listInTwo.Any()) continue;
                                        {
                                            var permissionLeft = permissions.Where(o => listInTwo.Contains(o.Name)).ToList();
                                            await _roleManager.ResetAllPermissionsAsync(role);
                                            await _roleManager.SetGrantedPermissionsAsync(role, permissionLeft);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }

        private Task SetFeatureValues(Edition edition, List<NameValueDto> featureValues)
        {
            return _editionManager.SetFeatureValuesAsync(edition.Id,
                featureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }
    }
}
