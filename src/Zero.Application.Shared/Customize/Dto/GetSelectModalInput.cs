using Abp.Application.Services.Dto;
using Zero.MultiTenancy.Dto;

namespace Zero.Dto
{
    public class GetSelectModalInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        
        public string ListSelected { get; set; }
        
        public string ListRemoved { get; set; }
    }
    
    public class GetSelectEmployeeModalInput : GetSelectModalInput
    {
        public int? WorkGroupId { get; set; }
        
        public long? WorkDepartmentId { get; set; }
        
        public int? WorkPositionId { get; set; }
        
        public int? EmployeeStatus { get; set; }

        public bool WorkingOnly { get; set; } = true;
    }
    
    public class GetTenancyForSelectViewDto
    {
        public TenantListDto Tenant { get; set; }
    }
    
}