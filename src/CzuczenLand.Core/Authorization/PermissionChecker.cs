using Abp.Authorization;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;

namespace CzuczenLand.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {

    }
}