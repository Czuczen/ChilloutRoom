using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using CzuczenLand.EntityFramework;

namespace CzuczenLand;

[DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(CzuczenLandCoreModule))]
public class CzuczenLandDataModule : AbpModule
{
    public override void PreInitialize()
    {
        Database.SetInitializer(new CreateDatabaseIfNotExists<CzuczenLandDbContext>());

        Configuration.DefaultNameOrConnectionString = "Default";
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
    }
}