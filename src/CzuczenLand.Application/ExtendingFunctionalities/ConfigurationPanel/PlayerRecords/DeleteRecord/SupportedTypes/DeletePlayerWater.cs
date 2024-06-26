﻿using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord.SupportedTypes;

/// <summary>
/// Klasa do usuwania obiektów typu Water.
/// </summary>
public class DeletePlayerWater : DeletePlayerRecord<Water>, IDeletePlayerRecord<Water>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium wód.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantService">Serwis roślin.</param>
    public DeletePlayerWater(
        IRepository<Water> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository, 
        IPlantService plantService
    ) 
        : base(repository, districtRepository, generatedTypeRepository, plantService)
    {
    }
}