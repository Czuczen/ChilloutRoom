using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.App;

public interface ISeedAppService : 
    IAsyncCrudAppService<SeedDto, int, PagedResultRequestDto, SeedCreateDto, SeedUpdateDto>, 
    IProductAsyncCrudAppService<SeedCreateDto, SeedUpdateDto>
{
        
}