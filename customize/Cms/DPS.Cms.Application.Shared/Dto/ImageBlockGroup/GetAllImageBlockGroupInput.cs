using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.ImageBlockGroup
{
    public class GetAllImageBlockGroupInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}