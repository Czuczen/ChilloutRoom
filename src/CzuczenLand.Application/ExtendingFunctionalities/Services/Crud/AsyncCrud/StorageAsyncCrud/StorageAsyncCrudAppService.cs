using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.StorageAsyncCrud;

public class StorageAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> 
    : EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>,
        IStorageAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TEntity : class, IUserStorageEntity
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    protected StorageAsyncCrudAppService(
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
        
        if (IsAdmin) 
            ret = ObjectMapper.Map<List<TEntityDto>>(await Repository.GetAllListAsync());
        
        if (IsDistrictWarden) 
            ret.Add(ObjectMapper.Map<TEntityDto>(
                (await Repository.GetAllListAsync(item => item.UserId == UserId)).SingleOrDefault()));
        
        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }

    public override async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
    {
        if (IsDistrictWarden) 
            return await ResponseBuilder.WithInfo("Nie możesz edytować wielu").Build(EntityAsyncCrudActions.ActionUpdateMany);

        return await base.ActionUpdateMany(request);
    }
        
    public override async Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids)
    {
        if (IsDistrictWarden) 
            return await ResponseBuilder.WithInfo("Nie możesz usuwać wielu").Build(EntityAsyncCrudActions.ActionDeleteMany);

        return await base.ActionDeleteMany(ids);
    }
}