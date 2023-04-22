using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.Roles.Dto;
using CzuczenLand.Users.Dto;

namespace CzuczenLand.Users;

public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>
{
    Task<ListResultDto<RoleDto>> GetRoles();
}