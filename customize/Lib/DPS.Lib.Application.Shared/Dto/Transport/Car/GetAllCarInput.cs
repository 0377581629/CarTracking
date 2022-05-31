using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.Car
{
    public class GetAllCarInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}