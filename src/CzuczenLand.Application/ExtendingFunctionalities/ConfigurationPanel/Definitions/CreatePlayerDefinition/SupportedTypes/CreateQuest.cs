using System.Threading.Tasks;
using System.Transactions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do tworzenia obiektów typu Quest.
/// </summary>
public class CreateQuest : CreateDefinition<Quest, QuestCreateDto>, ICreateDefinition<QuestCreateDto>
{
    /// <summary>
    /// Repozytorium magazynów graczy.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Menedżer jednostki pracy.
    /// </summary>
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium zadań.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynów graczy.</param>
    /// <param name="unitOfWorkManager">Menedżer jednostki pracy.</param>
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
        
    /// <summary>
    /// Tworzy rekordy graczy na podstawie utworzonej definicji zadania.
    /// </summary>
    /// <param name="entity">DTO utworzonej definicji zadania.</param>
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