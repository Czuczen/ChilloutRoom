using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "DriedFruit".
/// Rozszerza interfejsy IAsyncCrudAppService oraz IProductAsyncCrudAppService.
/// </summary>
public interface IDriedFruitAppService : 
    IAsyncCrudAppService<DriedFruitDto, int, PagedResultRequestDto, DriedFruitCreateDto, DriedFruitUpdateDto>, 
    IProductAsyncCrudAppService<DriedFruitCreateDto, DriedFruitUpdateDto>
{
        
}