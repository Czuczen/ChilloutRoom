using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Manure".
/// </summary>
[AbpAuthorize]
public class ManureAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Manure, ManureDto, PagedResultRequestDto, ManureCreateDto, ManureUpdateDto>, 
    IManureAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "Manure".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "Manure".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="playerRecordCreator">Klasa odpowiadająca za tworzenie encji "Manure" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="playerRecordUpdater">Klasa odpowiadająca za aktualizację encji "Manure" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="playerRecordDeleter">Klasa odpowiadająca za usuwanie encji "Manure" u użytkowników na podstawie usuniętej definicji.</param>
    public ManureAppService(
        IRepository<ExtendingModels.Models.Products.Manure, int> repository,
        IResponseBuilder<ManureDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreatePlayerRecord<ManureCreateDto> playerRecordCreator,
        IUpdatePlayerRecord<ManureUpdateDto> playerRecordUpdater,
        IDeletePlayerRecord<ExtendingModels.Models.Products.Manure> playerRecordDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, playerRecordCreator, playerRecordUpdater, playerRecordDeleter)
    {
    }
}