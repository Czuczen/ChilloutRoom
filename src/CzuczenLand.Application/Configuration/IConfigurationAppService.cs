using System.Threading.Tasks;
using Abp.Application.Services;
using CzuczenLand.Configuration.Dto;

namespace CzuczenLand.Configuration;

public interface IConfigurationAppService: IApplicationService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}