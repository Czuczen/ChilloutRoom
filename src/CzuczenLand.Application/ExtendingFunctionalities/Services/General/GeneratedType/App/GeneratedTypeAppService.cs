using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.App;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "GeneratedType".
/// </summary>
[AbpAuthorize]
public class GeneratedTypeAppService : 
    DistrictEntityAsyncCrudAppService<ExtendingModels.Models.General.GeneratedType, GeneratedTypeDto, PagedResultRequestDto, GeneratedTypeCreateDto, GeneratedTypeUpdateDto>, 
    IGeneratedTypeAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium typu generowanego.</param>
    /// <param name="responseBuilder">Budowniczy odpowiedzi dla typu generowanego.</param>
    public GeneratedTypeAppService(
        IRepository<ExtendingModels.Models.General.GeneratedType, int> repository,
        IResponseBuilder<GeneratedTypeDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }
}