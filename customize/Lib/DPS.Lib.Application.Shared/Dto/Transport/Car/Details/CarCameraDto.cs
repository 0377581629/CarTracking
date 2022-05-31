using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.Car.Details
{
    public class CarCameraDto: EntityDto
    {
        public int CarId { get; set; }
        
        public string Name { get; set; }
        
        public string Position { get; set; }
        
        public int Rotation { get; set; }
    }
}