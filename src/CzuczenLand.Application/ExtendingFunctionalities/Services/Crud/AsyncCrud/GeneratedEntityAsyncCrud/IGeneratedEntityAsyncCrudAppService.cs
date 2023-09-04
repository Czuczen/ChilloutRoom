using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.GeneratedEntityAsyncCrud;

/// <summary>
/// Interfejs abstrakcyjnego serwisu bazowego do obsługi operacji na encjach generowanych użytkownikom na podstawie definicji. 
/// </summary>
/// <typeparam name="TCreateInput">Typ wejściowy dla operacji tworzenia encji.</typeparam>
/// <typeparam name="TUpdateInput">Typ wejściowy dla operacji aktualizacji encji.</typeparam>
public interface IGeneratedEntityAsyncCrudAppService<TCreateInput, TUpdateInput> : 
    IEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TUpdateInput : IEntityDto<int>
    where TCreateInput : class
{
}