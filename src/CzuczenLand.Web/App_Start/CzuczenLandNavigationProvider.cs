using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using CzuczenLand.Authorization;

namespace CzuczenLand.Web;

/// <summary>
/// Ta klasa definiuje menu dla aplikacji.
/// Wykorzystuje system menu ABP.
/// Gdy dodasz tutaj elementy menu, automatycznie pojawią się one w aplikacji Angular.
/// Aby dowiedzieć się, jak renderować menu, zobacz plik Views/Layout/_TopMenu.cshtml.
/// </summary>
public class CzuczenLandNavigationProvider : NavigationProvider
{
    public override void SetNavigation(INavigationProviderContext context)
    {
        context.Manager.MainMenu
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Home,
                    L("HomePage"),
                    url: "",
                    icon: "home",
                    requiresAuthentication: true
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.Plantation,
                    L("Plantation"),
                    url: "Plantation",
                    icon: "dashboard",
                    requiresAuthentication: true
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.TicTacToe,
                    L("TicTacToe"),
                    url: "TicTacToe",
                    icon: "cancel",
                    requiresAuthentication: true
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.ConfigurationPanel,
                    L("ConfigurationPanel"),
                    url: "ConfigurationPanel",
                    icon: "build",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_ConfigurationPanel)
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.Tenants,
                    L("Tenants"),
                    url: "Tenants",
                    icon: "business",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.Users,
                    L("Users"),
                    url: "Users",
                    icon: "people",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.Roles,
                    L("Roles"),
                    url: "Roles",
                    icon: "local_offer",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                )
            );
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, CzuczenLandConsts.LocalizationSourceName);
    }
}