using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Lamp".
/// Rozszerza interfejsy IAsyncCrudAppService oraz IProductAsyncCrudAppService.
/// </summary>
public interface ILampAppService : 
    IAsyncCrudAppService<LampDto, int, PagedResultRequestDto, LampCreateDto, LampUpdateDto>, 
    IProductAsyncCrudAppService<LampCreateDto, LampUpdateDto>
{
        
}