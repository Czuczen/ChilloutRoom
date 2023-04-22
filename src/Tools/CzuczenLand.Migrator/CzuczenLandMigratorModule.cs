using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using CzuczenLand.EntityFramework;

namespace CzuczenLand.Migrator;

[DependsOn(typeof(CzuczenLandDataModule))]
public class CzuczenLandMigratorModule : AbpModule
{
    public override void PreInitialize()
    {
        Database.SetInitializer<CzuczenLandDbContext>(null);

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
    }
}