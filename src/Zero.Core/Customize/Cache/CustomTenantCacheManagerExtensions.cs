using Abp.Runtime.Caching;

namespace Zero.Customize.Cache
{
    public static class CustomTenantCacheManagerExtensions
    {
        public static ITypedCache<int, CustomTenantCacheItem> GetTenantCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, CustomTenantCacheItem>(CustomTenantCacheItem.CacheName);
        }

        public static ITypedCache<string, int?> GetTenantByNameCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, int?>(CustomTenantCacheItem.ByNameCacheName);
        }
        
        public static ITypedCache<string, int?> GetTenantByDomainCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, int?>(CustomTenantCacheItem.ByDomainCacheName);
        }
    }
}