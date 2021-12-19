using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Zero.Configuration;
using Zero.Customize.Cache;
using Zero.MultiTenancy;
using Zero.Url;

namespace Zero.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor configurationAccessor, ICustomTenantCache customTenantCache, IHttpContextAccessor httpContextAccessor, IRepository<Tenant> tenantRepository) :
            base(configurationAccessor, customTenantCache, httpContextAccessor,tenantRepository)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:WebSiteRootAddress";
    }
}