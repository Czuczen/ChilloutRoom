using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.App;

public interface ISoilAppService : 
    IAsyncCrudAppService<SoilDto, int, PagedResultRequestDto, SoilCreateDto, SoilUpdateDto>, 
    IProductAsyncCrudAppService<SoilCreateDto, SoilUpdateDto>
{
}