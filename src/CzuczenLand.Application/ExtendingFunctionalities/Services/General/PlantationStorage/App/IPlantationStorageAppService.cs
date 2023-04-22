using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.App;

public interface IPlantationStorageAppService : 
    IAsyncCrudAppService<PlantationStorageDto, int, PagedResultRequestDto, PlantationStorageCreateDto, PlantationStorageUpdateDto>,
    IEntityAsyncCrudAppService<PlantationStorageCreateDto, PlantationStorageUpdateDto>
{
}