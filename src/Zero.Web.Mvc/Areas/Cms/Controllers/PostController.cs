using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.Post;
using DPS.Cms.Application.Shared.Interfaces;
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
        public PostController(IPostAppService postAppService)
        {
            _postAppService = postAppService;
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
        public async Task<PartialViewResult> CreateOrEdit(int? id)
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
                Post = objEdit.Post
            };

            return PartialView("CreateOrEdit", viewModel);
        }
    }
}