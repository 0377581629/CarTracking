using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Zero.Authorization.Users;
using Zero.MultiTenancy;

namespace Zero.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}