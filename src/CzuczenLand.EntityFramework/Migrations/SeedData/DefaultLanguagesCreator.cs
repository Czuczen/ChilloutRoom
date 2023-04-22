using System.Collections.Generic;
using System.Linq;
using Abp.Localization;
using CzuczenLand.EntityFramework;

namespace CzuczenLand.Migrations.SeedData;

public class DefaultLanguagesCreator
{
    public static List<ApplicationLanguage> InitialLanguages { get; private set; }

    private readonly CzuczenLandDbContext _context;

    static DefaultLanguagesCreator()
    {
        InitialLanguages = new List<ApplicationLanguage>
        {
            new ApplicationLanguage(null, "pl", "Polish", "famfamfam-flags pl"),
        };
    }

    public DefaultLanguagesCreator(CzuczenLandDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateLanguages();
    }

    private void CreateLanguages()
    {
        foreach (var language in InitialLanguages)
        {
            AddLanguageIfNotExists(language);
        }
    }

    private void AddLanguageIfNotExists(ApplicationLanguage language)
    {
        if (_context.Languages.Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
        {
            return;
        }

        _context.Languages.Add(language);

        _context.SaveChanges();
    }
}