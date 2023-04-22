using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.App;

[AbpAuthorize]
public class LampAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Lamp, LampDto, PagedResultRequestDto, LampCreateDto, LampUpdateDto>, 
    ILampAppService
{
    public LampAppService(
        IRepository<ExtendingModels.Models.Products.Lamp, int> repository,
        IResponseBuilder<LampDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<LampCreateDto> definitionCreator,
        IUpdateDefinition<LampUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Lamp> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}