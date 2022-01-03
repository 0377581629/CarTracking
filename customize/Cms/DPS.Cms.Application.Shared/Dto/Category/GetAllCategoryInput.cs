using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Category
{
    public class GetAllCategoryInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}