using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.ImageBlock
{
    public class GetAllImageBlockInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        
        public int? ImageBlockGroupId { get; set; }
    }
}