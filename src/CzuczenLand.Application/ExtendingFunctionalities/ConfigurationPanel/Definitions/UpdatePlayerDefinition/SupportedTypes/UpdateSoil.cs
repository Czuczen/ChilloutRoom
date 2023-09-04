using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Soil.
/// </summary>
public class UpdateSoil : UpdateDefinition<Soil, SoilUpdateDto, SoilUpdateDefinitionDto>, IUpdateDefinition<SoilUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium gleb.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdateSoil(
        IRepository<Soil> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}