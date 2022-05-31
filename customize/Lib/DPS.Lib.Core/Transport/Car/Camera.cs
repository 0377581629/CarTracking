using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace DPS.Lib.Core.Transport.Car
{
    [Table("Lib_Transport_Car_Camera")]
    public class Camera: Entity
    {
        public int CarId { get; set; }
        
        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }
        
        public string Name { get; set; }
        
        public string Position { get; set; }
        
        public int Rotation { get; set; }
    }
}