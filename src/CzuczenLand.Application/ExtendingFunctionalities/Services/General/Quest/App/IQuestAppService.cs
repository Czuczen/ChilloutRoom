using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Quest".
/// </summary>
public interface IQuestAppService : 
    IAsyncCrudAppService<QuestDto, int, PagedResultRequestDto, QuestCreateDto, QuestUpdateDto>,
    IEntityAsyncCrudAppService<QuestCreateDto, QuestUpdateDto>
{
}