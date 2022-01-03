using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.PageLayout
{
    public class GetAllPageLayoutInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        
        public int? PageThemeId { get; set; }
    }
}