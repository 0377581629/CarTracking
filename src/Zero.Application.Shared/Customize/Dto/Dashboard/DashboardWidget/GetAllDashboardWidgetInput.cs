using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.Dashboard.DashboardWidget
{
    public class GetAllDashboardWidgetInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}