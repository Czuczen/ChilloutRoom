using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.App;

[AbpAuthorize]
public class SoilAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Soil, SoilDto, PagedResultRequestDto, SoilCreateDto, SoilUpdateDto>, 
    ISoilAppService
{
    public SoilAppService(
        IRepository<ExtendingModels.Models.Products.Soil, int> repository,
        IResponseBuilder<SoilDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<SoilCreateDto> definitionCreator,
        IUpdateDefinition<SoilUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Soil> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
  
    }
}