using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Customize.Dto.Dashboard.DashboardWidget;
using Zero.Customize.Interfaces;
using Zero.Web.Areas.App.Models.DashboardWidget;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.DashboardWidget)]
    public class DashboardWidgetController : ZeroControllerBase
    {
        private readonly IDashboardWidgetAppService _dashboardWidgetAppService;
        public DashboardWidgetController(IDashboardWidgetAppService dashboardWidgetAppService)
        {
	        _dashboardWidgetAppService = dashboardWidgetAppService;
        }

        public ActionResult Index()
        {
            var model = new DashboardWidgetViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.DashboardWidget_Create, AppPermissions.DashboardWidget_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetDashboardWidgetForEditOutput getDashboardWidgetForEditOutput;

			if (id.HasValue){
				getDashboardWidgetForEditOutput = await _dashboardWidgetAppService.GetDashboardWidgetForEdit(new EntityDto { Id = (int) id });
			}
			else{
				getDashboardWidgetForEditOutput = new GetDashboardWidgetForEditOutput{
					DashboardWidget = new CreateOrEditDashboardWidgetDto()
					{
						WidgetId = StringHelper.Identity(),
						Width = 12,
						Height = 6
					}
				};
			}

			var viewModel = new CreateOrEditDashboardWidgetModalViewModel()
            {
				DashboardWidget = getDashboardWidgetForEditOutput.DashboardWidget
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}