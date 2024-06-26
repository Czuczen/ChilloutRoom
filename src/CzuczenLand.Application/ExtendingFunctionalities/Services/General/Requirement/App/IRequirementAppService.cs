﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Requirement".
/// </summary>
public interface IRequirementAppService : 
    IAsyncCrudAppService<RequirementDto, int, PagedResultRequestDto, RequirementCreateDto, RequirementUpdateDto>,
    IEntityAsyncCrudAppService<RequirementCreateDto, RequirementUpdateDto>
{
}