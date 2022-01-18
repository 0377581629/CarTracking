using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.ImageBlock;
using DPS.Cms.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.ImageBlock;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.ImageBlock)]
    public class ImageBlockController : ZeroControllerBase
    {
        private readonly IImageBlockAppService _imageBlockAppService;
        public ImageBlockController(IImageBlockAppService imageBlockAppService)
        {
	        _imageBlockAppService = imageBlockAppService;
        }

        public ActionResult Index()
        {
            var model = new ImageBlockViewModel
			{
				FilterText = ""
			};

            return View(model);
        }
        
        [AbpMvcAuthorize(CmsPermissions.ImageBlock_Create, CmsPermissions.ImageBlock_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetImageBlockForEditOutput objEdit;

			if (id.HasValue){
				objEdit = await _imageBlockAppService.GetImageBlockForEdit(new EntityDto { Id = (int) id });
			}
			else 
			{
				objEdit = new GetImageBlockForEditOutput{
					ImageBlock = new CreateOrEditImageBlockDto()
					{
						Code = StringHelper.ShortIdentity(),
						IsActive = true
					}
				};
			}
			
			var viewModel = new CreateOrEditImageBlockModalViewModel()
            {
				ImageBlock = objEdit.ImageBlock
            };
			
            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}