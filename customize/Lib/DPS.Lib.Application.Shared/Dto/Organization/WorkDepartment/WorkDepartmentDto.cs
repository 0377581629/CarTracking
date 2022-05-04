using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Organization.WorkDepartment
{
    public class WorkDepartmentDto : EntityDto<long>
    {
        public int? TenantId { get; set; }
        
        public long? ParentId { get; set; }

        public string Code { get; set; }
        
        public string DepartmentCode { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Describe { get; set; }
        
        public List<WorkDepartmentDto> Children { get; set; }
    }
}