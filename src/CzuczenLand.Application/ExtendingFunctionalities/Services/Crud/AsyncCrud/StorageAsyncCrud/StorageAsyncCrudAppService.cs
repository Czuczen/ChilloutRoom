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

/// <summary>
/// Abstrakcyjna klasa bazowa do obsługi operacji na magazynach.
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być obsługiwany.</typeparam>
/// <typeparam name="TEntityDto">Typ DTO encji.</typeparam>
/// <typeparam name="TGetAllInput">Typ danych wejściowych dla operacji pobierania rekordów.</typeparam>
/// <typeparam name="TCreateInput">Typ danych wejściowych dla operacji tworzenia rekordu.</typeparam>
/// <typeparam name="TUpdateInput">Typ danych wejściowych dla operacji aktualizacji rekordu.</typeparam>
public class StorageAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> 
    : EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>,
        IStorageAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TEntity : class, IUserStorageEntity
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji.</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi na zapytania.</param>
    protected StorageAsyncCrudAppService(
        IRepository<TEntity, int> repository,
        IResponseBuilder<TEntityDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }
      
    /// <summary>
    /// Wykonuje operację pobrania dostępnych rekordów.
    /// </summary>
    /// <returns>Odpowiedź zawierająca dostępne rekordy.</returns>
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

    /// <summary>
    /// Wykonuje operację aktualizacji wielu rekordów.
    /// </summary>
    /// <param name="request">Dane wejściowe z informacjami o aktualizacji wielu rekordów.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji wielu rekordów.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
    {
        if (IsDistrictWarden) 
            return await ResponseBuilder.WithInfo("Nie możesz edytować wielu").Build(EntityAsyncCrudActions.ActionUpdateMany);

        return await base.ActionUpdateMany(request);
    }
    
    /// <summary>
    /// Wykonuje operację usunięcia wielu rekordów.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów rekordów do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usunięcia wielu rekordów.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids)
    {
        if (IsDistrictWarden) 
            return await ResponseBuilder.WithInfo("Nie możesz usuwać wielu").Build(EntityAsyncCrudActions.ActionDeleteMany);

        return await base.ActionDeleteMany(ids);
    }
}