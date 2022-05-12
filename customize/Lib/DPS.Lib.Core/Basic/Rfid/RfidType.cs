using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using Zero;
using Zero.Authorization.Users;

namespace DPS.Lib.Core.Basic.Rfid
{
    [Table("Lib_Basic_RfidType")]
    public class RfidType: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string CardNumber { get; set; }
        
        public string CardDer { get; set; }
        
        public DateTime RegisterDate { get; set; }
        
        public long? UserId { get; set; }
        
        [ForeignKey("UserId")]
        [CanBeNull]
        public User User { get; set; }
        
        public bool IsBlackList { get; set; }
        
        public string SerialNumber { get; set; }
        
        public int CardType { get; set; }
    }
}