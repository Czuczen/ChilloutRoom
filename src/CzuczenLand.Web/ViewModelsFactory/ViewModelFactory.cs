using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abp.ObjectMapping;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Others;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;
using CzuczenLand.Web.Models.ExtendingModels.Home;
using CzuczenLand.Web.Models.ExtendingModels.Plantation;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.GameStorage;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.StoragesInfo;
using CzuczenLand.Web.Models.ExtendingModels.TicTacToe;
using Newtonsoft.Json;

namespace CzuczenLand.Web.ViewModelsFactory;

public static class ViewModelFactory
{
    public static PlantationViewModel CreatePlantationViewModel(Plantation plantation, IObjectMapper objectMapper, ILogger logger)
    {
        var ret = new PlantationViewModel();
        var playerStorageInfo = new PlayerStorageInfoViewModel();
        var plantationStorageInfo = new PlantationStorageInfoViewModel();

        ret.UserId = plantation.PlantationStorage?.UserId ?? 0;
        ret.TooLowLevel = plantation.TooLowLevel;
        ret.DistrictLoadingMessage = plantation.DistrictLoadingMessage;
        ret.MustBuyHollow = plantation.MustBuyHollow;
        ret.HollowPrice = plantation.HollowPrice;
            
        ret.IsNewPlayer = plantation.IsNewPlayer;
        ret.S2DistrictsList = plantation.S2DistrictsList;
        ret.SelectedDistrictId = plantation.SelectedDistrictId;
            
        ret.PlayerStorageInfoViewModel = objectMapper.Map(plantation.PlayerStorage, playerStorageInfo);
        ret.PlantationStorageInfoViewModel = objectMapper.Map(plantation.PlantationStorage, plantationStorageInfo);
        ret.DonData = plantation.DonData;
            
        var activeBonuses = plantation.UserBonuses?.Where(item => item.IsActive).ToList() ?? new List<Bonus>();
        
        ret.PlantsInfo = CreatePlantsInfoViewModel(plantation, objectMapper, logger);
        ret.QuestsViewModel = CreateQuestsViewModel(plantation, objectMapper);
        ret.StorageViewModel = CreateStorageViewModel(plantation, objectMapper);
        ret.Bonuses = CreateBonusesViewModel(activeBonuses, objectMapper);
            
        return ret;
    }
        
    private static List<PlantViewModel> CreatePlantsInfoViewModel(Plantation plantation, IObjectMapper objectMapper, ILogger logger)
    {
        var ret = new List<PlantViewModel>();
        
        if (plantation.Plants == null) return ret;
        
        foreach (var plant in plantation.Plants)
        {
            try
            {
                var lamp = plantation.UserLamps.Single(item => item.Id == plant.LampId);
                var manure = plantation.UserManures.Single(item => item.Id == plant.ManureId);
                var soil = plantation.UserSoils.Single(item => item.Id == plant.SoilId);
                var water = plantation.UserWaters.Single(item => item.Id == plant.WaterId);
                var seed = plantation.UserSeeds.Single(item => item.Id == plant.SeedId);
                var pot = plantation.UserPots.Single(item => item.Id == plant.PotId);

                var plantInfoViewModel = CreatePlantViewModel(objectMapper, lamp, manure, soil, water, seed, pot, plant);
                ret.Add(plantInfoViewModel);
            }
            catch (Exception ex)
            {
                logger.Error("Blad =======//========", ex);
            }
        }

        return ret;
    }
        
    public static List<BonusViewModel> CreateBonusesViewModel(List<Bonus> activeBonuses, IObjectMapper objectMapper)
    {
        var ret = new List<BonusViewModel>();
        
        foreach (var bonus in activeBonuses)
            ret.Add(objectMapper.Map(bonus, new BonusViewModel()));

        return ret;
    }

    private static QuestsViewModel CreateQuestsViewModel(Plantation plantation, IObjectMapper objectMapper)
    {
        var ret = new QuestsViewModel();
        
        var filteredQuests = plantation.FilteredQuests;
        if (filteredQuests == null || !filteredQuests.Any()) return ret;

        var levelFromNoLongerTakeQuests = plantation.PlantationStorage.Level - plantation.District.LessLevelDoNotTakeQuests;
        var completedQuests = filteredQuests.Where(item => item.IsComplete && !item.InProgress);
        var inProgressQuests = filteredQuests.Where(item => item.InProgress && !item.IsComplete);
        var availableQuests = filteredQuests.Where(item => !item.InProgress && !item.IsComplete && item.PlantationLevelRequirement > levelFromNoLongerTakeQuests);

        foreach (var quest in inProgressQuests)
            ret.InProgressQuests.Add(CreateQuestInfoViewModel(quest, objectMapper, plantation.QuestInfoCreation));

        foreach (var quest in availableQuests)
            ret.AvailableQuests.Add(CreateQuestInfoViewModel(quest, objectMapper, plantation.QuestInfoCreation));

        foreach (var quest in completedQuests)
            ret.CompletedQuests.Add(CreateQuestInfoViewModel(quest, objectMapper, plantation.QuestInfoCreation));
            
        return ret;
    }

    public static QuestInfoViewModel CreateQuestInfoViewModel(Quest quest, IObjectMapper objectMapper, QuestInfoCreation questInfoCreation)
    {
        var questInfo = objectMapper.Map(quest, new QuestInfoViewModel());
        var questDefinition = questInfoCreation.QuestDefinitions.Single(item => item.GeneratedTypeId == quest.GeneratedTypeId);

        var relationsToDrops = questInfoCreation.DropQuestRelations.Where(item => item.QuestId == questDefinition.Id);
        foreach (var rel in relationsToDrops)
        {
            var drop = questInfoCreation.Drops.Single(item => item.Id == rel.DropId);
            questInfo.Drops.Add(objectMapper.Map(drop, new DropInfoViewModel()));    
        }

        var questProgress = questInfoCreation.QuestsRequirementsProgress.Single(item => item.QuestId == quest.Id);
        questInfo.RequirementsProgress = questProgress.RequirementsProgress;
        
        var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
        foreach (var reqProgress in requirementsProgress)
        {
            var req = questInfoCreation.Requirements.Single(item => item.Id == reqProgress.Key);
            questInfo.Requirements.Add(objectMapper.Map(req, new RequirementInfoViewModel()));
        }

        return questInfo;
    }

    public static PlantViewModel CreatePlantViewModel(IObjectMapper objectMapper, Lamp lamp, Manure manure, 
        Soil soil, Water water, Seed seed, Pot pot, Plant plant)
    {
        var ret = new PlantViewModel();
        
        var lampInfo = new LampInfoViewModel();
        var manureInfo = new ManureInfoViewModel();
        var soilInfo = new SoilInfoViewModel();
        var waterInfo = new WaterInfoViewModel();
        var seedInfo = new SeedInfoViewModel();
        var potInfo = new PotInfoViewModel();
        var plantInfo = new PlantInfoViewModel();

        ret.LampInfoViewModel = objectMapper.Map(lamp, lampInfo);
        ret.ManureInfoViewModel = objectMapper.Map(manure, manureInfo);
        ret.SoilInfoViewModel = objectMapper.Map(soil, soilInfo);
        ret.WaterInfoViewModel = objectMapper.Map(water, waterInfo);
        ret.SeedInfoViewModel = objectMapper.Map(seed, seedInfo);
        ret.PotInfoViewModel = objectMapper.Map(pot, potInfo);
        ret.PlantInfoViewModel = objectMapper.Map(plant, plantInfo);

        return ret;
    }

    private static StorageViewModel CreateStorageViewModel(Plantation plantation, IObjectMapper objectMapper)
    {
        var ret = new StorageViewModel();
        
        if (plantation.PlantationStorage == null) return ret;
            
        foreach (var driedFruit in plantation.UserDriedFruits)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new DriedFruitInfoViewModel();
                
            var mappedObj = objectMapper.Map(driedFruit, storageProduct);
            var mappedObjInfo = objectMapper.Map(driedFruit, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.DriedFruit;
                
            ret.DriedFruits.Add(mappedObj);
        }
            
        foreach (var lamp in plantation.UserLamps)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new LampInfoViewModel();
                
            var mappedObj = objectMapper.Map(lamp, storageProduct);
            var mappedObjInfo = objectMapper.Map(lamp, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.Lamp;
                
            ret.Lamps.Add(mappedObj);
        }
            
        foreach (var manure in plantation.UserManures)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new ManureInfoViewModel();
                
            var mappedObj = objectMapper.Map(manure, storageProduct);
            var mappedObjInfo = objectMapper.Map(manure, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.Manure;
                
            ret.Manures.Add(mappedObj);
        }
            
        foreach (var pot in plantation.UserPots)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new PotInfoViewModel();
                
            var mappedObj = objectMapper.Map(pot, storageProduct);
            var mappedObjInfo = objectMapper.Map(pot, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.Pot;
                
            ret.Pots.Add(mappedObj);
        }
            
        foreach (var seed in plantation.UserSeeds)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new SeedInfoViewModel();
                
            var mappedObj = objectMapper.Map(seed, storageProduct);
            var mappedObjInfo = objectMapper.Map(seed, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.Seed;
                
            ret.Seeds.Add(mappedObj);
        }
            
        foreach (var soil in plantation.UserSoils)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new SoilInfoViewModel();
                
            var mappedObj = objectMapper.Map(soil, storageProduct);
            var mappedObjInfo = objectMapper.Map(soil, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.Soil;
                
            ret.Soils.Add(mappedObj);
        }
            
        foreach (var water in plantation.UserWaters)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new WaterInfoViewModel();
                
            var mappedObj = objectMapper.Map(water, storageProduct);
            var mappedObjInfo = objectMapper.Map(water, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.Water;
                
            ret.Waters.Add(mappedObj);
        }
            
        foreach (var water in plantation.UserBonuses)
        {
            var storageProduct = new StorageProductViewModel();
            var objInfo = new BonusInfoViewModel();
                
            var mappedObj = objectMapper.Map(water, storageProduct);
            var mappedObjInfo = objectMapper.Map(water, objInfo);
            
            mappedObj.ProductInfo = mappedObjInfo;
            mappedObj.EntityName = EntitiesDbNames.Bonus;
                
            ret.Bonuses.Add(mappedObj);
        }
            
        foreach (var quest in plantation.QuestInfoCreation.QuestDefinitions)
            ret.QuestsInfo.Add(CreateQuestInfoViewModel(quest, objectMapper, plantation.QuestInfoCreation));

        foreach (var district in plantation.AvailableDistricts)
        {
            var districtInfo = objectMapper.Map(district, new DistrictViewModel());
            districtInfo.WardenName = plantation.WardensNames[district.Id];
            ret.Districts.Add(districtInfo);
        }
        
        ret.CurrencyExchanges = CreateCurrencyExchangesViewModel(plantation.CurrencyExchanges, objectMapper);
        ret.PlantationGold = plantation.PlantationStorage.Gold.ToString(CultureInfo.InvariantCulture);
        ret.PlayerGold = plantation.PlayerStorage.Gold;
            
        return ret;
    }

    private static List<CurrencyExchangeViewModel> CreateCurrencyExchangesViewModel(List<CurrencyExchange> currencyExchanges, IObjectMapper objectMapper)
    {
        var ret = new List<CurrencyExchangeViewModel>();
        
        foreach (var currency in currencyExchanges)
        {
            var mappedCurrency = objectMapper.Map(currency, new CurrencyExchangeViewModel());
            ret.Add(mappedCurrency);
        }
            
        return ret;
    }
        
    public static TicTacToeViewModel CreateTicTacToeViewModel(TicTacToeStorage storage, IObjectMapper objectMapper) =>
        objectMapper.Map(storage, new TicTacToeViewModel());

    public static StructureTestsViewModel CreateStructureTestsViewModel(List<StructureTest> structureTests)
    {
        var structureViewModel = new StructureTestsViewModel();
        structureViewModel.StructureTests = structureTests;
            
        return structureViewModel;
    }
        
    public static List<NewsViewModel> CreateNewsViewModel(List<News> allNews, IObjectMapper objectMapper)
    {
        var ret = new List<NewsViewModel>();
        
        if (allNews == null || allNews.Count <= 0) return ret;
            
        foreach (var news in allNews)
        {
            var newsViewModel = new NewsViewModel();
            var mappedNews = objectMapper.Map(news, newsViewModel);
                    
            ret.Add(mappedNews);
        }

        return ret;
    }

    public static List<NewsViewModel> CreateDistrictNewsViewModel(List<District> districtsToRelease)
    {
        var ret = new List<NewsViewModel>();
        
        if (districtsToRelease == null || districtsToRelease.Count <= 0) return ret;

        foreach (var district in districtsToRelease)
        {
            var news = new NewsViewModel
            {
                Header = "DZIELNICA " + district.Name,
                TextColor = "black",
                ImageUrl = "https://gethome-s3.s3.amazonaws.com/media/rp/investment/7886_nowa-5-dzielnica-zdjecie-inwestycji_4cDAVNM.jpg",
                MessageContentOne = "Start " + district.StartTime.ToString("G") + (district.EndTime != null ? " Koniec " + district.EndTime?.ToString("G") : ""),
                MessageContentTwo = (district.EndTime != null ? "Dzielnica ograniczona czasowo. " : "") + "Szybkość więdnięcia " + district.WiltSpeed + "/sec. Exp do pierwszego poziomu " + district.StartExpToNextLevel,
                MessageContentThree = "Koszt dziupli " + district.HollowPrice + ". Początkowa kasa plantacji " + district.StartGold,
            };

            ret.Add(news);
        }

        return ret;
    }

    public static NewsViewModel CreateTopPlantationPlayersNewsViewModel(List<PlayerStorage> topPlayers)
    {
        var topPlayerNews = new NewsViewModel
        {
            Header = "Top 10 graczy plantacji",
            TextColor = "whitesmoke",
            ImageUrl = "https://www.gry-online.pl/Galeria/Html/Wiadomosci/9894296.jpg",
        };

        for (var i = 0; i < topPlayers.Count; i++)
        {
            if (i <= 2)
                topPlayerNews.MessageContentOne += topPlayers[i].PlayerName + " ";
            else if (i < 6)
                topPlayerNews.MessageContentTwo += topPlayers[i].PlayerName + " ";
            else
                topPlayerNews.MessageContentThree += topPlayers[i].PlayerName + " ";
        }

        return topPlayerNews;
    }
        
    public static NewsViewModel CreateTopTicTacToePlayersNewsViewModel(List<TicTacToeStorage> topPlayers)
    {
        var topPlayerNews = new NewsViewModel
        {
            Header = "Top 10 graczy w kółko i krzyżyk",
            TextColor = "black",
            ImageUrl = "https://annadesign.com.pl/wp-content/uploads/2020/08/kolko-i-krzyzyk.jpg",
        };

        for (var i = 0; i < topPlayers.Count; i++)
        {
            if (i <= 2)
                topPlayerNews.MessageContentOne += topPlayers[i].PlayerName + " ";
            else if (i < 6)
                topPlayerNews.MessageContentTwo += topPlayers[i].PlayerName + " ";
            else
                topPlayerNews.MessageContentThree += topPlayers[i].PlayerName + " ";
        }

        return topPlayerNews;
    }

    public static EditOrCreateViewModel CreateEditOrCreateViewModel(EditOrCreate editOrCreate, IObjectMapper objectMapper) =>
        objectMapper.Map(editOrCreate, new EditOrCreateViewModel());
        
    public static LogsViewModel CreateLogsViewModel(Logs logs, IObjectMapper objectMapper) => 
        objectMapper.Map(logs, new LogsViewModel());

    public static DistrictCloneViewModel DistrictCloneViewModel(List<DistrictContext> clones)
    {
        var ret = new DistrictCloneViewModel();
        ret.DistrictContexts = clones;
        
        return ret;
    }
}
