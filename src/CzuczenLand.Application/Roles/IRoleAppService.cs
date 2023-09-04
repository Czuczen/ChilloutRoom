using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.Roles.Dto;

namespace CzuczenLand.Roles;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi ról.
/// </summary>
public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedResultRequestDto, CreateRoleDto, RoleDto>
{
    /// <summary>
    /// Pobiera wszystkie uprawnienia.
    /// </summary>
    /// <returns>Lista wszystkich uprawnień jako DTO.</returns>
    Task<ListResultDto<PermissionDto>> GetAllPermissions();
}