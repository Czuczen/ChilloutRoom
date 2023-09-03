using System;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Info;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Don;

/// <summary>
/// Klasa wykonująca pracę w cyklach związaną z ustawianiem dona dzielnicy.
/// </summary>
public class DonWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium donów dzielnic.
    /// </summary>
    private readonly IRepository<DistrictDon> _districtDonRepository;
    
    /// <summary>
    /// Repozytorium magazynów graczy.
    /// </summary>
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    
    /// <summary>
    /// Repozytorium magazynów plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Kontekst Huba informacyjnego.
    /// </summary>
    private IHubContext _infoHub;
    
    /// <summary>
    /// Okres czasu (w milisekundach) między cyklami pracy.
    /// </summary>
    private const int PeriodTime = 1000; // 1sec
        

    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="districtDonRepository">Repozytorium donów dzielnic.</param>
    /// <param name="playerStorageRepository">Repozytorium magazynów graczy.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    /// <param name="timer">AbpTimer do określania czasu cyklu pracy.</param>
    public DonWorker(
        IRepository<District> districtRepository,
        IRepository<DistrictDon> districtDonRepository,
        IRepository<PlayerStorage> playerStorageRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        AbpTimer timer
    )
        : base(timer)
    {
        _districtRepository = districtRepository;
        _districtDonRepository = districtDonRepository;
        _playerStorageRepository = playerStorageRepository;
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
    /// Analizuje i ustawia donów dzielnic.
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje
    /// </summary>
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        var districtQuery = _districtRepository.GetAll().Where(item => item.IsDefined);
        var filteredStorages = _playerStorageRepository.GetAll().Join(_plantationStorageRepository.GetAll().Join(
                districtQuery,
                plantationStorage => plantationStorage.DistrictId,
                district => district.Id,
                (plantationStorage, district) => plantationStorage),

            playerStorage => playerStorage.UserId,
            plantationStorage => plantationStorage.UserId,
            (playerStorage, plantationStorage) => new {playerStorage, plantationStorage}).ToList();

        var districtsDons = _districtDonRepository.GetAll().Join(
            districtQuery,
            don => don.DistrictId,
            district => district.Id,
            (don, district) => don).ToList();

        var definedDistricts = districtQuery.ToList();
        var plantationStorages = filteredStorages.Select(item => item.plantationStorage).ToList();
        var playerStorages = filteredStorages.Select(item => item.playerStorage).GroupBy(item => item.Id)
            .Select(item => item.First()).ToList();

        _infoHub ??= GlobalHost.ConnectionManager.GetHubContext<InfoHub>();

        foreach (var district in definedDistricts)
        {
            try
            {
                var districtPlantationStorages = plantationStorages.Where(item => item.DistrictId == district.Id).ToList();
                if (!districtPlantationStorages.Any()) continue;

                var bestPlantationStorage = districtPlantationStorages.Aggregate((i1, i2) => i1.Prestige > i2.Prestige ? i1 : i2);
                var playerStorage = playerStorages.Single(item => item.UserId == bestPlantationStorage.UserId);
                var districtDon = districtsDons.FirstOrDefault(item => item.DistrictId == district.Id);

                if (districtDon == null && bestPlantationStorage.Prestige >= district.PrestigeToBecomeDon)
                {
                    _districtDonRepository.Insert(new DistrictDon{DistrictId = district.Id, PlantationStorageId = bestPlantationStorage.Id});
                    SendDonData(true, bestPlantationStorage, playerStorage, district);
                }
                else if (districtDon != null && districtDon.PlantationStorageId != bestPlantationStorage.Id && bestPlantationStorage.Prestige >= district.PrestigeToBecomeDon)
                {
                    districtDon.PlantationStorageId = bestPlantationStorage.Id;
                    SendDonData(true, bestPlantationStorage, playerStorage, district);
                }
                else if (bestPlantationStorage.Prestige < district.PrestigeToBecomeDon)
                {
                    _districtDonRepository.Delete(item => item.DistrictId == district.Id);
                    SendDonData(false, bestPlantationStorage, playerStorage, district);
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

    /// <summary>
    /// Wysyła dane o donie dzielnicy do użytkownika.
    /// </summary>
    /// <param name="weHaveDon">Flaga informująca, czy dzielnica ma dona.</param>
    /// <param name="plantationStorage">Magazyn plantacji.</param>
    /// <param name="playerStorage">Magazyn gracza.</param>
    /// <param name="district">Dzielnica dla której są aktualizowane dane o donie.</param>
    private void SendDonData(bool weHaveDon, PlantationStorage plantationStorage, 
        PlayerStorage playerStorage, District district)
    {
        var donData = new DonData
        {
            WeHaveDon = weHaveDon, 
            DonName = playerStorage.PlayerName,
            DonCharityPercentage = district.DonCharityPercentage, 
            DonId = plantationStorage.UserId,
            DistrictId = plantationStorage.DistrictId
        };
                
        _infoHub.Clients.All.updateDon(donData);
    }
}