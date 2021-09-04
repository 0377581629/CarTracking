using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.EmailTemplate
{
    public class  CreateOrEditEmailTemplateDto : EntityDto<int?>
    {
	    public int? EmailTemplateType { get; set; }
	    
	    public int? TenantId { get; set; }
        
	    public string Title { get; set; }
        
	    public string Content { get; set; }
        
	    public string Sign { get; set; }
        
	    public string Note { get; set; }
        
	    public bool AutoCreateForNewTenant { get; set; }

	    public bool IsActive { get; set; }
    }
}