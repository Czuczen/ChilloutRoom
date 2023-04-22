using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
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

public class Deleter : IDeleter
{
    private readonly IRepository<Bonus> _bonusRepository;
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    private readonly IRepository<Lamp> _lampRepository;
    private readonly IRepository<Manure> _manureRepository;
    private readonly IRepository<Pot> _potRepository;
    private readonly IRepository<Seed> _seedRepository;
    private readonly IRepository<Soil> _soilRepository;
    private readonly IRepository<Water> _waterRepository;
    private readonly IRepository<Drop> _dropRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    private readonly IRepository<Quest> _questRepository;
    private readonly IRepository<BlackMarketTransaction> _blackMarketTransactionRepository;
    private readonly IRepository<Requirement> _requirementRepository;
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    private readonly IRepository<DropQuest> _dropQuestRepository;
    private readonly IRepository<DistrictDon> _districtDonRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IPlantService _plantService;

    
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
        
    public async Task DeleteConnected(District entity)
    {
        await _generatedTypeRepository.DeleteAsync(item => item.DistrictId == entity.Id);
        await _plantationStorageRepository.DeleteAsync(item => item.DistrictId == entity.Id);
            
        await _dropRepository.DeleteAsync(item => item.DistrictId == entity.Id);
        await _requirementRepository.DeleteAsync(item => item.DistrictId == entity.Id);
        await _districtDonRepository.DeleteAsync(item => item.DistrictId == entity.Id);
    }
        
    public async Task DeleteConnected(User entity)
    {
        await _playerStorageRepository.DeleteAsync(item => item.UserId == entity.Id);
    }
        
    public async Task DeleteConnected(PlayerStorage entity)
    {
        await _plantationStorageRepository.DeleteAsync(item => item.UserId == entity.UserId);
    }
        
    public async Task DeleteConnected(PlantationStorage entity)
    {
        var objectId = entity.Id;
        await _questRepository.DeleteAsync(item => item.PlantationStorageId == objectId);
        await _blackMarketTransactionRepository.DeleteAsync(item => item.SellerId == entity.UserId);
        await _plantService.DeletePlantAndSetUseCountConnectedProductsAsync(RelationFieldsNames.PlantationStorageId, objectId, false);
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

    [UnitOfWork] // Value cannot be null. Parameter name: unitOfWork. Custom repository factory potrzebuje UnitOfWork
    public virtual async Task DeleteConnected(GeneratedType entity)
    {
        await DeleteConnectedEntityToGeneratedType(entity);
        await DeleteOthersConnectedEntitiesToGeneratedType(entity);
    }

    public async Task DeleteConnected(Quest entity)
    {
        await _dropQuestRepository.DeleteAsync(item => item.QuestId == entity.Id);
        await _questRequirementsProgressRepository.DeleteAsync(item => item.QuestId == entity.Id);   
    }
        
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
                await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(new List<int> {currRel.QuestId}, questRepo,
                    EntitiesDbNames.Quest, _plantService);
                await _questRepository.DeleteAsync(currRel.QuestId);
            }
        }
            
        await _dropQuestRepository.DeleteAsync(item => item.DropId == entity.Id);
    }

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
                    
                await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(new List<int> {quest.Id}, questRepo,
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
    
    private async Task DeleteConnectedEntityToGeneratedType(GeneratedType generatedType)
    {
        var currRepo = CustomRepositoryFactory.GetRepository(generatedType.EntityName);
        var generatedTypeDefinition = (await currRepo.GetAllListAsync())?.Cast<IPlantationGeneratedEntity>().ToList()
            .SingleOrDefault(item => generatedType.Id == item.GeneratedTypeId && item.PlantationStorageId == null);
        
        if (generatedTypeDefinition == null) return;
            
        await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(new List<int> {generatedTypeDefinition.Id}, currRepo, generatedType.EntityName, _plantService);
        await currRepo.DeleteAsync(generatedTypeDefinition.Id);
    }
        
    private async Task DeleteOthersConnectedEntitiesToGeneratedType(GeneratedType generatedType)
    {
        if (generatedType.EntityName == EntitiesDbNames.Seed)
        {
            var driedFruitDef = (await _driedFruitRepository.GetAllListAsync(item => item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null)).SingleOrDefault();
            var driedFruitRepo = CustomRepositoryFactory.GetRepository(EntitiesDbNames.DriedFruit);
       
            if (driedFruitDef != null)
            {
                await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(new List<int> {driedFruitDef.Id}, driedFruitRepo, EntitiesDbNames.DriedFruit, _plantService);
                await driedFruitRepo.DeleteAsync(driedFruitDef.Id);
            }
        }
        
        await _requirementRepository.DeleteAsync(item => item.GeneratedTypeId == generatedType.Id);
        await _dropRepository.DeleteAsync(item => item.GeneratedTypeId == generatedType.Id);
        await _blackMarketTransactionRepository.DeleteAsync(item => item.GeneratedTypeId == generatedType.Id);
    }
}
