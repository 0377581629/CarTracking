using System;
using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.AssignmentRoute
{
    public class CreateOrEditAssignmentRouteDto: FullAuditedEntityDto<int?>
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public int? ManagementUnitId { get; set; }
        
        public string ManagementUnitCode { get; set; }
        
        public string ManagementUnitName { get; set; }
        
        public int? CarId { get; set; }
        
        public string CarCode { get; set; }
        
        public string CarLicensePlate { get; set; }
        
        public int? DriverId { get; set; }
        
        public string DriverCode { get; set; }
        
        public string DriverName { get; set; }
        
        public int? RouteId { get; set; }
        
        public string RouteCode { get; set; }
        
        public string RouteName { get; set; }
        
        public int? TreasurerId { get; set; }
        
        public string TreasurerCode { get; set; }
        
        public string TreasurerName { get; set; }
        
        public int? TechnicianId { get; set; }
        
        public string TechnicianCode { get; set; }
        
        public string TechnicianName { get; set; }
        
        public string Guard { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string DayOfWeeks { get; set; }
        
        public bool IsAssignment { get; set; }
    }
}