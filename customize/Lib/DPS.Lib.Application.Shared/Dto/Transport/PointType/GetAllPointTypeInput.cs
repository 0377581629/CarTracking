using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.PointType
{
    public class GetAllPointTypeInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}