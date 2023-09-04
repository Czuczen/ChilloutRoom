using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Configuration.Startup;
using Abp.Zero.Configuration;
using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using CzuczenLand.Api;
using Castle.MicroKernel.Registration;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.AppReviver;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Bonuses;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.CustomerZone;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Districts.TimeControl;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Don;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Plant;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Quests.Reset;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Quests.TimeControl;
using Microsoft.Owin.Security;

namespace CzuczenLand.Web;

[DependsOn(
    typeof(CzuczenLandDataModule),
    typeof(CzuczenLandApplicationModule),
    typeof(CzuczenLandWebApiModule),
    typeof(AbpWebSignalRModule),
    //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
    typeof(AbpWebMvcModule))]
public class CzuczenLandWebModule : AbpModule
{
    public override void PreInitialize()
    {
        //Enable database based localization
        Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
        Configuration.EntityHistory.IsEnabledForAnonymousUsers = true; // workery aktualizują np. gold plantacji
        Configuration.Modules.AbpWebCommon().SendAllExceptionsToClients = false;

        //Configure navigation/menu
        Configuration.Navigation.Providers.Add<CzuczenLandNavigationProvider>();
            
        //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        //Configuration.BackgroundJobs.UseHangfire(configuration =>
        //{
        //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
        //});
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        
        IocManager.IocContainer.Register(
            Component
                .For<IAuthenticationManager>()
                .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                .LifestyleTransient()
        );

        AreaRegistration.RegisterAllAreas();
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
        workManager.Add(IocManager.Resolve<AppReviverWorker>());
        workManager.Add(IocManager.Resolve<DonWorker>());
        workManager.Add(IocManager.Resolve<CustomerZoneWorker>());
        workManager.Add(IocManager.Resolve<ResetQuestsWorker>());
        workManager.Add(IocManager.Resolve<TimeLimitedQuestsWorker>());
        workManager.Add(IocManager.Resolve<TimeLimitedDistrictsWorker>());
        workManager.Add(IocManager.Resolve<GrowWorker>());
        workManager.Add(IocManager.Resolve<BlackMarketWorker>());
        workManager.Add(IocManager.Resolve<BonusesWorker>());
    }
}