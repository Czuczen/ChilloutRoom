using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Pot".
/// </summary>
[AbpAuthorize]
public class PotAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Pot, PotDto, PagedResultRequestDto, PotCreateDto, PotUpdateDto>, 
    IPotAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "Pot".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "Pot".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "Pot" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "Pot" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "Pot" u użytkowników na podstawie usuniętej definicji.</param>
    public PotAppService(
        IRepository<ExtendingModels.Models.Products.Pot, int> repository,
        IResponseBuilder<PotDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<PotCreateDto> definitionCreator,
        IUpdateDefinition<PotUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Pot> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}