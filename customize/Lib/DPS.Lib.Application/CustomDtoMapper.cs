using AutoMapper;
using DPS.Lib.Application.Shared.Dto.Basic.Device;
using DPS.Lib.Application.Shared.Dto.Basic.ManagementUnit;
using DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Application.Shared.Dto.Basic.Technician;
using DPS.Lib.Application.Shared.Dto.Basic.Treasurer;
using DPS.Lib.Application.Shared.Dto.Transport.AssignmentRoute;
using DPS.Lib.Application.Shared.Dto.Transport.Car;
using DPS.Lib.Application.Shared.Dto.Transport.Car.Details;
using DPS.Lib.Application.Shared.Dto.Transport.CarGroup;
using DPS.Lib.Application.Shared.Dto.Transport.CarType;
using DPS.Lib.Application.Shared.Dto.Transport.Driver;
using DPS.Lib.Application.Shared.Dto.Transport.Point;
using DPS.Lib.Application.Shared.Dto.Transport.PointType;
using DPS.Lib.Application.Shared.Dto.Transport.Route;
using DPS.Lib.Application.Shared.Dto.Transport.Route.Details;
using DPS.Lib.Core.Basic.Device;
using DPS.Lib.Core.Basic.ManagementUnit;
using DPS.Lib.Core.Basic.NetworkProvider;
using DPS.Lib.Core.Basic.Rfid;
using DPS.Lib.Core.Basic.Technician;
using DPS.Lib.Core.Basic.Treasurer;
using DPS.Lib.Core.Transport.AssignmentRoute;
using DPS.Lib.Core.Transport.Car;
using DPS.Lib.Core.Transport.CarGroup;
using DPS.Lib.Core.Transport.CarType;
using DPS.Lib.Core.Transport.Driver;
using DPS.Lib.Core.Transport.Point;
using DPS.Lib.Core.Transport.PointType;
using DPS.Lib.Core.Transport.Route;

namespace DPS.Lib.Application
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            #region Basic

            configuration.CreateMap<RfidTypeDto, RfidType>().ReverseMap();
            configuration.CreateMap<CreateOrEditRfidTypeDto, RfidType>().ReverseMap();
            configuration.CreateMap<RfidTypeDto, CreateOrEditRfidTypeDto>().ReverseMap();
            
            configuration.CreateMap<TechnicianDto, Technician>().ReverseMap();
            configuration.CreateMap<CreateOrEditTechnicianDto, Technician>().ReverseMap();
            configuration.CreateMap<TechnicianDto, CreateOrEditTechnicianDto>().ReverseMap();
            
            configuration.CreateMap<TreasurerDto, Treasurer>().ReverseMap();
            configuration.CreateMap<CreateOrEditTreasurerDto, Treasurer>().ReverseMap();
            configuration.CreateMap<TreasurerDto, CreateOrEditTreasurerDto>().ReverseMap();

            configuration.CreateMap<NetworkProviderDto, NetworkProvider>().ReverseMap();
            configuration.CreateMap<CreateOrEditNetworkProviderDto, NetworkProvider>().ReverseMap();
            configuration.CreateMap<NetworkProviderDto, CreateOrEditNetworkProviderDto>().ReverseMap();
            
            configuration.CreateMap<DeviceDto, Device>().ReverseMap();
            configuration.CreateMap<CreateOrEditDeviceDto, Device>().ReverseMap();
            configuration.CreateMap<DeviceDto, CreateOrEditDeviceDto>().ReverseMap();
            
            configuration.CreateMap<ManagementUnitDto, ManagementUnit>().ReverseMap();
            configuration.CreateMap<CreateOrEditManagementUnitDto, ManagementUnit>().ReverseMap();
            configuration.CreateMap<ManagementUnitDto, CreateOrEditManagementUnitDto>().ReverseMap();

            #endregion

            #region Transport

            configuration.CreateMap<DriverDto, Driver>().ReverseMap();
            configuration.CreateMap<CreateOrEditDriverDto, Driver>().ReverseMap();
            configuration.CreateMap<DriverDto, CreateOrEditDriverDto>().ReverseMap();
            
            configuration.CreateMap<CarTypeDto, CarType>().ReverseMap();
            configuration.CreateMap<CreateOrEditCarTypeDto, CarType>().ReverseMap();
            configuration.CreateMap<CarTypeDto, CreateOrEditCarTypeDto>().ReverseMap();
            
            configuration.CreateMap<CarGroupDto, CarGroup>().ReverseMap();
            configuration.CreateMap<CreateOrEditCarGroupDto, CarGroup>().ReverseMap();
            configuration.CreateMap<CarGroupDto, CreateOrEditCarGroupDto>().ReverseMap();
            
            configuration.CreateMap<CarDto, Car>().ReverseMap();
            configuration.CreateMap<CreateOrEditCarDto, Car>().ReverseMap();
            configuration.CreateMap<CarDto, CreateOrEditCarDto>().ReverseMap();
            configuration.CreateMap<CarCameraDto, Camera>().ReverseMap();
            
            configuration.CreateMap<PointTypeDto, PointType>().ReverseMap();
            configuration.CreateMap<CreateOrEditPointTypeDto, PointType>().ReverseMap();
            configuration.CreateMap<PointTypeDto, CreateOrEditPointTypeDto>().ReverseMap();
            
            configuration.CreateMap<PointDto, Point>().ReverseMap();
            configuration.CreateMap<CreateOrEditPointDto, Point>().ReverseMap();
            configuration.CreateMap<PointDto, CreateOrEditPointDto>().ReverseMap();
            
            configuration.CreateMap<RouteDto, Route>().ReverseMap();
            configuration.CreateMap<CreateOrEditRouteDto, Route>().ReverseMap();
            configuration.CreateMap<RouteDto, CreateOrEditRouteDto>().ReverseMap();
            configuration.CreateMap<RouteDetailDto, RouteDetail>().ReverseMap();
            
            configuration.CreateMap<AssignmentRouteDto, AssignmentRoute>().ReverseMap();
            configuration.CreateMap<CreateOrEditAssignmentRouteDto, AssignmentRoute>().ReverseMap();
            configuration.CreateMap<AssignmentRouteDto, CreateOrEditAssignmentRouteDto>().ReverseMap();

            #endregion
        }
    }
}