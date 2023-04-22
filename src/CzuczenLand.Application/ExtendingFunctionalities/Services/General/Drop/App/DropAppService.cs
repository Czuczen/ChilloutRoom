using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.DistrictEntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.General.Drop.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Drop.App;

[AbpAuthorize]
public class DropAppService : 
    DistrictEntityAsyncCrudAppService<ExtendingModels.Models.General.Drop, DropDto, PagedResultRequestDto, DropCreateDto, DropUpdateDto>, 
    IDropAppService
{
    public DropAppService(
        IRepository<ExtendingModels.Models.General.Drop, int> repository,
        IResponseBuilder<DropDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }
}