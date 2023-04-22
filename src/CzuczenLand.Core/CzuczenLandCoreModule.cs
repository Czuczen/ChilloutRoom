using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;
using CzuczenLand.Configuration;
using CzuczenLand.MultiTenancy;

namespace CzuczenLand;

[DependsOn(typeof(AbpZeroCoreModule))]
public class CzuczenLandCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true; // workery aktualizują np. gold plantacji

        //Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);

        //Remove the following line to disable multi-tenancy.
        // Configuration.MultiTenancy.IsEnabled = CzuczenLandConsts.MultiTenancyEnabled;

        //Add/remove localization sources here
        Configuration.Localization.Sources.Add(
            new DictionaryBasedLocalizationSource(
                CzuczenLandConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    Assembly.GetExecutingAssembly(),
                    "CzuczenLand.Localization.Source"
                )
            )
        );

        AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);
        Configuration.Authorization.Providers.Add<CzuczenLandAuthorizationProvider>();
        Configuration.Settings.Providers.Add<AppSettingProvider>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
    }
}