using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;
using CzuczenLand.Editions;
using CzuczenLand.MultiTenancy.Dto;
using Microsoft.AspNet.Identity;

namespace CzuczenLand.MultiTenancy;

/// <summary>
/// Serwis aplikacyjny dla operacji na dzierżawcach.
/// </summary>
[AbpAuthorize(PermissionNames.Pages_Tenants)]
public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
{
    /// <summary>
    /// Manager dzierżawców.
    /// </summary>
    private readonly TenantManager _tenantManager;
    
    /// <summary>
    /// Manager edycji.
    /// </summary>
    private readonly EditionManager _editionManager;
    
    /// <summary>
    /// Manager ról.
    /// </summary>
    private readonly RoleManager _roleManager;
    
    /// <summary>
    /// Manager użytkowników.
    /// </summary>
    private readonly UserManager _userManager;
    
    /// <summary>
    /// Migrator bazy danych ABP Zero.
    /// </summary>
    private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
    

    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium dzierżawców.</param>
    /// <param name="tenantManager">Manager dzierżawców.</param>
    /// <param name="editionManager">Manager edycji.</param>
    /// <param name="userManager">Manager użytkowników.</param>
    /// <param name="roleManager">Manager ról.</param>
    /// <param name="abpZeroDbMigrator">Migrator bazy danych ABP Zero.</param>
    public TenantAppService(
        IRepository<Tenant, int> repository,

        TenantManager tenantManager,
        EditionManager editionManager,
        UserManager userManager,

        RoleManager roleManager,
        IAbpZeroDbMigrator abpZeroDbMigrator
    ) : base(repository)
    {
        _tenantManager = tenantManager;
        _editionManager = editionManager;
        _roleManager = roleManager;
        _abpZeroDbMigrator = abpZeroDbMigrator;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Tworzy nowego dzierżawcę.
    /// </summary>
    /// <param name="input">Dane wejściowe dla nowego dzierżawcy.</param>
    /// <returns>Dto dzierżawcy.</returns>
    public override async Task<TenantDto> CreateAsync(CreateTenantDto input)
    {
        CheckCreatePermission();

        //Create tenant
        var tenant = ObjectMapper.Map<Tenant>(input);
        tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
            ? null
            : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

        var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
        if (defaultEdition != null)
        {
            tenant.EditionId = defaultEdition.Id;
        }

        await _tenantManager.CreateAsync(tenant);
        await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

        //Create tenant database
        _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

        //We are working entities of new tenant, so changing tenant filter
        using (CurrentUnitOfWork.SetTenantId(tenant.Id))
        {
            //Create static roles for new tenant
            CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

            await CurrentUnitOfWork.SaveChangesAsync(); //To get static role ids

            //grant all permissions to admin role
            var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
            await _roleManager.GrantAllPermissionsAsync(adminRole);

            //Create admin user for the tenant
            var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, User.DefaultPassword);
            CheckErrors(await _userManager.CreateAsync(adminUser));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get admin user's id

            //Assign admin user to role!
            CheckErrors(await _userManager.AddToRoleAsync(adminUser.Id, adminRole.Name));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        return MapToEntityDto(tenant);
    }

    /// <summary>
    /// Mapuje dane wejściowe na encję dzierżawcy.
    /// </summary>
    /// <param name="updateInput">Dane wejściowe do aktualizacji dzierżawcy.</param>
    /// <param name="entity">Encja dzierżawcy.</param>
    protected override void MapToEntity(TenantDto updateInput, Tenant entity)
    {
        //Manually mapped since TenantDto contains non-editable properties too.
        entity.Name = updateInput.Name;
        entity.TenancyName = updateInput.TenancyName;
        entity.IsActive = updateInput.IsActive;
    }

    /// <summary>
    /// Usuwa dzierżawcę.
    /// </summary>
    /// <param name="input">Identyfikator dzierżawcy do usunięcia.</param>
    public override async Task DeleteAsync(EntityDto<int> input)
    {
        CheckDeletePermission();

        var tenant = await _tenantManager.GetByIdAsync(input.Id);
        await _tenantManager.DeleteAsync(tenant);
    }

    /// <summary>
    /// Sprawdza błędy w wyniku operacji IdentityResult i obsługuje je.
    /// </summary>
    /// <param name="identityResult">Wynik operacji IdentityResult.</param>
    private void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}