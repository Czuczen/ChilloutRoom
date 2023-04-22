using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.GeneratedEntityAsyncCrud;

public abstract class GeneratedEntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> 
    : EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>,
        IGeneratedEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TEntity : class, IPlantationGeneratedEntity
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    protected readonly IRepository<GeneratedType> GeneratedTypeRepository;
    private readonly ICreateDefinition<TCreateInput> _definitionCreator;
    private readonly IUpdateDefinition<TUpdateInput> _definitionUpdater;
    private readonly IDeleteDefinition<TEntity> _definitionDeleter;

    
    protected GeneratedEntityAsyncCrudAppService(
        IRepository<TEntity, int> repository,
        IRepository<GeneratedType> generatedTypeRepository,
        IResponseBuilder<TEntityDto> responseBuilder,
        ICreateDefinition<TCreateInput> definitionCreator,
        IUpdateDefinition<TUpdateInput> definitionUpdater,
        IDeleteDefinition<TEntity> definitionDeleter
    ) 
        : base(repository, responseBuilder)
    {
        GeneratedTypeRepository = generatedTypeRepository;
        _definitionCreator = definitionCreator;
        _definitionUpdater = definitionUpdater;
        _definitionDeleter = definitionDeleter;
    }

    public override async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<TCreateInput> input)
    {
        var ret = await base.ActionCreate(input);
        await _definitionCreator.Create(input.Input);
        
        return ret;
    }

    public override async Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<TUpdateInput> input)
    {
        var ret = await base.ActionUpdate(input);
        await _definitionUpdater.Update(input.Input);
        
        return ret;
    }

    public override async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
    {
        var isUpdateVisible = request.Ids != null && request.Ids.Count > 0;
        var isUpdateAll = request.Ids == null || request.Ids.Count == 0;
        
        var items = await Repository.GetAll().Join(
            GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == WardenDistrictId),
            entity => entity.GeneratedTypeId,
            generatedType => generatedType.Id,
            (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null).ToListAsync();

        if (isUpdateVisible)
        {
            foreach (var id in request.Ids)
            {
                var itemToUpdate = ObjectMapper.Map<TUpdateInput>(items.Single(item => item.Id == id));
                var updatedItem = (TUpdateInput) UpdateDefinitionHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        else if (isUpdateAll)
        {
            foreach (var itemToUpdate in items)
            {
                var mappedItemToUpdate = ObjectMapper.Map<TUpdateInput>(itemToUpdate);
                var updatedItem = (TUpdateInput) UpdateDefinitionHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, mappedItemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }

        await _definitionUpdater.Update(request.FieldsToUpdate, request.Ids);
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionUpdateMany);
    }

    public override async Task<EntityAsyncCrudResponse> ActionDelete(int objectId)
    {
        await _definitionDeleter.Delete(objectId);
        return await base.ActionDelete(objectId);
    }

    public override async Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids)
    {
        await _definitionDeleter.Delete(ids);
            
        var isDeleteVisible = ids != null && ids.Count > 0;
        var isDeleteAll = ids == null || ids.Count == 0;
        
        if (isDeleteVisible)
        {
            foreach (var id in ids)
            {
                await DeleteAsync(new EntityDto(id));
                ResponseBuilder.AddItems(id);
            }
        }
        else if (isDeleteAll)
        {
            var itemsIds = await Repository.GetAll().Join(
                    GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == WardenDistrictId),
                    entity => entity.GeneratedTypeId,
                    generatedType => generatedType.Id,
                    (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null)
                .Select(item => item.Id).ToListAsync();

            foreach (var id in itemsIds)
            {
                await DeleteAsync(new EntityDto(id));
                ResponseBuilder.AddItems(id);
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionDeleteMany);
    }
}