using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.App;

[AbpAuthorize]
public class PotAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Pot, PotDto, PagedResultRequestDto, PotCreateDto, PotUpdateDto>, 
    IPotAppService
{
    public PotAppService(
        IRepository<ExtendingModels.Models.Products.Pot, int> repository,
        IResponseBuilder<PotDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<PotCreateDto> definitionCreator,
        IUpdateDefinition<PotUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Pot> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}