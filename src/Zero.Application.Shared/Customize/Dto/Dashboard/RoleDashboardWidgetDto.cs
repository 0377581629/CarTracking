using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.Dashboard
{
	public class RoleDashboardWidgetDto : EntityDto
    {
	    public int RoleId { get; set; }
        
	    public int DashboardWidgetId { get; set; }
    }
}