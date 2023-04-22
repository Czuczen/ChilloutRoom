using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.TestBase;
using CzuczenLand.Authorization.Users;
using CzuczenLand.EntityFramework;
using CzuczenLand.Migrations.SeedData;
using CzuczenLand.MultiTenancy;
using Castle.MicroKernel.Registration;
using Effort;
using EntityFramework.DynamicFilters;

namespace CzuczenLand.Tests;

public abstract class CzuczenLandTestBase : AbpIntegratedTestBase<CzuczenLandTestModule>
{
    private DbConnection _hostDb;
    private Dictionary<int, DbConnection> _tenantDbs; //only used for db per tenant architecture

    protected CzuczenLandTestBase()
    {
        //Seed initial data for host
        AbpSession.TenantId = null;
        UsingDbContext(context =>
        {
            new InitialHostDbBuilder(context).Create();
            new DefaultTenantCreator(context).Create();
        });

        //Seed initial data for default tenant
        AbpSession.TenantId = 1;
        UsingDbContext(context =>
        {
            new TenantRoleAndUserBuilder(context, 1).Create();
        });

        LoginAsDefaultTenantAdmin();

        // Ten kod ustawia kulturę dla bieżącego wątku na polską
        Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
    }

    protected override void PreInitialize()
    {
        base.PreInitialize();

        /* You can switch database architecture here: */
        UseSingleDatabase();
        //UseDatabasePerTenant();
    }

    /* Uses single database for host and all tenants.
     */
    private void UseSingleDatabase()
    {
        _hostDb = DbConnectionFactory.CreateTransient();

        LocalIocManager.IocContainer.Register(
            Component.For<DbConnection>()
                .UsingFactoryMethod(() => _hostDb)
                .LifestyleSingleton()
        );
    }

    /* Uses single database for host and Default tenant,
     * but dedicated databases for all other tenants.
     */
    private void UseDatabasePerTenant()
    {
        _hostDb = DbConnectionFactory.CreateTransient();
        _tenantDbs = new Dictionary<int, DbConnection>();

        LocalIocManager.IocContainer.Register(
            Component.For<DbConnection>()
                .UsingFactoryMethod((kernel) =>
                {
                    lock (_tenantDbs)
                    {
                        var currentUow = kernel.Resolve<ICurrentUnitOfWorkProvider>().Current;
                        var abpSession = kernel.Resolve<IAbpSession>();

                        var tenantId = currentUow != null ? currentUow.GetTenantId() : abpSession.TenantId;

                        if (tenantId == null || tenantId == 1) //host and default tenant are stored in host db
                        {
                            return _hostDb;
                        }

                        if (!_tenantDbs.ContainsKey(tenantId.Value))
                        {
                            _tenantDbs[tenantId.Value] = DbConnectionFactory.CreateTransient();
                        }

                        return _tenantDbs[tenantId.Value];
                    }
                }, true)
                .LifestyleTransient()
        );
    }

    #region UsingDbContext

    protected IDisposable UsingTenantId(int? tenantId)
    {
        var previousTenantId = AbpSession.TenantId;
        AbpSession.TenantId = tenantId;
        return new DisposeAction(() => AbpSession.TenantId = previousTenantId);
    }

    protected void UsingDbContext(Action<CzuczenLandDbContext> action)
    {
        UsingDbContext(AbpSession.TenantId, action);
    }

    protected Task UsingDbContextAsync(Func<CzuczenLandDbContext, Task> action)
    {
        return UsingDbContextAsync(AbpSession.TenantId, action);
    }

    protected T UsingDbContext<T>(Func<CzuczenLandDbContext, T> func)
    {
        return UsingDbContext(AbpSession.TenantId, func);
    }

    protected Task<T> UsingDbContextAsync<T>(Func<CzuczenLandDbContext, Task<T>> func)
    {
        return UsingDbContextAsync(AbpSession.TenantId, func);
    }

    protected void UsingDbContext(int? tenantId, Action<CzuczenLandDbContext> action)
    {
        using (UsingTenantId(tenantId))
        {
            using (var context = LocalIocManager.Resolve<CzuczenLandDbContext>())
            {
                context.DisableAllFilters();
                action(context);
                context.SaveChanges();
            }
        }
    }

    protected async Task UsingDbContextAsync(int? tenantId, Func<CzuczenLandDbContext, Task> action)
    {
        using (UsingTenantId(tenantId))
        {
            using (var context = LocalIocManager.Resolve<CzuczenLandDbContext>())
            {
                context.DisableAllFilters();
                await action(context);
                await context.SaveChangesAsync();
            }
        }
    }

    protected T UsingDbContext<T>(int? tenantId, Func<CzuczenLandDbContext, T> func)
    {
        T result;

        using (UsingTenantId(tenantId))
        {
            using (var context = LocalIocManager.Resolve<CzuczenLandDbContext>())
            {
                context.DisableAllFilters();
                result = func(context);
                context.SaveChanges();
            }
        }

        return result;
    }

    protected async Task<T> UsingDbContextAsync<T>(int? tenantId, Func<CzuczenLandDbContext, Task<T>> func)
    {
        T result;

        using (UsingTenantId(tenantId))
        {
            using (var context = LocalIocManager.Resolve<CzuczenLandDbContext>())
            {
                context.DisableAllFilters();
                result = await func(context);
                await context.SaveChangesAsync();
            }
        }

        return result;
    }

    #endregion

    #region Login

    protected void LoginAsHostAdmin()
    {
        LoginAsHost(AbpUserBase.AdminUserName);
    }

    protected void LoginAsDefaultTenantAdmin()
    {
        LoginAsTenant(AbpTenantBase.DefaultTenantName, AbpUserBase.AdminUserName);
    }

    protected void LogoutAsDefaultTenant()
    {
        LogoutAsTenant(AbpTenantBase.DefaultTenantName);
    }

    protected void LoginAsHost(string userName)
    {
        AbpSession.TenantId = null;

        var user =
            UsingDbContext(
                context =>
                    context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
        if (user == null)
        {
            throw new Exception("There is no user: " + userName + " for host.");
        }

        AbpSession.UserId = user.Id;
    }

    protected void LogoutAsHost()
    {
        Resolve<IMultiTenancyConfig>().IsEnabled = true;

        AbpSession.TenantId = null;
        AbpSession.UserId = null;
    }

    protected void LoginAsTenant(string tenancyName, string userName)
    {
        var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
        if (tenant == null)
        {
            throw new Exception("There is no tenant: " + tenancyName);
        }

        AbpSession.TenantId = tenant.Id;

        var user =
            UsingDbContext(
                context =>
                    context.Users.FirstOrDefault(u => u.TenantId == AbpSession.TenantId && u.UserName == userName));
        if (user == null)
        {
            throw new Exception("There is no user: " + userName + " for tenant: " + tenancyName);
        }

        AbpSession.UserId = user.Id;
    }

    protected void LogoutAsTenant(string tenancyName)
    {
        var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
        if (tenant == null)
        {
            throw new Exception("There is no tenant: " + tenancyName);
        }

        AbpSession.TenantId = tenant.Id;
        AbpSession.UserId = null;
    }

    #endregion

    /// <summary>
    /// Pobiera bieżącego użytkownika, jeśli właściwość <see cref="IAbpSession.UserId"/> nie jest pusta.
    /// Wyrzuca wyjątek, jeśli jest pusta.
    /// </summary>
    protected async Task<User> GetCurrentUserAsync()
    {
        var userId = AbpSession.GetUserId();
        return await UsingDbContext(context => context.Users.SingleAsync(u => u.Id == userId));
    }

    /// <summary>
    /// Pobiera bieżącego najemcę, jeśli właściwość <see cref="IAbpSession.TenantId"/> nie jest pusta.
    /// Wyrzuca wyjątek, jeśli nie ma bieżącego najemcy.
    /// </summary>
    protected async Task<Tenant> GetCurrentTenantAsync()
    {
        var tenantId = AbpSession.GetTenantId();
        return await UsingDbContext(context => context.Tenants.SingleAsync(t => t.Id == tenantId));
    }
}