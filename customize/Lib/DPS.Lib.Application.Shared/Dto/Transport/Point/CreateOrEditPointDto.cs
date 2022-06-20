using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.Point
{
    public class CreateOrEditPointDto: FullAuditedEntityDto<int?>
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Note { get; set; }
        
        public int? PointTypeId { get; set; }
        
        public string PointTypeCode { get; set; }
        
        public string PointTypeName { get; set; }

        public int? ManagementUnitId { get; set; }
        
        public string ManagementUnitCode { get; set; }
        
        public string ManagementUnitName { get; set; }

        public string Address { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
        
        public string Phone { get; set; }
        
        public string Fax { get; set; }
        
        public string ContactPerson { get; set; }
    }
}