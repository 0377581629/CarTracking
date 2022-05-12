using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zero.Authorization.Users.Dto;

namespace DPS.Lib.Application.Shared.Interface.Common
{
    public interface ILibAppService: IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetPagedUsers(GetUsersInput input);
    }
}