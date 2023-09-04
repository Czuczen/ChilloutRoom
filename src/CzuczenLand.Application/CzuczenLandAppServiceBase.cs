using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using CzuczenLand.Authorization.Users;
using CzuczenLand.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace CzuczenLand;

/// <summary>
/// Abstrakcyjna klasa bazowa dla serwisów aplikacyjnych.
/// </summary>
public abstract class CzuczenLandAppServiceBase : ApplicationService
{
    /// <summary>
    /// Menadżer dzierżawców.
    /// </summary>
    public TenantManager TenantManager { get; set; }

    /// <summary>
    /// Menadżer użytkowników.
    /// </summary>
    public UserManager UserManager { get; set; }

    
    /// <summary>
    /// Konstruktor klasy CzuczenLandAppServiceBase.
    /// </summary>
    protected CzuczenLandAppServiceBase()
    {
        LocalizationSourceName = CzuczenLandConsts.LocalizationSourceName;
    }
    
    /// <summary>
    /// Pobiera aktualnego użytkownika asynchronicznie.
    /// </summary>
    /// <returns>Obiekt użytkownika.</returns>
    protected virtual async Task<User> GetCurrentUserAsync()
    {
        var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());
        if (user == null)
        {
            throw new ApplicationException("Brak aktualnego użytkownika!");
        }

        return user;
    }

    /// <summary>
    /// Pobiera aktualnego dzierżawce asynchronicznie.
    /// </summary>
    /// <returns>Obiekt najemcy.</returns>
    protected virtual Task<Tenant> GetCurrentTenantAsync()
    {
        return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
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