using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Tags
{
    public class GetAllTagsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}