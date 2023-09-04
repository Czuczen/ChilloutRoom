using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;

/// <summary>
/// Bazowa klasa abstrakcyjna do aktualizacji rekordów graczy generowanych na podstawie definicji.
/// </summary>
/// <typeparam name="TEntity">Typ encji, który ma być aktualizowany.</typeparam>
/// <typeparam name="TUpdateDto">Typ DTO używany do aktualizacji encji.</typeparam>
/// <typeparam name="TUpdateDefinitionDto">Typ DTO używany do określenia aktualizowanych właściwości w rekordach graczy.</typeparam>
public abstract class UpdateDefinition<TEntity, TUpdateDto, TUpdateDefinitionDto>
    where TEntity : class, IPlantationGeneratedEntity
    where TUpdateDto : class, IEntityDto<int>
    where TUpdateDefinitionDto : class
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
    protected UpdateDefinition(
        IRepository<TEntity> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    )
    {
        AbpSession = NullAbpSession.Instance;
        Repository = repository;
        DistrictRepository = districtRepository;
        GeneratedTypeRepository = generatedTypeRepository;
    }
        
    /// <summary>
    /// Aktualizuje rekordy graczy na podstawie zaktualizowanej definicji.
    /// Aktualizuje tylko pola zawarte w modelu - TUpdateDefinitionDto.
    /// </summary>
    /// <param name="entity">DTO do aktualizacji encji.</param>
    public async Task Update(TUpdateDto entity)
    {
        var fullEntity = await Repository.GetAsync(entity.Id);
        var items = await Repository.GetAllListAsync(item =>
            item.GeneratedTypeId == fullEntity.GeneratedTypeId && item.Id != fullEntity.Id);
        
        foreach (var item in items)
            UpdateDefinitionHelper.UpdateObject<TUpdateDefinitionDto>(entity, item);
    }

    /// <summary>
    /// Aktualizuje rekordy graczy na podstawie zaktualizowanych pól dla wielu definicji.
    /// Aktualizuje tylko pola zawarte w modelu - TUpdateDefinitionDto.
    /// </summary>
    /// <param name="updatedFields">Zaktualizowane pola encji.</param>
    /// <param name="ids">Lista identyfikatorów zaktualizowanych definicji.</param>
    public async Task Update(object updatedFields, List<int> ids)
    {
        if (updatedFields == null) return;

        var isUpdateVisible = ids != null && ids.Count > 0;
        var isUpdateAll = ids == null || ids.Count == 0;
 
        var wardenId = AbpSession.GetUserId();
        var wardenDistrict = await DistrictRepository.SingleAsync(item => item.UserId == wardenId);
        var items = await Repository.GetAll().Join(
            GeneratedTypeRepository.GetAll().Where(item => item.DistrictId == wardenDistrict.Id),
            entity => entity.GeneratedTypeId,
            generatedType => generatedType.Id,
            (entity, generatedType) => entity).ToListAsync();
        
        if (isUpdateAll)
            foreach (var item in items.Where(item => item.PlantationStorageId != null))
                UpdateDefinitionHelper.UpdateObject<TUpdateDefinitionDto>(updatedFields, item);
        else if (isUpdateVisible)
            foreach (var id in ids)
            {
                var currItem = items.Single(item => item.Id == id);
                var playersDefinitionsToUpdate = items.Where(item =>
                    item.GeneratedTypeId == currItem.GeneratedTypeId && item.Id != currItem.Id);

                foreach (var item in playersDefinitionsToUpdate)
                    UpdateDefinitionHelper.UpdateObject<TUpdateDefinitionDto>(updatedFields, item);
            }
    }
}