using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.CarType
{
    public class GetAllCarTypeInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}