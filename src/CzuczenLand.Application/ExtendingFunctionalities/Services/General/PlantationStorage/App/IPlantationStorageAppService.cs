using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na magazynie plantacji.
/// </summary>
public interface IPlantationStorageAppService : 
    IAsyncCrudAppService<PlantationStorageDto, int, PagedResultRequestDto, PlantationStorageCreateDto, PlantationStorageUpdateDto>,
    IEntityAsyncCrudAppService<PlantationStorageCreateDto, PlantationStorageUpdateDto>
{
}