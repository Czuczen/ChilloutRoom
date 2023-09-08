using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Soil".
/// </summary>
public interface ISoilAppService : 
    IAsyncCrudAppService<SoilDto, int, PagedResultRequestDto, SoilCreateDto, SoilUpdateDto>, 
    IEntityAsyncCrudAppService<SoilCreateDto, SoilUpdateDto>
{
}