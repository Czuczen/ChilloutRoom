using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition.SupportedTypes;

public class DeleteQuest : DeleteDefinition<Quest>, IDeleteDefinition<Quest>
{
    public DeleteQuest(
        IRepository<Quest> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IPlantService plantService
    ) 
        : base(repository, districtRepository, generatedTypeRepository, plantService)
    {
    }
}