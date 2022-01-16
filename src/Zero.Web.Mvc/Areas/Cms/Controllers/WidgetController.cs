using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.Widget;
using DPS.Cms.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.Widget;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Widget)]
    public class WidgetController : ZeroControllerBase
    {
        private readonly IWidgetAppService _widgetAppService;
        public WidgetController(IWidgetAppService widgetAppService)
        {
	        _widgetAppService = widgetAppService;
        }

        public ActionResult Index()
        {
            var model = new WidgetViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.Widget_Create, CmsPermissions.Widget_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetWidgetForEditOutput objEdit;

			if (id.HasValue){
				objEdit = await _widgetAppService.GetWidgetForEdit(new EntityDto { Id = (int) id });
			}
			else{
				objEdit = new GetWidgetForEditOutput{
					Widget = new CreateOrEditWidgetDto()
					{
						Code = StringHelper.ShortIdentity(),
						IsActive = true,
						PageThemesIds = new List<int?>()
					}
				};
			}

			var viewModel = new CreateOrEditWidgetModalViewModel()
            {
				Widget = objEdit.Widget
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}