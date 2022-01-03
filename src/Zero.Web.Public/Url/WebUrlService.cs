using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Zero.Configuration;
using Zero.Customize.Cache;
using Zero.MultiTenancy;
using Zero.Url;
using Zero.Web.Url;

namespace Zero.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor, ICustomTenantCache customTenantCache, IHttpContextAccessor httpContextAccessor, IRepository<Tenant> tenantRepository) :
            base(appConfigurationAccessor, customTenantCache, httpContextAccessor, tenantRepository)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}