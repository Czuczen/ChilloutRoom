using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "Seed".
/// </summary>
public interface ISeedAppService : 
    IAsyncCrudAppService<SeedDto, int, PagedResultRequestDto, SeedCreateDto, SeedUpdateDto>, 
    IEntityAsyncCrudAppService<SeedCreateDto, SeedUpdateDto>
{
}