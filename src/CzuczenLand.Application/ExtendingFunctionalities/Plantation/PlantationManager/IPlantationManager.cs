using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.ObjectMapping;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager;

public interface IPlantationManager : ITransientDependency
{
    Task<Plantation> GetPlantation(long userId, int? districtId, bool heWantPayForHollow);

    Task<BlackMarket> CreatePlayerBlackMarket(long userId, IObjectMapper objectMapper);

    Task<List<object>> GetAvailablePlayerProducts(long userId, string entity, string valueToSearch);

    Task<CreatePlant> CreatePlayerPlant(long userId, string userName, PlantData plantData);

    Task<List<string>> CollectPlayerPlant(int id);
        
    Task<List<string>> RemovePlayerPlant(int id);

    Task<CompleteQuest> ProcessCompletedQuest(long userId, int questId, IObjectMapper objectMapper);

    Task<QuestInfoCreation> CreateQuestInfoCreationModel(Quest quest);
}