using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.CarGroup
{
    public class GetAllCarGroupInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}