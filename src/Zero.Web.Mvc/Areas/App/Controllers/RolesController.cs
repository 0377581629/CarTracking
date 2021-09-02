using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Authorization.Permissions;
using Zero.Authorization.Permissions.Dto;
using Zero.Authorization.Roles;
using Zero.Customize.Interfaces;
using Zero.Web.Areas.App.Models.Roles;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Roles)]
    public class RolesController : ZeroControllerBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IDashboardAppService _dashboardAppService;
        private readonly IPermissionAppService _permissionAppService;
        private readonly IZeroAppService _zeroAppService;
        
        public RolesController(
            IRoleAppService roleAppService,
            IPermissionAppService permissionAppService, IDashboardAppService dashboardAppService, IZeroAppService zeroAppService)
        {
            _roleAppService = roleAppService;
            _permissionAppService = permissionAppService;
            _dashboardAppService = dashboardAppService;
            _zeroAppService = zeroAppService;
        }

        public ActionResult Index()
        {
            var permissions = _permissionAppService.GetAllPermissions().Items.ToList();

            var model = new RoleListViewModel
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).ToList(),
                GrantedPermissionNames = new List<string>()
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Roles_Create, AppPermissions.Pages_Administration_Roles_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            var output = await _roleAppService.GetRoleForEdit(new NullableIdDto { Id = id });
            var viewModel = ObjectMapper.Map<CreateOrEditRoleModalViewModel>(output);
            var currentEditionId = await _zeroAppService.GetCurrentEditionId();
            if (currentEditionId == 0 && AbpSession.MultiTenancySide == MultiTenancySides.Host)
                viewModel.DashboardWidgets = await _dashboardAppService.GetAllDashboardWidget();    
            else
                viewModel.DashboardWidgets = await _dashboardAppService.GetAllDashboardWidgetByEdition(currentEditionId);
            if (id != null)
                viewModel.GrantedDashboardWidgets = await _dashboardAppService.GetAllDashboardWidgetByRole((int) id);
            
            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}