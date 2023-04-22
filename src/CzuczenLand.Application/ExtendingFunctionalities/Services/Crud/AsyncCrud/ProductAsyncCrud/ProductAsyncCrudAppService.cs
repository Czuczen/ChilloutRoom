using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.GeneratedEntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.ProductAsyncCrud;

public abstract class ProductAsyncCrudAppService<TProduct, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput> : 
    GeneratedEntityAsyncCrudAppService<TProduct, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>, 
    IProductAsyncCrudAppService<TCreateInput, TUpdateInput>
    where TProduct : Product, IEntity<int>
    where TEntityDto : class, IEntityDto<int>
    where TUpdateInput : class, IEntityDto<int>
    where TCreateInput : class
{
    protected ProductAsyncCrudAppService(
        IRepository<TProduct, int> repository, 
        IResponseBuilder<TEntityDto> responseBuilder,
        IRepository<GeneratedType> generatedTypeRepository,
        ICreateDefinition<TCreateInput> definitionCreator,
        IUpdateDefinition<TUpdateInput> definitionUpdater,
        IDeleteDefinition<TProduct> definitionDeleter
    )
        : base(repository, generatedTypeRepository, responseBuilder, definitionCreator, definitionUpdater, definitionDeleter)
    {
    }

    public override async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        var ret = new List<TEntityDto>();
        await base.ActionGetAvailableRecords();
        
        if (IsDistrictWarden)
        {
            ret = ObjectMapper.Map<List<TEntityDto>>(await Repository.GetAll().Join(
                GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == WardenDistrictId),
                entity => entity.GeneratedTypeId,
                generatedType => generatedType.Id,
                (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null).ToListAsync());
        }

        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }
}