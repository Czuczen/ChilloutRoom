using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Lamp".
/// </summary>
public interface ILampAppService : 
    IAsyncCrudAppService<LampDto, int, PagedResultRequestDto, LampCreateDto, LampUpdateDto>, 
    IEntityAsyncCrudAppService<LampCreateDto, LampUpdateDto>
{
}