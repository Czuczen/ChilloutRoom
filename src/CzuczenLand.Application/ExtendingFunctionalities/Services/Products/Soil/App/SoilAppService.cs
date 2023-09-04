using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Soil".
/// </summary>
[AbpAuthorize]
public class SoilAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Soil, SoilDto, PagedResultRequestDto, SoilCreateDto, SoilUpdateDto>, 
    ISoilAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "Soil".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "Soil".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "Soil" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "Soil" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "Soil" u użytkowników na podstawie usuniętej definicji.</param>
    public SoilAppService(
        IRepository<ExtendingModels.Models.Products.Soil, int> repository,
        IResponseBuilder<SoilDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<SoilCreateDto> definitionCreator,
        IUpdateDefinition<SoilUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Soil> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
  
    }
}