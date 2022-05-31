using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.CarType
{
    public class CarTypeDto: FullAuditedEntityDto
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}