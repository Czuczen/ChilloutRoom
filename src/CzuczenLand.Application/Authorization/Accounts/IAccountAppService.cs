using System.Threading.Tasks;
using Abp.Application.Services;
using CzuczenLand.Authorization.Accounts.Dto;

namespace CzuczenLand.Authorization.Accounts;

/// <summary>
/// Interfejs usługi obsługującej konto użytkownika.
/// </summary>
public interface IAccountAppService : IApplicationService
{
    /// <summary>
    /// Sprawdza dostępność dzierżawy.
    /// </summary>
    /// <param name="input">Dane wejściowe dotyczące dostępności dzierżawy.</param>
    /// <returns>Wynik dostępności dzierżawy.</returns>
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    /// <summary>
    /// Rejestruje użytkownika.
    /// </summary>
    /// <param name="input">Dane wejściowe dotyczące rejestracji użytkownika.</param>
    /// <returns>Wynik rejestracji.</returns>
    Task<RegisterOutput> Register(RegisterInput input);
}