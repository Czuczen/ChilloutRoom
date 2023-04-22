using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using CzuczenLand.Configuration.Dto;

namespace CzuczenLand.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : CzuczenLandAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}