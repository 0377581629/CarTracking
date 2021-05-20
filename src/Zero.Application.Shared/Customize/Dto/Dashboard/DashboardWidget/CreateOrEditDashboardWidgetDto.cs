using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.Dashboard.DashboardWidget
{
    public class  CreateOrEditDashboardWidgetDto : EntityDto<int?>
    {
	    public string WidgetId { get; set; }

	    public string Name { get; set; }
	    public string Description { get; set; }
	    public byte Height { get; set; }

	    public byte Width { get; set; }

	    public byte PositionX { get; set; }

	    public byte PositionY { get; set; }
        
	    public string ViewName { get; set; }
        
	    public string JsPath { get; set; }
        
	    public string CssPath { get; set; }

	    public string Filters { get; set; }
	    
	    public bool IsDefault { get; set; }
    }
}