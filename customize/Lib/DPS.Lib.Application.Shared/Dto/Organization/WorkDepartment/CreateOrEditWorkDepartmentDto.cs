using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using DPS.Lib.Core.Shared;

namespace DPS.Lib.Application.Shared.Dto.Organization.WorkDepartment
{
    public class CreateOrEditWorkDepartmentDto: EntityDto<long?>
    {
        public int? TenantId { get; set; }
        
        public long? ParentId { get; set; }

        public int WorkGroupId { get; set; }
        
        [Required]
        [StringLength(LibConsts.MaxCodeLength, MinimumLength = LibConsts.MinCodeLength)]
        public string DepartmentCode { get; set; }
        
        [Required]
        [StringLength(LibConsts.MaxStrLength, MinimumLength = LibConsts.MinStrLength)]
        public string DisplayName { get; set; }
        
        public string Describe { get; set; }
    }
}