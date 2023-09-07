using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;

/// <summary>
/// Abstrakcyjna klasa bazowa do obsługi operacji na encjach generowanych użytkownikom na podstawie definicji.
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być obsługiwany.</typeparam>
/// <typeparam name="TEntityDto">Typ DTO encji.</typeparam>
/// <typeparam name="TGetAllInput">Typ danych wejściowych dla operacji pobierania rekordów.</typeparam>
/// <typeparam name="TCreateInput">Typ danych wejściowych dla operacji tworzenia rekordu.</typeparam>
/// <typeparam name="TUpdateInput">Typ danych wejściowych dla operacji aktualizacji rekordu.</typeparam>
public abstract class GeneratedEntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> : 
    EntityAsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>
    where TEntity : class, IPlantationGeneratedEntity
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    /// <summary>
    /// Repozytorium typu generowanego.
    /// </summary>
    protected readonly IRepository<GeneratedType> GeneratedTypeRepository;
    
    /// <summary>
    /// Klasa odpowiadająca za tworzenie encji "Quest" dla użytkowników na podstawie stworzonej definicji.
    /// </summary>
    private readonly ICreateDefinition<TCreateInput> _definitionCreator;
    
    /// <summary>
    /// Klasa odpowiadająca za aktualizację encji "Quest" u użytkowników na podstawie aktualizowanej definicji.
    /// </summary>
    private readonly IUpdateDefinition<TUpdateInput> _definitionUpdater;
    
    /// <summary>
    /// Klasa odpowiadająca za usuwanie encji "Quest" u użytkowników na podstawie usuniętej definicji.
    /// </summary>
    private readonly IDeleteDefinition<TEntity> _definitionDeleter;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji.</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi na zapytania.</param>
    /// <param name="generatedTypeRepository">Repozytorium typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "Quest" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "Quest" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "Quest" u użytkowników na podstawie usuniętej definicji.</param>
    protected GeneratedEntityAsyncCrudAppService(
        IRepository<TEntity, int> repository,
        IRepository<GeneratedType> generatedTypeRepository,
        IResponseBuilder<TEntityDto> responseBuilder,
        ICreateDefinition<TCreateInput> definitionCreator,
        IUpdateDefinition<TUpdateInput> definitionUpdater,
        IDeleteDefinition<TEntity> definitionDeleter
    ) 
        : base(repository, responseBuilder)
    {
        GeneratedTypeRepository = generatedTypeRepository;
        _definitionCreator = definitionCreator;
        _definitionUpdater = definitionUpdater;
        _definitionDeleter = definitionDeleter;
    }

    /// <summary>
    /// Wykonuje operację tworzenia encji będącej definicją oraz na jej podstawie tworzy rekordy użytkownikom.
    /// </summary>
    /// <param name="input">Dane wejściowe dla operacji tworzenia.</param>
    /// <returns>Odpowiedź z wynikiem akcji tworzenia encji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<TCreateInput> input)
    {
        var ret = await base.ActionCreate(input);
        await _definitionCreator.Create(input.Input);
        
        return ret;
    }

    /// <summary>
    /// Wykonuje operację aktualizacji encji będącej definicją oraz na jej podstawie aktualizuje rekordy użytkownikom.
    /// </summary>
    /// <param name="input">Dane wejściowe dla operacji aktualizacji.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji encji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<TUpdateInput> input)
    {
        var ret = await base.ActionUpdate(input);
        await _definitionUpdater.Update(input.Input);
        
        return ret;
    }

    /// <summary>
    /// Wykonuje operację aktualizacji wielu encji będących definicją oraz na ich podstawie aktualizuje rekordy użytkownikom.
    /// </summary>
    /// <param name="request">Dane wejściowe dla operacji aktualizacji wielu encji.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji wielu encji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
    {
        var isUpdateVisible = request.Ids != null && request.Ids.Count > 0;
        var isUpdateAll = request.Ids == null || request.Ids.Count == 0;
        
        var items = await Repository.GetAll().Join(
            GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == WardenDistrictId),
            entity => entity.GeneratedTypeId,
            generatedType => generatedType.Id,
            (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null).ToListAsync();

        if (isUpdateVisible)
        {
            foreach (var id in request.Ids)
            {
                var itemToUpdate = ObjectMapper.Map<TUpdateInput>(items.Single(item => item.Id == id));
                var updatedItem = (TUpdateInput) UpdateDefinitionHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, itemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }
        else if (isUpdateAll)
        {
            foreach (var itemToUpdate in items)
            {
                var mappedItemToUpdate = ObjectMapper.Map<TUpdateInput>(itemToUpdate);
                var updatedItem = (TUpdateInput) UpdateDefinitionHelper.UpdateObject<TUpdateInput>(request.FieldsToUpdate, mappedItemToUpdate);
                ResponseBuilder.AddItems(await UpdateAsync(updatedItem));
            }
        }

        await _definitionUpdater.Update(request.FieldsToUpdate, request.Ids);
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionUpdateMany);
    }

    /// <summary>
    /// Wykonuje operację usuwania encji będącej definicją oraz na jej podstawie usuwa rekordy użytkownikom.
    /// </summary>
    /// <param name="objectId">Identyfikator encji do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usuwania encji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionDelete(int objectId)
    {
        await _definitionDeleter.Delete(objectId);
        return await base.ActionDelete(objectId);
    }

    /// <summary>
    /// Wykonuje operację usuwania wielu encji będących definicją oraz na ich podstawie usuwa rekordy użytkownikom.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów encji do usunięcia.</param>
    /// <returns>Odpowiedź z wynikiem akcji usuwania wielu encji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids)
    {
        await _definitionDeleter.Delete(ids);
            
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
            var itemsIds = await Repository.GetAll().Join(
                    GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == WardenDistrictId),
                    entity => entity.GeneratedTypeId,
                    generatedType => generatedType.Id,
                    (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null)
                .Select(item => item.Id).ToListAsync();

            foreach (var id in itemsIds)
            {
                await DeleteAsync(new EntityDto(id));
                ResponseBuilder.AddItems(id);
            }
        }
        
        return await ResponseBuilder.Build(EntityAsyncCrudActions.ActionDeleteMany);
    }
}