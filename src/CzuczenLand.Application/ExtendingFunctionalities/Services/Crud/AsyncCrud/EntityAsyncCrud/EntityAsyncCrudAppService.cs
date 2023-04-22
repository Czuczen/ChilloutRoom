using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using CzuczenLand.Authorization;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;

public abstract class EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> :
    AsyncCrudAppService<TEntity, TEntityDto, int, TGetAllInput, TCreateInput, TUpdateInput>,
    IEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<int>
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : IEntityDto<int>
    where TCreateInput : class
{
    private bool? _isAdmin;
    private bool? _isDistrictWarden;
    protected const string NotCreatedEntity = "CanNotBeCreated";
        
    protected sealed override string GetPermissionName         
    {
        get => base.GetPermissionName;
        set => base.GetPermissionName = value;
    }
        
    protected sealed override string GetAllPermissionName         
    {
        get => base.GetAllPermissionName;
        set => base.GetAllPermissionName = value;
    }
        
    protected sealed override string CreatePermissionName
    {
        get => base.CreatePermissionName;
        set => base.CreatePermissionName = value;
    }
        
    protected sealed override string UpdatePermissionName         
    {
        get => base.UpdatePermissionName;
        set => base.UpdatePermissionName = value;
    }
        
    protected sealed override string DeletePermissionName        
    {
        get => base.DeletePermissionName;
        set => base.DeletePermissionName = value;
    }

    protected long UserId => ResponseBuilder.UserId;
        
    protected int? WardenDistrictId => ResponseBuilder.DistrictWardenId;

    protected bool IsAdmin => _isAdmin ?? (_isAdmin = IsGranted(PermissionNames.Crud_Admin)) ?? false;
        
    protected bool IsDistrictWarden
    {
        get
        {
            _isDistrictWarden ??= IsGranted(PermissionNames.Crud_DistrictWarden);
        
            if (IsAdmin)
                _isDistrictWarden = false;

            return (bool) _isDistrictWarden;
        }
    }

    protected IResponseBuilder<TEntityDto> ResponseBuilder { get; }

        
    protected EntityAsyncCrudAppService(
        IRepository<TEntity, int> repository, 
        IResponseBuilder<TEntityDto> responseBuilder
    )
        : base(repository)
    {
        ResponseBuilder = responseBuilder.WithDistrictWardenId();
    }
    
    public virtual async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        ResponseBuilder.WithCanCreate(CreatePermissionName != NotCreatedEntity);
        return await Task.FromResult<EntityAsyncCrudResponse>(null);
    }

    public virtual async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<TCreateInput> input) => 
        await ResponseBuilder.AddItems(await CreateAsync(input.Input)).Build(EntityAsyncCrudActions.ActionCreate);
        
    public virtual async Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<TUpdateInput> input) => 
        await ResponseBuilder.AddItems(await UpdateAsync(input.Input)).Build(EntityAsyncCrudActions.ActionUpdate);

    public virtual async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
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
            var allItems = ObjectMapper.Map<List<TUpdateInput>>(await Repository.GetAllListAsync());
            foreach (var itemToUpdate in allItems)
            {
                var updatedItem = (TUpdateInput) UpdateDefinitionHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionUpdateMany);
    }

    public virtual async Task<EntityAsyncCrudResponse> ActionDelete(int objectId) 
    {
        await DeleteAsync(new EntityDto(objectId));
        return await ResponseBuilder.AddItems(objectId).Build(EntityAsyncCrudActions.ActionDelete);
    }
        
    public virtual async Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids)
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
            var allItems = await Repository.GetAllListAsync();
            foreach (var item in allItems)
            {
                await DeleteAsync(new EntityDto(item.Id));
                ResponseBuilder.AddItems(item.Id);
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionDeleteMany);
    }
}