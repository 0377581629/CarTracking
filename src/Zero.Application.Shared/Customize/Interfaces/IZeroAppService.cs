using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zero.Dto;
using Zero.Editions.Dto;
using Zero.MultiTenancy.Dto;

namespace Zero.Customize.Interfaces
{
    public interface IZeroAppService : IApplicationService 
    {
	    #region Edition
	    Task<EditionListDto> GetCurrentEdition();

	    Task<int> GetCurrentEditionId();
	    #endregion
	    
	    #region Tenancy
	    Task<TenantListDto> GetTenantById(int tenantId);
	    
	    /// <summary>
	    /// Return list child tenants by target Tenant
	    /// </summary>
	    /// <param name="targetTenant">default = currentTenant</param>
	    /// <returns></returns>
	    Task<List<TenantListDto>> GetChildTenants(int? targetTenant = default);
	    
	    /// <summary>
	    /// Return list child tenant's id by target Tenant
	    /// </summary>
	    /// <param name="targetTenant">default = currentTenant</param>
	    /// <returns></returns>
	    Task<List<int>> GetChildTenantIds(int? targetTenant = default);
	    
	    /// <summary>
	    /// Return list child tenant mapped to SelectListItem by target Tenant
	    /// </summary>
	    /// <param name="targetTenant">default = currentTenant</param>
	    /// <param name="currentSelect">id selected</param>
	    /// <returns></returns>
	    Task<List<SelectListItem>> GetChildTenantsDropDown(int? targetTenant = default, int? currentSelect = default);
	    
	    Task<PagedResultDto<GetTenancyForSelectViewDto>> SearchTenancy(GetSelectModalInput input);

	    #endregion
	    
	    #region Role
	    Task<List<int>> GetCurrentRoleIds();
	    #endregion
    }
}