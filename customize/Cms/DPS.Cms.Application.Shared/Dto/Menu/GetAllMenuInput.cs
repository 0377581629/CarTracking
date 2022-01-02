using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.Menu
{
    public class GetAllMenuInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        
        public int? MenuGroupId { get; set; }
    }
}