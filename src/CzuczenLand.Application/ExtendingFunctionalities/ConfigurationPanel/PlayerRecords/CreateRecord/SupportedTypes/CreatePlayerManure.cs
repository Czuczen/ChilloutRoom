﻿using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord.SupportedTypes;

/// <summary>
/// Klasa do tworzenia obiektów typu Manure.
/// </summary>
public class CreatePlayerManure : CreatePlayerRecord<Manure, ManureCreateDto>, ICreatePlayerRecord<ManureCreateDto>
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium nawozów.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    public CreatePlayerManure(
        IRepository<Manure> repository, 
        IRepository<GeneratedType> generatedTypeRepository, 
        IRepository<PlantationStorage> plantationStorageRepository
    ) 
        : base(repository, generatedTypeRepository, plantationStorageRepository)
    {
    }
}