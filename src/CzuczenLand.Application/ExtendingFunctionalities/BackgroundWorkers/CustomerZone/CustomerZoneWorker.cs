using System;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.CustomerZone.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Customers;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.CustomerZone;

/// <summary>
/// Klasa wykonująca pracę w cyklach związaną z wystawianiem ofert w strefie klienta.
/// </summary>
public class CustomerZoneWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    /// <summary>
    /// Repozytorium dla suszów.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium dla roślin.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.Plant> _plantRepository;
    
    /// <summary>
    /// Repozytorium dla typów generowanych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium dla dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Kontekst Huba strefy klienta.
    /// </summary>
    private IHubContext _customersHub;
    
    /// <summary>
    /// Generator liczb losowych.
    /// </summary>
    private readonly Random _random = new();
    
    /// <summary>
    /// Okres czasu (w milisekundach) między cyklami pracy.
    /// </summary>
    private const int PeriodTime = 20000; // 20s 
        

    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="timer">AbpTimer do określania czasu cyklu pracy.</param>
    /// <param name="driedFruitRepository">Repozytorium dla suszów.</param>
    /// <param name="plantRepository">Repozytorium dla roślin.</param>
    /// <param name="generatedTypeRepository">Repozytorium dla typów generowanych.</param>
    /// <param name="districtRepository">Repozytorium dla dzielnic.</param>
    public CustomerZoneWorker(
        AbpTimer timer,
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<ExtendingModels.Models.General.Plant> plantRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<District> districtRepository
    )
        : base(timer)
    {
        Timer.Period = PeriodTime;
        _driedFruitRepository = driedFruitRepository;
        _plantRepository = plantRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _districtRepository = districtRepository;
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
    /// Tworzy oferty klientów dla strefy klienta i wysyła je do graczy.
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje 
    /// </summary>
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        var availableDriedFruitsDefs = _driedFruitRepository.GetAllList(item => item.PlantationStorageId == null && item.AvailableInCustomerZone);
        var allLivingPlants = _plantRepository.GetAllList(item => item.WiltLevel < 100);
        var allDriedFruitGeneratedTypes = _generatedTypeRepository.GetAllList(item => item.EntityName == EntitiesDbNames.Seed);
        var allDistricts = _districtRepository.GetAllList();
        _customersHub ??= GlobalHost.ConnectionManager.GetHubContext<CustomersHub>();
            
        var tempHowManyTrying = allLivingPlants.Count / 10;
        var howManyTrying = tempHowManyTrying > 0 ? tempHowManyTrying : 1;

        for (var i = 0; i < howManyTrying; i++)
        {
            foreach (var driedFruit in availableDriedFruitsDefs)
            {
                try
                {
                    var generatedType = allDriedFruitGeneratedTypes.Single(item => item.Id == driedFruit.GeneratedTypeId);       
                    var district = allDistricts.Single(item => item.Id == generatedType.DistrictId);
                    
                    if (!district.IsDefined) continue;
                    if (_random.Next(1, 100) > driedFruit.OfferChance) continue;

                    var amount = _random.Next(1, 100) <= district.ChanceForSpecialOfferInCustomerZone
                        ? _random.Next(district.SpecialOfferQuantityLowerRangeInCustomerZone, district.SpecialOfferQuantityHighRangeInCustomerZone)
                        : _random.Next(district.StandardOfferQuantityLowerRangeInCustomerZone, district.StandardOfferQuantityHighRangeInCustomerZone);

                    var clientOffer = new ClientOffer
                    {
                        Id = Guid.NewGuid(),
                        Name = driedFruit.Name,
                        DistrictId = generatedType.DistrictId,
                        TypeId = driedFruit.GeneratedTypeId,
                        OfferTime = district.OfferTimeInCustomerZone,
                        BuyPrice = (decimal) driedFruit.CustomerZonePrice,
                        Amount = amount,
                        MeasureUnit = PlantationManagerHelper.GetMeasureUnitByType(typeof(DriedFruit)), 
                    };

                    _customersHub.Clients.All.getOffer(clientOffer);
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
}