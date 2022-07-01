using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using JetBrains.Annotations;

namespace DPS.Lib.Core.Transport.Route
{
    [Table("Lib_Transport_RouteDetail")]
    public class RouteDetail: Entity
    {
        public int? RouteId { get; set; }
        [ForeignKey("RouteId")]
        [CanBeNull]
        public Route Route { get; set; }
        
        public int? EndPointId { get; set; }
        [ForeignKey("EndPointId")]
        [CanBeNull]
        public Point.Point EndPoint { get; set; }
        
        public string Time { get; set; }
    }
}