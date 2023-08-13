using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na magazynie gracza.
/// Rozszerza interfejsy IAsyncCrudAppService oraz IEntityAsyncCrudAppService.
/// </summary>
public interface IPlayerStorageAppService :
    IAsyncCrudAppService<PlayerStorageDto, int, PagedResultRequestDto, PlayerStorageCreateDto, PlayerStorageUpdateDto>,
    IEntityAsyncCrudAppService<PlayerStorageCreateDto, PlayerStorageUpdateDto>
{
}