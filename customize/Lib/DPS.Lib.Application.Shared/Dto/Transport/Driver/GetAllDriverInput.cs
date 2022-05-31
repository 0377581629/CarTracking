using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.Driver
{
    public class GetAllDriverInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}