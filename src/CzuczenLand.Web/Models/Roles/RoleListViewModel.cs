using System.Collections.Generic;
using CzuczenLand.Roles.Dto;

namespace CzuczenLand.Web.Models.Roles;

public class RoleListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }

    public IReadOnlyList<PermissionDto> Permissions { get; set; }
}