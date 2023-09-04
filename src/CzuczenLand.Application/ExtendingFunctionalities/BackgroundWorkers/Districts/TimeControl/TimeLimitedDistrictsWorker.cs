using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Info;
using CzuczenLand.ExtendingModels.Models.General;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Districts.TimeControl;

/// <summary>
/// Klasa wykonująca pracę w cyklach związaną z ograniczonymi czasowo dzielnicami.
/// </summary>
public class TimeLimitedDistrictsWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium magazynów plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium magazynów graczy.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Kontekst Huba informacyjnego.
    /// </summary>
    private IHubContext _infoHub;
    
    /// <summary>
    /// Okres czasu (w milisekundach) między cyklami pracy.
    /// </summary>
    private const int PeriodTime = 60000; // 1min
    
    
    /// <summary>
    /// Lista zdefiniowanych dzielnic.
    /// </summary>
    private List<District> DefinedDistricts { get; set; }

    /// <summary>
    /// Lista magazynów plantacji zdefiniowanych dzielnic.
    /// </summary>
    private List<PlantationStorage> PlantationStorages { get; set; }
    
    /// <summary>
    /// Lista magazynów graczy zdefiniowanych dzielnic.
    /// </summary>
    private List<PlayerStorage> PlayerStorages { get; set; }

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynów gracza.</param>
    /// <param name="timer">AbpTimer do określania czasu cyklu pracy.</param>
    public TimeLimitedDistrictsWorker(
        IRepository<District> districtRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<PlayerStorage> playerStorageRepository,
        AbpTimer timer
    ) 
        : base(timer)
    {
        Timer.Period = PeriodTime;
        _districtRepository = districtRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _playerStorageRepository = playerStorageRepository;
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
    /// Przygotowuje dane potrzebne do analizy dzielnic ograniczonych czasowo.
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje
    /// </summary>
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        var filteredStorages = _playerStorageRepository.GetAll().Join(_plantationStorageRepository.GetAll().Join(
                _districtRepository.GetAll().Where(item => item.IsDefined),
                plantationStorage => plantationStorage.DistrictId,
                district => district.Id,
                (plantationStorage, district) => plantationStorage),
            
            playerStorage => playerStorage.UserId,
            plantationStorage => plantationStorage.UserId,
            (playerStorage, plantationStorage) => new {playerStorage, plantationStorage}).ToList();

        DefinedDistricts = _districtRepository.GetAllList(item => item.IsDefined);
        PlantationStorages = filteredStorages.Select(item => item.plantationStorage).ToList();
        PlayerStorages = filteredStorages.Select(item => item.playerStorage).GroupBy(item => item.Id)
            .Select(item => item.First()).ToList();
        
        _infoHub ??= GlobalHost.ConnectionManager.GetHubContext<InfoHub>();

        SendDistrictEndTime();
        DeleteCyclicDistrictsPlantationStorages();
        SetCyclicTime(); // musi być ostatnie
    }
    
    /// <summary>
    /// Ustawia nagrody otrzymane za grę na dzielnicy, której czas się skończył i wysyła informacje o końcu dostępności dzielnicy. 
    /// Jeśli dzielnica zaraz ma się skończyć a użytkownikowi akurat tworzy się dziupla to czasami może ona mu się nie usunąć
    /// i będzie mógł grać na takiej dzielnicy póki nie odświeży strony ale to co zdążył zrobić zostanie zachowane do czasu końca dzielnicy.
    /// Dlatego dodono sprawdzanie dzielnic których czas ma dopiero nadejść czy czasem taka dzielnica ma jakieś magazyny plantacji
    /// </summary>
    private void SendDistrictEndTime()
    {
        var currDateTime = Clock.Now;
        var districts = DefinedDistricts.Where(item => item.EndTime != null && item.EndTime < currDateTime).ToList();
        var districtsWithFutureStartTime = DefinedDistricts.Where(item => item.StartTime > currDateTime).ToList();
        foreach (var district in districtsWithFutureStartTime)
        {
            var districtPlantationStorages = PlantationStorages.Where(item => 
                item.DistrictId == district.Id && item.UserId != district.UserId).ToList();
            foreach (var plantationStorage in districtPlantationStorages)
                _infoHub.Clients.User(plantationStorage.UserId.ToString()).districtChanged(district.Id);
        }

        foreach (var district in districts)
        {
            try
            {
                var districtPlantationStorages = PlantationStorages.Where(item => 
                    item.DistrictId == district.Id && item.UserId != district.UserId).ToList();
                var districtPlantationStoragesWithPrestige = districtPlantationStorages.Where(item => item.Prestige > 0).ToList();
                
                if (!districtPlantationStorages.Any()) continue;

                if (districtPlantationStoragesWithPrestige.Any())
                {
                    var topThree = districtPlantationStoragesWithPrestige.OrderByDescending(item => item.Prestige).Take(3).ToList();
                    if (topThree.Count == 1)
                    {
                        var firstPlace = topThree.Single();
                        var firstPlayerStorage = PlayerStorages.Single(item => item.UserId == firstPlace.UserId);
                        firstPlayerStorage.Honor += district.HonorForTakingFirstPlace;
                    }
                    else if (topThree.Count == 2)
                    {
                        var firstPlace = topThree.First();
                        var secondPlace = topThree.Last();
                        
                        var firstPlayerStorage = PlayerStorages.Single(item => item.UserId == firstPlace.UserId);
                        var secondPlayerStorage = PlayerStorages.Single(item => item.UserId == secondPlace.UserId);
                        
                        firstPlayerStorage.Honor += district.HonorForTakingFirstPlace;
                        secondPlayerStorage.Honor += district.HonorForTakingSecondPlace;
                    }
                    else
                    {
                        var firstPlace = topThree.Aggregate((i1, i2) => i1.Prestige > i2.Prestige ? i1 : i2);
                        var thirdPlace = topThree.Aggregate((i1, i2) => i1.Prestige < i2.Prestige ? i1 : i2);
                        var secondPlace = topThree.Single(item => item.Id != firstPlace.Id && item.Id != thirdPlace.Id);
                        
                        var firstPlayerStorage = PlayerStorages.Single(item => item.UserId == firstPlace.UserId);
                        var secondPlayerStorage = PlayerStorages.Single(item => item.UserId == secondPlace.UserId);
                        var thirdPlayerStorage = PlayerStorages.Single(item => item.UserId == thirdPlace.UserId);
                        
                        firstPlayerStorage.Honor += district.HonorForTakingFirstPlace;
                        secondPlayerStorage.Honor += district.HonorForTakingSecondPlace;
                        thirdPlayerStorage.Honor += district.HonorForTakingThirdPlace;
                    }

                    foreach (var plantationStorage in districtPlantationStoragesWithPrestige)
                    {
                        var playerStorage = PlayerStorages.Single(item => item.UserId == plantationStorage.UserId);
                        playerStorage.Honor += district.HonorConsolationPrize;
                    }
                }

                foreach (var plantationStorage in districtPlantationStorages)
                    _infoHub.Clients.User(plantationStorage.UserId.ToString()).districtChanged(district.Id);
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

    /// <summary>
    /// Usuwa magazyny plantacji dla dzielnic cyklicznych, które już zakończyły swój czas aktywności.
    /// </summary>
    private void DeleteCyclicDistrictsPlantationStorages()
    {
        var currDateTime = Clock.Now;
        var districts = DefinedDistricts.Where(item =>
            item.EndTime < currDateTime && item.CyclicTime != null &&
            item.CyclicTime > 0).ToList();

        var districtsWithFutureStartTime = DefinedDistricts.Where(item => 
            item.StartTime > currDateTime && item.CyclicTime != null && item.CyclicTime > 0).ToList();

        foreach (var district in districtsWithFutureStartTime)
            _plantationStorageRepository.Delete(item => item.DistrictId == district.Id && item.UserId != district.UserId);
        
        foreach (var district in districts)
            _plantationStorageRepository.Delete(item => item.DistrictId == district.Id && item.UserId != district.UserId);
    }
    
    /// <summary>
    /// Metoda do aktualizacji czasu cyklicznych dzielnic.
    /// </summary>
    private void SetCyclicTime()
    {
        var currDateTime = Clock.Now;
        var districts = DefinedDistricts.Where(item =>
            item.EndTime < currDateTime && item.CyclicTime != null &&
            item.CyclicTime > 0).ToList();

        foreach (var district in districts)
        {
            var timeToAdd = TimeSpan.FromMinutes(double.Parse(district.CyclicTime.ToString()));
            district.StartTime += timeToAdd;
            district.EndTime += timeToAdd;   
        }
    }
}
