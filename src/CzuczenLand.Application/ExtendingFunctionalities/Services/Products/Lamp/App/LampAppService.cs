using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Lamp".
/// </summary>
[AbpAuthorize]
public class LampAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Lamp, LampDto, PagedResultRequestDto, LampCreateDto, LampUpdateDto>, 
    ILampAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "Lamp".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "Lamp".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "Lamp" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "Lamp" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "Lamp" u użytkowników na podstawie usuniętej definicji.</param>
    public LampAppService(
        IRepository<ExtendingModels.Models.Products.Lamp, int> repository,
        IResponseBuilder<LampDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<LampCreateDto> definitionCreator,
        IUpdateDefinition<LampUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Lamp> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}