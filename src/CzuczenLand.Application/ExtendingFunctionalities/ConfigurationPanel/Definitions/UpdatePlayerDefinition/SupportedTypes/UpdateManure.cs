using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Manure.
/// </summary>
public class UpdateManure : UpdateDefinition<Manure, ManureUpdateDto, ManureUpdateDefinitionDto>, IUpdateDefinition<ManureUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium nawozów.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdateManure(
        IRepository<Manure> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}