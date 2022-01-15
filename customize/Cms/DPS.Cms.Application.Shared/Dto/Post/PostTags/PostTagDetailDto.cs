using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Post.PostTags
{
    public class PostTagDetailDto : EntityDto<int?>
    {
        public int? TenantId { get; set; }

        public int? PostId { get; set; }

        public int? TagId { get; set; }
        
        public string TagName { get; set; }
        
        public string TagCode { get; set; }
        
        public string TagNote { get; set; }

    }
}