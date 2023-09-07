using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Water".
/// </summary>
[AbpAuthorize]
public class WaterAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Water, WaterDto, PagedResultRequestDto, WaterCreateDto, WaterUpdateDto>, 
    IWaterAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "Water".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "Water".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "Water" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "Water" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "Water" u użytkowników na podstawie usuniętej definicji.</param>
    public WaterAppService(
        IRepository<ExtendingModels.Models.Products.Water, int> repository,
        IResponseBuilder<WaterDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<WaterCreateDto> definitionCreator,
        IUpdateDefinition<WaterUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Water> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}