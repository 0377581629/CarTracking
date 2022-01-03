using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Page
{
    public class GetAllPageInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        
        public int? PageThemeId { get; set; }
    }
}