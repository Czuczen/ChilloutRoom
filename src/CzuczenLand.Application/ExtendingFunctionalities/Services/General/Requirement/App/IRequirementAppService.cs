using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.App;

public interface IRequirementAppService : 
    IAsyncCrudAppService<RequirementDto, int, PagedResultRequestDto, RequirementCreateDto, RequirementUpdateDto>,
    IEntityAsyncCrudAppService<RequirementCreateDto, RequirementUpdateDto>
{
        
}