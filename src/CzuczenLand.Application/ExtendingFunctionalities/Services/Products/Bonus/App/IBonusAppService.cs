using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.App;

public interface IBonusAppService : 
    IAsyncCrudAppService<BonusDto, int, PagedResultRequestDto, BonusCreateDto, BonusUpdateDto>, 
    IProductAsyncCrudAppService<BonusCreateDto, BonusUpdateDto>
{
}