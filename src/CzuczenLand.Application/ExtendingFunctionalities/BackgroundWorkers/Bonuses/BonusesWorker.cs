using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Bonuses;

public class BonusesWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    private readonly IRepository<Bonus> _bonusRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private const int PeriodTime = 1000; // 1min
    
    private List<Bonus> ActiveBuffs { get; set; }
    
    private List<PlantationStorage> PlantationStorages { get; set; }
    
    private List<District> Districts { get; set; }


    public BonusesWorker(
        IRepository<Bonus> bonusRepository,
        IRepository<District> districtRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        AbpTimer timer
    ) 
        : base(timer)
    {
        _bonusRepository = bonusRepository;
        _districtRepository = districtRepository;
        _plantationStorageRepository = plantationStorageRepository;
        Timer.Period = PeriodTime;
    }

    protected override void DoWork()
    {
        var watch = Stopwatch.StartNew();
        ExecuteWork();
        watch.Stop();
        Timer.Period = WorkersHelper.CalculatePeriodTime(PeriodTime, watch);
    }
        
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        var filteredPlantationStorageBonus = _plantationStorageRepository.GetAll().Join(
            _bonusRepository.GetAll().Where(item => item.IsActive && !item.IsArtifact),
            plantationStorage => plantationStorage.Id,
            bonus => bonus.PlantationStorageId,
            (storage, bonus) => new {storage, bonus}).ToList();

        Districts = _districtRepository.GetAllList();
        ActiveBuffs = filteredPlantationStorageBonus.Select(item => item.bonus).ToList();
        PlantationStorages = filteredPlantationStorageBonus.Select(item => item.storage).GroupBy(item => item.Id)
            .Select(item => item.First()).ToList();

        ReduceAvailabilityTime();
    }

    private void ReduceAvailabilityTime()
    {
        foreach (var buff in ActiveBuffs)
        {
            try
            {
                var playerPlantationStorage = PlantationStorages.Single(item => item.Id == buff.PlantationStorageId);
                var district = Districts.Single(item => item.Id == playerPlantationStorage.DistrictId);
                if (!district.IsDefined) continue;

                if (buff.RemainingActiveTime > 0)
                    buff.RemainingActiveTime--;
                else
                {
                    buff.IsActive = false;
                    playerPlantationStorage.BuffSlotsInUse--;
                }
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