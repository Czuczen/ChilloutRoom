using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.DistrictEntityAsyncCrud;

/// <summary>
/// Interfejs abstrakcyjnego serwisu bazowego do obsługi operacji na encjach przynależących do dzielnicy nie będących magazynem. 
/// </summary>
/// <typeparam name="TCreateInput">Typ wejściowy dla operacji tworzenia encji.</typeparam>
/// <typeparam name="TUpdateInput">Typ wejściowy dla operacji aktualizacji encji.</typeparam>
public interface IDistrictEntityAsyncCrudAppService<TCreateInput, TUpdateInput> :
    IEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TUpdateInput : IEntityDto<int>
    where TCreateInput : class
{
}