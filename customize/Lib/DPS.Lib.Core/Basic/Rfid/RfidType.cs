using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Zero.Customize.Base;

namespace DPS.Lib.Core.Basic.Rfid
{
    [Table("Lib_RfidType")]
    public class RfidType: SimpleFullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
    }
}