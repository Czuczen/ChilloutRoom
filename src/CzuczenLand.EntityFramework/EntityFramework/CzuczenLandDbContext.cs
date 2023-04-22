using System.Data.Common;
using System.Data.Entity;
using Abp.DynamicEntityProperties;
using Abp.Zero.EntityFramework;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Others;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.MultiTenancy;

namespace CzuczenLand.EntityFramework;

public class CzuczenLandDbContext : AbpZeroDbContext<Tenant, Role, User>
{
    public DbSet<DriedFruit> DriedFruits { get; set; }
        
    public DbSet<Lamp> Lamps { get; set; }
        
    public DbSet<Manure> Manures { get; set; }
        
    public DbSet<Pot> Pots { get; set; }
        
    public DbSet<Seed> Seeds { get; set; }
        
    public DbSet<Soil> Soils { get; set; }
        
    public DbSet<Water> Waters { get; set; }
        
    public DbSet<Drop> Drops { get; set; }
        
    public DbSet<PlantationStorage> PlantationStorages { get; set; }
        
    public DbSet<Plant> Plants { get; set; }
        
    public DbSet<PlayerStorage> PlayerStorages { get; set; }
        
    public DbSet<Quest> Quests { get; set; }
        
    public DbSet<Bonus> Bonus { get; set; }

    public DbSet<Requirement> Requirements { get; set; }
        
    public DbSet<District> Districts { get; set; }

    public DbSet<GeneratedType> GeneratedTypes { get; set; }
        
    public DbSet<BlackMarketTransaction> BlackMarketTransactions { get; set; }
        
    public DbSet<QuestRequirementsProgress> QuestRequirementsProgresses { get; set; }
        
    public DbSet<IgnoreChange> IgnoreChanges { get; set; }
        
    public DbSet<DistrictDon> DistrictDons { get; set; }

    public DbSet<DropQuest> DropQuests { get; set; }
        
    public DbSet<News> News { get; set; }
        
    public DbSet<TicTacToeStorage> TicTacToeStorages { get; set; }
        

    /* NOTE: 
     *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
     *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
     *   pass connection string name to base classes. ABP works either way.
     */
    public CzuczenLandDbContext()
        : base("Default")
    {

    }

    /* NOTE:
     *   This constructor is used by ABP to pass connection string defined in CzuczenLandDataModule.PreInitialize.
     *   Notice that, actually you will not directly create an instance of CzuczenLandDbContext since ABP automatically handles it.
     */
    public CzuczenLandDbContext(string nameOrConnectionString)
        : base(nameOrConnectionString)
    {

    }

    //This constructor is used in tests
    public CzuczenLandDbContext(DbConnection existingConnection)
        : base(existingConnection, false)
    {

    }

    public CzuczenLandDbContext(DbConnection existingConnection, bool contextOwnsConnection)
        : base(existingConnection, contextOwnsConnection)
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DynamicProperty>().Property(p => p.PropertyName).HasMaxLength(250);
        modelBuilder.Entity<DynamicEntityProperty>().Property(p => p.EntityFullName).HasMaxLength(250);
    }
}