using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdateBonus : UpdateDefinition<Bonus, BonusUpdateDto, BonusUpdateDefinitionDto>, IUpdateDefinition<BonusUpdateDto>
{
    public UpdateBonus(
        IRepository<Bonus> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
            
    }
}