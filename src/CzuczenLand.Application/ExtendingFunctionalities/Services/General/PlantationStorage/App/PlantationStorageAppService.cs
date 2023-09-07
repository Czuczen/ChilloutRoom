using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "PlantationStorage".
/// </summary>
[AbpAuthorize]
public class PlantationStorageAppService : 
    StorageAsyncCrudAppService<ExtendingModels.Models.General.PlantationStorage, PlantationStorageDto, PagedResultRequestDto, PlantationStorageCreateDto, PlantationStorageUpdateDto>, 
    IPlantationStorageAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// Ustawia także brak możliwości tworzenia magazynu plantacji przez użytkownika. 
    /// </summary>
    /// <param name="repository">Repozytorium magazynu plantacji.</param>
    /// <param name="responseBuilder">Budowniczy odpowiedzi dla magazynu plantacji.</param>
    public PlantationStorageAppService(
        IRepository<ExtendingModels.Models.General.PlantationStorage, int> repository,
        IResponseBuilder<PlantationStorageDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
        CreatePermissionName = NotCreatedEntity;
    }
}