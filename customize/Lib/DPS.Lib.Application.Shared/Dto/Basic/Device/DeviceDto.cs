using System;
using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Device
{
    public class DeviceDto: FullAuditedEntityDto
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string SimCard { get; set; }
        
        public int? NetworkProviderId { get; set; }
        
        public string NetworkProviderCode { get; set; }
        
        public string NetworkProviderName { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public string Imei { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool NeedUpdate { get; set; }
    }
}