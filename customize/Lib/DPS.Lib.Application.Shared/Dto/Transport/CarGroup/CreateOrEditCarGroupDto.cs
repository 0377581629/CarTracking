using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.CarGroup
{
    public class CreateOrEditCarGroupDto: FullAuditedEntityDto<int?>
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsSpecialGroup { get; set; }
        
        public string City { get; set; }
    }
}