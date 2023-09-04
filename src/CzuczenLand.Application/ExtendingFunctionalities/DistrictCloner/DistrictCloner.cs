using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.EntityFramework;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;
using CzuczenLand.ExtendingFunctionalities.ModelsFactory;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.Roles;
using CzuczenLand.Roles.Dto;
using CzuczenLand.Users;
using CzuczenLand.Users.Dto;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner;

/// <summary>
/// Klasa umożliwiająca klonowanie dzielnic w różnych kontekstach.
/// </summary>
public class DistrictCloner : IDistrictCloner
{
    /// <summary>
    /// Dostawca kontekstu bazy danych AbpDbContext.
    /// </summary>
    private readonly IDbContextProvider<AbpDbContext> _dbContextProvider;
    
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium typów generowanych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium suszu.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium lamp.
    /// </summary>
    private readonly IRepository<Lamp> _lampRepository;
    
    /// <summary>
    /// Repozytorium nawozów.
    /// </summary>
    private readonly IRepository<Manure> _manureRepository;
    
    /// <summary>
    /// Repozytorium doniczek.
    /// </summary>
    private readonly IRepository<Pot> _potRepository;
    
    /// <summary>
    /// Repozytorium nasion.
    /// </summary>
    private readonly IRepository<Seed> _seedRepository;
    
    /// <summary>
    /// Repozytorium gleb.
    /// </summary>
    private readonly IRepository<Soil> _soilRepository;
    
    /// <summary>
    /// Repozytorium wód.
    /// </summary>
    private readonly IRepository<Water> _waterRepository;
    
    /// <summary>
    /// Repozytorium nagród.
    /// </summary>
    private readonly IRepository<Drop> _dropRepository;
    
    /// <summary>
    /// Repozytorium zadań.
    /// </summary>
    private readonly IRepository<Quest> _questRepository;
    
    /// <summary>
    /// Repozytorium bonusów.
    /// </summary>
    private readonly IRepository<Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium wymagań.
    /// </summary>
    private readonly IRepository<Requirement> _requirementRepository;
    
    /// <summary>
    /// Repozytorium postępu wymagań zadań.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Repozytorium tabeli łączącej nagrody z zadaniami.
    /// </summary>
    private readonly IRepository<DropQuest> _dropQuestRepository;
    
    /// <summary>
    /// Repozytorium ról użytkowników.
    /// </summary>
    private readonly IRepository<Role> _roleRepository;
    
    /// <summary>
    /// Usługa aplikacyjna użytkowników.
    /// </summary>
    private readonly IUserAppService _userAppService;
    
    /// <summary>
    /// Usługa aplikacyjna ról użytkowników.
    /// </summary>
    private readonly IRoleAppService _roleAppService;


    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="dbContextProvider">Dostawca kontekstu bazy danych.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="driedFruitRepository">Repozytorium suszu.</param>
    /// <param name="lampRepository">Repozytorium lamp.</param>
    /// <param name="manureRepository">Repozytorium nawozów.</param>
    /// <param name="potRepository">Repozytorium doniczek.</param>
    /// <param name="seedRepository">Repozytorium nasion.</param>
    /// <param name="soilRepository">Repozytorium gleb.</param>
    /// <param name="waterRepository">Repozytorium wód.</param>
    /// <param name="dropRepository">Repozytorium dropów.</param>
    /// <param name="questRepository">Repozytorium zadań.</param>
    /// <param name="bonusRepository">Repozytorium bonusów.</param>
    /// <param name="requirementRepository">Repozytorium wymagań.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium postępu wymagań zadań.</param>
    /// <param name="dropQuestRepository">Repozytorium tabeli łączącej nagrody z zadaniami.</param>
    /// <param name="roleRepository">Repozytorium ról użytkowników.</param>
    /// <param name="userAppService">Usługa aplikacyjna użytkowników.</param>
    /// <param name="roleAppService">Usługa aplikacyjna ról użytkowników.</param>
    public DistrictCloner(
        IDbContextProvider<AbpDbContext> dbContextProvider,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<Lamp> lampRepository,
        IRepository<Manure> manureRepository,
        IRepository<Pot> potRepository,
        IRepository<Seed> seedRepository,
        IRepository<Soil> soilRepository,
        IRepository<Water> waterRepository,
        IRepository<Drop> dropRepository,
        IRepository<Quest> questRepository,
        IRepository<Bonus> bonusRepository,
        IRepository<Requirement> requirementRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        IRepository<DropQuest> dropQuestRepository,
        IRepository<Role> roleRepository,
        IUserAppService userAppService,
        IRoleAppService roleAppService
    )
    {
        _dbContextProvider = dbContextProvider;
        _districtRepository = districtRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _driedFruitRepository = driedFruitRepository;
        _lampRepository = lampRepository;
        _manureRepository = manureRepository;
        _potRepository = potRepository;
        _seedRepository = seedRepository;
        _soilRepository = soilRepository;
        _waterRepository = waterRepository;
        _dropRepository = dropRepository;
        _questRepository = questRepository;
        _bonusRepository = bonusRepository;
        _requirementRepository = requirementRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _dropQuestRepository = dropQuestRepository;
        _roleRepository = roleRepository;
        _userAppService = userAppService;
        _roleAppService = roleAppService;
    }

    /// <summary>
    /// Wykonuje klonowanie z plików zamieszczonych w projekcie do bazy danych.
    /// </summary>
    /// <returns>Kontekst dzielnicy po klonowaniu.</returns>
    public async Task<DistrictContext> Clone()
    {
        var ctx = await _dbContextProvider.GetDbContextAsync();
            
        var localRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
        var contentPath = Path.Combine(localRoot, "Content");
        var definitionsPath = Path.Combine(contentPath, "Definitions");
        var importPath = Path.Combine(definitionsPath, "Import");
        if (!Directory.Exists(importPath))
            Directory.CreateDirectory(importPath);
            
        var entityNameToObjects = DistrictClonerHelper.GetXlsDocumentsAsEntityDictListObjects(importPath);
        var arrangedInOrder = DistrictClonerHelper.ArrangeInOrder(entityNameToObjects);
        var createdObjects = await ExecuteCloning(ctx, arrangedInOrder);

        return DistrictClonerHelper.GetAsDistrictContext(createdObjects);
    }

    /// <summary>
    /// Wykonuje klonowanie istniejącej dzielnicy do plików projektu w formatach .csv/.xls.
    /// https://cloudconvert.com/xls-to-xlsx
    /// xls dlatego, że konwerter z csv do xls dodawał do pierwszej kolumny znaki UTF-8 BOM
    /// z xls który tak naprawdę jest sformatowany jak csv konwertujemy na xlsx i wszystko jest ok
    /// </summary>
    /// <param name="districtId">Identyfikator dzielnicy do sklonowania.</param>
    /// <returns>Kontekst dzielnicy po klonowaniu.</returns>
    public async Task<DistrictContext> Clone(int districtId)
    {
        var ret = await GetDistrictContext(districtId);
            
        var localRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
        var contentPath = Path.Combine(localRoot, "Content");
        var definitionsPath = Path.Combine(contentPath, "Definitions");
        var exportPath = Path.Combine(definitionsPath, "Export");
        if (!Directory.Exists(exportPath))
            Directory.CreateDirectory(exportPath);

        var objects = new Dictionary<string, List<object>>
        {
            [EntitiesDbNames.District] = new() {ret.District},
            [EntitiesDbNames.GeneratedType] = ret.GeneratedTypes.Cast<object>().ToList(),
            [EntitiesDbNames.DriedFruit] = ret.DriedFruits.Cast<object>().ToList(),
            [EntitiesDbNames.Lamp] = ret.Lamps.Cast<object>().ToList(),
            [EntitiesDbNames.Manure] = ret.Manures.Cast<object>().ToList(),
            [EntitiesDbNames.Pot] = ret.Pots.Cast<object>().ToList(),
            [EntitiesDbNames.Seed] = ret.Seeds.Cast<object>().ToList(),
            [EntitiesDbNames.Soil] = ret.Soils.Cast<object>().ToList(),
            [EntitiesDbNames.Water] = ret.Waters.Cast<object>().ToList(),
            [EntitiesDbNames.Drop] = ret.Drops.Cast<object>().ToList(),
            [EntitiesDbNames.Quest] = ret.Quests.Cast<object>().ToList(),
            [EntitiesDbNames.Bonus] = ret.Bonuses.Cast<object>().ToList(),
            [EntitiesDbNames.Requirement] = ret.Requirements.Cast<object>().ToList(),
            [EntitiesDbNames.QuestRequirementsProgress] = ret.QuestsRequirementsProgress.Cast<object>().ToList(),
            [EntitiesDbNames.DropQuest] = ret.DropsQuests.Cast<object>().ToList()
        };
            
        foreach (var pair in objects)
        {
            if (pair.Value == null || !pair.Value.Any()) continue;
            var lines = new List<string>();

            var properties = pair.Value.First().GetType().GetProperties();
            var columnNames = properties.Select(item => item.Name);

            var header = string.Join(",", columnNames.Select(name => $"\"{name}\""));
            var valueLines = pair.Value
                .Select(obj => string.Join(",",
                    properties.Select(prop =>
                    {
                        var val = pair.Key == EntitiesDbNames.QuestRequirementsProgress // W QuestRequirementsProgresses jedno pole zawiera json  
                            ? prop.GetValue(obj)?.ToString().Replace("\"", "'")
                            : prop.GetValue(obj);
                        return $"\"{val}\"";
                    }))).ToList();

            lines.Add(header);
            lines.AddRange(valueLines);

            var fullPath = Path.Combine(exportPath, pair.Key);
            File.WriteAllLines(fullPath + ".xls", lines, Encoding.UTF8);
        }

        return ret;
    }
    
    /// <summary>
    /// Wykonuje klonowanie z plików (xls) na dysku Google do bazy danych.
    /// </summary>
    /// <param name="filesIds">Identyfikatory plików na dysku Google do sklonowania.</param>
    /// <returns>Kontekst dzielnicy po klonowaniu.</returns>
    public async Task<DistrictContext> Clone(List<string> filesIds)
    {
        var ctx = await _dbContextProvider.GetDbContextAsync();

        var localRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
        var appDataPath = Path.Combine(localRoot, "App_Data");
        var definitionsPath = Path.Combine(appDataPath, "Definitions");
        if (!Directory.Exists(definitionsPath))
            Directory.CreateDirectory(definitionsPath);
            
        var filesIdsAndNames = DistrictClonerHelper.DownloadFilesAndGetIdsAndNames(definitionsPath, filesIds);
        var entityNameToObjects = DistrictClonerHelper.GetXlsDocumentsAsEntityDictListObjects(definitionsPath, filesIdsAndNames);
        var arrangedInOrder = DistrictClonerHelper.ArrangeInOrder(entityNameToObjects);
        var createdObjects = await ExecuteCloning(ctx, arrangedInOrder);
            
        return DistrictClonerHelper.GetAsDistrictContext(createdObjects);
    }

    /// <summary>
    /// Wykonuje klonowanie istniejącej dzielnicy do bazy danych w określonej liczbie kopii.
    /// </summary>
    /// <param name="districtId">Identyfikator dzielnicy do sklonowania.</param>
    /// <param name="howMany">Liczba kopii do utworzenia.</param>
    /// <returns>Lista kontekstów dzielnic po klonowaniu.</returns>
    public async Task<List<DistrictContext>> Clone(int districtId, int howMany)
    {
        var ret = new List<DistrictContext>();
            
        var ctx = await _dbContextProvider.GetDbContextAsync();
        var districtContext = await GetDistrictContext(districtId);

        for (var i = 0; i < howMany; i++)
        {
            // =================================== nazwa encji, stare id, nowe id
            var entityNameOldIdNewIdList = new List<Tuple<string, int, int>>();
            var entitiesMaxIdNumbers = await GetEntitiesMaxIdNumber();
                
            var wardenPassword = StringUtils.RandomString(16);
            var user = await CreateWardenForDistrict(districtContext.District, wardenPassword);
            var entitiesIds = await DistrictClonerHelper.GenerateNewObjectsAndClone(districtContext, ctx,
                entityNameOldIdNewIdList, entitiesMaxIdNumbers, user.Id);
                
            var createdObjects = new DistrictContext
            {
                WardenName = user.Name,
                WardenPassword = wardenPassword,
                    
                District = (await GetCreatedObjects(EntitiesDbNames.District, entitiesIds.DistrictIds)).Cast<District>().Single(),
                GeneratedTypes = (await GetCreatedObjects(EntitiesDbNames.GeneratedType, entitiesIds.GeneratedTypeIds)).Cast<GeneratedType>().ToList(),
                Requirements = (await GetCreatedObjects(EntitiesDbNames.Requirement, entitiesIds.RequirementIds)).Cast<Requirement>().ToList(),
                Drops = (await GetCreatedObjects(EntitiesDbNames.Drop, entitiesIds.DropIds)).Cast<Drop>().ToList(),
                DriedFruits = (await GetCreatedObjects(EntitiesDbNames.DriedFruit, entitiesIds.DriedFruitIds)).Cast<DriedFruit>().ToList(),
                Lamps = (await GetCreatedObjects(EntitiesDbNames.Lamp, entitiesIds.LampIds)).Cast<Lamp>().ToList(),
                Manures = (await GetCreatedObjects(EntitiesDbNames.Manure, entitiesIds.ManureIds)).Cast<Manure>().ToList(),
                Pots = (await GetCreatedObjects(EntitiesDbNames.Pot, entitiesIds.PotIds)).Cast<Pot>().ToList(),
                Seeds = (await GetCreatedObjects(EntitiesDbNames.Seed, entitiesIds.SeedIds)).Cast<Seed>().ToList(),
                Soils = (await GetCreatedObjects(EntitiesDbNames.Soil, entitiesIds.SoilIds)).Cast<Soil>().ToList(),
                Waters = (await GetCreatedObjects(EntitiesDbNames.Water, entitiesIds.WaterIds)).Cast<Water>().ToList(),
                Bonuses = (await GetCreatedObjects(EntitiesDbNames.Bonus, entitiesIds.BonusIds)).Cast<Bonus>().ToList(),
                Quests = (await GetCreatedObjects(EntitiesDbNames.Quest, entitiesIds.QuestIds)).Cast<Quest>().ToList(),
                QuestsRequirementsProgress = (await GetCreatedObjects(EntitiesDbNames.QuestRequirementsProgress, entitiesIds.QuestRequirementsProgressIds)).Cast<QuestRequirementsProgress>().ToList(),
                DropsQuests = (await GetCreatedObjects(EntitiesDbNames.DropQuest, entitiesIds.DropQuestIds)).Cast<DropQuest>().ToList(),
            };

            ret.Add(createdObjects);
        }

        return ret;
    }

    /// <summary>
    /// Wykonuje proces klonowania.
    /// </summary>
    /// <param name="ctx">Kontekst bazy danych.</param>
    /// <param name="arrangedInOrder">Uporządkowane dane do utworzenia obiektów.</param>
    /// <returns>Słownik zawierający utworzone obiekty po procesie klonowania.</returns>
    private async Task<Dictionary<string, List<object>>> ExecuteCloning(AbpDbContext ctx, 
        Dictionary<string, List<Dictionary<string, object>>> arrangedInOrder)
    {
        var ret = new Dictionary<string, List<object>>();
            
        var dbParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Db);
        var entitiesMaxIdNumbers = await GetEntitiesMaxIdNumber();
            
        // ================================== nazwa encji, stare id, nowe id
        var entityNameOldIdNewIdList = new List<Tuple<string, int, int>>();
        foreach (var pair in arrangedInOrder)
        {
            var entityProperties = DbModelFactory.GetDbEntityTypeByEntityDbName(pair.Key).GetProperties().ToList();
            var parsedObjects = pair.Value?.Select(obj => dbParser.Parse(obj, entityProperties)).ToList();
            var objects = DistrictClonerHelper.GetObjects(pair.Key, JsonConvert.SerializeObject(parsedObjects));
            if (pair.Key == EntitiesDbNames.District)
            {
                var district = (District) objects.Single();
                var wardenPassword = StringUtils.RandomString(16);
                var user = await CreateWardenForDistrict(district, wardenPassword);
                district.UserId = user.Id;
                district.IsDefined = false;
                ret[DistrictClonerHelper.WardenNameField] = new List<object> {user.Name};
                ret[DistrictClonerHelper.WardenPasswordField] = new List<object> {wardenPassword};
            }
                
            var ids = await DistrictClonerHelper.CloneObjects(ctx, objects, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
            ret[pair.Key] = await GetCreatedObjects(pair.Key, ids);
        }

        return ret;
    } 
        
    /// <summary>
    /// Pobiera kontekst dzielnicy.
    /// </summary>
    /// <param name="districtId">Identyfikator dzielnicy.</param>
    /// <returns>Kontekst dzielnicy z danymi obiektów skojarzonych z dzielnicą.</returns>
    private async Task<DistrictContext> GetDistrictContext(int districtId)
    {
        var ret = new DistrictContext();
            
        ret.District = await _districtRepository.GetAsync(districtId);

        var districtGeneratedTypes = await _generatedTypeRepository.GetAllListAsync(item => item.DistrictId == districtId);
        var districtDrops = await _dropRepository.GetAllListAsync(item => item.DistrictId == districtId);
            
        ret.GeneratedTypes = districtGeneratedTypes;
        ret.Drops = districtDrops;
        ret.Requirements = await _requirementRepository.GetAllListAsync(item => item.DistrictId == districtId);
            
        ret.DriedFruits = (await _driedFruitRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
            
        ret.Lamps = (await _lampRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
            
        ret.Manures = (await _manureRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
            
        ret.Pots = (await _potRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
            
        ret.Seeds = (await _seedRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
            
        ret.Soils = (await _soilRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
            
        ret.Waters = (await _waterRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
            
        ret.Bonuses = (await _bonusRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
             
        var districtQuests = (await _questRepository.GetAllListAsync())?.Where(item =>
            districtGeneratedTypes.Any(type => type.Id == item.GeneratedTypeId) &&
            item.PlantationStorageId == null).ToList();
        ret.Quests = districtQuests;
            
        ret.QuestsRequirementsProgress = (await _questRequirementsProgressRepository.GetAllListAsync())
            ?.Where(item => districtQuests != null && districtQuests.Any(quest => quest.Id == item.QuestId)).ToList();

        ret.DropsQuests = (await _dropQuestRepository.GetAllListAsync())?.Where(item =>
            (districtQuests != null && districtQuests.Any(quest => quest.Id == item.QuestId)) ||
            (districtDrops != null && districtDrops.Any(drop => drop.Id == item.DropId))).ToList();

        return ret;
    }

    /// <summary>
    /// Pobiera maksymalne numery ID dla poszczególnych encji.
    /// </summary>
    /// <returns>Słownik zawierający maksymalne numery ID dla poszczególnych encji.</returns>
    private async Task<Dictionary<string, int>> GetEntitiesMaxIdNumber()
    {
        var districts = await _districtRepository.GetAllListAsync();
        var generatedTypes = await _generatedTypeRepository.GetAllListAsync();
        var driedFruits = await _driedFruitRepository.GetAllListAsync();
        var lamps = await _lampRepository.GetAllListAsync();
        var manures = await _manureRepository.GetAllListAsync();
        var pots = await _potRepository.GetAllListAsync();
        var seeds = await _seedRepository.GetAllListAsync();
        var soils = await _soilRepository.GetAllListAsync();
        var waters = await _waterRepository.GetAllListAsync();
        var drops = await _dropRepository.GetAllListAsync();
        var quests = await _questRepository.GetAllListAsync();
        var bonuses = await _bonusRepository.GetAllListAsync();
        var requirements = await _requirementRepository.GetAllListAsync();
        var questsRequirementsProgress = await _questRequirementsProgressRepository.GetAllListAsync();
        var dropsQuests = await _dropQuestRepository.GetAllListAsync();
            
        var districtsMax = districts != null && districts.Any() ? districts.Max(item => item.Id) : 0;
        var generatedTypesMax = generatedTypes != null && generatedTypes.Any() ? generatedTypes.Max(item => item.Id) : 0;
        var driedFruitsMax = driedFruits != null && driedFruits.Any() ? driedFruits.Max(item => item.Id) : 0;
        var lampsMax = lamps != null && lamps.Any() ? lamps.Max(item => item.Id) : 0;
        var manuresMax = manures != null && manures.Any() ? manures.Max(item => item.Id) : 0;
        var potsMax = pots != null && pots.Any() ? pots.Max(item => item.Id) : 0;
        var seedsMax = seeds != null && seeds.Any() ? seeds.Max(item => item.Id) : 0;
        var soilsMax = soils != null && soils.Any() ? soils.Max(item => item.Id) : 0;
        var watersMax = waters != null && waters.Any() ? waters.Max(item => item.Id) : 0;
        var dropsMax = drops != null && drops.Any() ? drops.Max(item => item.Id) : 0;
        var questsMax = quests != null && quests.Any() ? quests.Max(item => item.Id) : 0;
        var bonusesMax = bonuses != null && bonuses.Any() ? bonuses.Max(item => item.Id) : 0;
        var requirementsMax = requirements != null && requirements.Any() ? requirements.Max(item => item.Id) : 0;
        var questsRequirementsProgressMax = questsRequirementsProgress != null && questsRequirementsProgress.Any() ? questsRequirementsProgress.Max(item => item.Id) : 0;
        var dropsQuestsMax = dropsQuests != null && dropsQuests.Any() ? dropsQuests.Max(item => item.Id) : 0;

        return new Dictionary<string, int>
        {
            [EntitiesDbNames.District] = districtsMax,
            [EntitiesDbNames.GeneratedType] = generatedTypesMax,
            [EntitiesDbNames.DriedFruit] = driedFruitsMax,
            [EntitiesDbNames.Lamp] = lampsMax,
            [EntitiesDbNames.Manure] = manuresMax,
            [EntitiesDbNames.Pot] = potsMax,
            [EntitiesDbNames.Seed] = seedsMax,
            [EntitiesDbNames.Soil] = soilsMax,
            [EntitiesDbNames.Water] = watersMax,
            [EntitiesDbNames.Drop] = dropsMax,
            [EntitiesDbNames.Quest] = questsMax,
            [EntitiesDbNames.Bonus] = bonusesMax,
            [EntitiesDbNames.Requirement] = requirementsMax,
            [EntitiesDbNames.QuestRequirementsProgress] = questsRequirementsProgressMax,
            [EntitiesDbNames.DropQuest] = dropsQuestsMax
        };
    }
        
    /// <summary>
    /// Pobiera listę stworzonych obiektów.
    /// </summary>
    /// <param name="entityName">Nazwa encji.</param>
    /// <param name="ids">Lista identyfikatorów obiektów.</param>
    /// <returns>Lista stworzonych obiektów.</returns>
    private async Task<List<object>> GetCreatedObjects(string entityName, List<int> ids)
    {
        var ret = new List<object>();
        switch (entityName)
        {
            case EntitiesDbNames.District:
                foreach (var id in ids)
                    ret.Add(await _districtRepository.GetAsync(id));
                break;
            case EntitiesDbNames.GeneratedType:
                foreach (var id in ids)
                    ret.Add(await _generatedTypeRepository.GetAsync(id));
                break;
            case EntitiesDbNames.DriedFruit:
                foreach (var id in ids)
                    ret.Add(await _driedFruitRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Lamp:
                foreach (var id in ids)
                    ret.Add(await _lampRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Manure:
                foreach (var id in ids)
                    ret.Add(await _manureRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Pot:
                foreach (var id in ids)
                    ret.Add(await _potRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Seed:
                foreach (var id in ids)
                    ret.Add(await _seedRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Soil:
                foreach (var id in ids)
                    ret.Add(await _soilRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Water:
                foreach (var id in ids)
                    ret.Add(await _waterRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Drop:
                foreach (var id in ids)
                    ret.Add(await _dropRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Quest:
                foreach (var id in ids)
                    ret.Add(await _questRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Bonus:
                foreach (var id in ids)
                    ret.Add(await _bonusRepository.GetAsync(id));
                break;
            case EntitiesDbNames.Requirement:
                foreach (var id in ids)
                    ret.Add(await _requirementRepository.GetAsync(id));
                break;
            case EntitiesDbNames.QuestRequirementsProgress:
                foreach (var id in ids)
                    ret.Add(await _questRequirementsProgressRepository.GetAsync(id));
                break;
            case EntitiesDbNames.DropQuest:
                foreach (var id in ids)
                    ret.Add(await _dropQuestRepository.GetAsync(id));
                break;
            default:
                throw new ArgumentOutOfRangeException(entityName);
        }

        return ret;
    }

    /// <summary>
    /// Tworzy użytkownika typu opiekun dla określonej dzielnicy.
    /// </summary>
    /// <param name="district">Dzielnica, dla której tworzony jest opiekun.</param>
    /// <param name="wardenPassword">Hasło opiekuna.</param>
    /// <returns>Nowo utworzony użytkownik typu opiekun dzielnicy.</returns>
    private async Task<UserDto> CreateWardenForDistrict(District district, string wardenPassword)
    {
        await CreateWardenRoleIfNotExist();
        var regex = new Regex(@"[!@#$%\^&*\(\)-+=\/\\{\}\[\]\|:;""'<>,.\?\~`;]");
        var validDistrictName = regex.Replace(district.Name.Replace(" ",  "_"), "");
            
        var userName = "Opiekun_" + validDistrictName + "_" + StringUtils.RandomString(10);
        var user = new CreateUserDto
        {
            UserName = userName,
            Name = userName,
            Surname = "Brak",
            EmailAddress = userName + OtherConsts.ChillOutRoomEmail,
            IsActive = true,
            RoleNames = new[]
            {
                OtherConsts.WardenRoleName
            },
            Password = wardenPassword
        };

        return await _userAppService.CreateAsync(user); 
    }
        
    /// <summary>
    /// Tworzy rolę opiekuna dzielnicy, jeśli nie istnieje.
    /// </summary>
    private async Task CreateWardenRoleIfNotExist()
    {
        var wardenRole = await _roleRepository.FirstOrDefaultAsync(item => item.Name == OtherConsts.WardenRoleName);
        if (wardenRole != null) return;
            
        var role = new CreateRoleDto
        {
            Name = OtherConsts.WardenRoleName,
            DisplayName = OtherConsts.WardenRoleName,
            NormalizedName = null,
            Description = OtherConsts.WardenRoleDescription,
            IsStatic = false,
            GrantedPermissions = new List<string>
            {
                PermissionNames.Pages_ConfigurationPanel,
                PermissionNames.Crud_DistrictWarden
            }
        };
            
        await _roleAppService.CreateAsync(role); 
    }
}