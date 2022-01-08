using DPS.Cms.Application.Shared.Dto.Category;

namespace Zero.Web.Areas.Cms.Models.Category
{
    public class CreateOrEditCategoryModalViewModel
    {
        public CreateOrEditCategoryDto Category { get; set; }

        public bool IsEditMode => Category.Id.HasValue;
    }
}