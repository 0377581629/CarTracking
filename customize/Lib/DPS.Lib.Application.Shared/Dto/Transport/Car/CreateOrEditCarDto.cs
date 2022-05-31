using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.Car.Details;

namespace DPS.Lib.Application.Shared.Dto.Transport.Car
{
    public class CreateOrEditCarDto: FullAuditedEntityDto<int?>
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }

        public string LicensePlate { get; set; }
        
        public int? DeviceId { get; set; }
        
        public string DeviceCode { get; set; }
        
        public string DeviceSimCard { get; set; }

        public int? CarTypeId { get; set; }
        
        public string CarTypeCode { get; set; }
        
        public string CarTypeName { get; set; }
        
        public int? CarGroupId { get; set; }
        
        public string CarGroupCode { get; set; }
        
        public string CarGroupName { get; set; }
        
        public int? DriverId { get; set; }
        
        public string DriverCode { get; set; }
        
        public string DriverName { get; set; }
        
        public int? RfidTypeId { get; set; }
        
        public string RfidTypeCode { get; set; }
        
        public string RfidTypeCardNumber { get; set; }
        
        public string Note { get; set; }
        
        public int FuelType { get; set; }
        
        public double Quota { get; set; }

        public int SpeedLimit { get; set; }
        
        public List<CarCameraDto> Cameras { get; set; }
    }
}