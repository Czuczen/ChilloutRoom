using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Comparers;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Conditions;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.DistrictEntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.App;

[AbpAuthorize]
public class RequirementAppService : 
    DistrictEntityAsyncCrudAppService<ExtendingModels.Models.General.Requirement, RequirementDto, PagedResultRequestDto, RequirementCreateDto, RequirementUpdateDto>, 
    IRequirementAppService
{
    public RequirementAppService(
        IRepository<ExtendingModels.Models.General.Requirement, int> repository,
        IResponseBuilder<RequirementDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }

    public override async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<RequirementCreateDto> input)
    {
        switch (input.Input.Condition)
        {
            case DbRequirementConditionsNames.Deliver when input.Input.Comparer == DbComparers.Level:
            case DbRequirementConditionsNames.Decrease when input.Input.Comparer == DbComparers.Level:
                return await ResponseBuilder.WithInfo("Nie można zmniejszyć poziomu").Build(EntityAsyncCrudActions.ActionCreate); // TODO: SPZ - Wrócić do tego jak będzie tworzony system pól zależnych
            case DbRequirementConditionsNames.Deliver when input.Input.Comparer == DbComparers.GainedExperience:
            case DbRequirementConditionsNames.Decrease when input.Input.Comparer == DbComparers.GainedExperience:
                return await ResponseBuilder.WithInfo("Nie można zmniejszyć doświadczenia").Build(EntityAsyncCrudActions.ActionCreate);
            default:
                return await base.ActionCreate(input);
        }
    }
}