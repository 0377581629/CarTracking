using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using Zero;

namespace DPS.Lib.Core.Basic.Device
{
    [Table("Lib_Basic_Device")]
    public class Device: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string SimCard { get; set; }
        
        public int? NetworkProviderId { get; set; }
        
        [ForeignKey("NetworkProviderId")]
        [CanBeNull]
        public NetworkProvider.NetworkProvider NetworkProvider { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public string Imei { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool NeedUpdate { get; set; }
    }
}