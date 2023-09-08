using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Water".
/// </summary>
public interface IWaterAppService : 
    IAsyncCrudAppService<WaterDto, int, PagedResultRequestDto, WaterCreateDto, WaterUpdateDto>, 
    IEntityAsyncCrudAppService<WaterCreateDto, WaterUpdateDto>
{
}