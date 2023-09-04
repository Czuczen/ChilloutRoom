using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;

/// <summary>
/// Bazowa klasa abstrakcyjna do usuwania rekordów graczy generowanych na podstawie definicji.
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być usuwany.</typeparam>
public abstract class DeleteDefinition<TEntity>
    where TEntity : class, IPlantationGeneratedEntity
{
    /// <summary>
    /// Repozytorium encji generowanej.
    /// </summary>
    protected readonly IRepository<TEntity> Repository;
    
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    protected readonly IRepository<District> DistrictRepository;
    
    /// <summary>
    /// Repozytorium typów generowanych.
    /// </summary>
    protected readonly IRepository<GeneratedType> GeneratedTypeRepository;
    
    /// <summary>
    /// Serwis roślin.
    /// </summary>
    protected readonly IPlantService PlantService;

    
    /// <summary>
    /// Właściwość pozwalająca na uzyskanie dostępu do sesji Abp, która przechowuje informacje dotyczące aktualnie zalogowanego użytkownika.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public IAbpSession AbpSession { get; set; }

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji generowanej.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="plantService">Serwis roślin.</param>
    protected DeleteDefinition(
        IRepository<TEntity> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository, 
        IPlantService plantService
    )
    {
        AbpSession = NullAbpSession.Instance;
        Repository = repository;
        DistrictRepository = districtRepository;
        GeneratedTypeRepository = generatedTypeRepository;
        PlantService = plantService;
    }
        
    /// <summary>
    /// Usuwa rekordy graczy na podstawie usuniętej definicji.
    /// </summary>
    /// <param name="objectId">Identyfikator usuwanej definicji.</param>
    public async Task Delete(int objectId)
    {
        var entityName = typeof(TEntity).Name;
        var currRepo = CustomRepositoryFactory.GetRepository(entityName); 
        await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(new List<int>{objectId}, currRepo, entityName, PlantService);
    }
        
    /// <summary>
    /// Usuwa rekordy graczy na podstawie usuwanych definicji.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów usuwanych definicji.</param>
    public async Task Delete(List<int> ids)
    {
        var entityName = typeof(TEntity).Name;
        var currRepo = CustomRepositoryFactory.GetRepository(entityName);

        var isDeleteVisible = ids != null && ids.Count > 0;
        var isDeleteAll = ids == null || ids.Count == 0;
   
        if (isDeleteAll)
        {
            var wardenId = AbpSession.GetUserId();
            var wardenDistrict = await DistrictRepository.SingleAsync(item => item.UserId == wardenId);
            var itemsIds = await Repository.GetAll().Join(
                    GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == wardenDistrict.Id),
                    entity => entity.GeneratedTypeId,
                    generatedType => generatedType.Id,
                    (entity, generatedType) => entity).Where(item => item.PlantationStorageId == null)
                .Select(item => item.Id).ToListAsync();

            await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(itemsIds, currRepo, entityName, PlantService);
        }
        else if (isDeleteVisible)
            await DeleteDefinitionHelper.DeleteAllDefinitionsAsync(ids, currRepo, entityName, PlantService);
    }
}