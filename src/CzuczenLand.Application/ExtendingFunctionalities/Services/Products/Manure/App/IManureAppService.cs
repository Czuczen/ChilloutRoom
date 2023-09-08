using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Manure".
/// </summary>
public interface IManureAppService : 
    IAsyncCrudAppService<ManureDto, int, PagedResultRequestDto, ManureCreateDto, ManureUpdateDto>, 
    IEntityAsyncCrudAppService<ManureCreateDto, ManureUpdateDto>
{
}