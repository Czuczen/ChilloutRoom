using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;

public abstract class UpdateDefinition<TEntity, TUpdateDto, TUpdateDefinitionDto>
    where TEntity : class, IPlantationGeneratedEntity
    where TUpdateDto : class, IEntityDto<int>
    where TUpdateDefinitionDto : class
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IRepository<District> DistrictRepository;
    protected readonly IRepository<GeneratedType> GeneratedTypeRepository;

    public IAbpSession AbpSession { get; set; }

    
    protected UpdateDefinition(
        IRepository<TEntity> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    )
    {
        AbpSession = NullAbpSession.Instance;
        Repository = repository;
        DistrictRepository = districtRepository;
        GeneratedTypeRepository = generatedTypeRepository;
    }
        
    public async Task Update(TUpdateDto entity)
    {
        var fullEntity = await Repository.GetAsync(entity.Id);
        var items = await Repository.GetAllListAsync(item =>
            item.GeneratedTypeId == fullEntity.GeneratedTypeId && item.Id != fullEntity.Id);
        
        foreach (var item in items)
            UpdateDefinitionHelper.UpdateObject<TUpdateDefinitionDto>(entity, item);
    }

    public async Task Update(object updatedFields, List<int> ids)
    {
        if (updatedFields == null) return;

        var isUpdateVisible = ids != null && ids.Count > 0;
        var isUpdateAll = ids == null || ids.Count == 0;
 
        var wardenId = AbpSession.GetUserId();
        var wardenDistrict = await DistrictRepository.SingleAsync(item => item.UserId == wardenId);
        var items = await Repository.GetAll().Join(
            GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == wardenDistrict.Id),
            entity => entity.GeneratedTypeId,
            generatedType => generatedType.Id,
            (entity, generatedType) => entity).ToListAsync();
        
        if (isUpdateAll)
            foreach (var item in items.Where(item => item.PlantationStorageId != null))
                UpdateDefinitionHelper.UpdateObject<TUpdateDefinitionDto>(updatedFields, item);
        else if (isUpdateVisible)
            foreach (var id in ids)
            {
                var currItem = items.Single(item => item.Id == id);
                var playersDefinitionsToUpdate = items.Where(item =>
                    item.GeneratedTypeId == currItem.GeneratedTypeId && item.Id != currItem.Id);

                foreach (var item in playersDefinitionsToUpdate)
                    UpdateDefinitionHelper.UpdateObject<TUpdateDefinitionDto>(updatedFields, item);
            }
    }
}