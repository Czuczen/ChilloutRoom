using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.DistrictEntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.App;

[AbpAuthorize]
public class GeneratedTypeAppService : 
    DistrictEntityAsyncCrudAppService<ExtendingModels.Models.General.GeneratedType, GeneratedTypeDto, PagedResultRequestDto, GeneratedTypeCreateDto, GeneratedTypeUpdateDto>, 
    IGeneratedTypeAppService
{
    public GeneratedTypeAppService(
        IRepository<ExtendingModels.Models.General.GeneratedType, int> repository,
        IResponseBuilder<GeneratedTypeDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }
}