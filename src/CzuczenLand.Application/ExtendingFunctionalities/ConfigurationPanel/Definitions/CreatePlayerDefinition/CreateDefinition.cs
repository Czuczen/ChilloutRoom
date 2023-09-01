using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;

/// <summary>
/// Bazowa klasa abstrakcyjna do tworzenia rekordów graczy generowanych na podstawie definicji.
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być tworzony.</typeparam>
/// <typeparam name="TCreateDto">Typ DTO używany do tworzenia encji.</typeparam>
public abstract class CreateDefinition<TEntity, TCreateDto>
    where TEntity : class, IPlantationGeneratedEntity
    where TCreateDto : class, IGeneratedEntity
{
    /// <summary>
    /// Repozytorium encji generowanej.
    /// </summary>
    protected readonly IRepository<TEntity> Repository;
    
    /// <summary>
    /// Repozytorium typów generowanych.
    /// </summary>
    protected readonly IRepository<GeneratedType> GeneratedTypeRepository;
    
    /// <summary>
    /// Repozytorium magazynów plantacji.
    /// </summary>
    protected readonly IRepository<PlantationStorage> PlantationStorageRepository;


    /// <summary>
    /// Konstruktor klasy bazowej dla definicji tworzenia encji.
    /// </summary>
    /// <param name="repository">Repozytorium encji generowanej.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    protected CreateDefinition(
        IRepository<TEntity> repository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<PlantationStorage> plantationStorageRepository
    )
    {
        Repository = repository;
        GeneratedTypeRepository = generatedTypeRepository;
        PlantationStorageRepository = plantationStorageRepository;
    }

    /// <summary>
    /// Tworzy rekordy graczy na podstawie utworzonej definicji.
    /// </summary>
    /// <param name="entity">DTO utworzonej definicji.</param>
    public virtual async Task Create(TCreateDto entity)
    {
        var districtPlantationStorages = await GetPlantationStoragesByGeneratedType(entity);
        var newObjectAsDict = GetAsDictionary(entity);
        
        foreach (var storage in districtPlantationStorages)
        {
            newObjectAsDict[RelationFieldsNames.PlantationStorageId] = storage.Id;
            var mappedObject =  JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(newObjectAsDict));
            await Repository.InsertAndGetIdAsync(mappedObject);
        }
    }

    /// <summary>
    /// Pobiera magazyny plantacji należące do tej samej dzielnicy co utworzona encja.
    /// </summary>
    /// <param name="entity">DTO utworzonej encji.</param>
    protected async Task<List<PlantationStorage>> GetPlantationStoragesByGeneratedType(TCreateDto entity)
    {
        var generatedType = await GeneratedTypeRepository.GetAsync(entity.GeneratedTypeId);
        var districtPlantationStorages = await PlantationStorageRepository.GetAllListAsync(
            item => item.DistrictId == generatedType.DistrictId);

        return districtPlantationStorages;
    }

    /// <summary>
    /// Konwertuje DTO utworzonej encji na słownik.
    /// </summary>
    protected Dictionary<string, object> GetAsDictionary(TCreateDto entity)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(entity));
    }
}