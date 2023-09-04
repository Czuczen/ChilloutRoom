using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;

/// <summary>
/// Interfejs abstrakcyjnego serwisu bazowego do obsługi operacji na encjach rozszerzających aspnetboilerplate. 
/// </summary>
/// <typeparam name="TCreateInput">Typ wejściowy dla operacji tworzenia encji.</typeparam>
/// <typeparam name="TUpdateInput">Typ wejściowy dla operacji aktualizacji encji.</typeparam>
public interface IEntityAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TUpdateInput : IEntityDto<int>
    where TCreateInput : class
{
    /// <summary>
    /// Wykonuje operację pobierania dostępnych rekordów.
    /// </summary>
    /// <returns>Odpowiedź zawierająca dostępne rekordy.</returns>
    Task<EntityAsyncCrudResponse> ActionGetAvailableRecords();

    /// <summary>
    /// Wykonuje operację tworzenia encji.
    /// </summary>
    /// <param name="input">Dane wejściowe dla operacji tworzenia encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji tworzenia encji.</returns>
    Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<TCreateInput> input);
        
    /// <summary>
    /// Wykonuje operację aktualizacji encji.
    /// </summary>
    /// <param name="input">Dane wejściowe dla operacji aktualizacji encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji encji.</returns>
    Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<TUpdateInput> input);

    /// <summary>
    /// Wykonuje operację aktualizacji wielu encji.
    /// </summary>
    /// <param name="request">Dane wejściowe dla operacji aktualizacji wielu encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji wielu encji.</returns>
    Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request);
        
    /// <summary>
    /// Wykonuje operację usuwania encji.
    /// </summary>
    /// <param name="objectId">Identyfikator encji do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usuwania encji.</returns>
    Task<EntityAsyncCrudResponse> ActionDelete(int objectId);
        
    /// <summary>
    /// Wykonuje operację usuwania wielu encji.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów encji do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usuwania wielu encji.</returns>
    Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids);
}