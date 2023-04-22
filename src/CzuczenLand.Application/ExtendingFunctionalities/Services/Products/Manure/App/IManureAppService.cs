using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.App;

public interface IManureAppService : 
    IAsyncCrudAppService<ManureDto, int, PagedResultRequestDto, ManureCreateDto, ManureUpdateDto>, 
    IProductAsyncCrudAppService<ManureCreateDto, ManureUpdateDto>
{
        
}