﻿using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do aktualizacji obiektów typu DriedFruit.
/// </summary>
public class UpdateDriedFruit : UpdateDefinition<DriedFruit, DriedFruitUpdateDto, DriedFruitUpdateDefinitionDto>, IUpdateDefinition<DriedFruitUpdateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium suszów.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    public UpdateDriedFruit(
        IRepository<DriedFruit> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}