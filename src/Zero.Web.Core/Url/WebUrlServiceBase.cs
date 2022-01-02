using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Zero.Configuration;
using Zero.Customize.Cache;
using Zero.MultiTenancy;

namespace Zero.Web.Url
{
    public abstract class WebUrlServiceBase
    {
        public const string TenancyNamePlaceHolder = "{TENANCY_NAME}";

        public abstract string WebSiteRootAddressFormatKey { get; }

        public abstract string ServerRootAddressFormatKey { get; }

        public string WebSiteRootAddressFormat => _appConfiguration[WebSiteRootAddressFormatKey] ?? "https://localhost:44302/";

        public string ServerRootAddressFormat => _appConfiguration[ServerRootAddressFormatKey] ?? "https://localhost:44302/";

        public bool SupportsTenancyNameInUrl
        {
            get
            {
                var siteRootFormat = WebSiteRootAddressFormat;
                return !siteRootFormat.IsNullOrEmpty() && siteRootFormat.Contains(TenancyNamePlaceHolder);
            }
        }

        readonly IConfigurationRoot _appConfiguration;

        readonly ICustomTenantCache _customTenantCache;
        
        readonly IHttpContextAccessor _httpContextAccessor;
        
        readonly IRepository<Tenant> _tenantRepository;
        
        public WebUrlServiceBase(IAppConfigurationAccessor configurationAccessor, ICustomTenantCache customTenantCache, IHttpContextAccessor httpContextAccessor, IRepository<Tenant> tenantRepository)
        {
            _customTenantCache = customTenantCache;
            _httpContextAccessor = httpContextAccessor;
            _tenantRepository = tenantRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public string GetSiteRootAddress(string tenancyName = null)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return ReplaceTenancyNameInUrl(WebSiteRootAddressFormat, tenancyName);
            
            var scheme = httpContext.Request.Scheme;
            var hostName = httpContext.Request.Host.Host.RemovePreFix("http://", "https://").RemovePostFix("/");
            var tenantByDomain = _customTenantCache.GetOrNullByDomain(hostName);
            
            return tenantByDomain != null ? $"{scheme}://{hostName}/" : ReplaceTenancyNameInUrl(WebSiteRootAddressFormat, tenancyName);
        }

        public string GetServerRootAddress(string tenancyName = null)
        {
            return ReplaceTenancyNameInUrl(ServerRootAddressFormat, tenancyName);
        }

        public List<string> GetRedirectAllowedExternalWebSites()
        {
            var values = _appConfiguration["App:RedirectAllowedExternalWebSites"];
            var tenantDomains = _tenantRepository.GetAll().Where(o => o.IsActive && !o.IsDeleted && o.Domain != null && o.Domain.Length > 0).Select(o=>o.Domain).Distinct().ToList();
            var res = values?.Split(',').ToList() ?? new List<string>();
            if (tenantDomains.Any())
            {
                res.AddRange(tenantDomains);
            }
            return res;
        }

        private string ReplaceTenancyNameInUrl(string siteRootFormat, string tenancyName)
        {
            if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
            {
                return siteRootFormat;
            }

            if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
            {
                siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);
            }

            if (tenancyName.IsNullOrEmpty())
            {
                return siteRootFormat.Replace(TenancyNamePlaceHolder, "");
            }

            return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + ".");
        }
    }
}