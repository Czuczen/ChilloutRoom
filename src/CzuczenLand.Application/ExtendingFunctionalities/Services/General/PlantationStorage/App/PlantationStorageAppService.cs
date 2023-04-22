using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.StorageAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.App;

[AbpAuthorize]
public class PlantationStorageAppService : 
    StorageAsyncCrudAppService<ExtendingModels.Models.General.PlantationStorage, PlantationStorageDto, PagedResultRequestDto, PlantationStorageCreateDto, PlantationStorageUpdateDto>, 
    IPlantationStorageAppService
{
    public PlantationStorageAppService(
        IRepository<ExtendingModels.Models.General.PlantationStorage, int> repository,
        IResponseBuilder<PlantationStorageDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
        CreatePermissionName = NotCreatedEntity;
    }
}