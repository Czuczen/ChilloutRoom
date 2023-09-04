using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.UI;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;
using CzuczenLand.Roles.Dto;
using Microsoft.AspNet.Identity;

namespace CzuczenLand.Roles;

/// <summary>
/// Serwis aplikacyjny do obsługi ról.
/// </summary>
[AbpAuthorize(PermissionNames.Pages_Roles)]
public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
{
    /// <summary>
    /// Menadżer ról.
    /// </summary>
    private readonly RoleManager _roleManager;
    
    /// <summary>
    /// Menadżer użytkowników.
    /// </summary>
    private readonly UserManager _userManager;
    
    /// <summary>
    /// Repozytorium użytkowników.
    /// </summary>
    private readonly IRepository<User, long> _userRepository;
    
    /// <summary>
    /// Repozytorium przypisania ról do użytkowników.
    /// </summary>
    private readonly IRepository<UserRole, long> _userRoleRepository;
    
    /// <summary>
    /// Repozytorium ról.
    /// </summary>
    private readonly IRepository<Role> _roleRepository;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium ról.</param>
    /// <param name="roleManager">Menadżer ról.</param>
    /// <param name="userManager">Menadżer użytkowników.</param>
    /// <param name="userRepository">Repozytorium użytkowników.</param>
    /// <param name="userRoleRepository">Repozytorium przypisania ról do użytkowników.</param>
    /// <param name="roleRepository">Repozytorium ról.</param>
    public RoleAppService(
        IRepository<Role> repository,
        RoleManager roleManager,
        UserManager userManager,
        IRepository<User, long> userRepository,
        IRepository<UserRole, long> userRoleRepository,
        IRepository<Role> roleRepository)
        : base(repository)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// Tworzy nową rolę.
    /// </summary>
    /// <param name="input">Dane do utworzenia roli.</param>
    /// <returns>Stworzona rola jako DTO.</returns>
    public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
    {
        CheckCreatePermission();

        var role = ObjectMapper.Map<Role>(input);

        CheckErrors(await _roleManager.CreateAsync(role));

        UnitOfWorkManager.Current.SaveChanges();

        var grantedPermissions = PermissionManager
            .GetAllPermissions()
            .Where(p => input.GrantedPermissions.Contains(p.Name))
            .ToList();

        await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

        return MapToEntityDto(role);
    }

    /// <summary>
    /// Aktualizuje istniejącą rolę.
    /// </summary>
    /// <param name="input">Dane do aktualizacji roli.</param>
    /// <returns>Zaktualizowana rola jako DTO.</returns>
    public override async Task<RoleDto> UpdateAsync(RoleDto input)
    {
        CheckUpdatePermission();

        var role = await _roleManager.GetRoleByIdAsync(input.Id);

        ObjectMapper.Map(input, role);

        CheckErrors(await _roleManager.UpdateAsync(role));

        var grantedPermissions = PermissionManager
            .GetAllPermissions()
            .Where(p => input.GrantedPermissions.Contains(p.Name))
            .ToList();

        await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

        return MapToEntityDto(role);
    }

    /// <summary>
    /// Usuwa rolę z systemu.
    /// </summary>
    /// <param name="input">Identyfikator roli do usunięcia.</param>
    public override async Task DeleteAsync(EntityDto<int> input)
    {
        CheckDeletePermission();

        var role = await _roleManager.FindByIdAsync(input.Id);
        if (role.IsStatic)
        {
            throw new UserFriendlyException("Nie można usunąć roli statycznej");
        }

        var users = await GetUsersInRoleAsync(role.Name);

        foreach (var user in users)
        {
            CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.Name));
        }

        CheckErrors(await _roleManager.DeleteAsync(role));
    }

    /// <summary>
    /// Pobiera listę identyfikatorów użytkowników należących do danej roli.
    /// </summary>
    /// <param name="roleName">Nazwa roli.</param>
    /// <returns>Lista identyfikatorów użytkowników.</returns>
    private Task<List<long>> GetUsersInRoleAsync(string roleName)
    {
        var users = (from user in _userRepository.GetAll()
            join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
            join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
            where role.Name == roleName
            select user.Id).Distinct().ToList();

        return Task.FromResult(users);
    }

    /// <summary>
    /// Pobiera wszystkie uprawnienia.
    /// </summary>
    /// <returns>Lista wszystkich uprawnień jako DTO.</returns>
    public Task<ListResultDto<PermissionDto>> GetAllPermissions()
    {
        var permissions = PermissionManager.GetAllPermissions();

        return Task.FromResult(new ListResultDto<PermissionDto>(
            ObjectMapper.Map<List<PermissionDto>>(permissions)
        ));
    }

    /// <summary>
    /// Tworzy zapytanie do bazy danych z uwzględnieniem filtrowania.
    /// </summary>
    /// <param name="input">Parametry zapytania.</param>
    /// <returns>Zapytanie do bazy danych z uwzględnieniem filtrowania.</returns>
    protected override IQueryable<Role> CreateFilteredQuery(PagedResultRequestDto input)
    {
        return Repository.GetAllIncluding(x => x.Permissions);
    }

    /// <summary>
    /// Pobiera rolę z uwzględnieniem jej uprawnień.
    /// </summary>
    /// <param name="id">Identyfikator roli.</param>
    /// <returns>Rola wraz z uprawnieniami.</returns>
    protected override Task<Role> GetEntityByIdAsync(int id)
    {
        var role = Repository.GetAllIncluding(x => x.Permissions).FirstOrDefault(x => x.Id == id);
        return Task.FromResult(role);
    }

    /// <summary>
    /// Sortuje wyniki zapytania.
    /// </summary>
    /// <param name="query">Zapytanie do posortowania.</param>
    /// <param name="input">Parametry zapytania.</param>
    /// <returns>Posortowane zapytanie.</returns>
    protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedResultRequestDto input)
    {
        return query.OrderBy(r => r.DisplayName);
    }

    /// <summary>
    /// Sprawdza błędy wynikające z operacji Identity.
    /// </summary>
    /// <param name="identityResult">Rezultat operacji Identity.</param>
    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}