using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Zero.MultiTenancy;

namespace Zero.Customize.Cache
{
    public class CustomTenantCache<TTenant> : ICustomTenantCache, IEventHandler<EntityChangedEventData<TTenant>>
        where TTenant : Tenant
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CustomTenantCache(
            ICacheManager cacheManager,
            IRepository<TTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual CustomTenantCacheItem Get(int tenantId)
        {
            var cacheItem = GetOrNull(tenantId);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given id: " + tenantId);
            }

            return cacheItem;
        }

        public CustomTenantCacheItem GetOrNull(int tenantId)
        {
            return _cacheManager
                .GetTenantCache()
                .Get(
                    tenantId,
                    () =>
                    {
                        var tenant = GetTenantOrNull(tenantId);
                        if (tenant == null)
                        {
                            return null;
                        }

                        return CreateCustomTenantCacheItem(tenant);
                    }
                );
        }
        
        public virtual CustomTenantCacheItem Get(string tenancyName)
        {
            var cacheItem = GetOrNull(tenancyName);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given tenancy name: " + tenancyName);
            }

            return cacheItem;
        }

        public virtual CustomTenantCacheItem GetOrNull(string tenancyName)
        {
            var tenantId = _cacheManager
                .GetTenantByNameCache()
                .Get(
                    tenancyName.ToLowerInvariant(),
                    () => GetTenantOrNull(tenancyName)?.Id
                );

            if (tenantId == null)
            {
                return null;
            }

            return Get(tenantId.Value);
        }
        
        public virtual CustomTenantCacheItem GetByDomain(string domain)
        {
            var cacheItem = GetOrNullByDomain(domain);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given domain name: " + domain);
            }

            return cacheItem;
        }

        public virtual CustomTenantCacheItem GetOrNullByDomain(string domain)
        {
            var tenantId = _cacheManager
                .GetTenantByDomainCache()
                .Get(
                    domain.ToLowerInvariant(),
                    () => GetTenantOrNullByDomain(domain)?.Id
                );

            if (tenantId == null)
            {
                return null;
            }

            return Get(tenantId.Value);
        }
        
        public virtual async Task<CustomTenantCacheItem> GetAsync(int tenantId)
        {
            var cacheItem = await GetOrNullAsync(tenantId);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given id: " + tenantId);
            }

            return cacheItem;
        }

        public virtual async Task<CustomTenantCacheItem> GetAsync(string tenancyName)
        {
            var cacheItem = await GetOrNullAsync(tenancyName);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given tenancy name: " + tenancyName);
            }

            return cacheItem;
        }

        public virtual async Task<CustomTenantCacheItem> GetOrNullAsync(string tenancyName)
        {
            var tenantId = await _cacheManager
                .GetTenantByNameCache()
                .GetAsync(
                    tenancyName.ToLowerInvariant(), async _ => (await GetTenantOrNullAsync(tenancyName))?.Id
                );

            if (tenantId == null)
            {
                return null;
            }

            return await GetAsync(tenantId.Value);
        }

        public virtual async Task<CustomTenantCacheItem> GetOrNullAsync(int tenantId)
        {
            return await _cacheManager
                .GetTenantCache()
                .GetAsync(
                    tenantId, async _ =>
                    {
                        var tenant = await GetTenantOrNullAsync(tenantId);
                        if (tenant == null)
                        {
                            return null;
                        }

                        return CreateCustomTenantCacheItem(tenant);
                    }
                );
        }

        protected virtual CustomTenantCacheItem CreateCustomTenantCacheItem(TTenant tenant)
        {
            return new CustomTenantCacheItem
            {
                Id = tenant.Id,
                Name = tenant.Name,
                TenancyName = tenant.TenancyName,
                EditionId = tenant.EditionId,
                ConnectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString),
                IsActive = tenant.IsActive,
                Domain = tenant.Domain
            };
        }

        [UnitOfWork]
        protected virtual TTenant GetTenantOrNull(int tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.FirstOrDefault(tenantId);
            }
        }

        [UnitOfWork]
        protected virtual TTenant GetTenantOrNull(string tenancyName)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.FirstOrDefault(t => t.TenancyName == tenancyName);
            }
        }

        [UnitOfWork]
        protected virtual TTenant GetTenantOrNullByDomain(string domain)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.FirstOrDefault(t => t.Domain == domain);
            }
        }
        
        [UnitOfWork]
        protected virtual async Task<TTenant> GetTenantOrNullAsync(int tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return await _tenantRepository.FirstOrDefaultAsync(tenantId);
            }
        }

        [UnitOfWork]
        protected virtual async Task<TTenant> GetTenantOrNullAsync(string tenancyName)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
            }
        }

        public virtual void HandleEvent(EntityChangedEventData<TTenant> eventData)
        {
            var existingCacheItem = _cacheManager.GetTenantCache().GetOrDefault(eventData.Entity.Id);

            _cacheManager
                .GetTenantByNameCache()
                .Remove(
                    existingCacheItem != null
                        ? existingCacheItem.TenancyName.ToLowerInvariant()
                        : eventData.Entity.TenancyName.ToLowerInvariant()
                );

            _cacheManager
                .GetTenantCache()
                .Remove(eventData.Entity.Id);
        }
    }
}