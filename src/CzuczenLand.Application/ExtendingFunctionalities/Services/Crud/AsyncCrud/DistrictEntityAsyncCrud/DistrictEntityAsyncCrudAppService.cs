using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.DistrictEntityAsyncCrud;

public class DistrictEntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> 
    : EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>,
        IDistrictEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TEntity : class, IDistrictEntity
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    protected DistrictEntityAsyncCrudAppService(
        IRepository<TEntity, int> repository,
        IResponseBuilder<TEntityDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }
        
    public override async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        var ret = new List<TEntityDto>();
        await base.ActionGetAvailableRecords();
        
        if (IsDistrictWarden)
            ret = ObjectMapper.Map<List<TEntityDto>>(await Repository.GetAllListAsync(item => item.DistrictId == WardenDistrictId));
            
        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }

    public override async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
    {
        var isUpdateVisible = request.Ids != null && request.Ids.Count > 0;
        var isUpdateAll = request.Ids == null || request.Ids.Count == 0;
        
        if (isUpdateVisible)
        {
            foreach (var id in request.Ids)
            {
                var itemToUpdate = ObjectMapper.Map<TUpdateInput>(await GetEntityByIdAsync(id));
                var updatedItem = (TUpdateInput) UpdateDefinitionHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        else if (isUpdateAll)
        {
            var districtItemsToUpdate = ObjectMapper.Map<List<TUpdateInput>>(
                await Repository.GetAllListAsync(item => item.DistrictId == WardenDistrictId));
                
            foreach (var itemToUpdate in districtItemsToUpdate)
            {
                var updatedItem = (TUpdateInput) UpdateDefinitionHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionUpdateMany);
    }
        
    public override async Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids)
    {
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
            var districtItemsToDelete = ObjectMapper.Map<List<TUpdateInput>>(
                await Repository.GetAllListAsync(item => item.DistrictId == WardenDistrictId));
                
            foreach (var item in districtItemsToDelete)
            {
                await DeleteAsync(new EntityDto(item.Id));
                ResponseBuilder.AddItems(item.Id);
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionDeleteMany);
    }
}