using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Cms.Application.Shared.Dto.Tags;
using DPS.Cms.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.Tags;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Tags)]
    public class TagsController : ZeroControllerBase
    {
        private readonly ITagsAppService _tagsAppService;
        public TagsController(ITagsAppService tagsAppService)
        {
	        _tagsAppService = tagsAppService;
        }

        public ActionResult Index()
        {
            var model = new TagsViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(CmsPermissions.Tags_Create, CmsPermissions.Tags_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetTagsForEditOutput objEdit;

			if (id.HasValue){
				objEdit = await _tagsAppService.GetTagsForEdit(new EntityDto { Id = (int) id });
			}
			else{
				objEdit = new GetTagsForEditOutput{
					Tags = new CreateOrEditTagsDto()
					{
						Code = StringHelper.ShortIdentity(),
						IsActive = true
					}
				};
			}

			var viewModel = new CreateOrEditTagsModalViewModel()
            {
				Tags = objEdit.Tags
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}