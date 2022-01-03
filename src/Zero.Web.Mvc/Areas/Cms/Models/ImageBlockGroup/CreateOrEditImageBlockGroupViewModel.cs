using DPS.Cms.Application.Shared.Dto.ImageBlockGroup;

namespace Zero.Web.Areas.Cms.Models.ImageBlockGroup
{
    public class CreateOrEditImageBlockGroupModalViewModel
    {
        public CreateOrEditImageBlockGroupDto ImageBlockGroup { get; set; }

        public bool IsEditMode => ImageBlockGroup.Id.HasValue;
    }
}