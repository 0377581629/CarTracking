using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Dto.PageLayout;

namespace Zero.Web.Areas.Cms.Models.PageLayout
{
    public class CreateOrEditPageLayoutModalViewModel
    {
        public CreateOrEditPageLayoutDto PageLayout { get; set; }

        public bool IsEditMode => PageLayout.Id.HasValue;
    }
}