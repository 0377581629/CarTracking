using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DPS.Lib.Core.Basic.ManagementUnit;
using DPS.Lib.Core.Basic.Technician;
using DPS.Lib.Core.Basic.Treasurer;
using JetBrains.Annotations;
using Zero;

namespace DPS.Lib.Core.Transport.AssignmentRoute
{
    [Table("Lib_Transport_AssignmentRoute")]
    public class AssignmentRoute: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public int? ManagementUnitId { get; set; }
        [ForeignKey("ManagementUnitId")]
        [CanBeNull]
        public ManagementUnit ManagementUnit { get; set; }
        
        public int? CarId { get; set; }
        [ForeignKey("CarId")]
        [CanBeNull]
        public Car.Car Car { get; set; }
        
        public int? DriverId { get; set; }
        [ForeignKey("DriverId")]
        [CanBeNull]
        public Driver.Driver Driver { get; set; }
        
        public int? RouteId { get; set; }
        [ForeignKey("RouteId")]
        [CanBeNull]
        public Route.Route Route { get; set; }
        
        public int? TreasurerId { get; set; }
        [ForeignKey("TreasurerId")]
        [CanBeNull]
        public Treasurer Treasurer { get; set; }
        
        public int? TechnicianId { get; set; }
        [ForeignKey("TechnicianId")]
        [CanBeNull]
        public Technician Technician { get; set; }
        
        public string Guard { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string DayOfWeeks { get; set; }
        
        public bool IsAssignment { get; set; }
    }
}