using System.Collections.Generic;
using Zero.Customize.Dto.Dashboard.DashboardWidget;

namespace Zero.Web.Areas.App.Models.Dashboard
{
    public class DashboardViewModel
    {
        public Customize.Dto.Dashboard.Config.Dashboard UserDashboard { get; set; }
        
        public List<DashboardWidgetDto> AvailableWidgets { get; set; }
    }
}