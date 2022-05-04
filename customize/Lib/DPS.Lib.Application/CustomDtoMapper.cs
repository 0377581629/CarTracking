using AutoMapper;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Core.Basic.Rfid;

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

            #endregion
        }
    }
}