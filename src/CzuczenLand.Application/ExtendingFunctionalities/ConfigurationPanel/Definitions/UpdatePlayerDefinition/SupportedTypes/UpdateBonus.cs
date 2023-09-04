using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Bonus.
/// </summary>
public class UpdateBonus : UpdateDefinition<Bonus, BonusUpdateDto, BonusUpdateDefinitionDto>, IUpdateDefinition<BonusUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium bonusów.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdateBonus(
        IRepository<Bonus> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
            
    }
}