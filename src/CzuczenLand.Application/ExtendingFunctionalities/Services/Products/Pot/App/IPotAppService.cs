using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Pot".
/// </summary>
public interface IPotAppService : 
    IAsyncCrudAppService<PotDto, int, PagedResultRequestDto, PotCreateDto, PotUpdateDto>, 
    IEntityAsyncCrudAppService<PotCreateDto, PotUpdateDto>
{
}