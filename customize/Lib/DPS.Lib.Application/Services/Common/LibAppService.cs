using Abp.Authorization;
using Abp.Domain.Repositories;
using DPS.Lib.Application.Shared.Interface.Common;
using Zero;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;

namespace DPS.Lib.Application.Services.Common
{
    [AbpAuthorize]
    public class LibAppService : ZeroAppServiceBase, ILibAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IRepository<User, long> _userRepository;

        #region Constructor
        public LibAppService(RoleManager roleManager, IRepository<User, long> userRepository)
        {
            _roleManager = roleManager;
            _userRepository = userRepository;
        }
        #endregion

    }
}