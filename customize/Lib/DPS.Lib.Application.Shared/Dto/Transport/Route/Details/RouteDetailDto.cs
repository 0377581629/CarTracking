using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Transport.Route.Details
{
    public class RouteDetailDto: EntityDto
    {
        public int? RouteId { get; set; }
        
        public string RouteCode { get; set; }
        
        public string RouteName { get; set; }

        public int? EndPointId { get; set; }
        
        public string EndPointCode { get; set; }
        
        public string EndPointName { get; set; }
        
        public string Time { get; set; }
    }
}