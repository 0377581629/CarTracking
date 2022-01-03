using DPS.Cms.Application.Shared.Dto.Tags;

namespace Zero.Web.Areas.Cms.Models.Tags
{
    public class CreateOrEditTagsModalViewModel
    {
        public CreateOrEditTagsDto Tags { get; set; }

        public bool IsEditMode => Tags.Id.HasValue;
    }
}