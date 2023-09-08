using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using CzuczenLand.Authorization;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;

/// <summary>
/// Abstrakcyjna klasa bazowa do obsługi operacji na encjach rozszerzających aspnetboilerplate.
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być obsługiwany.</typeparam>
/// <typeparam name="TEntityDto">Typ DTO encji.</typeparam>
/// <typeparam name="TGetAllInput">Typ danych wejściowych dla operacji pobierania rekordów.</typeparam>
/// <typeparam name="TCreateInput">Typ danych wejściowych dla operacji tworzenia rekordu.</typeparam>
/// <typeparam name="TUpdateInput">Typ danych wejściowych dla operacji aktualizacji rekordu.</typeparam>
public abstract class EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> :
    AsyncCrudAppService<TEntity, TEntityDto, int, TGetAllInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<int>
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    /// <summary>
    /// Flaga wskazująca, czy użytkownik ma uprawnienia administratora.
    /// </summary>
    private bool? _isAdmin;
    
    /// <summary>
    /// Flaga wskazująca, czy użytkownik ma uprawnienia opiekuna dzielnicy.
    /// </summary>
    private bool? _isDistrictWarden;
    
    /// <summary>
    /// Stała reprezentująca wiadomość o niemożliwości utworzenia encji. 
    /// </summary>
    protected const string NotCreatedEntity = "CanNotBeCreated";
        
    
    /// <summary>
    /// Nazwa uprawnienia do pobierania pojedynczej encji.
    /// </summary>
    protected sealed override string GetPermissionName         
    {
        get => base.GetPermissionName;
        set => base.GetPermissionName = value;
    }
        
    /// <summary>
    /// Nazwa uprawnienia do pobierania wszystkich encji.
    /// </summary>
    protected sealed override string GetAllPermissionName         
    {
        get => base.GetAllPermissionName;
        set => base.GetAllPermissionName = value;
    }
    
    /// <summary>
    /// Nazwa uprawnienia do tworzenia encji.
    /// </summary>
    protected sealed override string CreatePermissionName
    {
        get => base.CreatePermissionName;
        set => base.CreatePermissionName = value;
    }
        
    /// <summary>
    /// Nazwa uprawnienia do aktualizacji encji.
    /// </summary>
    protected sealed override string UpdatePermissionName         
    {
        get => base.UpdatePermissionName;
        set => base.UpdatePermissionName = value;
    }
        
    /// <summary>
    /// Nazwa uprawnienia do usuwania encji.
    /// </summary>
    protected sealed override string DeletePermissionName        
    {
        get => base.DeletePermissionName;
        set => base.DeletePermissionName = value;
    }

    
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    protected long UserId => ResponseBuilder.UserId;
        
    /// <summary>
    /// Identyfikator dzielnicy opiekuna.
    /// </summary>
    protected int? WardenDistrictId => ResponseBuilder.WardenDistrictId;

    /// <summary>
    /// Określa czy użytkownik ma uprawnienia administratora.
    /// </summary>
    protected bool IsAdmin => _isAdmin ?? (_isAdmin = IsGranted(PermissionNames.Crud_Admin)) ?? false;

    /// <summary>
    /// Określa czy użytkownik ma uprawnienia opiekuna dzielnicy.
    /// </summary>
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

    /// <summary>
    /// Klasa budująca odpowiedzi na zapytania.
    /// </summary>
    protected IResponseBuilder<TEntityDto> ResponseBuilder { get; }

        
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji.</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi na zapytania.</param>
    protected EntityAsyncCrudAppService(
        IRepository<TEntity, int> repository, 
        IResponseBuilder<TEntityDto> responseBuilder
    )
        : base(repository)
    {
        ResponseBuilder = responseBuilder.WithWardenDistrictId();
    }
    
    /// <summary>
    /// Wykonuje operację pobierania dostępnych rekordów.
    /// </summary>
    /// <returns>Odpowiedź zawierająca dostępne rekordy.</returns>
    public virtual async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        ResponseBuilder.WithCanCreate(CreatePermissionName != NotCreatedEntity);
        return await Task.FromResult<EntityAsyncCrudResponse>(null);
    }

    /// <summary>
    /// Wykonuje operację tworzenia encji.
    /// </summary>
    /// <param name="input">Dane wejściowe dla operacji tworzenia encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji tworzenia encji.</returns>
    public virtual async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<TCreateInput> input) => 
        await ResponseBuilder.AddItems(await CreateAsync(input.Input)).Build(EntityAsyncCrudActions.ActionCreate);
        
    /// <summary>
    /// Wykonuje operację aktualizacji encji.
    /// </summary>
    /// <param name="input">Dane wejściowe dla operacji aktualizacji encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji encji.</returns>
    public virtual async Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<TUpdateInput> input) => 
        await ResponseBuilder.AddItems(await UpdateAsync(input.Input)).Build(EntityAsyncCrudActions.ActionUpdate);

    /// <summary>
    /// Wykonuje operację aktualizacji wielu encji.
    /// </summary>
    /// <param name="request">Dane wejściowe dla operacji aktualizacji wielu encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji wielu encji.</returns>
    public virtual async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
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
            var allItems = ObjectMapper.Map<List<TUpdateInput>>(await Repository.GetAllListAsync());
            foreach (var itemToUpdate in allItems)
            {
                var updatedItem = (TUpdateInput) AsyncCrudHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionUpdateMany);
    }

    /// <summary>
    /// Wykonuje operację usuwania encji.
    /// </summary>
    /// <param name="objectId">Identyfikator encji do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usuwania encji.</returns>
    public virtual async Task<EntityAsyncCrudResponse> ActionDelete(int objectId) 
    {
        await DeleteAsync(new EntityDto(objectId));
        return await ResponseBuilder.AddItems(objectId).Build(EntityAsyncCrudActions.ActionDelete);
    }
       
    /// <summary>
    /// Wykonuje operację usuwania wielu encji.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów encji do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usuwania wielu encji.</returns>
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