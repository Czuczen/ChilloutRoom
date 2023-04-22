using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.StorageAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.App;

[AbpAuthorize]
public class PlayerStorageAppService :
    StorageAsyncCrudAppService<ExtendingModels.Models.General.PlayerStorage, PlayerStorageDto, PagedResultRequestDto, PlayerStorageCreateDto, PlayerStorageUpdateDto>, 
    IPlayerStorageAppService
{
    public PlayerStorageAppService(
        IRepository<ExtendingModels.Models.General.PlayerStorage, int> repository,
        IResponseBuilder<PlayerStorageDto> responseBuilder
    ) 
        : base(repository, responseBuilder) 
    {
        CreatePermissionName = NotCreatedEntity;
    }
}