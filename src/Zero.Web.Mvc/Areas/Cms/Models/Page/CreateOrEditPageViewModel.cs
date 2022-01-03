using DPS.Cms.Application.Shared.Dto.Page;

namespace Zero.Web.Areas.Cms.Models.Page
{
    public class CreateOrEditPageModalViewModel
    {
        public CreateOrEditPageDto Page { get; set; }

        public bool IsEditMode => Page.Id.HasValue;
    }
}