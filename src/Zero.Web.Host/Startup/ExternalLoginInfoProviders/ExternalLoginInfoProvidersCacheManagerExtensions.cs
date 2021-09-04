using Abp.AspNetZeroCore.Web.Authentication.External;
using Abp.Runtime.Caching;

namespace Zero.Web.Startup.ExternalLoginInfoProviders
{
    public static class ExternalLoginInfoProvidersCacheManagerExtensions
    {
        private const string CacheName = "AppExternalLoginInfoProvidersCache";

        public static ITypedCache<string, ExternalLoginProviderInfo>
            GetExternalLoginInfoProviderCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, ExternalLoginProviderInfo>(CacheName);
        }
    }
}