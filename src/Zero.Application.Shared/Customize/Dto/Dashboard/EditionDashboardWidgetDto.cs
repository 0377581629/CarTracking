using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.Dashboard
{
	public class EditionDashboardWidgetDto : EntityDto
    {
	    public int EditionId { get; set; }
        
	    public int DashboardWidgetId { get; set; }
    }
}