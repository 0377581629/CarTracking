using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.PageTheme;
using DPS.Cms.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.PageTheme;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.PageTheme)]
    public class PageThemeController: ZeroControllerBase
    {
        private readonly IPageThemeAppService _pageThemeAppService;

        public PageThemeController(IPageThemeAppService pageThemeAppService)
        {
            _pageThemeAppService = pageThemeAppService;
        }


        public ActionResult Index()
        {
            var model = new PageThemeViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.PageTheme_Create, CmsPermissions.PageTheme_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetPageThemeForEditOutput objEdit;

            if (id.HasValue){
                objEdit = await _pageThemeAppService.GetPageThemeForEdit(new EntityDto { Id = (int) id });
            }
            else{
                objEdit = new GetPageThemeForEditOutput{
                    PageTheme = new CreateOrEditPageThemeDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsActive = true
                    }
                };
            }

            var viewModel = new CreateOrEditPageThemeViewModel()
            {
                PageTheme = objEdit.PageTheme
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}