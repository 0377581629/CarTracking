using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.ManagementUnit
{
    public class GetAllManagementUnitInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}