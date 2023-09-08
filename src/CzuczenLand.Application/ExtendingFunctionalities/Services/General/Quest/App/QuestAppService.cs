using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.CreateRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.DeleteRecord;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Db;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Quest".
/// </summary>
[AbpAuthorize]
public class QuestAppService : 
    GeneratedEntityAsyncCrudAppService<ExtendingModels.Models.General.Quest, QuestDto, PagedResultRequestDto, QuestCreateDto, QuestUpdateDto>, 
    IQuestAppService
{
    /// <summary>
    /// Serwis podstawowy dla zadań.
    /// </summary>
    private readonly IQuestService _questService;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium zadań.</param>
    /// <param name="responseBuilder">Budowniczy odpowiedzi dla zadań.</param>
    /// <param name="generatedTypeRepository">Repozytorium typu generowanego.</param>
    /// <param name="playerRecordCreator">Klasa odpowiadająca za tworzenie encji "Quest" dla użytkowników na podstawie stworzonej definicji.</param>
    /// <param name="playerRecordUpdater">Klasa odpowiadająca za aktualizację encji "Quest" u użytkowników na podstawie aktualizowanej definicji.</param>
    /// <param name="playerRecordDeleter">Klasa odpowiadająca za usuwanie encji "Quest" u użytkowników na podstawie usuniętej definicji.</param>
    /// <param name="questService">Serwis podstawowy dla zadań.</param>
    public QuestAppService(
        IRepository<ExtendingModels.Models.General.Quest, int> repository,
        IResponseBuilder<QuestDto> responseBuilder,
        IRepository<ExtendingModels.Models.General.GeneratedType> generatedTypeRepository,
        ICreatePlayerRecord<QuestCreateDto> playerRecordCreator,
        IUpdatePlayerRecord<QuestUpdateDto> playerRecordUpdater,
        IDeletePlayerRecord<ExtendingModels.Models.General.Quest> playerRecordDeleter,
        IQuestService questService
    )
        : base(repository, responseBuilder, generatedTypeRepository, playerRecordCreator, playerRecordUpdater, playerRecordDeleter)
    {
        _questService = questService;
    }

    /// <summary>
    /// Pobiera dostępne rekordy zadań.
    /// </summary>
    /// <returns>Odpowiedź z listą dostępnych zadań.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        var ret = new List<QuestDto>();
        await base.ActionGetAvailableRecords();
        if (IsDistrictWarden)
        {
            ret = ObjectMapper.Map<List<QuestDto>>(await Repository.GetAll().Join(
                GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == WardenDistrictId),
                entity => entity.GeneratedTypeId,
                generatedType => generatedType.Id,
                (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null).ToListAsync());
        }

        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }

    /// <summary>
    /// Tworzy nowe zadanie na podstawie dostarczonych informacji i połączeń, uwzględniając zależności.
    /// </summary>
    /// <param name="input">Dane wejściowe obejmujące informacje o zadaniu oraz połączeniach.</param>
    /// <returns>Odpowiedź z wynikiem akcji tworzenia.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<QuestCreateDto> input)
    {
        var entityAsyncCrudResponse = await base.ActionCreate(input);
        var createdObjId = int.Parse(((Dictionary<string, object>) entityAsyncCrudResponse.Records.First())[EntitiesFieldsDbNames.Id].ToString()); 
        await _questService.SetQuestDependencies(createdObjId, input.Connections);
        return entityAsyncCrudResponse;
    }
        
    /// <summary>
    /// Aktualizuje istniejące zadanie na podstawie dostarczonych informacji i połączeń, uwzględniając zależności.
    /// </summary>
    /// <param name="input">Dane wejściowe obejmujące informacje o zadaniu oraz połączeniach.</param>
    /// <returns>Odpowiedź z wynikiem akcji aktualizacji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<QuestUpdateDto> input)
    {
        var entityAsyncCrudResponse = await base.ActionUpdate(input);
            
        await _questService.UpdateQuestDependencies(input.Input.Id, input.Connections);

        return entityAsyncCrudResponse;
    }
}