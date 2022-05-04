using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Organization.WorkDepartment
{
    public class GetAllWorkDepartmentInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        
    }
}