using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.Drop.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Drop.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Drop".
/// </summary>
[AbpAuthorize]
public class DropAppService : 
    DistrictEntityAsyncCrudAppService<ExtendingModels.Models.General.Drop, DropDto, PagedResultRequestDto, DropCreateDto, DropUpdateDto>, 
    IDropAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium nagród.</param>
    /// <param name="responseBuilder">Budowniczy odpowiedzi dla nagród.</param>
    public DropAppService(
        IRepository<ExtendingModels.Models.General.Drop, int> repository,
        IResponseBuilder<DropDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }
}