using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Quest".
/// Rozszerza interfejsy IAsyncCrudAppService oraz IEntityAsyncCrudAppService.
/// </summary>
public interface IQuestAppService : 
    IAsyncCrudAppService<QuestDto, int, PagedResultRequestDto, QuestCreateDto, QuestUpdateDto>,
    IEntityAsyncCrudAppService<QuestCreateDto, QuestUpdateDto>
{
}