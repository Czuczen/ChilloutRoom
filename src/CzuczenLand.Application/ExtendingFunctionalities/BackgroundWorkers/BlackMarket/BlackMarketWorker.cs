using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Loader;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket;

/// <summary>
/// Klasa wykonująca pracę w cyklach związaną z transakcjami czarnego rynku.
/// </summary>
public class BlackMarketWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    /// <summary>
    /// Repozytorium transakcji na czarnym rynku.
    /// </summary>
    private readonly IRepository<BlackMarketTransaction> _blackMarketTransactionRepository;
    
    /// <summary>
    /// Repozytorium magazynów plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium typów generowanych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium donów dzielnic.
    /// </summary>
    private readonly IRepository<DistrictDon> _districtDonRepository;
    
    /// <summary>
    /// Interfejs do ładowania niestandardowych repozytoriów.
    /// </summary>
    private readonly ICustomRepositoryLoader _customRepositoryLoader;
    
    /// <summary>
    /// Generator liczb losowych.
    /// </summary>
    private readonly Random _random = new();
    
    /// <summary>
    /// Okres czasu (w milisekundach) między cyklami pracy.
    /// </summary>
    private const int PeriodTime = 5000; // 5s
    

    /// <summary>
    /// Lista wszystkich transakcji na czarnym rynku.
    /// </summary>
    private List<BlackMarketTransaction> AllBlackMarketTransactions { get; set; }
    
    /// <summary>
    /// Lista wszystkich typów generowanych.
    /// </summary>
    private List<GeneratedType> AllGeneratedTypes { get; set; }
    
    /// <summary>
    /// Lista wszystkich magazynów plantacji.
    /// </summary>
    private List<PlantationStorage> AllPlantationStorages { get; set; }
    
    /// <summary>
    /// Lista wszystkich dzielnic.
    /// </summary>
    private List<District> AllDistricts { get; set; }
    
    /// <summary>
    /// Lista wszystkich donów dzielnic.
    /// </summary>
    private List<DistrictDon> AllDistrictDons { get; set; }
        

    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="blackMarketTransactionRepository">Repozytorium transakcji na czarnym rynku.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="districtDonRepository">Repozytorium donów dzielnic.</param>
    /// <param name="customRepositoryLoader">Interfejs do ładowania niestandardowych repozytoriów.</param>
    /// <param name="timer">AbpTimer do określania czasu cyklu pracy.</param>
    public BlackMarketWorker(
        IRepository<BlackMarketTransaction> blackMarketTransactionRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<District> districtRepository,
        IRepository<DistrictDon> districtDonRepository,
        ICustomRepositoryLoader customRepositoryLoader,
        AbpTimer timer
    )
        : base(timer)
    {
        _blackMarketTransactionRepository = blackMarketTransactionRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _districtRepository = districtRepository;
        _districtDonRepository = districtDonRepository;
        _customRepositoryLoader = customRepositoryLoader;
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
    /// Przygotowuje dane potrzebne do wystawiania i skupowania transakcji na czarnym rynku.
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje
    /// CustomRepositoryFactory potrzebuje jednostkę pracy
    /// </summary>
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        AllBlackMarketTransactions = _blackMarketTransactionRepository.GetAllList();
        AllGeneratedTypes = _generatedTypeRepository.GetAllList();
        AllPlantationStorages = _plantationStorageRepository.GetAllList();
        AllDistricts = _districtRepository.GetAllList();
        AllDistrictDons = _districtDonRepository.GetAllList();

        Buy();
        Issue();
    }

    /// <summary>
    /// Skupuje transakcje na czarnym rynku.
    /// </summary>
    private void Buy()
    {
        var bought = new List<BlackMarketTransaction>();
        var groupedByDistrict = AllBlackMarketTransactions.GroupBy(item => item.DistrictId);
        foreach (var group in groupedByDistrict)
        {
            try
            {
                var district = AllDistricts.Single(item => item.Id == group.Key);
                if (!district.IsDefined) continue;
                
                var districtTransactions = group.ToList();
                if (districtTransactions.Count <= district.MaxBlackMarketTransactionsCount) continue;
                foreach (var transaction in districtTransactions)
                {
                    var randomInt = _random.Next(1, 100);
                    if (randomInt <= district.ChanceForAddBuyBlackMarketTransaction) 
                        bought.Add(transaction);
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

        if (!bought.Any()) return;
        
        var groupedBought = bought.GroupBy(item => item.DistrictId);
        foreach (var group in groupedBought)
        {
            try
            {
                var drawnTransactions = new List<BlackMarketTransaction>();
                var districtTransactions = group.ToList();
                var district = AllDistricts.Single(item => item.Id == group.Key);
            
                for (var i = 0; i < district.NumberDrawnTransactionsToBuy; i++)
                    drawnTransactions.Add(districtTransactions[_random.Next(districtTransactions.Count)]);
            
                if (district.BuyUsersTransactions)
                    drawnTransactions.AddRange(AllBlackMarketTransactions.Where(item => item.DistrictId == group.Key && item.SellerId != null));

                foreach (var transaction in drawnTransactions.GroupBy(item => item.Id).Select(item => item.First()))
                {
                    if (transaction.SellerId != null)
                    {
                        var cost = transaction.Price * transaction.Quantity;
                        var plantationStorage = AllPlantationStorages.Single(item =>
                            item.UserId == transaction.SellerId && item.DistrictId == transaction.DistrictId);
                        plantationStorage.Gold += cost;
                        var isDon = AllDistrictDons.FirstOrDefault(
                            item => item.DistrictId == plantationStorage.DistrictId &&
                                    item.PlantationStorageId == plantationStorage.Id) != null;
                        
                        TokensOperator.BlackMarketTokenProfit(district, plantationStorage, cost);
                        TokensOperator.DonTokenProfit(isDon, district, plantationStorage);
                    }

                    _blackMarketTransactionRepository.Delete(transaction.Id);
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
    /// Wystawia transakcje na czarnym rynku.
    /// </summary>
    private void Issue()
    {
        var toIssued = new List<IBlackMarketWorkerProduct>();
        foreach (var district in AllDistricts)
        {
            if (!district.IsDefined) continue;
            
            var districtTransactions = AllBlackMarketTransactions.Where(item => item.DistrictId == district.Id).ToList();
            if (districtTransactions.Count >= district.MaxBlackMarketTransactionsCount + 5) continue;
            var districtProductsGeneratedTypes = AllGeneratedTypes.Where(item =>
                item.DistrictId == district.Id && item.EntityName != EntitiesDbNames.Quest);

            foreach (var generatedType in districtProductsGeneratedTypes)
            {
                try
                {
                    var randomIntForBuy = _random.Next(1, 100);
                    if (randomIntForBuy > district.ChanceForAddIssueBlackMarketTransaction) continue;

                    var randomInt = _random.Next(1, 100);
                    var seedTypeChance = randomInt <= district.ChanceForIssueSeedAgainstDriedFruitInBlackMarket;
                    var entityName = generatedType.EntityName == EntitiesDbNames.Seed
                        ? seedTypeChance ? generatedType.EntityName : EntitiesDbNames.DriedFruit
                        : generatedType.EntityName;

                    var blackMarketItem = (IBlackMarketWorkerProduct) _customRepositoryLoader.GetRepository(entityName)
                        .GetWhere(RelationFieldsNames.GeneratedTypeId, generatedType.Id)
                        .Cast<IPlantationGeneratedEntity>()
                        .SingleOrDefault(item => item.PlantationStorageId == null);
                    if (blackMarketItem == null || !blackMarketItem.IsBlackMarketWorkerItem) continue;

                    toIssued.Add(blackMarketItem);
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

        if (!toIssued.Any()) return;

        var groupedIssued = toIssued.GroupBy(item => AllGeneratedTypes.Single(type => type.Id == item.GeneratedTypeId).DistrictId);
        foreach (var group in groupedIssued)
        {
            try
            {
                var drawnTransactions = new List<IBlackMarketWorkerProduct>();
                var districtIssued = group.ToList();
                var district = AllDistricts.Single(item => item.Id == group.Key);
                
                for (var i = 0; i < district.NumberDrawnTransactionsToIssue; i++)
                    drawnTransactions.Add(districtIssued[_random.Next(districtIssued.Count)]);

                foreach (var drawnItem in drawnTransactions.GroupBy(item => item.Id).Select(item => item.First()))
                {
                    var price = decimal.Parse(DecimalUtils.NextDecimal((decimal) drawnItem.BlackMarketMinSellPrice, (decimal) drawnItem.BlackMarketMaxSellPrice).ToString("0.##"));
                    var entityName = drawnItem.GetType().Name;

                    var newTransaction = new BlackMarketTransaction
                    {
                        ItemId = drawnItem.Id,
                        ItemName = drawnItem.Name, 
                        ItemEntityName = entityName,
                        SellerId = null,
                        GeneratedTypeId = drawnItem.GeneratedTypeId,
                        SellerName = "BOT",
                        Price = price,
                        Quantity = BlackMarketWorkerHelper.CalculateQuantity(entityName, district),
                        DistrictId = district.Id
                    };

                    var randomIntChanceForPayDonTribute = _random.Next(1, 100);
                    var districtDon = AllDistrictDons.FirstOrDefault(item => item.DistrictId == district.Id);
                    if (districtDon != null && randomIntChanceForPayDonTribute <= district.ChanceForPayDonTribute)
                    {
                        var donPlantationStorage = AllPlantationStorages.Single(item => item.Id == districtDon.PlantationStorageId);
                        var transactionCost = newTransaction.Price * newTransaction.Quantity;
                        var donTribute = district.DonCharityPercentage * transactionCost;
                        donPlantationStorage.Gold += donTribute;
                    }

                    _blackMarketTransactionRepository.Insert(newTransaction);
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
