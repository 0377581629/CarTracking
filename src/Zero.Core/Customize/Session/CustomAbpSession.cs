using System;
using System.Linq;
using System.Security.Claims;
using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Zero.Customize.MultiTenancy;

namespace Zero.Customize.Session
{
    public class CustomAbpSession : ClaimsAbpSession
    {
        public override long? UserId
        {
            get
            {
                if (OverridedValue != null)
                    return OverridedValue.UserId;
                var principal = PrincipalAccessor.Principal;
                var claim = principal?.Claims.FirstOrDefault((Func<Claim, bool>)(c => c.Type == AbpClaimTypes.UserId));
                if (string.IsNullOrEmpty(claim?.Value))
                    return new long?();
                return !long.TryParse(claim.Value, out var result) ? new long?() : result;
            }
        }

        public override int? TenantId
        {
            get
            {
                if (!MultiTenancy.IsEnabled)
                    return 1;
                if (OverridedValue != null)
                    return OverridedValue.TenantId;
                var principal = PrincipalAccessor.Principal;
                var claim = principal?.Claims.FirstOrDefault((Func<Claim, bool>)(c => c.Type == AbpClaimTypes.TenantId));
                if (!string.IsNullOrEmpty(claim?.Value))
                    return Convert.ToInt32(claim.Value);
                return !UserId.HasValue ?  CustomDomainTenantResolver.ResolveTenantId() : new int?();
            }
        }

        private ICustomDomainTenantResolver CustomDomainTenantResolver { get; }

        public CustomAbpSession(
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider, 
            ICustomDomainTenantResolver customDomainTenantResolveContributor)
            : base(principalAccessor, multiTenancy , tenantResolver, sessionOverrideScopeProvider)
        { 
            CustomDomainTenantResolver = customDomainTenantResolveContributor;
        }
    }
}