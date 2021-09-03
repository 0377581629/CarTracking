using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Zero.Authorization;
using Zero.Authorization.Roles;

namespace Zero.Customize
{
    public static class StaticRolesHelper
    {
        private static List<string> AdminRequiredPermissions = new()
        {
            AppPermissions.Pages_Administration_Roles,
            AppPermissions.Pages_Administration_Roles_Create,
            AppPermissions.Pages_Administration_Roles_Edit,
            AppPermissions.Pages_Administration_Roles_Delete,

            AppPermissions.Pages_Administration_Users,
            AppPermissions.Pages_Administration_Users_Create,
            AppPermissions.Pages_Administration_Users_Edit,
            AppPermissions.Pages_Administration_Users_Delete,
            AppPermissions.Pages_Administration_Users_ChangePermissions,
            AppPermissions.Pages_Administration_Users_Unlock,
            
            AppPermissions.Pages_Administration_Tenant_SubscriptionManagement,
        };

        public static List<Permission> AddRequiredPermissions(Role role, List<Permission> permissions)
        {
            permissions ??= new List<Permission>();
            if (role.Name != StaticRoleNames.Tenants.Admin) return permissions;
            foreach (var requiredPermissionName in AdminRequiredPermissions.Where(requiredPermissionName => permissions.FirstOrDefault(o => o.Name == requiredPermissionName) == null))
                permissions.Add(new Permission(requiredPermissionName));
            return permissions;
        }
    }
}