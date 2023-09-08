using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Bonus".
/// </summary>
public interface IBonusAppService : 
    IAsyncCrudAppService<BonusDto, int, PagedResultRequestDto, BonusCreateDto, BonusUpdateDto>, 
    IEntityAsyncCrudAppService<BonusCreateDto, BonusUpdateDto>
{
}