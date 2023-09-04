using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;

/// <summary>
/// Klasa pomocnicza do usuwania rekordów graczy generowanych na podstawie definicji.
/// </summary>
public static class DeleteDefinitionHelper
{
    /// <summary>
    /// Usuwa wszystkie rekordy graczy na podstawie usuwanych definicji.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów usuwanych definicji.</param>
    /// <param name="repo">Repozytorium niestandardowe.</param>
    /// <param name="entityName">Nazwa encji.</param>
    /// <param name="plantService">Serwis roślin.</param>
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