using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;

public abstract class DeleteDefinition<TEntity>
    where TEntity : class, IPlantationGeneratedEntity
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IRepository<District> DistrictRepository;
    protected readonly IRepository<GeneratedType> GeneratedTypeRepository;
    protected readonly IPlantService PlantService;

    public IAbpSession AbpSession { get; set; }

    
    protected DeleteDefinition(
        IRepository<TEntity> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository, 
        IPlantService plantService
    )
    {
        AbpSession = NullAbpSession.Instance;
        Repository = repository;
        DistrictRepository = districtRepository;
        GeneratedTypeRepository = generatedTypeRepository;
        PlantService = plantService;
    }
        
    public async Task Delete(int objectId)
    {
        var entityName = typeof(TEntity).Name;
        var currRepo = CustomRepositoryFactory.GetRepository(entityName); 
        await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(new List<int>{objectId}, currRepo, entityName, PlantService);
    }
        
    public async Task Delete(List<int> ids)
    {
        var entityName = typeof(TEntity).Name;
        var currRepo = CustomRepositoryFactory.GetRepository(entityName);

        var isDeleteVisible = ids != null && ids.Count > 0;
        var isDeleteAll = ids == null || ids.Count == 0;
   
        if (isDeleteAll)
        {
            var wardenId = AbpSession.GetUserId();
            var wardenDistrict = await DistrictRepository.SingleAsync(item => item.UserId == wardenId);
            var itemsIds = await Repository.GetAll().Join(
                    GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == wardenDistrict.Id),
                    entity => entity.GeneratedTypeId,
                    generatedType => generatedType.Id,
                    (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null)
                .Select(item => item.Id).ToListAsync();

            await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(itemsIds, currRepo, entityName, PlantService);
        }
        else if (isDeleteVisible)
            await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(ids, currRepo, entityName, PlantService);
    }
}