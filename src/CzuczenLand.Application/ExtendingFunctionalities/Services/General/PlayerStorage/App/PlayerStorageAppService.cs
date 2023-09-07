using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "PlayerStorage".
/// </summary>
[AbpAuthorize]
public class PlayerStorageAppService :
    StorageAsyncCrudAppService<ExtendingModels.Models.General.PlayerStorage, PlayerStorageDto, PagedResultRequestDto, PlayerStorageCreateDto, PlayerStorageUpdateDto>, 
    IPlayerStorageAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// Ustawia także brak możliwości tworzenia magazynu gracza przez użytkownika. 
    /// </summary>
    /// <param name="repository">Repozytorium magazynu gracza.</param>
    /// <param name="responseBuilder">Budowniczy odpowiedzi dla magazynu gracza.</param>
    public PlayerStorageAppService(
        IRepository<ExtendingModels.Models.General.PlayerStorage, int> repository,
        IResponseBuilder<PlayerStorageDto> responseBuilder
    ) 
        : base(repository, responseBuilder) 
    {
        CreatePermissionName = NotCreatedEntity;
    }
}