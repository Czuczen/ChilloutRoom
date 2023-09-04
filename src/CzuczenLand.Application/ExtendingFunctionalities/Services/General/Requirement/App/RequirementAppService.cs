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

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "Requirement".
/// </summary>
[AbpAuthorize]
public class RequirementAppService : 
    DistrictEntityAsyncCrudAppService<ExtendingModels.Models.General.Requirement, RequirementDto, PagedResultRequestDto, RequirementCreateDto, RequirementUpdateDto>, 
    IRequirementAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium wymagań.</param>
    /// <param name="responseBuilder">Budowniczy odpowiedzi dla wymagań.</param>
    public RequirementAppService(
        IRepository<ExtendingModels.Models.General.Requirement, int> repository,
        IResponseBuilder<RequirementDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }

    /// <summary>
    /// Tworzy nowe wymaganie na podstawie dostarczonych informacji, uwzględniając warunki i porównania.
    /// </summary>
    /// <param name="input">Dane wejściowe obejmujące informacje o wymaganiu.</param>
    /// <returns>Odpowiedź z informacją lub wynik akcji tworzenia.</returns>
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