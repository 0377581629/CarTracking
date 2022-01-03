using DPS.Cms.Application.Shared.Dto.PageTheme;

namespace Zero.Web.Areas.Cms.Models.PageTheme
{
    public class CreateOrEditPageThemeViewModel
    {
        public CreateOrEditPageThemeDto PageTheme { get; set; }

        public bool IsEditMode => PageTheme.Id.HasValue;
    }
}