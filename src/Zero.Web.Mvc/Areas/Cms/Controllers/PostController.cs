using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.Post;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Application.Shared.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.Post;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Post)]
    public class PostController : ZeroControllerBase
    {
        private readonly IPostAppService _postAppService;
        private readonly ICmsAppService _cmsAppService;
        private readonly ICategoryAppService _categoryAppService;

        public PostController(IPostAppService postAppService, ICmsAppService cmsAppService, ICategoryAppService categoryAppService)
        {
            _postAppService = postAppService;
            _cmsAppService = cmsAppService;
            _categoryAppService = categoryAppService;
        }

        public ActionResult Index()
        {
            var model = new PostViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.Post_Create, CmsPermissions.Post_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            GetPostForEditOutput objEdit;

            if (id.HasValue){
                objEdit = await _postAppService.GetPostForEdit(new EntityDto { Id = (int) id });
            }
            else{
                objEdit = new GetPostForEditOutput{
                    Post = new CreateOrEditPostDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsActive = true
                    }
                };
            }
            var viewModel = new CreateOrEditPostViewModel()
            {
                Post = objEdit.Post,
                ListCategory = await _cmsAppService.GetCategory(),
                Categories = await _categoryAppService.GetCategories()
            };

            return View("CreateOrEdit", viewModel);
        }
    }
}