using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.Device;
using DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Application.Shared.Dto.Transport.CarGroup;
using DPS.Lib.Application.Shared.Dto.Transport.CarType;
using DPS.Lib.Application.Shared.Dto.Transport.Driver;
using Zero.Authorization.Users.Dto;

namespace DPS.Lib.Application.Shared.Interface.Common
{
    public interface ILibAppService: IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetPagedUsers(GetUsersInput input);

        Task<List<RfidTypeDto>> GetAllRfidTypes();

        Task<PagedResultDto<RfidTypeDto>> GetPagedRfidTypes(GetAllRfidTypeInput input);
        
        Task<List<NetworkProviderDto>> GetAllNetworkProviders();

        Task<PagedResultDto<NetworkProviderDto>> GetPagedNetworkProviders(GetAllNetworkProviderInput input);

        Task<List<DeviceDto>> GetAllDevices();

        Task<PagedResultDto<DeviceDto>> GetPagedDevices(GetAllDeviceInput input);

        Task<List<CarTypeDto>> GetAllCarTypes();
        
        Task<PagedResultDto<CarTypeDto>> GetPagedCarTypes(GetAllCarTypeInput input);

        Task<List<CarGroupDto>> GetAllCarGroups();

        Task<PagedResultDto<CarGroupDto>> GetPagedCarGroups(GetAllCarGroupInput input);

        Task<List<DriverDto>> GetAllDrivers();

        Task<PagedResultDto<DriverDto>> GetPagedDrivers(GetAllDriverInput input);
    }
}