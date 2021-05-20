using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Zero.Authorization.Permissions.Dto;

namespace Zero.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
