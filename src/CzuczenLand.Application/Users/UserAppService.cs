using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;
using CzuczenLand.Roles.Dto;
using CzuczenLand.Users.Dto;
using Microsoft.AspNet.Identity;

namespace CzuczenLand.Users;

/// <summary>
/// Serwis aplikacyjny do zarządzania użytkownikami.
/// </summary>
[AbpAuthorize(PermissionNames.Pages_Users)]
public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>, IUserAppService
{
    /// <summary>
    /// Menadżer użytkowników.
    /// </summary>
    private readonly UserManager _userManager;
    
    /// <summary>
    /// Menadżer ról.
    /// </summary>
    private readonly RoleManager _roleManager;
    
    /// <summary>
    /// Repozytorium ról.
    /// </summary>
    private readonly IRepository<Role> _roleRepository;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium użytkowników.</param>
    /// <param name="userManager">Menadżer użytkowników.</param>
    /// <param name="roleRepository">Repozytorium ról.</param>
    /// <param name="roleManager">Menadżer ról.</param>
    public UserAppService(
        IRepository<User, long> repository,
        UserManager userManager,
        IRepository<Role> roleRepository,
        RoleManager roleManager)
        : base(repository)
    {
        _userManager = userManager;
        _roleRepository = roleRepository;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Pobiera użytkownika wraz z rolami.
    /// </summary>
    /// <param name="input">Identyfikator użytkownika.</param>
    /// <returns>Obiekt UserDto.</returns>
    public override async Task<UserDto> GetAsync(EntityDto<long> input)
    {
        var user = await base.GetAsync(input);
        var userRoles = await _userManager.GetRolesAsync(user.Id);
        user.Roles = userRoles.Select(ur => ur).ToArray();
        return user;
    }
        
    /// <summary>
    /// Tworzy nowego użytkownika.
    /// </summary>
    /// <param name="input">Dane do utworzenia użytkownika.</param>
    /// <returns>Obiekt UserDto utworzonego użytkownika.</returns>
    public override async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        CheckCreatePermission();

        var user = ObjectMapper.Map<User>(input);

        user.TenantId = AbpSession.TenantId;
        user.Password = new PasswordHasher().HashPassword(input.Password);
        user.IsEmailConfirmed = true;

        //Assign roles
        user.Roles = new Collection<UserRole>();
        foreach (var roleName in input.RoleNames)
        {
            var role = await _roleManager.GetRoleByNameAsync(roleName);
            user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
        }

        CheckErrors(await _userManager.CreateAsync(user));

        await CurrentUnitOfWork.SaveChangesAsync();

        return MapToEntityDto(user);
    }

    /// <summary>
    /// Aktualizuje dane użytkownika.
    /// </summary>
    /// <param name="input">Dane do aktualizacji użytkownika.</param>
    /// <returns>Obiekt UserDto zaktualizowanego użytkownika.</returns>
    public override async Task<UserDto> UpdateAsync(UpdateUserDto input)
    {
        CheckUpdatePermission();

        var user = await _userManager.GetUserByIdAsync(input.Id);

        MapToEntity(input, user);

        CheckErrors(await _userManager.UpdateAsync(user));

        if (input.RoleNames != null)
        {
            CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
        }

        return await GetAsync(input);
    }
    
    /// <summary>
    /// Usuwa użytkownika.
    /// </summary>
    /// <param name="input">Identyfikator użytkownika do usunięcia.</param>
    public override async Task DeleteAsync(EntityDto<long> input)
    {
        var user = await _userManager.GetUserByIdAsync(input.Id);
        await _userManager.DeleteAsync(user);
    }
    
    /// <summary>
    /// Pobiera listę ról.
    /// </summary>
    /// <returns>Lista ról w formacie RoleDto.</returns>
    public async Task<ListResultDto<RoleDto>> GetRoles()
    {
        var roles = await _roleRepository.GetAllListAsync();
        return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
    }

    /// <summary>
    /// Mapuje dane z obiektu CreateUserDto na obiekt User.
    /// </summary>
    /// <param name="createInput">Dane do utworzenia użytkownika.</param>
    /// <returns>Obiekt użytkownika.</returns>
    protected override User MapToEntity(CreateUserDto createInput)
    {
        var user = ObjectMapper.Map<User>(createInput);
        return user;
    }

    /// <summary>
    /// Mapuje dane z obiektu UpdateUserDto na obiekt User.
    /// </summary>
    /// <param name="input">Dane do aktualizacji użytkownika.</param>
    /// <param name="user">Obiekt użytkownika, do którego będą mapowane dane.</param>
    protected override void MapToEntity(UpdateUserDto input, User user)
    {
        ObjectMapper.Map(input, user);
    }

    /// <summary>
    /// Tworzy zapytanie filtrowane dla użytkowników z uwzględnieniem ról.
    /// </summary>
    /// <param name="input">Parametry paginacji i filtrowania.</param>
    /// <returns>Query do bazy danych.</returns>
    protected override IQueryable<User> CreateFilteredQuery(PagedResultRequestDto input)
    {
        return Repository.GetAllIncluding(x => x.Roles);
    }

    /// <summary>
    /// Pobiera użytkownika po identyfikatorze asynchronicznie z uwzględnieniem ról.
    /// </summary>
    /// <param name="id">Identyfikator użytkownika.</param>
    /// <returns>Obiekt użytkownika.</returns>
    protected override async Task<User> GetEntityByIdAsync(long id)
    {
        var user = Repository.GetAllIncluding(x => x.Roles).FirstOrDefault(x => x.Id == id);
        return await Task.FromResult(user);
    }

    /// <summary>
    /// Sortuje użytkowników według nazwy użytkownika.
    /// </summary>
    /// <param name="query">Query zawierające użytkowników.</param>
    /// <param name="input">Parametry paginacji i filtrowania.</param>
    /// <returns>Posortowane query.</returns>
    protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedResultRequestDto input)
    {
        return query.OrderBy(r => r.UserName);
    }

    /// <summary>
    /// Sprawdza błędy IdentityResult i obsługuje lokalizację błędów.
    /// </summary>
    /// <param name="identityResult">Wynik operacji Identity.</param>
    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}