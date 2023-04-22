using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.App;

[AbpAuthorize]
public class BonusAppService : 
    ProductAsyncCrudAppService<ExtendingModels.Models.Products.Bonus, BonusDto, PagedResultRequestDto, BonusCreateDto, BonusUpdateDto>,
    IBonusAppService
{
    public BonusAppService(
        IRepository<ExtendingModels.Models.Products.Bonus, int> repository,
        IRepository<GeneratedType> generatedTypeRepository,
        IResponseBuilder<BonusDto> responseBuilder,
        ICreateDefinition<BonusCreateDto> definitionCreator,
        IUpdateDefinition<BonusUpdateDto> definitionUpdater,
        IDeleteDefinition<ExtendingModels.Models.Products.Bonus> definitionDeleter
    )
        : base(repository, responseBuilder, generatedTypeRepository, definitionCreator, definitionUpdater, definitionDeleter)
    {
            
    }
}