using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Zero;

namespace DPS.Lib.Core.Transport.CarGroup
{
    [Table("Lib_Transport_CarGroup")]
    public class CarGroup: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsSpecialGroup { get; set; }
        
        public string City { get; set; }
    }
}