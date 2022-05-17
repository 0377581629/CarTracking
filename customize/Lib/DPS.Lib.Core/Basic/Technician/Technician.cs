using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DPS.Lib.Core.Basic.Rfid;
using JetBrains.Annotations;
using Zero;

namespace DPS.Lib.Core.Basic.Technician
{
    [Table("Lib_Basic_Technician")]
    public class Technician: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public int? RfidTypeId { get; set; }
        
        [ForeignKey("RfidTypeId")]
        [CanBeNull]
        public RfidType RfidType { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        public string Avatar { get; set; }
        
        public string Email { get; set; }
        
        public bool IsStopWorking { get; set; }
        
        public bool Gender { get; set; }
    }
}