using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Authorization.Permissions;
using Zero.Authorization.Permissions.Dto;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Customize.Interfaces;
using Zero.Editions;
using Zero.MultiTenancy;
using Zero.Web.Areas.App.Models.Editions;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Editions)]
    public class EditionsController : ZeroControllerBase
    {
        private readonly IEditionAppService _editionAppService;
        private readonly IDashboardAppService _dashboardAppService;
        private readonly IPermissionAppService _permissionAppService;
        private readonly TenantManager _tenantManager;

        public EditionsController(
            IEditionAppService editionAppService, 
            TenantManager tenantManager, 
            IDashboardAppService dashboardAppService, 
            IPermissionAppService permissionAppService)
        {
            _editionAppService = editionAppService;
            _tenantManager = tenantManager;
            _dashboardAppService = dashboardAppService;
            _permissionAppService = permissionAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Editions_Create)]
        public async Task<PartialViewResult> CreateModal(int? id)
        {
            var output = await _editionAppService.GetEditionForEdit(new NullableIdDto { Id = id });
            var viewModel = ObjectMapper.Map<CreateEditionModalViewModel>(output);
            viewModel.EditionItems = await _editionAppService.GetEditionComboboxItems(); ;
            viewModel.FreeEditionItems = await _editionAppService.GetEditionComboboxItems(output.Edition.ExpiringEditionId, false, true); ;
  
            var permissions = _permissionAppService.GetAllPermissions(true).Items.ToList();
            viewModel.Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).ToList();
            viewModel.DashboardWidgets = await _dashboardAppService.GetAllDashboardWidget();
            
            if (id != null)
            {
                viewModel.GrantedPermissionNames = await _permissionAppService.GetAllPermissionsByEdition((int) id);
                viewModel.GrantedDashboardWidgets = await _dashboardAppService.GetAllDashboardWidgetByEdition((int) id);
            }
            else
            {
                viewModel.GrantedPermissionNames = new List<string>();
                viewModel.GrantedDashboardWidgets = new List<DashboardWidgetDto>();
            }
            
            return PartialView("_CreateModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Editions_Create, AppPermissions.Pages_Editions_Edit)]
        public async Task<PartialViewResult> EditModal(int? id)
        {
            var output = await _editionAppService.GetEditionForEdit(new NullableIdDto { Id = id });
            var viewModel = ObjectMapper.Map<EditEditionModalViewModel>(output);
            viewModel.EditionItems = await _editionAppService.GetEditionComboboxItems(); ;
            viewModel.FreeEditionItems = await _editionAppService.GetEditionComboboxItems(output.Edition.ExpiringEditionId, false, true); ;

            var permissions = _permissionAppService.GetAllPermissions(true).Items.ToList();
            viewModel.Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).ToList();
            viewModel.DashboardWidgets = await _dashboardAppService.GetAllDashboardWidget();

            if (id != null)
            {
                viewModel.GrantedPermissionNames = await _permissionAppService.GetAllPermissionsByEdition((int) id);
                viewModel.GrantedDashboardWidgets = await _dashboardAppService.GetAllDashboardWidgetByEdition((int) id);
            }
            else
            {
                viewModel.GrantedPermissionNames = new List<string>();
                viewModel.GrantedDashboardWidgets = new List<DashboardWidgetDto>();
            }
            
            return PartialView("_EditModal", viewModel);
        }

        public async Task<PartialViewResult> MoveTenantsToAnotherEdition(int id)
        {
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var tenantCount = _tenantManager.Tenants.Count(t => t.EditionId == id);

            var viewModel = new MoveTenantsToAnotherEditionViewModel
            {
                EditionId = id,
                TenantCount = tenantCount,
                EditionItems = editionItems
            };

            return PartialView("_MoveTenantsToAnotherEdition", viewModel);
        }
    }
}