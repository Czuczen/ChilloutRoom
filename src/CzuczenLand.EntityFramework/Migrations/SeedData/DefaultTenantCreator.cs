using System.Linq;
using Abp.MultiTenancy;
using CzuczenLand.EntityFramework;
using CzuczenLand.MultiTenancy;

namespace CzuczenLand.Migrations.SeedData;

public class DefaultTenantCreator
{
    private readonly CzuczenLandDbContext _context;

    public DefaultTenantCreator(CzuczenLandDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateUserAndRoles();
    }

    private void CreateUserAndRoles()
    {
        //Default tenant

        var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == AbpTenantBase.DefaultTenantName);
        if (defaultTenant == null)
        {
            _context.Tenants.Add(new Tenant {TenancyName = AbpTenantBase.DefaultTenantName, Name = AbpTenantBase.DefaultTenantName});
            _context.SaveChanges();
        }
    }
}