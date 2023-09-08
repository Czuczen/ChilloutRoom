using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
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
    /// <param name="playerRecordCreator">Klasa odpowiadająca za tworzenie encji "Bonus" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="playerRecordUpdater">Klasa odpowiadająca za aktualizację encji "Bonus" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="playerRecordDeleter">Klasa odpowiadająca za usuwanie encji "Bonus" u użytkowników na podstawie usuniętej definicji.</param>
    public BonusAppService(
        IRepository<ExtendingModels.Models.Products.Bonus, int> repository,
        IResponseBuilder<BonusDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreatePlayerRecord<BonusCreateDto> playerRecordCreator,
        IUpdatePlayerRecord<BonusUpdateDto> playerRecordUpdater,
        IDeletePlayerRecord<ExtendingModels.Models.Products.Bonus> playerRecordDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, playerRecordCreator, playerRecordUpdater, playerRecordDeleter)
    {
            
    }
}