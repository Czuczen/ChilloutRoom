﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.App;

public interface IQuestAppService : 
    IAsyncCrudAppService<QuestDto, int, PagedResultRequestDto, QuestCreateDto, QuestUpdateDto>,
    IEntityAsyncCrudAppService<QuestCreateDto, QuestUpdateDto>
{
}