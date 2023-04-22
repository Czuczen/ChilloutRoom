using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition.SupportedTypes;

public class DeleteDriedFruit : DeleteDefinition<DriedFruit>, IDeleteDefinition<DriedFruit>
{
    public DeleteDriedFruit(
        IRepository<DriedFruit> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IPlantService plantService
    ) 
        : base(repository, districtRepository, generatedTypeRepository, plantService)
    {
    }
}