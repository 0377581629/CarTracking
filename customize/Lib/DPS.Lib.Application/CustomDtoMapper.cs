using AutoMapper;
using DPS.Lib.Application.Shared.Dto.Basic.Driver;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Application.Shared.Dto.Basic.Technician;
using DPS.Lib.Application.Shared.Dto.Basic.Treasurer;
using DPS.Lib.Core.Basic.Driver;
using DPS.Lib.Core.Basic.Rfid;
using DPS.Lib.Core.Basic.Technician;
using DPS.Lib.Core.Basic.Treasurer;

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
            
            configuration.CreateMap<DriverDto, Driver>().ReverseMap();
            configuration.CreateMap<CreateOrEditDriverDto, Driver>().ReverseMap();
            configuration.CreateMap<DriverDto, CreateOrEditDriverDto>().ReverseMap();

            #endregion
        }
    }
}