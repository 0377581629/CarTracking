using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zero.Customize.Dto.EmailTemplate;

namespace ZERO.Web.Areas.App.Models.EmailTemplate
{
    public class CreateOrEditEmailTemplateModalViewModel
    {
        public CreateOrEditEmailTemplateDto EmailTemplate { get; set; }
        
        public List<SelectListItem> ListEmailTemplateType { get; set; }

        public bool IsEditMode => EmailTemplate.Id.HasValue;
    }
}