using Abp.Application.Services.Dto;

namespace DPS.Cms.Application.Shared.Dto.MenuGroup
{
    public class GetAllMenuGroupInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}