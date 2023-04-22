using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;

public abstract class CreateDefinition<TEntity, TCreateDto>
    where TEntity : class, IPlantationGeneratedEntity
    where TCreateDto : class, IGeneratedEntity
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IRepository<GeneratedType> GeneratedTypeRepository;
    protected readonly IRepository<PlantationStorage> PlantationStorageRepository;


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

    protected async Task<List<PlantationStorage>> GetPlantationStoragesByGeneratedType(TCreateDto entity)
    {
        var generatedType = await GeneratedTypeRepository.GetAsync(entity.GeneratedTypeId);
        var districtPlantationStorages = await PlantationStorageRepository.GetAllListAsync(
            item => item.DistrictId == generatedType.DistrictId);

        return districtPlantationStorages;
    }

    protected Dictionary<string, object> GetAsDictionary(TCreateDto entity)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(entity));
    }
}