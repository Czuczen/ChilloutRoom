using System.Collections.Generic;
using System.Linq;
using CzuczenLand.Roles.Dto;

namespace CzuczenLand.Web.Models.Roles;

public class EditRoleModalViewModel
{
    public RoleDto Role { get; set; }

    public IReadOnlyList<PermissionDto> Permissions { get; set; }

    public bool HasPermission(PermissionDto permission)
    {
        return Permissions != null && Role.GrantedPermissions.Any(p => p == permission.Name);
    }
}