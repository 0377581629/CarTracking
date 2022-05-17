using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Treasurer
{
    public class GetAllTreasurerInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}