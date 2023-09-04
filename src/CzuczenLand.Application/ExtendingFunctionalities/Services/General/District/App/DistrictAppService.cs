using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.District.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.District.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "District".
/// </summary>
[AbpAuthorize]
public class DistrictAppService : 
    EntityAsyncCrudAppService<ExtendingModels.Models.General.District, DistrictDto, PagedResultRequestDto, DistrictCreateDto, DistrictUpdateDto>, 
    IDistrictAppService
{
    /// <summary>
    /// Repozytorium magazynu plantacji.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.PlantationStorage> _plantationStorageRepository;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium dla dzielnicy.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynu plantacji.</param>
    /// <param name="responseBuilder">Budowniczy odpowiedzi dla dzielnicy.</param>
    public DistrictAppService(
        IRepository<ExtendingModels.Models.General.District, int> repository,
        IRepository<ExtendingModels.Models.General.PlantationStorage> plantationStorageRepository,
        IResponseBuilder<DistrictDto> responseBuilder
    ) 
        : base(repository, responseBuilder)
    {
        _plantationStorageRepository = plantationStorageRepository;
    }

    /// <summary>
    /// Metoda obsługująca akcję pobrania dostępnych rekordów dla encji District.
    /// </summary>
    /// <returns>Odpowiedź asynchroniczna zawierająca listę dostępnych rekordów encji District.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        var ret = new List<DistrictDto>();
        await base.ActionGetAvailableRecords();
        
        if (IsAdmin)
            ret = ObjectMapper.Map<List<DistrictDto>>(await Repository.GetAllListAsync());
        
        if (IsDistrictWarden)
        {
            ResponseBuilder.WithCanCreate(false);
            ret = ObjectMapper.Map<List<DistrictDto>>(new List<ExtendingModels.Models.General.District>
                {await Repository.SingleAsync(item => item.UserId == UserId)});
        }
            
        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }

    /// <summary>
    /// Metoda obsługująca akcję tworzenia rekordu encji District.
    /// </summary>
    /// <param name="input">Obiekt wejściowy zawierający dane dla nowego rekordu District.</param>
    /// <returns>Odpowiedź asynchroniczna zawierająca informacje o rezultacie akcji tworzenia.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionCreate(InputWithConnections<DistrictCreateDto> input)
    {
        var districtByUser = (await Repository.GetAllListAsync(item => item.UserId == input.Input.UserId)).SingleOrDefault();
        if (districtByUser != null)
            return await ResponseBuilder.WithInfo("Użytkownik jest już opiekunem").Build(EntityAsyncCrudActions.ActionCreate);

        return await base.ActionCreate(input);
    }

    /// <summary>
    /// Metoda obsługująca akcję aktualizacji rekordu encji District.
    /// </summary>
    /// <param name="input">Obiekt wejściowy zawierający dane do aktualizacji rekordu District.</param>
    /// <returns>Odpowiedź asynchroniczna zawierająca informacje o rezultacie akcji aktualizacji.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionUpdate(InputWithConnections<DistrictUpdateDto> input)
    {
        // Attaching an entity of type "" failed because another entity of the same type already has the same primary key value
        // Rozwiązuje problem z wyjątkiem. TransactionScopeOption.RequiresNew jest wymagane.
        using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew)) 
        {
            var districtByUser = (await Repository.GetAllListAsync(item => item.UserId == input.Input.UserId)).SingleOrDefault();
            var districtById = await Repository.GetAsync(input.Input.Id);

            // Przy edycji takie połączenie może już istnieć. Sprawdza czy to ten sam co edytujemy.
            if (districtByUser != null && districtById != null && districtByUser.Id != districtById.Id)
                return await ResponseBuilder.WithInfo("Użytkownik jest już opiekunem").Build(EntityAsyncCrudActions.ActionUpdate);

            await unitOfWork.CompleteAsync();
        }

            
        var district = await Repository.GetAsync(input.Input.Id);
        var districtPlantationStorages = await _plantationStorageRepository.GetAllListAsync(item => item.DistrictId == district.Id);
            
        foreach (var storage in districtPlantationStorages.Where(storage => storage.MaxBuffsSlots != district.MaxBuffsSlots))
            storage.MaxBuffsSlots = district.MaxBuffsSlots;

        foreach (var storage in districtPlantationStorages.Where(storage => storage.MaxArtifactSlots != district.MaxArtifactSlots))
            storage.MaxArtifactSlots = district.MaxArtifactSlots;
            
        foreach (var storage in districtPlantationStorages.Where(storage => storage.MaxDailyQuestsCount != district.MaxDailyQuestsCount))
            storage.MaxDailyQuestsCount = district.MaxDailyQuestsCount;
            
        foreach (var storage in districtPlantationStorages.Where(storage => storage.MaxWeeklyQuestsCount != district.MaxWeeklyQuestsCount))
            storage.MaxWeeklyQuestsCount = district.MaxWeeklyQuestsCount;
            
        return await base.ActionUpdate(input);
    }

    /// <summary>
    /// Metoda obsługująca akcję aktualizacji wielu rekordów encji District.
    /// </summary>
    /// <param name="request">Odpowiedź asynchroniczna zawierająca informacje o rezultacie akcji aktualizacji wielu rekordów.</param>
    public override async Task<EntityAsyncCrudResponse> ActionUpdateMany(UpdateManyRequest request)
    {
        if (IsDistrictWarden)
            return await ResponseBuilder.WithInfo("Nie możesz edytować wielu dzielnic").Build(EntityAsyncCrudActions.ActionUpdateMany);
            
        var district = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(request.FieldsToUpdate));
        if (DictionaryUtils.DictKeyExistAndHasValue(district, RelationFieldsNames.UserId))
            return await ResponseBuilder.WithInfo("Operacja niemożliwa. Użytkownik może być opiekunem tylko jednej dzielnicy").Build(EntityAsyncCrudActions.ActionUpdateMany);
        
        return await base.ActionUpdateMany(request);
    }
        
    /// <summary>
    /// Metoda obsługująca akcję usuwania rekordu encji District.
    /// </summary>
    /// <param name="objectId">Identyfikator rekordu District do usunięcia.</param>
    /// <returns>Odpowiedź asynchroniczna zawierająca informacje o rezultacie akcji usuwania.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionDelete(int objectId) 
    {
        if (IsDistrictWarden)
            return await ResponseBuilder.WithInfo("Nie możesz usuwać dzielnic").Build(EntityAsyncCrudActions.ActionDelete);
            
        return await base.ActionDelete(objectId);
    }
        
    /// <summary>
    /// Metoda obsługująca akcję usuwania wielu rekordów encji District.
    /// </summary>
    /// <param name="ids">Lista identyfikatorów rekordów encji District do usunięcia.</param>
    /// <returns>Odpowiedź asynchroniczna zawierająca informacje o rezultacie akcji usuwania wielu rekordów.</returns>
    public override async Task<EntityAsyncCrudResponse> ActionDeleteMany(List<int> ids)
    {
        if (IsDistrictWarden)
            return await ResponseBuilder.WithInfo("Nie możesz usuwać dzielnic").Build(EntityAsyncCrudActions.ActionDeleteMany);
            
        return await base.ActionDeleteMany(ids);
    }
}