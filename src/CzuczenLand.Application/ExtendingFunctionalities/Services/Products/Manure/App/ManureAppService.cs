using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.App;

[AbpAuthorize]
public class ManureAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Manure, ManureDto, PagedResultRequestDto, ManureCreateDto, ManureUpdateDto>, 
    IManureAppService
{
    public ManureAppService(
        IRepository<ExtendingModels.Models.Products.Manure, int> repository,
        IResponseBuilder<ManureDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<ManureCreateDto> definitionCreator,
        IUpdateDefinition<ManureUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Manure> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}