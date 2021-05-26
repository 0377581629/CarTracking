using System.Collections.Generic;
using Zero.Customize.Dto.Dashboard.DashboardWidget;

namespace Zero.Web.Areas.App.Models.Dashboard
{
    public class AddWidgetViewModel
    {
        public List<DashboardWidgetDto> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
