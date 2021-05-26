using System.Collections.Generic;
using System.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using System.Threading.Tasks;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Customize.Interfaces;
using Zero.Web.Areas.App.Models.Dashboard;
using Zero.Web.Controllers;
using AddWidgetViewModel = Zero.Web.Areas.App.Models.Dashboard.AddWidgetViewModel;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Dashboard)]
    public class DashboardController : ZeroControllerBase
    {
        private IDashboardAppService _dashboardAppService;
        private IZeroAppService _zeroAppService;
        public DashboardController(IDashboardAppService dashboardAppService, IZeroAppService zeroAppService)
        {
            _dashboardAppService = dashboardAppService;
            _zeroAppService = zeroAppService;
        }

        public async Task<ActionResult> Index()
        {
            var roleIds = await _zeroAppService.GetCurrentRoleIds();
            var viewModel = new DashboardViewModel()
            {
                UserDashboard = await _dashboardAppService.GetUserDashboard(),
                AvailableWidgets = new List<DashboardWidgetDto>()
            };
            if (roleIds != null)
                viewModel.AvailableWidgets = await _dashboardAppService.GetAllDashboardWidgetByRoles(roleIds);
            return View(viewModel);
        }
        
        public async Task<PartialViewResult> AddWidgetModal(string pageId)
        {
            var userDashboard = await _dashboardAppService.GetUserDashboard();
            var roleIds = await _zeroAppService.GetCurrentRoleIds();
            var lstAvailableWidgets = new List<DashboardWidgetDto>();
            if (roleIds != null)
                lstAvailableWidgets = await _dashboardAppService.GetAllDashboardWidgetByRoles(roleIds);
            var page = userDashboard.Pages.Single(p => p.Id == pageId);

            var filteredWidgetsByPermission = lstAvailableWidgets
                .Where(widgetDef => page.Widgets.All(widgetOnPage => widgetOnPage.WidgetId != widgetDef.WidgetId))
                .ToList();

            var viewModel = new AddWidgetViewModel
            {
                Widgets = filteredWidgetsByPermission,
                DashboardName = "Dashboard",
                PageId = pageId
            };

            return PartialView("_AddWidgetModal", viewModel);
        }

    }
}