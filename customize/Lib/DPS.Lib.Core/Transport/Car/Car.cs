using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DPS.Lib.Core.Basic.Device;
using DPS.Lib.Core.Basic.Rfid;
using JetBrains.Annotations;
using Zero;

namespace DPS.Lib.Core.Transport.Car
{
    [Table("Lib_Transport_Car")]
    public class Car : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }

        public string LicensePlate { get; set; }
        
        public int? DeviceId { get; set; }
        
        [ForeignKey("DeviceId")]
        [CanBeNull]
        public Device Device { get; set; }
        
        public int? CarTypeId { get; set; }
        
        [ForeignKey("CarTypeId")]
        [CanBeNull]
        public CarType.CarType CarType { get; set; }
        
        public int? CarGroupId { get; set; }
        
        [ForeignKey("CarGroupId")]
        [CanBeNull]
        public CarGroup.CarGroup CarGroup { get; set; }
        
        public int? DriverId { get; set; }
        
        [ForeignKey("DriverId")]
        [CanBeNull]
        public Driver.Driver Driver { get; set; }
        
        public int? RfidTypeId { get; set; }
        
        [ForeignKey("RfidTypeId")]
        [CanBeNull]
        public RfidType RfidType { get; set; }
        
        public string Note { get; set; }

        public int FuelType { get; set; }
        
        public double Quota { get; set; }

        public int SpeedLimit { get; set; }
    }
}