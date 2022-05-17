using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Application.Shared.Interface.Common;
using DPS.Lib.Core.Basic.Rfid;
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
        private readonly IRepository<RfidType> _rfidTypeRepository;

        #region Constructor
        public LibAppService(RoleManager roleManager,
            IRepository<User, long> userRepository,
            IRepository<RfidType> rfidTypeRepository)
        {
            _roleManager = roleManager;
            _userRepository = userRepository;
            _rfidTypeRepository = rfidTypeRepository;
        }
        #endregion

        #region User

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

        #endregion
        
        #region RfidType

        private IQueryable<RfidTypeDto> RfidTypeDataQuery(GetAllRfidTypeInput input = null)
        {
            var query = from o in _rfidTypeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => e.Code.Contains(input.Filter) || e.CardNumber.Contains(input.Filter) ||
                             e.CardDer.Contains(input.Filter))
                select new RfidTypeDto
                {
                    TenantId = o.TenantId,
                    Id = o.Id,
                    Code = o.Code,
                    CardNumber = o.CardNumber,
                    CardDer = o.CardDer,
                    RegisterDate = o.RegisterDate,
                    UserId = o.UserId,
                    UserName = o.User.UserName,
                    IsBlackList = o.IsBlackList,
                    SerialNumber = o.SerialNumber,
                    CardType = o.CardType,
                };
            return query;
        }

        public async Task<List<RfidTypeDto>> GetAllRfidTypes()
        {
            return await RfidTypeDataQuery().ToListAsync();
        }

        public async Task<PagedResultDto<RfidTypeDto>> GetPagedRfidTypes(GetAllRfidTypeInput input)
        {
            var objQuery = RfidTypeDataQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<RfidTypeDto>(
                totalCount,
                res
            );
        }

        #endregion
    }
}