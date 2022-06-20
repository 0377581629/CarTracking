using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.Route
{
    public class GetAllRouteInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}