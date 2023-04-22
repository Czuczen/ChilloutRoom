using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.App;

[AbpAuthorize]
public class SeedAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Seed, SeedDto, PagedResultRequestDto, SeedCreateDto, SeedUpdateDto>, 
    ISeedAppService
{
    public SeedAppService(
        IRepository<ExtendingModels.Models.Products.Seed, int> repository,
        IResponseBuilder<SeedDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<SeedCreateDto> definitionCreator,
        IUpdateDefinition<SeedUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Seed> definitionDeleter
    )
        : base(repository, responseBuilder,  generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }
}