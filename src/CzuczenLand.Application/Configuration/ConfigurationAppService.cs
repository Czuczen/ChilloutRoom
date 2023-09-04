using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using CzuczenLand.Configuration.Dto;

namespace CzuczenLand.Configuration;

/// <summary>
/// Klasa obsługująca usługi konfiguracji.
/// </summary>
[AbpAuthorize]
public class ConfigurationAppService : CzuczenLandAppServiceBase, IConfigurationAppService
{
    /// <summary>
    /// Zmienia motyw interfejsu użytkownika.
    /// </summary>
    /// <param name="input">Dane wejściowe dotyczące zmiany motywu.</param>
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}