using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.PageLayout;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Application.Shared.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.PageLayout;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.PageLayout)]
    public class PageLayoutController : ZeroControllerBase
    {
        private readonly IPageLayoutAppService _pageLayoutAppService;
        
        public PageLayoutController(IPageLayoutAppService pageLayoutAppService)
        {
	        _pageLayoutAppService = pageLayoutAppService;
        }

        public ActionResult Index()
        {
            var model = new PageLayoutViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.PageLayout_Create, CmsPermissions.PageLayout_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetPageLayoutForEditOutput objEdit;

			if (id.HasValue){
				objEdit = await _pageLayoutAppService.GetPageLayoutForEdit(new EntityDto { Id = (int) id });
			}
			else{
				objEdit = new GetPageLayoutForEditOutput{
					PageLayout = new CreateOrEditPageLayoutDto()
					{
						Code = StringHelper.ShortIdentity(),
						IsActive = true
					}
				};
			}

			var viewModel = new CreateOrEditPageLayoutModalViewModel()
            {
				PageLayout = objEdit.PageLayout
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(CmsPermissions.PageLayout_Create, CmsPermissions.PageLayout_Edit)]
        public async Task<ActionResult> Config(int id)
        {
	        var model = new PageLayoutConfigViewModel
	        {
		        PageLayoutConfig = await _pageLayoutAppService.GetPageLayoutForConfig(new EntityDto { Id = id })
	        };
	        
	        return View(model);
        }
        
        [AbpMvcAuthorize(CmsPermissions.PageLayout_Create, CmsPermissions.PageLayout_Edit)]
        public PartialViewResult BlockConfigDetail(int columnCount, string parentUniqueId, string parentColumnUniqueId)
        {
	        var res = new PageLayoutBlockDto
	        {
		        ColumnCount = columnCount,
		        ParentBlockUniqueId = parentUniqueId,
		        ParentColumnUniqueId = parentColumnUniqueId
	        };

	        return PartialView("Components/Config/BlockConfigDetail", res);
        }
    }
}