using DPS.Cms.Application.Shared.Dto.MenuGroup;

namespace Zero.Web.Areas.Cms.Models.MenuGroup
{
    public class CreateOrEditMenuGroupViewModel
    {
        public CreateOrEditMenuGroupDto MenuGroup { get; set; }

        public bool IsEditMode => MenuGroup.Id.HasValue;
    }
}