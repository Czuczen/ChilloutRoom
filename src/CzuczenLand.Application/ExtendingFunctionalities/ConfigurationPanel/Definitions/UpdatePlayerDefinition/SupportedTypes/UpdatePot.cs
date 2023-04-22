using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdatePot : UpdateDefinition<Pot, PotUpdateDto, PotUpdateDefinitionDto>, IUpdateDefinition<PotUpdateDto>
{
    public UpdatePot(
        IRepository<Pot> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}