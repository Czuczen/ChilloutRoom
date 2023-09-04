using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu Seed.
/// </summary>
public class UpdateSeed : UpdateDefinition<Seed, SeedUpdateDto, SeedUpdateDefinitionDto>, IUpdateDefinition<SeedUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium nasion.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdateSeed(
        IRepository<Seed> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}