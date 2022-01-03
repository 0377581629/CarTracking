using DPS.Cms.Application.Shared.Dto.Menu;

namespace Zero.Web.Areas.Cms.Models.Menu
{
    public class CreateOrEditMenuViewModel
    {
        public CreateOrEditMenuDto Menu { get; set; }

        public bool IsEditMode => Menu.Id.HasValue;
    }
}