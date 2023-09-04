using System.Threading.Tasks;
using Abp.Zero.Configuration;
using CzuczenLand.Authorization.Accounts.Dto;
using CzuczenLand.Authorization.Users;
using Abp.Configuration;

namespace CzuczenLand.Authorization.Accounts;

/// <summary>
/// Klasa obsługująca usługi dla konta użytkownika.
/// </summary>
public class AccountAppService : CzuczenLandAppServiceBase, IAccountAppService
{
    /// <summary>
    /// Manager rejestracji użytkownika.
    /// </summary>
    private readonly UserRegistrationManager _userRegistrationManager;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="userRegistrationManager">Manager rejestracji użytkownika.</param>
    public AccountAppService(
        UserRegistrationManager userRegistrationManager)
    {
        _userRegistrationManager = userRegistrationManager;
    }

    /// <summary>
    /// Sprawdza dostępność dzierżawy.
    /// </summary>
    /// <param name="input">Dane wejściowe dotyczące dostępności dzierżawy.</param>
    /// <returns>Wynik dostępności dzierżawy.</returns>
    public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
    {
        var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
        if (tenant == null)
        {
            return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
        }

        if (!tenant.IsActive)
        {
            return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
        }

        return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
    }

    /// <summary>
    /// Rejestruje użytkownika.
    /// </summary>
    /// <param name="input">Dane wejściowe dotyczące rejestracji użytkownika.</param>
    /// <returns>Wynik rejestracji.</returns>
    public async Task<RegisterOutput> Register(RegisterInput input)
    {
        var user = await _userRegistrationManager.RegisterAsync(
            input.Name,
            input.Surname,
            input.EmailAddress,
            input.UserName,
            input.Password,
            false
        );

        var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

        return new RegisterOutput
        {
            CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
        };
    }
}