using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using Zero.Authorization.Users.Dto;

namespace DPS.Lib.Application.Shared.Interface.Common
{
    public interface ILibAppService: IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetPagedUsers(GetUsersInput input);

        Task<List<RfidTypeDto>> GetAllRfidTypes();

        Task<PagedResultDto<RfidTypeDto>> GetPagedRfidTypes(GetAllRfidTypeInput input);
    }
}