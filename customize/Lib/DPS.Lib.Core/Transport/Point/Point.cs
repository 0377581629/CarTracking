using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DPS.Lib.Core.Basic.ManagementUnit;
using JetBrains.Annotations;
using Zero;

namespace DPS.Lib.Core.Transport.Point
{
    [Table("Lib_Transport_Point")]
    public class Point: FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [Required]
        [StringLength(ZeroConst.MaxCodeLength, MinimumLength = ZeroConst.MinCodeLength)]
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Note { get; set; }
        
        public int? PointTypeId { get; set; }
        [ForeignKey("PointTypeId")]
        [CanBeNull]
        public PointType.PointType PointType { get; set; }
        
        public int? ManagementUnitId { get; set; }
        [ForeignKey("ManagementUnitId")]
        [CanBeNull]
        public ManagementUnit ManagementUnit { get; set; }
        
        public string Address { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
        
        public string Phone { get; set; }
        
        public string Fax { get; set; }
        
        public string ContactPerson { get; set; }
    }
}