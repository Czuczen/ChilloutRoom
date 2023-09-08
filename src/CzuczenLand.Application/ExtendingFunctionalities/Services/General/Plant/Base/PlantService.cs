using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;

/// <summary>
/// Serwis podstawowy obsługujący logikę biznesową związaną z rośliną.
/// </summary>
public class PlantService : IPlantService
{
    /// <summary>
    /// Repozytorium roślin.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.Plant> _plantRepository;
    
    /// <summary>
    /// Repozytorium lamp.
    /// </summary>
    private readonly IRepository<Lamp> _lampRepository;
    
    /// <summary>
    /// Repozytorium doniczek.
    /// </summary>
    private readonly IRepository<Pot> _potRepository;
    
    /// <summary>
    /// Repozytorium gleb.
    /// </summary>
    private readonly IRepository<Soil> _soilRepository;
    
    /// <summary>
    /// Serwis ignorowania zmian.
    /// </summary>
    private readonly IgnoreChangeService _ignoreChangeService;

    
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="plantRepository">Repozytorium roślin.</param>
    /// <param name="lampRepository">Repozytorium lamp.</param>
    /// <param name="potRepository">Repozytorium doniczek.</param>
    /// <param name="soilRepository">Repozytorium gleb.</param>
    /// <param name="ignoreChangeService">Serwis ignorowania zmian.</param>
    public PlantService(
        IRepository<ExtendingModels.Models.General.Plant> plantRepository,
        IRepository<Lamp> lampRepository,
        IRepository<Pot> potRepository,
        IRepository<Soil> soilRepository,
        IgnoreChangeService ignoreChangeService
    )
    {
        Logger = NullLogger.Instance;
        _plantRepository = plantRepository;
        _lampRepository = lampRepository;
        _potRepository = potRepository;
        _soilRepository = soilRepository;
        _ignoreChangeService = ignoreChangeService;
    }
        
    /// <summary>
    /// Tworzy roślinę gracza.
    /// </summary>
    /// <param name="createPlant">Obiekt zawierający informacje do utworzenia nowej rośliny.</param>
    /// <param name="district">Dzielnica, w której roślina jest tworzona.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <param name="plantationStorageId">Identyfikator magazynu plantacji.</param>
    /// <param name="activeBonuses">Aktywne bonusy.</param>
    /// <param name="generatedType">Typ generowany nasiona dla rośliny.</param>
    /// <returns>Identyfikator nowo utworzonej rośliny.</returns>
    public async Task<int> CreatePlayerPlant(CreatePlant createPlant, ExtendingModels.Models.General.District district, string userName, 
        int plantationStorageId, List<Bonus> activeBonuses, ExtendingModels.Models.General.GeneratedType generatedType)
    {
        var lamp = createPlant.Lamp;
        var manure = createPlant.Manure;
        var seed = createPlant.Seed;
        var soil = createPlant.Soil;
        var pot = createPlant.Pot;
        var water = createPlant.Water;

        var increaseGrowingSpeedFromBonuses = activeBonuses.Sum(item => item.IncreaseGrowingSpeed);
        var increaseChanceForSeedFromBonuses = activeBonuses.Sum(item => item.IncreaseChanceForSeed);
        var increaseTimeOfInsensitivityFromBonuses = activeBonuses.Sum(item => item.IncreaseTimeOfInsensitivity);
        var increaseDriedFruitAmountFromBonuses = activeBonuses.Sum(item => item.IncreaseDriedFruitAmount);
        var increaseSeedAmountFromBonuses = activeBonuses.Sum(item => item.IncreaseSeedAmount);
        var increaseGainedExpFromBonuses = activeBonuses.Sum(item => item.IncreaseGainedExp);
        var productsSetsNames = new List<string>{ pot.SetName, seed.SetName, water.SetName, soil.SetName, manure.SetName, lamp.SetName };
            
        var plant = new ExtendingModels.Models.General.Plant
        {
            Name = generatedType.Name.ToUpper() + " użytkownika " + userName,
            ImageUrl = seed.ImageUrl,
            GrowingSpeed = PlantServiceHelper.CalculateGrowingSpeed(lamp, manure, soil, water, seed, pot, increaseGrowingSpeedFromBonuses),
            ChanceForSeed = PlantServiceHelper.CalculateChanceForSeed(lamp, manure, soil, water, seed, pot, increaseChanceForSeedFromBonuses),
            TimeOfInsensitivity = PlantServiceHelper.CalculateTimeOfInsensitivity(lamp, manure, soil, water, seed, pot, increaseTimeOfInsensitivityFromBonuses),
            DriedFruitAmount = PlantServiceHelper.CalculateDriedFruitAmount(lamp, manure, soil, water, seed, pot, increaseDriedFruitAmountFromBonuses),
            SeedAmount = PlantServiceHelper.CalculateSeedAmount(lamp, manure, soil, water, seed, pot, increaseSeedAmountFromBonuses),
            GainedExp = PlantServiceHelper.CalculateGainedExp(lamp, manure, soil, water, seed, pot, increaseGainedExpFromBonuses),
            SetsBaf = PlantServiceHelper.CalculateSetsBuff(productsSetsNames, activeBonuses, district),
            WiltSpeed = district.WiltSpeed,
            GrowingSpeedDivider = district.GrowingSpeedDivider,
            Description = seed.Description,
            WaterId = water.Id,
            ManureId = manure.Id,
            SoilId = soil.Id,
            PotId = pot.Id,
            LampId = lamp.Id,
            SeedId = seed.Id,
            GeneratedTypeId = seed.GeneratedTypeId,
            PlantationStorageId = plantationStorageId
        };

        var plantId = await _plantRepository.InsertAndGetIdAsync(plant);
            
        return plantId;
    }
        
    /// <summary>
    /// Usuwa rośliny powiązane z usuniętym produktem.
    /// </summary>
    /// <param name="productId">Identyfikator produktu.</param>
    /// <param name="entityName">Nazwa encji produktu.</param>
    public async Task DeleteConnectedPlantsToProduct(int productId, string entityName)
    {
        switch (SelectListLoaderHelper.GetEntityEnum(entityName))
        {
            case EnumUtils.Entities.Lamp:
                await DeletePlantAndSetUseCountConnectedProducts("LampId", productId, false);
                break;
            case EnumUtils.Entities.Manure:
                await DeletePlantAndSetUseCountConnectedProducts("ManureId", productId, false);
                break;
            case EnumUtils.Entities.Pot:
                await DeletePlantAndSetUseCountConnectedProducts("PotId", productId, false);
                break;
            case EnumUtils.Entities.Seed:
                await DeletePlantAndSetUseCountConnectedProducts("SeedId", productId, false);
                break;
            case EnumUtils.Entities.Soil:
                await DeletePlantAndSetUseCountConnectedProducts("SoilId", productId, false);
                break;
            case EnumUtils.Entities.Water:
                await DeletePlantAndSetUseCountConnectedProducts("WaterId", productId, false);
                break;
        }
    }
    
    /// <summary>
    /// Usuwa roślinę i aktualizuje powiązane produkty.
    /// Jeśli dzielnica fast się skończyła a użytkownik miał jakieś rośliny to dodawały się rekordy ignorowania
    /// ale z racji że już nie było magazynu plantacji state updater builder nie usuwał ignorowania. Dlatego potrzebna jest flaga needIgnoreChange.
    /// </summary>
    /// <param name="fieldToCompare">Pole do porównania.</param>
    /// <param name="value">Wartość pola.</param>
    /// <param name="needIgnoreChange">Flaga określająca potrzebę ignorowania zmian.</param>
    public async Task DeletePlantAndSetUseCountConnectedProducts(string fieldToCompare, int value, bool needIgnoreChange = true)
    {
        var plantType = typeof(ExtendingModels.Models.General.Plant);
        var allPlants = await _plantRepository.GetAllListAsync();

        foreach (var plant in allPlants)
        {
            try
            {
                var fieldToCompareValue = (int?) plantType.GetProperty(fieldToCompare)?.GetValue(plant);
                if (fieldToCompareValue != value) continue;
                    
                await _plantRepository.DeleteAsync(plant.Id);
                        
                var plantPot = await _potRepository.GetAsync(plant.PotId);
                plantPot.InUseCount--;
                plantPot.OwnedAmount++;

                var plantLamp = await _lampRepository.GetAsync(plant.LampId);
                plantLamp.InUseCount--;
                plantLamp.OwnedAmount++;
                
                var plantSoil = await _soilRepository.GetAsync(plant.SoilId);
                var soilPenaltyCount = PlantServiceHelper.CountSoilPenalty(plantPot);
                var soilReturnCount = plantPot.Capacity - soilPenaltyCount;
                plantSoil.OwnedAmount += soilReturnCount;

                if (!needIgnoreChange) continue;
                
                await _ignoreChangeService.Add(plantPot);
                await _ignoreChangeService.Add(plantLamp);
                await _ignoreChangeService.Add(plantSoil);
            }
            catch (Exception ex)
            {
                Logger.Error("Blad======//=======", ex);
            }
        }
    }
}