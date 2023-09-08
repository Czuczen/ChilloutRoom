using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
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
    /// <param name="playerRecordCreator">Klasa odpowiadająca za tworzenie encji "Lamp" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="playerRecordUpdater">Klasa odpowiadająca za aktualizację encji "Lamp" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="playerRecordDeleter">Klasa odpowiadająca za usuwanie encji "Lamp" u użytkowników na podstawie usuniętej definicji.</param>
    public LampAppService(
        IRepository<ExtendingModels.Models.Products.Lamp, int> repository,
        IResponseBuilder<LampDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreatePlayerRecord<LampCreateDto> playerRecordCreator,
        IUpdatePlayerRecord<LampUpdateDto> playerRecordUpdater,
        IDeletePlayerRecord<ExtendingModels.Models.Products.Lamp> playerRecordDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, playerRecordCreator, playerRecordUpdater, playerRecordDeleter)
    {
    }
}