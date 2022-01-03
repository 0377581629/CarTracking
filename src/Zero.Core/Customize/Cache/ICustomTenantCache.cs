using System.Threading.Tasks;

namespace Zero.Customize.Cache
{
    public interface ICustomTenantCache
    {
        CustomTenantCacheItem Get(int tenantId);

        CustomTenantCacheItem GetOrNull(int tenantId);
        
        CustomTenantCacheItem Get(string tenancyName);

        CustomTenantCacheItem GetOrNull(string tenancyName);

        CustomTenantCacheItem GetByDomain(string domain);

        CustomTenantCacheItem GetOrNullByDomain(string domain);

        Task<CustomTenantCacheItem> GetAsync(int tenantId);

        Task<CustomTenantCacheItem> GetAsync(string tenancyName);

        Task<CustomTenantCacheItem> GetOrNullAsync(string tenancyName);

        Task<CustomTenantCacheItem> GetOrNullAsync(int tenantId);
    }
}