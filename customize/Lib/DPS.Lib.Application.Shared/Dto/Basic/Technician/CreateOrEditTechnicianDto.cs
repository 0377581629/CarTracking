using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Technician
{
    public class CreateOrEditTechnicianDto: FullAuditedEntityDto<int?>
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public int? RfidTypeId { get; set; }
        
        public string RfidTypeCardNumber { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        public string Avatar { get; set; }
        
        public string Email { get; set; }
        
        public bool IsStopWorking { get; set; }
        
        public bool Gender { get; set; }
    }
}