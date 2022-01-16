using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zero;
using Zero.Authorization;
using Zero.Customize;
using Zero.Customize.Dto.EmailTemplate;
using Zero.Customize.Interfaces;
using ZERO.Web.Areas.App.Models.EmailTemplate;
using Zero.Web.Controllers;

namespace ZERO.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_EmailTemplates)]
    public class EmailTemplateController : ZeroControllerBase
    {
        private readonly IEmailTemplateAppService _emailTemplateAppService;
        public EmailTemplateController(IEmailTemplateAppService emailTemplateAppService)
        {
	        _emailTemplateAppService = emailTemplateAppService;
        }
        
        public ActionResult Index()
        {
	        var model = new EmailTemplateViewModel
	        {
		        FilterText = ""
	        };
	        return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_EmailTemplates_Create, AppPermissions.Pages_EmailTemplates_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetEmailTemplateForEditOutput getEmailTemplateForEditOutput;

			if (id.HasValue){
				getEmailTemplateForEditOutput = await _emailTemplateAppService.GetEmailTemplateForEdit(new EntityDto { Id = (int) id });
			}
			else{
				getEmailTemplateForEditOutput = new GetEmailTemplateForEditOutput{
					EmailTemplate = new CreateOrEditEmailTemplateDto()
				};
			}

			var viewModel = new CreateOrEditEmailTemplateModalViewModel
			{
				EmailTemplate = getEmailTemplateForEditOutput.EmailTemplate,
				ListEmailTemplateType = EmailHelper.ListEmailTemplateType(0,LocalizationSource),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}