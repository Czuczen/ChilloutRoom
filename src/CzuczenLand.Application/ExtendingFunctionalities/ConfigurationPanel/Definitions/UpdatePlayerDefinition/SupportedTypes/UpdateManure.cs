using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdateManure : UpdateDefinition<Manure, ManureUpdateDto, ManureUpdateDefinitionDto>, IUpdateDefinition<ManureUpdateDto>
{
    public UpdateManure(
        IRepository<Manure> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}