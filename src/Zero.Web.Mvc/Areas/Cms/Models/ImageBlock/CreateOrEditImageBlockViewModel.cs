using DPS.Cms.Application.Shared.Dto.ImageBlock;

namespace Zero.Web.Areas.Cms.Models.ImageBlock
{
    public class CreateOrEditImageBlockModalViewModel
    {
        public CreateOrEditImageBlockDto ImageBlock { get; set; }

        public bool IsEditMode => ImageBlock.Id.HasValue;
    }
}