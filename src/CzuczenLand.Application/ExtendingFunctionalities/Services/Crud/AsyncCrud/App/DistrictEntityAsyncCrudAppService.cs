using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;

/// <summary>
/// Abstrakcyjna klasa bazowa do obsługi operacji na encjach przynależących do dzielnicy nie będących magazynem.
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być obsługiwany.</typeparam>
/// <typeparam name="TEntityDto">Typ DTO encji.</typeparam>
/// <typeparam name="TGetAllInput">Typ danych wejściowych dla operacji pobierania rekordów.</typeparam>
/// <typeparam name="TCreateInput">Typ danych wejściowych dla operacji tworzenia rekordu.</typeparam>
/// <typeparam name="TUpdateInput">Typ danych wejściowych dla operacji aktualizacji rekordu.</typeparam>
public abstract class DistrictEntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> : 
    EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>
    where TEntity : class, IDistrictEntity
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji.</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi na zapytania.</param>
    protected DistrictEntityAsyncCrudAppService(
        IRepository<TEntity, int> repository,
        IResponseBuilder<TEntityDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }
        
    /// <summary>
    /// Wykonuje operację pobierania dostępnych rekordów.
    /// </summary>
    /// <returns>Odpowiedź zawierająca dostępne rekordy.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        var ret = new List<TEntityDto>();
        await base.ActionGetAvailableRecords();
        
        if (IsDistrictWarden)
            ret = ObjectMapper.Map<List<TEntityDto>>(await Repository.GetAllListAsync(item => item.DistrictId == WardenDistrictId));
            
        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }

    /// <summary>
    /// Wykonuje operację aktualizacji wielu encji.
    /// </summary>
    /// <param name="request">Dane wejściowe dla operacji aktualizacji wielu encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji wielu encji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
    {
        var isUpdateVisible = request.Ids != null && request.Ids.Count > 0;
        var isUpdateAll = request.Ids == null || request.Ids.Count == 0;
        
        if (isUpdateVisible)
        {
            foreach (var id in request.Ids)
            {
                var itemToUpdate = ObjectMapper.Map<TUpdateInput>(await GetEntityByIdAsync(id));
                var updatedItem = (TUpdateInput) AsyncCrudHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        else if (isUpdateAll)
        {
            var districtItemsToUpdate = ObjectMapper.Map<List<TUpdateInput>>(
                await Repository.GetAllListAsync(item => item.DistrictId == WardenDistrictId));
                
            foreach (var itemToUpdate in districtItemsToUpdate)
            {
                var updatedItem = (TUpdateInput) AsyncCrudHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionUpdateMany);
    }

    /// <summary>
    /// Wykonuje operację usuwania wielu encji.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów encji do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usuwania wielu encji.</returns>
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