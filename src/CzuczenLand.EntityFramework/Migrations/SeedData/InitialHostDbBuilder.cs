using CzuczenLand.EntityFramework;
using EntityFramework.DynamicFilters;

namespace CzuczenLand.Migrations.SeedData;

public class InitialHostDbBuilder
{
    private readonly CzuczenLandDbContext _context;

    public InitialHostDbBuilder(CzuczenLandDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        _context.DisableAllFilters();

        new DefaultEditionsCreator(_context).Create();
        new DefaultLanguagesCreator(_context).Create();
        new HostRoleAndUserCreator(_context).Create();
        new DefaultSettingsCreator(_context).Create();
    }
}