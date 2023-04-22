using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;

public interface IEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TUpdateInput : IEntityDto<int>
    where TCreateInput : class
{
    Task<EntityAsyncCrudResponse> ActionGetAvailableRecords();

    Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<TCreateInput> input);
        
    Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<TUpdateInput> input);

    Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request);
        
    Task<EntityAsyncCrudResponse> ActionDelete(int objectId);
        
    Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids);
}