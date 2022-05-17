using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Technician
{
    public class GetAllTechnicianInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}