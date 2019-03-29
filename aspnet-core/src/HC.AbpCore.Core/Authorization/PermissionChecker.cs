using Abp.Authorization;
using HC.AbpCore.Authorization.Roles;
using HC.AbpCore.Authorization.Users;

namespace HC.AbpCore.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
