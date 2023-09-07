using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Bonus".
/// </summary>
[AbpAuthorize]
public class BonusAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Bonus, BonusDto, PagedResultRequestDto, BonusCreateDto, BonusUpdateDto>,
    IBonusAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "Bonus".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "Bonus".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="definitionCreator">Klasa odpowiadająca za tworzenie encji "Bonus" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="definitionUpdater">Klasa odpowiadająca za aktualizację encji "Bonus" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="definitionDeleter">Klasa odpowiadająca za usuwanie encji "Bonus" u użytkowników na podstawie usuniętej definicji.</param>
    public BonusAppService(
        IRepository<ExtendingModels.Models.Products.Bonus, int> repository,
        IResponseBuilder<BonusDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<BonusCreateDto> definitionCreator,
        IUpdateDefinition<BonusUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Bonus> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
            
    }
}