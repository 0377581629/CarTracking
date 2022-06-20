using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.Point
{
    public class GetAllPointInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}