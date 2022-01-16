using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.MenuGroup;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Application.Shared.Interfaces.Menu;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.MenuGroup;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.MenuGroup)]
    public class MenuGroupController: ZeroControllerBase
    {
        private readonly IMenuGroupAppService _menuGroupAppService;

        public MenuGroupController(IMenuGroupAppService menuGroupAppService)
        {
            _menuGroupAppService = menuGroupAppService;
        }


        public ActionResult Index()
        {
            var model = new MenuGroupViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.MenuGroup_Create, CmsPermissions.MenuGroup_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetMenuGroupForEditOutput objEdit;

            if (id.HasValue){
                objEdit = await _menuGroupAppService.GetMenuGroupForEdit(new EntityDto { Id = (int) id });
            }
            else{
                objEdit = new GetMenuGroupForEditOutput{
                    MenuGroup = new CreateOrEditMenuGroupDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsActive = true
                    }
                };
            }

            var viewModel = new CreateOrEditMenuGroupViewModel()
            {
                MenuGroup = objEdit.MenuGroup
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}