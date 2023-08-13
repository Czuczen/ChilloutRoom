using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.District.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.District.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "District".
/// Rozszerza interfejsy IAsyncCrudAppService oraz IEntityAsyncCrudAppService.
/// </summary>
public interface IDistrictAppService : 
    IAsyncCrudAppService<DistrictDto, int, PagedResultRequestDto, DistrictCreateDto, DistrictUpdateDto>,
    IEntityAsyncCrudAppService<DistrictCreateDto, DistrictUpdateDto>
{
}