using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdateLamp : UpdateDefinition<Lamp, LampUpdateDto, LampUpdateDefinitionDto>, IUpdateDefinition<LampUpdateDto>
{
    public UpdateLamp(
        IRepository<Lamp> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}