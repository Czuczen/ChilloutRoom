using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na magazynie gracza.
/// </summary>
public interface IPlayerStorageAppService :
    IAsyncCrudAppService<PlayerStorageDto, int, PagedResultRequestDto, PlayerStorageCreateDto, PlayerStorageUpdateDto>,
    IEntityAsyncCrudAppService<PlayerStorageCreateDto, PlayerStorageUpdateDto>
{
}