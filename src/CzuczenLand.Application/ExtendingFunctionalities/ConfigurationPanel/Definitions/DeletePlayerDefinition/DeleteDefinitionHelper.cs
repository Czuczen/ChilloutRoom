using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;

public static class DeleteDefinitionHelper
{
    public static async Task DeleteAllDefinitionsAsync(List<int> ids, ICustomRepository repo, string  entityName, IPlantService plantService)
    {
        if (ids == null) return;
        
        foreach (var recordId in ids)
        {
            var deletingDefinition  = (IGeneratedEntity) await repo.GetAsync(recordId);
            var filtered =
                (await repo.GetWhereAsync(RelationFieldsNames.GeneratedTypeId, deletingDefinition.GeneratedTypeId))
                .Where(item => item.Id != recordId);
                
            foreach (var item in filtered)
            {
                if (entityName != EntitiesDbNames.DriedFruit && entityName != EntitiesDbNames.Bonus && entityName != EntitiesDbNames.Quest)
                    await plantService.DeleteConnectedPlantsToProductDefinitionAsync(item.Id, entityName);

                await repo.DeleteAsync(item.Id);
            }
        }
    }
}