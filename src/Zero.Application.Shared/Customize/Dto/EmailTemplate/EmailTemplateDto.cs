
using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.EmailTemplate
{
	public class EmailTemplateDto : EntityDto
	{
		public int? EmailTemplateType { get; set; }
		
		public string Title { get; set; }
        
		public string Content { get; set; }
        
		public string Sign { get; set; }
        
		public string Note { get; set; }
        
		public bool AutoCreateForNewTenant { get; set; }

		public bool IsActive { get; set; }
	}
}