using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DPS.Lib.Core.Basic.ManagementUnit;
using JetBrains.Annotations;
using Zero;

namespace DPS.Lib.Core.Transport.Route
{
    [Table("Lib_Transport_Route")]
    public class Route: FullAuditedEntity, IMayHaveTenant
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
    }
}