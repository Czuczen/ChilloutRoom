using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.District.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.District.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "District".
/// </summary>
public interface IDistrictAppService : 
    IAsyncCrudAppService<DistrictDto, int, PagedResultRequestDto, DistrictCreateDto, DistrictUpdateDto>,
    IEntityAsyncCrudAppService<DistrictCreateDto, DistrictUpdateDto>
{
}