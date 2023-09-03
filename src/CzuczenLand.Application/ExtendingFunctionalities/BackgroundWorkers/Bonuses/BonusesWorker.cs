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

/// <summary>
/// Klasa wykonująca pracę w cyklach związaną z czasem aktywności wzmocnień.
/// </summary>
public class BonusesWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    /// <summary>
    /// Repozytorium dla bonusów.
    /// </summary>
    private readonly IRepository<Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium dla dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium dla magazynów plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Okres czasu (w milisekundach) między cyklami pracy.
    /// </summary>
    private const int PeriodTime = 1000; // 1min
    
    
    /// <summary>
    /// Lista aktywnych wzmocnień.
    /// </summary>
    private List<Bonus> ActiveBuffs { get; set; }
    
    /// <summary>
    /// Lista magazynów plantacji powiązanych z aktywnymi wzmocnieniami.
    /// </summary>
    private List<PlantationStorage> PlantationStorages { get; set; }
    
    /// <summary>
    /// Lista wszystkich dzielnic.
    /// </summary>
    private List<District> Districts { get; set; }

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="bonusRepository">Repozytorium dla bonusów.</param>
    /// <param name="districtRepository">Repozytorium dla dzielnic.</param>
    /// <param name="plantationStorageRepository">Repozytorium dla magazynów plantacji.</param>
    /// <param name="timer">AbpTimer do określania czasu cyklu pracy.</param>
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

    /// <summary>
    /// Metoda wykonywana w każdym cyklu pracy pracownika.
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
    /// Przygotowuje dane potrzebne do ustawienia czasu aktywności wzmocnień.
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje 
    /// </summary>
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

    /// <summary>
    /// Redukuje czas aktywności wzmocnień.
    /// </summary>
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