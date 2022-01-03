using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Widget
{
    public class GetAllWidgetInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        
        public int? PageThemeId { get; set; }
    }
}