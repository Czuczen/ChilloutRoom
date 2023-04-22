using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;

public interface IPlantService : ITransientDependency
{
    Task<int> CreatePlayerPlant(CreatePlant createPlant, ExtendingModels.Models.General.District district, string userName,
        int plantationStorageId, List<Bonus> activeBonuses, ExtendingModels.Models.General.GeneratedType generatedType);
        
    Task DeleteConnectedPlantsToProductDefinitionAsync(int productId, string entityName);

    Task DeletePlantAndSetUseCountConnectedProductsAsync(string fieldToCompare, int value, bool needIgnoreChange = true);
}