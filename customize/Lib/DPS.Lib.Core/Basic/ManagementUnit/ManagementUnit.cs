using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Zero;

namespace DPS.Lib.Core.Basic.ManagementUnit
{
    [Table("Lib_Basic_ManagementUnit")]
    public class ManagementUnit: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Note { get; set; }
    }
}