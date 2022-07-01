using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.AssignmentRoute
{
    public class GetAllAssignmentRouteInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}