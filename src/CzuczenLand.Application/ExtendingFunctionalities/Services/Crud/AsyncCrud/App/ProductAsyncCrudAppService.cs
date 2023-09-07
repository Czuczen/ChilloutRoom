using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;

/// <summary>
/// Abstrakcyjna klasa bazowa do obsługi operacji na produktach.
/// </summary>
/// <typeparam name="TProduct">Typ produktu.</typeparam>
/// <typeparam name="TEntityDto">Typ DTO encji.</typeparam>
/// <typeparam name="TGetAllInput">Typ danych wejściowych dla operacji pobierania rekordów.</typeparam>
/// <typeparam name="TCreateInput">Typ danych wejściowych dla operacji tworzenia rekordu.</typeparam>
/// <typeparam name="TUpdateInput">Typ danych wejściowych dla operacji aktualizacji rekordu.</typeparam>
public abstract class ProductAsyncCrudAppService<TProduct, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> : 
    GeneratedEntityAsyncCrudAppService<TProduct, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>
    where TProduct : Product, IEntity<int>
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium produktu.</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi na zapytania.</param>
    /// <param name="generatedTypeRepository">Repozytorium typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "Quest" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "Quest" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "Quest" u użytkowników na podstawie usuniętej definicji.</param>
    protected ProductAsyncCrudAppService(
        IRepository<TProduct, int> repository, 
        IResponseBuilder<TEntityDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<TCreateInput> definitionCreator,
        IUpdateDefinition<TUpdateInput> definitionUpdater,
        IDeleteDefinition<TProduct> definitionDeleter
    )
        : base(repository, generatedTypeRepository, responseBuilder, definitionCreator, definitionUpdater, definitionDeleter)
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
        
        if (IsDistrictWarden)
        {
            ret = ObjectMapper.Map<List<TEntityDto>>(await Repository.GetAll().Join(
                GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == WardenDistrictId),
                entity => entity.GeneratedTypeId,
                generatedType => generatedType.Id,
                (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null).ToListAsync());
        }

        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }
}