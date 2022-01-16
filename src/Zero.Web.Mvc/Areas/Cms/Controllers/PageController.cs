using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Application.Shared.Interfaces.Common;
using DPS.Cms.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.Page;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Page)]
    public class PageController : ZeroControllerBase
    {
	    private readonly ICmsAppService _cmsAppService;
        private readonly IPageAppService _pageAppService;
        
        public PageController(IPageAppService pageAppService, ICmsAppService cmsAppService)
        {
	        _pageAppService = pageAppService;
	        _cmsAppService = cmsAppService;
        }

        public ActionResult Index()
        {
            var model = new PageViewModel
			{
				FilterText = ""
			};
			
            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.Page_Create, CmsPermissions.Page_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetPageForEditOutput objEdit;
			if (id.HasValue){
				objEdit = await _pageAppService.GetPageForEdit(new EntityDto { Id = (int) id });
			}
			else{
				objEdit = new GetPageForEditOutput{
					Page = new CreateOrEditPageDto
					{
						Code = StringHelper.ShortIdentity(),
						IsActive = true,
						
						TitleDefault = true,
						DescriptionDefault = true,
						AuthorDefault = true,
						KeywordDefault = true
					}
				};
			}

			var viewModel = new CreateOrEditPageModalViewModel
			{
				Page = objEdit.Page
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
        
        [AbpMvcAuthorize(CmsPermissions.Page_Edit)]
        public async Task<ActionResult> Config(int id)
        {
	        var model = new PageConfigUIViewModel
	        {
				PageConfig = await _pageAppService.GetPageConfig(new EntityDto { Id = id }),
				AvailableWidgets = await _cmsAppService.GetAllWidget()
	        };

	        return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.Page_Edit)]
        public async Task<PartialViewResult> WidgetConfigDetail(int widgetId, string blockColumnId)
        {
	        var res = new PageWidgetDto();
	        
	        var widget = await _cmsAppService.GetWidget(new EntityDto {Id = widgetId});
	        res.PageBlockColumnId = blockColumnId;
	        res.WidgetId = widget.Id;
	        res.WidgetName = widget.Name;
	        res.WidgetContentType = widget.ContentType;
	        res.WidgetContentCount = widget.ContentCount;
	        
	        res.Details = new List<PageWidgetDetailDto>();
	        if (res.WidgetContentType == (int) CmsEnums.WidgetContentType.FixedContent) 
		        return PartialView("Components/Config/WidgetConfigDetail", res);
	        
	        for (var i = 0; i < res.WidgetContentCount; i++)
	        {
		        res.Details.Add(new PageWidgetDetailDto());
	        }

	        return PartialView("Components/Config/WidgetConfigDetail", res);
        }
    }
}