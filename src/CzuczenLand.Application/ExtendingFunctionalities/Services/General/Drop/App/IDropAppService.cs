﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.Drop.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Drop.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "GeneratedType".
/// </summary>
public interface IDropAppService : 
    IAsyncCrudAppService<DropDto, int, PagedResultRequestDto, DropCreateDto, DropUpdateDto>,
    IEntityAsyncCrudAppService<DropCreateDto, DropUpdateDto>
{
}