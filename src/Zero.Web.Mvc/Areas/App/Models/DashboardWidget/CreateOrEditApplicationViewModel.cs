using Zero.Customize.Dto.Dashboard.DashboardWidget;

namespace Zero.Web.Areas.App.Models.DashboardWidget
{
    public class CreateOrEditDashboardWidgetModalViewModel
    {
       public CreateOrEditDashboardWidgetDto DashboardWidget { get; set; }

       public bool IsEditMode => DashboardWidget.Id.HasValue;
    }
}