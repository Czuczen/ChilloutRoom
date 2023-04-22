using System;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Plant;

public class GrowWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    private readonly IRepository<ExtendingModels.Models.General.Plant> _plantRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private readonly IRepository<District> _districtRepository;
    private const int PeriodTime = 1000; // 1s 
        

    public GrowWorker(
        AbpTimer timer,
        IRepository<ExtendingModels.Models.General.Plant> plantRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<District> districtRepository
    )
        : base(timer)
    {
        Timer.Period = PeriodTime;
        _plantRepository = plantRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _districtRepository = districtRepository;
    }
        
    /// <summary>
    /// Nie wychodziło równo co sekundę. Dlatego robimy korektę.
    /// Czasami jeszcze łapie poślizg 15 milisekund ale jak dla mnie jest to już wystarczające.
    /// </summary>
    protected override void DoWork()
    {
        var watch = Stopwatch.StartNew();
        ExecuteWork();
        watch.Stop(); 
        Timer.Period = WorkersHelper.CalculatePeriodTime(PeriodTime, watch);
    }

    /// <summary>
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje 
    /// </summary>
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        var filteredPlantAndPlantationStorage = _plantationStorageRepository.GetAll().Join(
            _plantRepository.GetAll().Where(item => item.WiltLevel != 100),
            storage => storage.Id,
            plant => plant.PlantationStorageId,
            (storage, plant) => new {storage, plant}).ToList();
        
        var allDistricts = _districtRepository.GetAllList();
        var livingPlants = filteredPlantAndPlantationStorage.Select(item => item.plant).ToList();
        var plantationStorages = filteredPlantAndPlantationStorage.Select(item => item.storage)
            .GroupBy(item => item.Id).Select(item => item.First()).ToList();

        foreach (var plant in livingPlants)
        {
            try
            {
                var plantationStorage = plantationStorages.Single(item => item.Id == plant.PlantationStorageId);
                var district = allDistricts.Single(item => item.Id == plantationStorage.DistrictId);
                if (!district.IsDefined) continue;

                var wiltSpeed = Debugger.IsAttached ? plant.WiltSpeed * 200 : plant.WiltSpeed;
                var divider = plant.GrowingSpeedDivider;

                var growingSpeedWithBaf = plant.GrowingSpeed * plant.SetsBaf;
                var baseGrowingSpeed = growingSpeedWithBaf / divider;

                PlantServiceHelper.ProcessPlantGrowing(plant, wiltSpeed, baseGrowingSpeed);
                plant.TimeRemaining = PlantServiceHelper.CalculateTimeRemaining((decimal) plant.GrowingLevel, growingSpeedWithBaf, divider);
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException && ex.Message.Contains("Sequence contains no matching element")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        }
    }
}
