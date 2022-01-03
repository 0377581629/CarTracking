using DPS.Cms.Application.Shared.Dto.Widget;

namespace Zero.Web.Areas.Cms.Models.Widget
{
    public class CreateOrEditWidgetModalViewModel
    {
        public CreateOrEditWidgetDto Widget { get; set; }

        public bool IsEditMode => Widget.Id.HasValue;
    }
}