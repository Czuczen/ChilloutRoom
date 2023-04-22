using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.App;

public interface IPlayerStorageAppService :
    IAsyncCrudAppService<PlayerStorageDto, int, PagedResultRequestDto, PlayerStorageCreateDto, PlayerStorageUpdateDto>,
    IEntityAsyncCrudAppService<PlayerStorageCreateDto, PlayerStorageUpdateDto>
{
}