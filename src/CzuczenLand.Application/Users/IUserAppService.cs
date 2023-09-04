using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.Roles.Dto;
using CzuczenLand.Users.Dto;

namespace CzuczenLand.Users;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi użytkowników.
/// </summary>
public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>
{
    /// <summary>
    /// Pobiera listę ról.
    /// </summary>
    /// <returns>Lista ról w formacie RoleDto.</returns>
    Task<ListResultDto<RoleDto>> GetRoles();
}