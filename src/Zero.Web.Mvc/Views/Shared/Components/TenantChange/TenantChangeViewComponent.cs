using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zero.MultiTenancy;
using Zero.MultiTenancy.Dto;
using Zero.Web.Session;

namespace Zero.Web.Views.Shared.Components.TenantChange
{
    public class TenantChangeViewComponent : ZeroViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public TenantChangeViewComponent(IPerRequestSessionCache sessionCache, IRepository<Tenant> tenantRepository,IUnitOfWorkManager unitOfWorkManager)
        {
            _sessionCache = sessionCache;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loginInfo = await _sessionCache.GetCurrentLoginInformationsAsync();
            var model = ObjectMapper.Map<TenantChangeViewModel>(loginInfo);
            //using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                var availableTenants = await _tenantRepository.GetAllListAsync(o=>o.IsActive && o.Name != "Default") ?? new List<Tenant>();
                model.AvailableTenants = ObjectMapper.Map<List<TenantListDto>>(availableTenants);    
            }
            return View(model);
        }
    }
}
