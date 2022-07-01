using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Transport.Route.Details;

namespace DPS.Lib.Application.Shared.Dto.Transport.Route
{
    public class CreateOrEditRouteDto: FullAuditedEntityDto<int?>
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public int? ManagementUnitId { get; set; }
        
        public string ManagementUnitCode { get; set; }
        
        public string ManagementUnitName { get; set; }
        
        public string ListPoint { get; set; }
        
        public string ListTime { get; set; }
        
        public string RouteDetail { get; set; }
        
        public bool IsPermanentRoute { get; set; }
        
        public double MinuteLate { get; set; }
        
        public double Range { get; set; }
        
        public bool HasConstraintTime { get; set; } 
        
        public int RouteType { get; set; }
        
        public double EstimateDistance { get; set; }
        
        public double EstimatedTime { get; set; }
        
        public List<RouteDetailDto> RouteDetails { get; set; }
    }
}