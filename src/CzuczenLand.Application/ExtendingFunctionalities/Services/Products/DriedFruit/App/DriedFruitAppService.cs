using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
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
    /// <param name="playerRecordCreator">Klasa odpowiadająca za tworzenie encji "DriedFruit" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="playerRecordUpdater">Klasa odpowiadająca za aktualizację encji "DriedFruit" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="playerRecordDeleter">Klasa odpowiadająca za usuwanie encji "DriedFruit" u użytkowników na podstawie usuniętej definicji.</param>
    public DriedFruitAppService(
        IRepository<ExtendingModels.Models.Products.DriedFruit, int> repository,
        IResponseBuilder<DriedFruitDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreatePlayerRecord<DriedFruitCreateDto> playerRecordCreator,
        IUpdatePlayerRecord<DriedFruitUpdateDto> playerRecordUpdater,
        IDeletePlayerRecord<ExtendingModels.Models.Products.DriedFruit> playerRecordDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, playerRecordCreator, playerRecordUpdater, playerRecordDeleter)
    {
    }
}