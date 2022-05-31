using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Zero;

namespace DPS.Lib.Core.Basic.NetworkProvider
{
    [Table("Lib_Basic_NetworkProvider")]
    public class NetworkProvider: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string AccessPoint { get; set; }

        public string GprsUserName { get; set; }
        
        public string GprsPassword { get; set; }
    }
}