using System.Threading.Tasks;
using System.Transactions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition.SupportedTypes;

public class CreateQuest : CreateDefinition<Quest, QuestCreateDto>, ICreateDefinition<QuestCreateDto>
{
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    
    public CreateQuest(
        IRepository<Quest> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<PlayerStorage> playerStorageRepository,
        IUnitOfWorkManager unitOfWorkManager
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
        _playerStorageRepository = playerStorageRepository;
        _unitOfWorkManager = unitOfWorkManager;
    }
        
    public override async Task Create(QuestCreateDto entity)
    {
        var districtPlantationStorages = await GetPlantationStoragesByGeneratedType(entity);
        var newObjectAsDict = GetAsDictionary(entity);

        foreach (var storage in districtPlantationStorages)
        {
            var playerStorage = _playerStorageRepository.SingleAsync(item => item.UserId == storage.UserId);
                
            newObjectAsDict[RelationFieldsNames.PlantationStorageId] = storage.Id;
            newObjectAsDict[RelationFieldsNames.PlayerStorageId] = playerStorage?.Id;

            var mappedObject =  JsonConvert.DeserializeObject<Quest>(JsonConvert.SerializeObject(newObjectAsDict));

            using var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew); // musi być w foreach
            await Repository.InsertAndGetIdAsync(mappedObject);
            await unitOfWork.CompleteAsync();
        }
    }
}