using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.App;

[AbpAuthorize]
public class DriedFruitAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.DriedFruit, DriedFruitDto, PagedResultRequestDto, DriedFruitCreateDto, DriedFruitUpdateDto>, 
    IDriedFruitAppService
{
    public DriedFruitAppService(
        IRepository<ExtendingModels.Models.Products.DriedFruit, int> repository,
        IResponseBuilder<DriedFruitDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<DriedFruitCreateDto> definitionCreator,
        IUpdateDefinition<DriedFruitUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.DriedFruit> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}