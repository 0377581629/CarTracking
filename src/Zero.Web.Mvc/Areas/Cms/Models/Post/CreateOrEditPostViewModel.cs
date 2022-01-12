using DPS.Cms.Application.Shared.Dto.Post;

namespace Zero.Web.Areas.Cms.Models.Post
{
    public class CreateOrEditPostViewModel
    {
        public CreateOrEditPostDto Post { get; set; }

        public bool IsEditMode => Post.Id.HasValue;
    }
}