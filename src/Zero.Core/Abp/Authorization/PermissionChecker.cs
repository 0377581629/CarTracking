using Abp.Authorization;
using Zero.Authorization.Roles;
using Zero.Authorization.Users;

namespace Zero.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
