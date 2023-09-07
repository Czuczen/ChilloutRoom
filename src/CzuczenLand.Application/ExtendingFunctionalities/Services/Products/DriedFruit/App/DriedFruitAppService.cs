using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "DriedFruit".
/// </summary>
[AbpAuthorize]
public class DriedFruitAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.DriedFruit, DriedFruitDto, PagedResultRequestDto, DriedFruitCreateDto, DriedFruitUpdateDto>, 
    IDriedFruitAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "DriedFruit".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "DriedFruit".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "DriedFruit" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "DriedFruit" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "DriedFruit" u użytkowników na podstawie usuniętej definicji.</param>
    public DriedFruitAppService(
        IRepository<ExtendingModels.Models.Products.DriedFruit, int> repository,
        IResponseBuilder<DriedFruitDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<DriedFruitCreateDto> definitionCreator,
        IUpdateDefinition<DriedFruitUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.DriedFruit> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}