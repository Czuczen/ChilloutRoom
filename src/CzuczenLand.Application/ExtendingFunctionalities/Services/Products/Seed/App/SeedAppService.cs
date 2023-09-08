using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Seed".
/// </summary>
[AbpAuthorize]
public class SeedAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Seed, SeedDto, PagedResultRequestDto, SeedCreateDto, SeedUpdateDto>, 
    ISeedAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "Seed".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "Seed".</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typu generowanego.</param>
    /// <param name="playerRecordCreator">Klasa odpowiadająca za tworzenie encji "Seed" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="playerRecordUpdater">Klasa odpowiadająca za aktualizację encji "Seed" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="playerRecordDeleter">Klasa odpowiadająca za usuwanie encji "Seed" u użytkowników na podstawie usuniętej definicji.</param>
    public SeedAppService(
        IRepository<ExtendingModels.Models.Products.Seed, int> repository,
        IResponseBuilder<SeedDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreatePlayerRecord<SeedCreateDto> playerRecordCreator,
        IUpdatePlayerRecord<SeedUpdateDto> playerRecordUpdater,
        IDeletePlayerRecord<ExtendingModels.Models.Products.Seed> playerRecordDeleter
    )
        : base(repository, responseBuilder,  generatedTypeRepository, playerRecordCreator, playerRecordUpdater, playerRecordDeleter)
    {
    }
}