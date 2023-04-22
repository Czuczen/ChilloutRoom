using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Water.App;

[AbpAuthorize]
public class WaterAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Water, WaterDto, PagedResultRequestDto, WaterCreateDto, WaterUpdateDto>, 
    IWaterAppService
{
    public WaterAppService(
        IRepository<ExtendingModels.Models.Products.Water, int> repository,
        IResponseBuilder<WaterDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<WaterCreateDto> definitionCreator,
        IUpdateDefinition<WaterUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Water> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}