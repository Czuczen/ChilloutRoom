using System.Threading.Tasks;
using Abp.Application.Services;
using CzuczenLand.Configuration.Dto;

namespace CzuczenLand.Configuration;

/// <summary>
/// Interfejs dla usług konfiguracji.
/// </summary>
public interface IConfigurationAppService: IApplicationService
{
    /// <summary>
    /// Zmienia motyw interfejsu użytkownika.
    /// </summary>
    /// <param name="input">Dane wejściowe dotyczące zmiany motywu.</param>
    Task ChangeUiTheme(ChangeUiThemeInput input);
}