using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.StorageAsyncCrud;

public interface IStorageAsyncCrudAppService<TCreateInput, TUpdateInput> :
    IEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TUpdateInput : IEntityDto<int>
    where TCreateInput : class
{
}