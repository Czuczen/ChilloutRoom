using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Db;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.GeneratedEntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.App;

[AbpAuthorize]
public class QuestAppService : 
    GeneratedEntityAsyncCrudAppService<ExtendingModels.Models.General.Quest, QuestDto, PagedResultRequestDto, QuestCreateDto, QuestUpdateDto>, 
    IQuestAppService
{
    private readonly IQuestService _questService;

    
    public QuestAppService(
        IRepository<ExtendingModels.Models.General.Quest, int> repository,
        IQuestService questService,
        IRepository<ExtendingModels.Models.General.GeneratedType> generatedTypeRepository,
        IResponseBuilder<QuestDto> responseBuilder,
        ICreateDefinition<QuestCreateDto> definitionCreator,
        IUpdateDefinition<QuestUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.General.Quest> definitionDeleter
    )
        : base(repository, generatedTypeRepository, responseBuilder, definitionCreator, definitionUpdater, definitionDeleter)
    {
        _questService = questService;
    }

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

    public override async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<QuestCreateDto> input)
    {
        var entityAsyncCrudResponse = await base.ActionCreate(input);
        var createdObjId = int.Parse(((Dictionary<string, object>) entityAsyncCrudResponse.Records.First())[EntitiesFieldsDbNames.Id].ToString()); 
        await _questService.SetQuestDependencies(createdObjId, input.Connections);
        return entityAsyncCrudResponse;
    }
        
    public override async Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<QuestUpdateDto> input)
    {
        var entityAsyncCrudResponse = await base.ActionUpdate(input);
            
        await _questService.UpdateQuestDependencies(input.Input.Id, input.Connections);

        return entityAsyncCrudResponse;
    }
}