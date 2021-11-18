using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zero.Customize.Interfaces;
using Zero.Dto;
using Zero.Editions;
using Zero.Editions.Dto;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Dto;

namespace Zero.Customize
{
    [AbpAuthorize]
    public class ZeroAppService : ZeroAppServiceBase, IZeroAppService
    {
        #region Constructor

        private readonly IRepository<Tenant> _tenantRepository;
        private readonly EditionManager _editionManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        public ZeroAppService(
            IRepository<Tenant> tenantRepository,
            EditionManager editionManager, 
            IRepository<UserRole, long> userRoleRepository)
        {
            _tenantRepository = tenantRepository;
            _editionManager = editionManager;
            _userRoleRepository = userRoleRepository;
        }

        #endregion

        #region Edition

        public async Task<EditionListDto> GetCurrentEdition()
        {
            if (AbpSession.TenantId.HasValue)
            {
                var tenant = await _tenantRepository.FirstOrDefaultAsync(AbpSession.TenantId.Value);
                if (tenant != null && tenant.EditionId.HasValue)
                {
                    var edition = await _editionManager.FindByIdAsync(tenant.EditionId.Value);
                    return ObjectMapper.Map<EditionListDto>(edition);
                }
            }

            return null;
        }

        public async Task<int> GetCurrentEditionId()
        {
            var edition = await GetCurrentEdition();
            return edition?.Id ?? 0;
        }

        #endregion

        #region Tenant

        private IQueryable<TenantListDto> TenantSelectQuery(string filter = default, IReadOnlyCollection<long> listSelected = default,
            IReadOnlyCollection<long> listRemoved = default, bool mustIn = default, int? id = null)
        {
            var query = from o in _tenantRepository.GetAll()
                    .Where(o => !o.IsDeleted)
                    .WhereIf(!string.IsNullOrEmpty(filter), e => e.Name.ToUpper().Contains(filter.ToUpper()))
                    .WhereIf(!mustIn && listRemoved != null, e => !listRemoved.Contains(e.Id))
                    .WhereIf(mustIn && listSelected != null, e => listSelected.Contains(e.Id))
                    .WhereIf(id.HasValue, e => e.Id == id)
                join parent in _tenantRepository.GetAll() on o.ParentId equals parent.Id into parentGroup
                from parent in parentGroup.DefaultIfEmpty()
                select new TenantListDto()
                {
                    Selected = listSelected != null && listSelected.Contains(o.Id),
                    Id = o.Id,
                    Code = o.Code,
                    TenancyName = o.TenancyName,
                    Name = o.Name,
                    ParentId = o.ParentId,
                    ParentTenancyName = parent != null ? parent.TenancyName : "",
                    ParentName = parent != null ? parent.Name : "",
                    LoginLogoId = o.LoginLogoId,
                    LoginBackgroundId = o.LoginBackgroundId,
                    MenuLogoId = o.MenuLogoId
                };
            return query;
        }

        public async Task<TenantListDto> GetTenantById(int tenantId)
        {
            var query = TenantSelectQuery(id: tenantId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TenantListDto>> GetChildTenants(int? targetTenant = default)
        {
            var lstTenant = new List<TenantListDto>();
            if (targetTenant.HasValue && targetTenant > 0)
            {
                var tenant = await _tenantRepository.FirstOrDefaultAsync(targetTenant.Value);
                if (tenant != null)
                {
                    var lstTenantQuery = from o in _tenantRepository.GetAll()
                            .Where(o => o.Name != "Default" &&
                                        o.Id != targetTenant &&
                                        o.Code.ToLower().StartsWith(tenant.Code))
                            .OrderBy(o => o.Code)
                        select ObjectMapper.Map<TenantListDto>(o);
                    lstTenant = await lstTenantQuery.ToListAsync();
                }
            }
            else
            {
                if (AbpSession.MultiTenancySide == MultiTenancySides.Host)
                {
                    var lstTenantQuery = from o in _tenantRepository.GetAll()
                            .Where(o => o.Name != "Default")
                            .OrderBy(o => o.Code)
                        select ObjectMapper.Map<TenantListDto>(o);
                    lstTenant = await lstTenantQuery.ToListAsync();
                }
                else if (AbpSession.TenantId.HasValue)
                {
                    var tenant = await _tenantRepository.FirstOrDefaultAsync((int) AbpSession.TenantId);
                    if (tenant != null)
                    {
                        var lstTenantQuery = from o in _tenantRepository.GetAll()
                                .Where(o => o.Name != "Default" && o.Code.ToLower().StartsWith(tenant.Code))
                                .OrderBy(o => o.Code)
                            select ObjectMapper.Map<TenantListDto>(o);
                        lstTenant = await lstTenantQuery.ToListAsync();
                    }
                }
            }

            return lstTenant.Distinct().ToList();
        }

        public async Task<List<int>> GetChildTenantIds(int? targetTenant = default)
        {
            var tenants = await GetChildTenants(targetTenant);
            return tenants.Select(o => o.Id).ToList();
        }

        public async Task<List<SelectListItem>> GetChildTenantsDropDown(int? targetTenant = default, int? currentSelect = default)
        {
            var tenants = await GetChildTenants(targetTenant);
            return tenants.Select(o => new SelectListItem()
            {
                Selected = o.Id == (currentSelect ?? 0),
                Text = o.TenancyName,
                Value = o.Id.ToString()
            }).ToList();
        }

        public async Task<PagedResultDto<GetTenancyForSelectViewDto>> SearchTenancy(GetSelectModalInput input)
        {
            var lstRemoved = new List<long>();
            if (!string.IsNullOrEmpty(input.ListRemoved))
            {
                try
                {
                    lstRemoved = input.ListRemoved.Split(",").Where(o => !string.IsNullOrEmpty(o)).ToList()
                        .Select(o => Convert.ToInt64(o)).Where(o => o > 0).ToList();
                }
                catch
                {
                    // ignored
                }
            }

            var lstSelected = new List<long>();
            if (!string.IsNullOrEmpty(input.ListSelected))
            {
                try
                {
                    lstSelected = input.ListSelected.Split(",").Where(o => !string.IsNullOrEmpty(o)).ToList()
                        .Select(o => Convert.ToInt64(o)).Where(o => o > 0).ToList();
                }
                catch
                {
                    // ignored
                }
            }

            var query = TenantSelectQuery(input.Filter, lstSelected, lstRemoved);

            var pagedAndFilteredObjs = query.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetTenancyForSelectViewDto()
                {
                    Tenant = o
                };

            var totalCount = await query.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetTenancyForSelectViewDto>(
                totalCount,
                res
            );
        }
        
        #endregion

        #region Role

        public async Task<List<int>> GetCurrentRoleIds()
        {
            return !AbpSession.UserId.HasValue
                ? null
                : (await _userRoleRepository.GetAllListAsync(o => o.UserId == AbpSession.UserId.Value)).Select(o => o.RoleId).ToList();
        }

        #endregion
    }
}