using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;

public interface IQuestService : ITransientDependency
{
    Task SetQuestDependencies(int? entityId, Dictionary<string, List<int>> entitiesIds);
        
    Task UpdateQuestDependencies(int? entityId, Dictionary<string, List<int>> entitiesIds);

    Task SetStartValuesAsync(List<ExtendingModels.Models.General.Quest> playerDefinitions);
        
    void SetStartValues(List<ExtendingModels.Models.General.Quest> playerDefinitions);

    Task CreatePlayerQuestsRequirementsProgress(List<ExtendingModels.Models.General.Quest> questsDefinitions,
        List<ExtendingModels.Models.General.Quest> playerDefinitions);
}