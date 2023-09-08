using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.General;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.DeleteConnected;

/// <summary>
/// Klasa odpowiedzialna za usuwanie połączonych encji.
/// </summary>
public class Deleter : IDeleter
{
    /// <summary>
    /// Repozytorium dla encji Bonus.
    /// </summary>
    private readonly IRepository<Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium dla encji DriedFruit.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium dla encji Lamp.
    /// </summary>
    private readonly IRepository<Lamp> _lampRepository;
    
    /// <summary>
    /// Repozytorium dla encji Manure.
    /// </summary>
    private readonly IRepository<Manure> _manureRepository;
    
    /// <summary>
    /// Repozytorium dla encji Pot.
    /// </summary>
    private readonly IRepository<Pot> _potRepository;
    
    /// <summary>
    /// Repozytorium dla encji Seed.
    /// </summary>
    private readonly IRepository<Seed> _seedRepository;
    
    /// <summary>
    /// Repozytorium dla encji Soil.
    /// </summary>
    private readonly IRepository<Soil> _soilRepository;
    
    /// <summary>
    /// Repozytorium dla encji Water.
    /// </summary>
    private readonly IRepository<Water> _waterRepository;
    
    /// <summary>
    /// Repozytorium dla encji Drop.
    /// </summary>
    private readonly IRepository<Drop> _dropRepository;
    
    /// <summary>
    /// Repozytorium dla encji PlantationStorage.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium dla encji PlayerStorage.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Repozytorium dla encji Quest.
    /// </summary>
    private readonly IRepository<Quest> _questRepository;
    
    /// <summary>
    /// Repozytorium dla encji BlackMarketTransaction.
    /// </summary>
    private readonly IRepository<BlackMarketTransaction> _blackMarketTransactionRepository;
    
    /// <summary>
    /// Repozytorium dla encji Requirement.
    /// </summary>
    private readonly IRepository<Requirement> _requirementRepository;
    
    /// <summary>
    /// Repozytorium dla encji QuestRequirementsProgress.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Repozytorium dla encji GeneratedType.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium dla encji DropQuest.
    /// </summary>
    private readonly IRepository<DropQuest> _dropQuestRepository;
    
    /// <summary>
    /// Repozytorium dla encji DistrictDon.
    /// </summary>
    private readonly IRepository<DistrictDon> _districtDonRepository;
    
    /// <summary>
    /// Repozytorium dla encji District.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Usługa obsługująca operacje związane z roślinami.
    /// </summary>
    private readonly IPlantService _plantService;
    
    

    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="bonusRepository">Repozytorium dla encji Bonus.</param>
    /// <param name="driedFruitRepository">Repozytorium dla encji DriedFruit.</param>
    /// <param name="lampRepository">Repozytorium dla encji Lamp.</param>
    /// <param name="manureRepository">Repozytorium dla encji Manure.</param>
    /// <param name="potRepository">Repozytorium dla encji Pot.</param>
    /// <param name="seedRepository">Repozytorium dla encji Seed.</param>
    /// <param name="soilRepository">Repozytorium dla encji Soil.</param>
    /// <param name="waterRepository">Repozytorium dla encji Water.</param>
    /// <param name="dropRepository">Repozytorium dla encji Drop.</param>
    /// <param name="plantationStorageRepository">Repozytorium dla encji PlantationStorage.</param>
    /// <param name="playerStorageRepository">Repozytorium dla encji PlayerStorage.</param>
    /// <param name="questRepository">Repozytorium dla encji Quest.</param>
    /// <param name="blackMarketTransactionRepository">Repozytorium dla encji BlackMarketTransaction.</param>
    /// <param name="requirementRepository">Repozytorium dla encji Requirement.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium dla encji QuestRequirementsProgress.</param>
    /// <param name="generatedTypeRepository">Repozytorium dla encji GeneratedType.</param>
    /// <param name="dropQuestRepository">Repozytorium dla encji DropQuest.</param>
    /// <param name="districtDonRepository">Repozytorium dla encji DistrictDon.</param>
    /// <param name="districtRepository">Repozytorium dla encji District.</param>
    /// <param name="plantService">Usługa obsługująca operacje związane z roślinami.</param>
    public Deleter(
        IRepository<Bonus> bonusRepository,
        IRepository<DriedFruit> driedFruitRepository, 
        IRepository<Lamp> lampRepository, 
        IRepository<Manure> manureRepository, 
        IRepository<Pot> potRepository, 
        IRepository<Seed> seedRepository, 
        IRepository<Soil> soilRepository, 
        IRepository<Water> waterRepository, 
        IRepository<Drop> dropRepository, 
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<PlayerStorage> playerStorageRepository, 
        IRepository<Quest> questRepository,
        IRepository<BlackMarketTransaction> blackMarketTransactionRepository,
        IRepository<Requirement> requirementRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<DropQuest> dropQuestRepository,
        IRepository<DistrictDon> districtDonRepository,
        IRepository<District> districtRepository,
        IPlantService plantService
    )
    {
        _bonusRepository = bonusRepository;
        _driedFruitRepository = driedFruitRepository;
        _lampRepository = lampRepository;
        _manureRepository = manureRepository;
        _potRepository = potRepository;
        _seedRepository = seedRepository;
        _soilRepository = soilRepository;
        _waterRepository = waterRepository;
        _dropRepository = dropRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _playerStorageRepository = playerStorageRepository;
        _questRepository = questRepository;
        _blackMarketTransactionRepository = blackMarketTransactionRepository;
        _requirementRepository = requirementRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _dropQuestRepository = dropQuestRepository;
        _districtDonRepository = districtDonRepository;
        _districtRepository = districtRepository;
        _plantService = plantService;
    }
        
    /// <summary>
    /// Usuwa powiązane encje związane z daną dzielnicą.
    /// </summary>
    /// <param name="entity">Rekord dzielnicy, dla której usuwane są powiązane encje.</param>
    public async Task DeleteConnected(District entity)
    {
        await _generatedTypeRepository.DeleteAsync(item => item.DistrictId == entity.Id);
        await _plantationStorageRepository.DeleteAsync(item => item.DistrictId == entity.Id);
            
        await _dropRepository.DeleteAsync(item => item.DistrictId == entity.Id);
        await _requirementRepository.DeleteAsync(item => item.DistrictId == entity.Id);
        await _districtDonRepository.DeleteAsync(item => item.DistrictId == entity.Id);
    }
        
    /// <summary>
    /// Usuwa powiązane encje związane z danym użytkownikiem.
    /// </summary>
    /// <param name="entity">Rekord użytkownika, dla którego usuwane są powiązane encje.</param>
    public async Task DeleteConnected(User entity)
    {
        await _playerStorageRepository.DeleteAsync(item => item.UserId == entity.Id);
    }
        
    /// <summary>
    /// Usuwa powiązane encje związane z danym magazynem gracza.
    /// </summary>
    /// <param name="entity">Rekord magazynu gracza, dla którego usuwane są powiązane encje.</param>
    public async Task DeleteConnected(PlayerStorage entity)
    {
        await _plantationStorageRepository.DeleteAsync(item => item.UserId == entity.UserId);
    }
        
    /// <summary>
    /// Usuwa powiązane encje związane z danym magazynem plantacji.
    /// </summary>
    /// <param name="entity">Rekord magazynu plantacji, dla którego usuwane są powiązane encje.</param>
    public async Task DeleteConnected(PlantationStorage entity)
    {
        var objectId = entity.Id;
        await _questRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _blackMarketTransactionRepository.DeleteAsync(item => item.SellerId == entity.UserId);
        await _plantService.DeletePlantAndSetUseCountConnectedProducts(RelationFieldsNames.PlantationStorageId, objectId, false);
        await _driedFruitRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _lampRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _manureRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _potRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _seedRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _soilRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _waterRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _bonusRepository.DeleteAsync(item => item.PlantationStorageId == objectId);

        var districtDon = await _districtDonRepository.FirstOrDefaultAsync(item =>
            item.DistrictId == entity.DistrictId && item.PlantationStorageId == entity.Id);

        if (districtDon != null)
        {
            var district = await _districtRepository.FirstOrDefaultAsync(item => item.Id == entity.DistrictId);
            if (district != null)
            {
                var districtPlantationStorages = await _plantationStorageRepository.GetAllListAsync(item => item.DistrictId == entity.DistrictId);
                if (districtPlantationStorages.Any())
                {
                    var bestPlantationStorage = districtPlantationStorages.Aggregate((i1, i2) => i1.Prestige > i2.Prestige ? i1 : i2);
                
                    if (bestPlantationStorage.Prestige < district.PrestigeToBecomeDon)
                        await _districtDonRepository.DeleteAsync(districtDon.Id);
                    else
                        districtDon.PlantationStorageId = bestPlantationStorage.Id;   
                }
                else
                    await _districtDonRepository.DeleteAsync(districtDon.Id);   
            }
            else
                await _districtDonRepository.DeleteAsync(districtDon.Id);
        }
    }

    /// <summary>
    /// Usuwa powiązane encje związane z danym typem generowanym.
    /// </summary>
    /// <param name="entity">Rekord typu generowanego, dla którego usuwane są powiązane encje.</param>
    [UnitOfWork] // Value cannot be null. Parameter name: unitOfWork. Custom repository factory potrzebuje UnitOfWork
    public virtual async Task DeleteConnected(GeneratedType entity)
    {
        await DeleteConnectedEntityToGeneratedType(entity);
        await DeleteOthersConnectedEntitiesToGeneratedType(entity);
    }

    /// <summary>
    /// Usuwa powiązane encje związane z danym zadaniem.
    /// </summary>
    /// <param name="entity">Rekord zadania, dla którego usuwane są powiązane encje.</param>
    public async Task DeleteConnected(Quest entity)
    {
        await _dropQuestRepository.DeleteAsync(item => item.QuestId == entity.Id);
        await _questRequirementsProgressRepository.DeleteAsync(item => item.QuestId == entity.Id);   
    }
        
    /// <summary>
    /// Usuwa powiązane encje związane z daną nagrodą.
    /// </summary>
    /// <param name="entity">Rekord nagrody, dla której usuwane są powiązane encje.</param>
    [UnitOfWork]
    public virtual async Task DeleteConnected(Drop entity)
    {
        var dropRelations = await _dropQuestRepository.GetAllListAsync(item => item.DropId == entity.Id);
        var questRepo = CustomRepositoryFactory.GetRepository(EntitiesDbNames.Quest);
        foreach (var rel in dropRelations)
        {
            var questRelations = await _dropQuestRepository.GetAllListAsync(item => item.QuestId == rel.QuestId);
            if (questRelations.Count == 1)
            {
                var currRel = questRelations.First();
                await DeletePlayerRecordHelper.DeleteAllRecordsByDefs(new List<int> {currRel.QuestId}, questRepo,
                    EntitiesDbNames.Quest, _plantService);
                await _questRepository.DeleteAsync(currRel.QuestId);
            }
        }
            
        await _dropQuestRepository.DeleteAsync(item => item.DropId == entity.Id);
    }

    /// <summary>
    /// Usuwa powiązane encje związane z danym wymaganiem.
    /// </summary>
    /// <param name="entity">Rekord wymagania, dla którego usuwane są powiązane encje.</param>
    [UnitOfWork] // Value cannot be null. Parameter name: unitOfWork. Custom repository factory potrzebuje UnitOfWork
    public virtual async Task DeleteConnected(Requirement entity)
    {
        var questsAndQuestsProgress = await _questRequirementsProgressRepository.GetAll().Join(_questRepository.GetAll().Join(
                _generatedTypeRepository.GetAll().Where(item => item.DistrictId == entity.DistrictId),
                quest => quest.GeneratedTypeId,
                type => type.Id,
                (quest, type) => quest),

            progress => progress.QuestId,
            quest => quest.Id,
            (progress, quest) => new {progress, quest}).ToListAsync();

        var quests = questsAndQuestsProgress.Select(item => item.quest).ToList();
        var questsProgress = questsAndQuestsProgress.Select(item => item.progress).ToList();
        var questRepo = CustomRepositoryFactory.GetRepository(EntitiesDbNames.Quest);
        
        foreach (var quest in quests)
        {
            var questProgress = questsProgress.Single(item => item.QuestId == quest.Id);
            var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
            if (!requirementsProgress.ContainsKey(entity.Id)) continue;
                
            requirementsProgress.Remove(entity.Id);

            if (!requirementsProgress.Keys.Any())
            {
                if (quest.PlantationStorageId != null) continue;
                    
                await DeletePlayerRecordHelper.DeleteAllRecordsByDefs(new List<int> {quest.Id}, questRepo,
                    EntitiesDbNames.Quest, _plantService);
                await _questRepository.DeleteAsync(quest);
            }
            else
            {
                questProgress.RequirementsProgress = JsonConvert.SerializeObject(requirementsProgress, new DecimalFormatConverter());
                await _questRequirementsProgressRepository.UpdateAsync(questProgress);   
            }
        }
    }
    
    /// <summary>
    /// Usuwa powiązane encje związane z danym typem generowanym.
    /// </summary>
    /// <param name="generatedType">Typ generowany, dla którego usuwane są powiązane encje.</param>
    private async Task DeleteConnectedEntityToGeneratedType(GeneratedType generatedType)
    {
        var currRepo = CustomRepositoryFactory.GetRepository(generatedType.EntityName);
        var generatedTypeDefinition = (await currRepo.GetAllListAsync())?.Cast<IPlantationGeneratedEntity>().ToList()
            .SingleOrDefault(item => generatedType.Id == item.GeneratedTypeId && item.PlantationStorageId == null);
        
        if (generatedTypeDefinition == null) return;
            
        await DeletePlayerRecordHelper.DeleteAllRecordsByDefs(new List<int> {generatedTypeDefinition.Id}, currRepo, generatedType.EntityName, _plantService);
        await currRepo.DeleteAsync(generatedTypeDefinition.Id);
    }
       
    /// <summary>
    /// Usuwa inne powiązane encje związane z danym typem generowanym.
    /// </summary>
    /// <param name="generatedType">Typ wygenerowany, dla którego usuwane są powiązane encje.</param>
    private async Task DeleteOthersConnectedEntitiesToGeneratedType(GeneratedType generatedType)
    {
        if (generatedType.EntityName == EntitiesDbNames.Seed)
        {
            var driedFruitDef = (await _driedFruitRepository.GetAllListAsync(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null)).SingleOrDefault();
            var driedFruitRepo = CustomRepositoryFactory.GetRepository(EntitiesDbNames.DriedFruit);
       
            if (driedFruitDef != null)
            {
                await DeletePlayerRecordHelper.DeleteAllRecordsByDefs(new List<int> {driedFruitDef.Id}, driedFruitRepo, EntitiesDbNames.DriedFruit, _plantService);
                await driedFruitRepo.DeleteAsync(driedFruitDef.Id);
            }
        }
        
        await _requirementRepository.DeleteAsync(item => item.GeneratedTypeId == generatedType.Id);
        await _dropRepository.DeleteAsync(item => item.GeneratedTypeId == generatedType.Id);
        await _blackMarketTransactionRepository.DeleteAsync(item => item.GeneratedTypeId == generatedType.Id);
    }
}
