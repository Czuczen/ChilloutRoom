using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Water".
/// Rozszerza interfejsy IAsyncCrudAppService oraz IProductAsyncCrudAppService.
/// </summary>
public interface IWaterAppService : 
    IAsyncCrudAppService<WaterDto, int, PagedResultRequestDto, WaterCreateDto, WaterUpdateDto>, 
    IProductAsyncCrudAppService<WaterCreateDto, WaterUpdateDto>
{
        
}