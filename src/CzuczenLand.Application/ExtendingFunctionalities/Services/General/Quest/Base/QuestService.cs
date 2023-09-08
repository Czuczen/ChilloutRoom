using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Timing;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Conditions;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.CustomEntity;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Loader;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;

/// <summary>
/// Serwis podstawowy obsługujący logikę biznesową związaną z zadaniami.
/// </summary>
public class QuestService : IQuestService
{
    /// <summary>
    /// Repozytorium zadań.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.Quest> _questRepository;
    
    /// <summary>
    /// Repozytorium typu generowanego.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium magazynu gracza.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Repozytorium magazynu plantacji.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium wymagań.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.Requirement> _requirementRepository;
    
    /// <summary>
    /// Repozytorium nasion.
    /// </summary>
    private readonly IRepository<Seed> _seedRepository;
    
    /// <summary>
    /// Repozytorium suszu.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium tabeli łączącej nagrody z zadaniami.
    /// </summary>
    private readonly IRepository<DropQuest> _dropQuestRepository;
    
    /// <summary>
    /// Repozytorium postępu wymagań zadania.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Kontroluje tworzenie niestandardowych repozytoriów.
    /// </summary>
    private readonly ICustomRepositoryLoader _customRepositoryLoader;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="questRepository">Repozytorium zadań.</param>
    /// <param name="generatedTypeRepository">Repozytorium typu generowanego.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynu gracza.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynu plantacji.</param>
    /// <param name="requirementRepository">Repozytorium wymagań.</param>
    /// <param name="seedRepository">Repozytorium nasion.</param>
    /// <param name="driedFruitRepository">Repozytorium suszu.</param>
    /// <param name="dropQuestRepository">Repozytorium tabeli łączącej nagrody z zadaniami.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium postępu wymagań zadania.</param>
    /// <param name="customRepositoryLoader">Kontroluje tworzenie niestandardowych repozytoriów.</param>
    public QuestService(
        IRepository<ExtendingModels.Models.General.Quest> questRepository,
        IRepository<ExtendingModels.Models.General.GeneratedType> generatedTypeRepository,
        IRepository<ExtendingModels.Models.General.PlayerStorage> playerStorageRepository,
        IRepository<ExtendingModels.Models.General.PlantationStorage> plantationStorageRepository,
        IRepository<ExtendingModels.Models.General.Requirement> requirementRepository,
        IRepository<Seed> seedRepository,
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<DropQuest> dropQuestRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        ICustomRepositoryLoader customRepositoryLoader
    )
    {
        _questRepository = questRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _playerStorageRepository = playerStorageRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _requirementRepository = requirementRepository;
        _seedRepository = seedRepository;
        _driedFruitRepository = driedFruitRepository;
        _dropQuestRepository = dropQuestRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _customRepositoryLoader = customRepositoryLoader;
    }

    /// <summary>
    /// Ustawia synchronicznie wartości początkowe dla zadań gracza.
    /// </summary>
    /// <param name="playerRecords">Lista zadań gracza.</param>
    public void SetStartValues(List<ExtendingModels.Models.General.Quest> playerRecords)
    {
        if (!playerRecords.Any()) return;
        
        var currDateTime = Clock.Now;
        var firstQuest = playerRecords.First();
        var plantationStorage = _plantationStorageRepository.Single(item => item.Id == firstQuest.PlantationStorageId);
        var playerStorage = _playerStorageRepository.Single(item => item.UserId == plantationStorage.UserId);
        
        var generatedTypeQuery = _generatedTypeRepository.GetAll().Where(item => item.DistrictId == plantationStorage.DistrictId);

        var questsProgress = _questRequirementsProgressRepository.GetAll().Join(
            _questRepository.GetAll().Join(generatedTypeQuery,
                quest => quest.GeneratedTypeId,
                generatedType => generatedType.Id,
                (quest, generatedType) => quest),

            progress => progress.QuestId,
            quest => quest.Id,
            (progress, quest) => progress).ToList();

        var seeds = _seedRepository.GetAll().Join(generatedTypeQuery,
            seed => seed.GeneratedTypeId,
            generatedType => generatedType.Id,
            (seed, generatedType) => seed).ToList();

        var driedFruits = _driedFruitRepository.GetAll().Join(generatedTypeQuery,
            driedFruit => driedFruit.GeneratedTypeId,
            generatedType => generatedType.Id,
            (driedFruit, generatedType) => driedFruit).ToList();
        
        var requirements = _requirementRepository.GetAllList(item => item.DistrictId == plantationStorage.DistrictId);
        var generatedTypes = generatedTypeQuery.ToList(); 

        foreach (var playerRecord in playerRecords)
        {
            playerRecord.IsComplete = false; // Po edycji zadania przez opiekuna dzielnicy ukończone zadanie było w ukończonych/akordeon z nieukończonymi wymaganiami. IsComplete ustawiamy na false przez co teraz zadanie będzie w dostępnych.
            playerRecord.CurrentDuration = 0;
                
            switch (playerRecord.QuestType)
            {
                case DbQuestTypesNames.Daily:
                    break;
                case DbQuestTypesNames.Weekly:
                    break;
                case DbQuestTypesNames.Event:
                    // Kiedy opiekun dzielnicy edytuje zadanie które jest już nie dostępne czasowo ale jest ukończone to z racji że zostało zresetowane musi zostać usunięte z listy/akordeon ukończonych. Jeśli ustawimy WorkerSent na true to RemoveNotAvailableEventQuests ustawi zadanie tam gdzie jego miejsce.
                    if (playerRecord.EndTime < currDateTime) 
                        playerRecord.WorkerSent = true;
                    break;
                case DbQuestTypesNames.Achievement:
                    playerRecord.InProgress = true;
                    break;
            }

            var questProgress = questsProgress.Single(item => item.QuestId == playerRecord.Id);
            var questRequirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
            var newQuestRequirementsProgress = new Dictionary<int, decimal>();
            
            foreach (var reqProgress in questRequirementsProgress)
            {
                newQuestRequirementsProgress[reqProgress.Key] = reqProgress.Value;
                    
                var req = requirements.Single(item => item.Id == reqProgress.Key);
                if (req.Condition != DbRequirementConditionsNames.Possession &&
                    req.Condition != DbRequirementConditionsNames.Deliver) continue;
                    
                if (!string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    switch (req.CustomEntityName)
                    {
                        case DbCustomEntityNames.PlantationStorage:
                            var tempValue1 =  plantationStorage.GetType().GetProperty(req.Comparer).GetValue(plantationStorage);
                            var value1 = tempValue1 == null || tempValue1.ToString() == "" ? 0 : decimal.Parse(tempValue1.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value1 > req.Amount ? req.Amount : value1;
                            break;
                        case DbCustomEntityNames.PlayerStorage:
                            var tempValue2 = playerStorage.GetType().GetProperty(req.Comparer).GetValue(playerStorage);
                            var value2 = tempValue2 == null || tempValue2.ToString() == "" ? 0 : decimal.Parse(tempValue2.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value2 > req.Amount ? req.Amount : value2;
                            break;
                        case DbCustomEntityNames.Seed when req.GeneratedTypeId != null:
                            var seed = seeds.Single(item => item.GeneratedTypeId == req.GeneratedTypeId && item.PlantationStorageId == plantationStorage.Id);
                            var tempValue3 = seed.GetType().GetProperty(req.Comparer).GetValue(seed);
                            var value3 = tempValue3 == null || tempValue3.ToString() == "" ? 0 : decimal.Parse(tempValue3.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value3 > req.Amount ? req.Amount : value3;
                            break;
                        case DbCustomEntityNames.DriedFruit when req.GeneratedTypeId != null:
                            var driedFruit = driedFruits.Single(item => item.GeneratedTypeId == req.GeneratedTypeId && item.PlantationStorageId == plantationStorage.Id);
                            var tempValue4 = driedFruit.GetType().GetProperty(req.Comparer).GetValue(driedFruit);
                            var value4 = tempValue4 == null || tempValue4.ToString() == "" ? 0 : decimal.Parse(tempValue4.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value4 > req.Amount ? req.Amount : value4;
                            break;
                    }
                }
                else if (req.GeneratedTypeId != null)
                {
                    var generatedType = generatedTypes.Single(item => item.Id == (int) req.GeneratedTypeId);
                    var repo = _customRepositoryLoader.GetRepository(generatedType.EntityName);
                    var entity = repo.GetWhere(RelationFieldsNames.PlantationStorageId, playerRecord.PlantationStorageId)?
                        .Cast<IGeneratedEntity>()
                        .SingleOrDefault(item => item.GeneratedTypeId == generatedType.Id);
                        
                    if (entity == null) continue;
                    var type = entity.GetType();
                    var tempValue = type.GetProperty(req.Comparer).GetValue(entity);
                    var value = tempValue == null || tempValue.ToString() == "" ? 0 : decimal.Parse(tempValue.ToString().Replace(".", ","));
                    newQuestRequirementsProgress[req.Id] = value > req.Amount ? req.Amount : value;
                }
            }

            questProgress.RequirementsProgress = JsonConvert.SerializeObject(newQuestRequirementsProgress);
        }
    }

    /// <summary>
    /// Ustawia asynchronicznie wartości początkowe dla zadań gracza.
    /// </summary>
    /// <param name="playerRecords">Lista zadań gracza.</param>
    public async Task SetStartValuesAsync(List<ExtendingModels.Models.General.Quest> playerRecords)
    {
        if (!playerRecords.Any()) return;
        
        var currDateTime = Clock.Now;
        var firstQuest = playerRecords.First();
        var plantationStorage = await _plantationStorageRepository.SingleAsync(item => item.Id == firstQuest.PlantationStorageId);
        var playerStorage = await _playerStorageRepository.SingleAsync(item => item.UserId == plantationStorage.UserId);
        
        var generatedTypeQuery = _generatedTypeRepository.GetAll().Where(item => item.DistrictId == plantationStorage.DistrictId);

        var questsProgress = await _questRequirementsProgressRepository.GetAll().Join(
            _questRepository.GetAll().Join(generatedTypeQuery,
                quest => quest.GeneratedTypeId,
                generatedType => generatedType.Id,
                (quest, generatedType) => quest),

            progress => progress.QuestId,
            quest => quest.Id,
            (progress, quest) => progress).ToListAsync();

        var seeds = await _seedRepository.GetAll().Join(generatedTypeQuery,
            seed => seed.GeneratedTypeId,
            generatedType => generatedType.Id,
            (seed, generatedType) => seed).ToListAsync();

        var driedFruits = await _driedFruitRepository.GetAll().Join(generatedTypeQuery,
            driedFruit => driedFruit.GeneratedTypeId,
            generatedType => generatedType.Id,
            (driedFruit, generatedType) => driedFruit).ToListAsync();
        
        var requirements = await _requirementRepository.GetAllListAsync(item => item.DistrictId == plantationStorage.DistrictId);
        var generatedTypes = await generatedTypeQuery.ToListAsync(); 

        foreach (var playerRecord in playerRecords)
        {
            playerRecord.IsComplete = false; // Po edycji zadania przez opiekuna dzielnicy ukończone zadanie było w ukończonych/akordeon z nieukończonymi wymaganiami. IsComplete ustawiamy na false przez co teraz zadanie będzie w dostępnych.
            playerRecord.CurrentDuration = 0;
                
            switch (playerRecord.QuestType)
            {
                case DbQuestTypesNames.Daily:
                    break;
                case DbQuestTypesNames.Weekly:
                    break;
                case DbQuestTypesNames.Event:
                    // Kiedy opiekun dzielnicy edytuje zadanie które jest już nie dostępne czasowo ale jest ukończone to z racji że zostało zresetowane musi zostać usunięte z listy/akordeon ukończonych. Jeśli ustawimy WorkerSent na true to RemoveNotAvailableEventQuests ustawi zadanie tam gdzie jego miejsce.
                    if (playerRecord.EndTime < currDateTime) 
                        playerRecord.WorkerSent = true;
                    break;
                case DbQuestTypesNames.Achievement:
                    playerRecord.InProgress = true;
                    break;
            }

            var questProgress = questsProgress.Single(item => item.QuestId == playerRecord.Id);
            var questRequirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
            var newQuestRequirementsProgress = new Dictionary<int, decimal>();
            
            foreach (var reqProgress in questRequirementsProgress)
            {
                newQuestRequirementsProgress[reqProgress.Key] = reqProgress.Value;
                    
                var req = requirements.Single(item => item.Id == reqProgress.Key);
                if (req.Condition != DbRequirementConditionsNames.Possession &&
                    req.Condition != DbRequirementConditionsNames.Deliver) continue;
                    
                if (!string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    switch (req.CustomEntityName)
                    {
                        case DbCustomEntityNames.PlantationStorage:
                            var tempValue1 =  plantationStorage.GetType().GetProperty(req.Comparer).GetValue(plantationStorage);
                            var value1 = tempValue1 == null || tempValue1.ToString() == "" ? 0 : decimal.Parse(tempValue1.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value1 > req.Amount ? req.Amount : value1;
                            break;
                        case DbCustomEntityNames.PlayerStorage:
                            var tempValue2 = playerStorage.GetType().GetProperty(req.Comparer).GetValue(playerStorage);
                            var value2 = tempValue2 == null || tempValue2.ToString() == "" ? 0 : decimal.Parse(tempValue2.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value2 > req.Amount ? req.Amount : value2;
                            break;
                        case DbCustomEntityNames.Seed when req.GeneratedTypeId != null:
                            var seed = seeds.Single(item => item.GeneratedTypeId == req.GeneratedTypeId && item.PlantationStorageId == plantationStorage.Id);
                            var tempValue3 = seed.GetType().GetProperty(req.Comparer).GetValue(seed);
                            var value3 = tempValue3 == null || tempValue3.ToString() == "" ? 0 : decimal.Parse(tempValue3.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value3 > req.Amount ? req.Amount : value3;
                            break;
                        case DbCustomEntityNames.DriedFruit when req.GeneratedTypeId != null:
                            var driedFruit = driedFruits.Single(item => item.GeneratedTypeId == req.GeneratedTypeId && item.PlantationStorageId == plantationStorage.Id);
                            var tempValue4 = driedFruit.GetType().GetProperty(req.Comparer).GetValue(driedFruit);
                            var value4 = tempValue4 == null || tempValue4.ToString() == "" ? 0 : decimal.Parse(tempValue4.ToString().Replace(".", ","));
                            newQuestRequirementsProgress[req.Id] = value4 > req.Amount ? req.Amount : value4;
                            break;
                    }
                }
                else if (req.GeneratedTypeId != null)
                {
                    var generatedType = generatedTypes.Single(item => item.Id == (int) req.GeneratedTypeId);
                    var repo = _customRepositoryLoader.GetRepository(generatedType.EntityName);
                    var entity = (await repo.GetWhereAsync(RelationFieldsNames.PlantationStorageId, playerRecord.PlantationStorageId))?
                        .Cast<IGeneratedEntity>()
                        .SingleOrDefault(item => item.GeneratedTypeId == generatedType.Id);
                        
                    if (entity == null) continue;
                    var type = entity.GetType();
                    var tempValue = type.GetProperty(req.Comparer).GetValue(entity);
                    var value = tempValue == null || tempValue.ToString() == "" ? 0 : decimal.Parse(tempValue.ToString().Replace(".", ","));
                    newQuestRequirementsProgress[req.Id] = value > req.Amount ? req.Amount : value;
                }
            }

            questProgress.RequirementsProgress = JsonConvert.SerializeObject(newQuestRequirementsProgress);
        }
    }

    /// <summary>
    /// Tworzy połączenia dla zadania z określonej encji i przypisanych do niej identyfikatorów.
    /// </summary>
    /// <param name="entityId">Identyfikator zadania będącego definicją.</param>
    /// <param name="entitiesIds">Słownik zawierający nazwę encji i przypisane do niej identyfikatory.</param>
    /// <returns>Lista zadań graczy z przypisanymi połączeniami.</returns>
    private async Task<List<ExtendingModels.Models.General.Quest>> CreateQuestConnections(int entityId, Dictionary<string, List<int>> entitiesIds)
    {
        var questRequirementsProgress = new Dictionary<int, decimal>();
        var quest = await _questRepository.GetAsync(entityId);
        var generatedType = await _generatedTypeRepository.GetAsync(quest.GeneratedTypeId);
        
        var generatedTypeQuery = _generatedTypeRepository.GetAll().Where(item => item.DistrictId == generatedType.DistrictId);
        var questsProgress = await _questRequirementsProgressRepository.GetAll().Join(
            _questRepository.GetAll().Join(generatedTypeQuery,
                currQuest => currQuest.GeneratedTypeId,
                currGeneratedType => currGeneratedType.Id,
                (currQuest, currGeneratedType) => currQuest),

            progress => progress.QuestId,
            currQuest => currQuest.Id,
            (progress, currQuest) => progress).ToListAsync();

        foreach (var entityIdsPair in entitiesIds)
        {
            switch (entityIdsPair.Key)
            {
                case EntitiesDbNames.Drop:
                    foreach (var dropId in entityIdsPair.Value)
                    {
                        await _dropQuestRepository.InsertAsync(new DropQuest
                            {DropId = dropId, QuestId = quest.Id});
                    }
                    break;
                case EntitiesDbNames.Requirement:
                    foreach (var requirementId in entityIdsPair.Value)
                    {
                        questRequirementsProgress[requirementId] = 0;
                    }
                    break;
            }
        }
        
        var newRequirementsProgress = JsonConvert.SerializeObject(questRequirementsProgress);
        var questProgress = await _questRequirementsProgressRepository.FirstOrDefaultAsync(item => item.QuestId == quest.Id);
        if (questProgress == null)
        {
            await _questRequirementsProgressRepository.InsertAndGetIdAsync(new QuestRequirementsProgress
                {QuestId = quest.Id, RequirementsProgress = newRequirementsProgress});
        }
        else
            questProgress.RequirementsProgress = newRequirementsProgress;
            
        var playersRecords = await _questRepository.GetAllListAsync(item => item.GeneratedTypeId == quest.GeneratedTypeId && item.Id != entityId);
        foreach (var playerRecord in playersRecords)
        {
            var playerRecordProgress = questsProgress.SingleOrDefault(item => item.QuestId == playerRecord.Id);
            if (playerRecordProgress == null)
            {
                await _questRequirementsProgressRepository.InsertAndGetIdAsync(new QuestRequirementsProgress
                    {QuestId = playerRecord.Id, RequirementsProgress = newRequirementsProgress});
            }
            else
                playerRecordProgress.RequirementsProgress = newRequirementsProgress;
        }

        return playersRecords;
    }

    /// <summary>
    /// Ustawia zależności zadania i wartości początkowe.
    /// </summary>
    /// <param name="entityId">Identyfikator zadania będącego definicją.</param>
    /// <param name="entitiesIds">Słownik zawierający nazwę encji i przypisane do niej identyfikatory.</param>
    public async Task SetQuestDependencies(int? entityId, Dictionary<string, List<int>> entitiesIds)
    {
        if (entitiesIds == null || entitiesIds.Count == 0 || entityId == null) return;

        var playersRecords = await CreateQuestConnections((int) entityId, entitiesIds);
        foreach (var group in playersRecords.GroupBy(item => item.PlantationStorageId))
            await SetStartValuesAsync(group.ToList());
    }

    /// <summary>
    /// Aktualizuje zależności zadania.
    /// </summary>
    /// <param name="entityId">Identyfikator zadania będącego definicją.</param>
    /// <param name="entitiesIds">Słownik zawierający nazwę encji i przypisane do niej identyfikatory.</param>
    public async Task UpdateQuestDependencies(int? entityId, Dictionary<string, List<int>> entitiesIds)
    {
        await _dropQuestRepository.DeleteAsync(item => item.QuestId == entityId);
        await SetQuestDependencies(entityId, entitiesIds);
    }

    /// <summary>
    /// Tworzy postęp wymagań zadań dla gracza na podstawie zadań będących definicją.
    /// </summary>
    /// <param name="questsDefinitions">Lista definicji zadań.</param>
    /// <param name="playerRecords">Lista zadań gracza.</param>
    public async Task CreatePlayerQuestsRequirementsProgress(List<ExtendingModels.Models.General.Quest> questsDefinitions, 
        List<ExtendingModels.Models.General.Quest> playerRecords)
    {
        var firstQuestDef = questsDefinitions.First();
        var generatedType = await _generatedTypeRepository.GetAsync(firstQuestDef.GeneratedTypeId);
        
        var generatedTypeQuery = _generatedTypeRepository.GetAll().Where(item => item.DistrictId == generatedType.DistrictId);
        var questsProgress = await _questRequirementsProgressRepository.GetAll().Join(
            _questRepository.GetAll().Join(generatedTypeQuery,
                currQuest => currQuest.GeneratedTypeId,
                currGeneratedType => currGeneratedType.Id,
                (currQuest, currGeneratedType) => currQuest),

            progress => progress.QuestId,
            currQuest => currQuest.Id,
            (progress, currQuest) => progress).ToListAsync();

        foreach (var playerQuest in playerRecords)
        {
            var questDef = questsDefinitions.Single(item => item.GeneratedTypeId == playerQuest.GeneratedTypeId);
            var questDefRequirementsProgress = questsProgress.Single(item => item.QuestId == questDef.Id);
            var newQuestRequirementsProgress = new QuestRequirementsProgress
                {QuestId = playerQuest.Id, RequirementsProgress = questDefRequirementsProgress.RequirementsProgress};

            await _questRequirementsProgressRepository.InsertAndGetIdAsync(newQuestRequirementsProgress); // InsertAsync nie zapisywał od razu. Dlatego jest InsertAndGetIdAsync
        }
    }
}