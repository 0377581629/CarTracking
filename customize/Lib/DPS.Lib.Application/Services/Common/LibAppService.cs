using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using DPS.Lib.Application.Shared.Interface.Common;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;
using Zero.Authorization.Users.Dto;

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

        
        private IQueryable<User> GetUsersFilteredQuery(IGetUsersInput input)
        {
            var query = UserManager.Users
                .Where(o=>o.IsActive && !o.IsDeleted)
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(input.OnlyLockedUsers, u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(
                    !string.IsNullOrEmpty(input.Filter),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            return query;
        }
        
        public async Task<PagedResultDto<UserListDto>> GetPagedUsers(GetUsersInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            
            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
            );
        }
    }
}