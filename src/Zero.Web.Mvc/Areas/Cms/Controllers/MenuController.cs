using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Web.Areas.Cms.Models.Menu;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Menu)]
    public class MenuController: ZeroControllerBase
    {
        private readonly IMenuAppService _menuAppService;

        public MenuController(IMenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }


        public ActionResult Index()
        {
            var model = new MenuViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.Menu_Create, CmsPermissions.Menu_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetMenuForEditOutput objEdit;

            if (id.HasValue){
                objEdit = await _menuAppService.GetMenuForEdit(new EntityDto { Id = (int) id });
            }
            else{
                objEdit = new GetMenuForEditOutput{
                    Menu = new CreateOrEditMenuDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsActive = true
                    }
                };
            }

            var viewModel = new CreateOrEditMenuViewModel()
            {
                Menu = objEdit.Menu
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}