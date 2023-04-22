using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.App;

public interface IPotAppService : 
    IAsyncCrudAppService<PotDto, int, PagedResultRequestDto, PotCreateDto, PotUpdateDto>, 
    IProductAsyncCrudAppService<PotCreateDto, PotUpdateDto>
{
}