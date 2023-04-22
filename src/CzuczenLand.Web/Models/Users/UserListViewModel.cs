using System.Collections.Generic;
using CzuczenLand.Roles.Dto;
using CzuczenLand.Users.Dto;

namespace CzuczenLand.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<UserDto> Users { get; set; }

    public IReadOnlyList<RoleDto> Roles { get; set; }
}