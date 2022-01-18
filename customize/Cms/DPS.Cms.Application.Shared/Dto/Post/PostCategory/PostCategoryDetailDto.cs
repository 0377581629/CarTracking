using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Post.PostCategory
{
    public class PostCategoryDetailDto : EntityDto<int?>
    {
        public int? TenantId { get; set; }

        public int? PostId { get; set; }

        public int CategoryId { get; set; }
        
    }
}