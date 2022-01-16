using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.ImageBlockGroup;
using DPS.Cms.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.ImageBlockGroup;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.ImageBlockGroup)]
    public class ImageBlockGroupController : ZeroControllerBase
    {
        private readonly IImageBlockGroupAppService _imageBlockGroupAppService;
        public ImageBlockGroupController(IImageBlockGroupAppService imageBlockGroupAppService)
        {
	        _imageBlockGroupAppService = imageBlockGroupAppService;
        }

        public ActionResult Index()
        {
            var model = new ImageBlockGroupViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.ImageBlockGroup_Create, CmsPermissions.ImageBlockGroup_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetImageBlockGroupForEditOutput objEdit;

			if (id.HasValue){
				objEdit = await _imageBlockGroupAppService.GetImageBlockGroupForEdit(new EntityDto { Id = (int) id });
			}
			else{
				objEdit = new GetImageBlockGroupForEditOutput{
					ImageBlockGroup = new CreateOrEditImageBlockGroupDto()
					{
						Code = StringHelper.ShortIdentity(),
						IsActive = true
					}
				};
			}

			var viewModel = new CreateOrEditImageBlockGroupModalViewModel()
            {
				ImageBlockGroup = objEdit.ImageBlockGroup
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}