using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "GeneratedType".
/// Rozszerza interfejsy IAsyncCrudAppService oraz IEntityAsyncCrudAppService.
/// </summary>
public interface IGeneratedTypeAppService : 
    IAsyncCrudAppService<GeneratedTypeDto, int, PagedResultRequestDto, GeneratedTypeCreateDto, GeneratedTypeUpdateDto>,
    IEntityAsyncCrudAppService<GeneratedTypeCreateDto, GeneratedTypeUpdateDto>
{
}