﻿using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition.SupportedTypes;

/// <summary>
/// Klasa do usuwania obiektów typu Lamp.
/// </summary>
public class DeleteLamp : DeleteDefinition<Lamp>, IDeleteDefinition<Lamp>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium lamp.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantService">Serwis roślin.</param>
    public DeleteLamp(
        IRepository<Lamp> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IPlantService plantService
    ) 
        : base(repository, districtRepository, generatedTypeRepository, plantService)
    {
    }
}