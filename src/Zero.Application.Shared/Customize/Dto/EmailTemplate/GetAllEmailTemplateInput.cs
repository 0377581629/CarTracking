using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.EmailTemplate
{
    public class GetAllEmailTemplateInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}