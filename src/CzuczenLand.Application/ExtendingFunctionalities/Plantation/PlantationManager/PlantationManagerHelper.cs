using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Timing;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.ExtendingModels.Models.Shared;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager;

public static class PlantationManagerHelper
{
    public static string GetHrPropName(PropertyInfo currProp)
    {
        var propName = currProp?.CustomAttributes?.SingleOrDefault(currAttr =>
                currAttr?.AttributeType?.FullName?.Contains("DisplayNameAttribute") ?? false)?.ConstructorArguments
            .First().Value.ToString();

        return propName;
    }
    
    public static List<string> GetHrPropList(Type type)
    {
        var props = type?.GetProperties().Select(currProp => currProp.CustomAttributes?.SingleOrDefault(currAttr => 
                currAttr?.AttributeType?.FullName?.Contains("DisplayNameAttribute") ?? false)?.ConstructorArguments.First().Value.ToString())
            .Select(currPropName => currPropName ?? "").ToList();

        return props;
    }
        
    public static List<string> GetPropList(Type type)
    {
        var props = type?.GetProperties().Select(currProp => currProp.Name).ToList();
            
        return props;
    }

    public static string GetInputStepForProduct(string itemEntityName)
    {
        string ret;
        switch (itemEntityName)
        {
            case EntitiesDbNames.DriedFruit:
                ret = "0.01";
                break;
            case EntitiesDbNames.Lamp:
                ret = "1";
                break;
            case EntitiesDbNames.Manure:
                ret = "0.01";
                break;
            case EntitiesDbNames.Pot:
                ret = "1";
                break;
            case EntitiesDbNames.Seed:
                ret = "1";
                break;
            case EntitiesDbNames.Soil:
                ret = "0.01";
                break;
            case EntitiesDbNames.Water:
                ret = "0.01";
                break;
            case EntitiesDbNames.Bonus:
                ret = "1";
                break;
            default:
                throw new ArgumentOutOfRangeException(itemEntityName);
        }

        return ret;
    }

    public static string GetMeasureUnitByType(Type type)
    {
        return GetMeasureUnitByEntityName(type?.Name);
    }
    
    public static string GetMeasureUnitByEntityName(string entityName)
    {
        string ret;
        switch (entityName)
        {
            case EntitiesDbNames.DriedFruit:
                ret = "g";
                break;
            case EntitiesDbNames.Lamp:
                ret = "szt.";
                break;
            case EntitiesDbNames.Manure:
                ret = "g";
                break;
            case EntitiesDbNames.Pot:
                ret = "szt.";
                break;
            case EntitiesDbNames.Seed:
                ret = "szt.";
                break;
            case EntitiesDbNames.Soil:
                ret = "kg";
                break;
            case EntitiesDbNames.Water:
                ret = "l";
                break;
            case EntitiesDbNames.Bonus:
                ret = "szt.";
                break;
            default:
                throw new ArgumentOutOfRangeException(entityName);
        }

        return ret;
    }
        
    public static string GetMeasureUnitForCurrencyExchange(string currencyName)
    {
        string ret;
        switch (currencyName)
        {
            case PlantationStorageObservedFields.Gold:
                ret = "$";
                break;
            case PlantationStorageObservedFields.QuestToken:
                ret = "szt.";
                break;
            case PlantationStorageObservedFields.DealerToken:
                ret = "szt.";
                break;
            case PlantationStorageObservedFields.BlackMarketToken:
                ret = "szt.";
                break;
            case PlantationStorageObservedFields.DonToken:
                ret = "szt.";
                break;
            case PlantationStorageObservedFields.UnlockToken:
                ret = "szt.";
                break;
            case PlayerStorageObservedFields.Honor:
                ret = "pkt";
                break;
            case PlantationStorageObservedFields.Prestige:
                ret = "pkt";
                break;
            default:
                throw new ArgumentOutOfRangeException(currencyName);
        }

        return ret;
    }
        
    public static List<BlackMarketSellItem> GenerateBlackMarketSellItems<TEntity>(List<TEntity> items) 
        where TEntity : Product
    {
        var ret = new List<BlackMarketSellItem>();

        if (items != null && items.Count > 0)
        {
            var itemsEntityName = items.First().GetType().Name;
            foreach (var item in items)
            {
                var blackMarketSellItem = new BlackMarketSellItem
                {
                    ItemId = item.Id,
                    ItemName = item.Name,
                    ItemEntityName = itemsEntityName,
                    BlackMarketMinSellPrice = (decimal) item.BlackMarketMinSellPrice,
                    BlackMarketMaxSellPrice = (decimal) item.BlackMarketMaxSellPrice,
                    OwnedAmount = item.OwnedAmount,
                    QuantityInputStep = GetInputStepForProduct(itemsEntityName)
                };
                
                ret.Add(blackMarketSellItem);
            }   
        }

        return ret;
    }
    
    /// <summary>
    /// Zadania które ma ustawiony czas startu później niż końca nie da się rozpocząć.
    /// </summary>
    /// <param name="quest"></param>
    /// <param name="plantationStorage"></param>
    /// <param name="questHub"></param>
    /// <returns></returns>
    public static bool CheckQuestLimitsAndSetProgressStatus(Quest quest, PlantationStorage plantationStorage, IHubContext questHub)
    {
        var ret = false;
        var currDateTime = Clock.Now;
        var userId = plantationStorage.UserId;
            
        switch (quest.QuestType)
        {
            case DbQuestTypesNames.Achievement:
            case DbQuestTypesNames.Others:
                quest.InProgress = true;
                break;
            case DbQuestTypesNames.Daily:
                if (plantationStorage.StartedDailyQuestsCount < plantationStorage.UnlockedDailyQuestsCount)
                {
                    quest.InProgress = true;
                    plantationStorage.StartedDailyQuestsCount++;
                }
                else
                {
                    questHub.Clients.User(userId.ToString()).theLimitExpired(quest.Id, quest.QuestType, "Dzienny limit wykorzystany");
                    ret = true;
                }
                break;
            case DbQuestTypesNames.Weekly:
                if (plantationStorage.StartedWeeklyQuestsCount < plantationStorage.UnlockedWeeklyQuestsCount)
                {
                    quest.InProgress = true;
                    plantationStorage.StartedWeeklyQuestsCount++;
                }
                else
                {
                    questHub.Clients.User(userId.ToString()).theLimitExpired(quest.Id, quest.QuestType, "Tygodniowy limit wykorzystany");
                    ret = true;
                }
                break;
            case DbQuestTypesNames.Event:
                if (quest.StartTime <= currDateTime && quest.EndTime > currDateTime)
                {
                    quest.InProgress = true;    
                }
                else
                {
                    var phrase = quest.EndTime < Clock.Now ? "było" : "będzie";
                    var message = "Zadanie " + phrase +" dostępne od " + quest.StartTime + " do " + quest.EndTime;
                    questHub.Clients.User(userId.ToString()).theLimitExpired(quest.Id, quest.QuestType, message);
                    ret = true;
                }
                break;
        }

        return ret;
    }
        
    public static void SetCurrencyExchanges(Plantation plantation)
    {
        var plantationStorage = plantation.PlantationStorage;
            
        var goldExchangeRate = plantation.District.GoldExchangeRate;
        var questTokenExchangeRate = plantation.District.QuestTokenExchangeRate;
        var unlockTokenExchangeRate = plantation.District.UnlockTokenExchangeRate;
        var donTokenExchangeRate = plantation.District.DonTokenExchangeRate;
        var dealerTokenExchangeRate = plantation.District.DealerTokenExchangeRate;
        var blackMarketTokenExchangeRate = plantation.District.BlackMarketTokenExchangeRate;
        var prestigeExchangeRate = plantation.District.PrestigeExchangeRate;
        var currencyExchanges = PlantationStorageObservedFields.CurrencyExchanges;

        foreach (var currencyName in currencyExchanges)
        {
            const string startMessage = "Tu wymienisz ";
            const string endMessage = " na $ plantacji.";
            CurrencyExchange currency = null;
            switch (currencyName)
            {
                case PlantationStorageObservedFields.Gold:
                    if (plantation.District.EndTime == null)
                    {
                        currency = CreateCurrencyExchange(PlantationStorageObservedFields.Gold,
                            "Tu wymienisz $ plantacji na $ gracza i na odwrót.",
                            goldExchangeRate, plantationStorage.Id, plantation.PlayerStorage.Gold, 1, goldExchangeRate,
                            false);
                    }
                    break;
                case PlantationStorageObservedFields.UnlockToken:
                    currency = CreateCurrencyExchange(PlantationStorageObservedFields.UnlockToken,
                        startMessage + PlantationStorageFieldsHrNames.UnlockToken + endMessage,
                        unlockTokenExchangeRate, plantationStorage.Id, plantationStorage.UnlockToken, unlockTokenExchangeRate,
                        1, true);
                    break;
                case PlantationStorageObservedFields.DonToken:
                    currency = CreateCurrencyExchange(PlantationStorageObservedFields.DonToken,
                        startMessage + PlantationStorageFieldsHrNames.DonToken + endMessage,
                        donTokenExchangeRate, plantationStorage.Id, plantationStorage.DonToken, donTokenExchangeRate,
                        1, true);
                    break;
                case PlantationStorageObservedFields.QuestToken:
                    currency = CreateCurrencyExchange(PlantationStorageObservedFields.QuestToken,
                        startMessage + PlantationStorageFieldsHrNames.QuestToken + endMessage,
                        questTokenExchangeRate, plantationStorage.Id, plantationStorage.QuestToken, questTokenExchangeRate,
                        1, true);
                    break;
                case PlantationStorageObservedFields.DealerToken:
                    currency = CreateCurrencyExchange(PlantationStorageObservedFields.DealerToken,
                        startMessage + PlantationStorageFieldsHrNames.DealerToken + endMessage,
                        dealerTokenExchangeRate, plantationStorage.Id, plantationStorage.DealerToken, dealerTokenExchangeRate,
                        1, true);
                    break;
                case PlantationStorageObservedFields.BlackMarketToken:
                    currency = CreateCurrencyExchange(PlantationStorageObservedFields.BlackMarketToken,
                        startMessage + PlantationStorageFieldsHrNames.BlackMarketToken + endMessage,
                        blackMarketTokenExchangeRate, plantationStorage.Id, plantationStorage.BlackMarketToken, blackMarketTokenExchangeRate,
                        1, true);
                    break;
                case PlantationStorageObservedFields.Prestige:
                    currency = CreateCurrencyExchange(PlantationStorageObservedFields.Prestige,
                        startMessage + PlantationStorageFieldsHrNames.Prestige + endMessage,
                        prestigeExchangeRate, plantationStorage.Id, plantationStorage.Prestige, prestigeExchangeRate,
                        1, true);
                    break;
            }

            if (currency != null)
                plantation.CurrencyExchanges.Add(currency);
        }
    }

    private static CurrencyExchange CreateCurrencyExchange(string name, string description, int exchangeRate, int plantationStorageId,
        decimal ownedAmount, decimal sellPrice, decimal buyPrice, bool isOnlyToPlantationGoldTransfer)
    {
        return new CurrencyExchange
        {
            CurrencyName = name,
            Description = description,
            ExchangeRate = exchangeRate,
            PlantationStorageId = plantationStorageId,
            OwnedAmount = ownedAmount,
            SellPrice = sellPrice,
            BuyPrice = buyPrice,
            IsOnlyToPlantationGoldTransfer = isOnlyToPlantationGoldTransfer
        };
    }

    public static void AddLevels(District district, PlantationStorage playerPlantationStorage, decimal gainedExp, List<string> dropsNotification, PlayerStorage playerStorage,
        bool isManyLevel = false, decimal tempPlantGainedExp = decimal.MinValue, decimal tempExpToNexLevel = decimal.MinValue)
    {
        if ( !isManyLevel ? (playerPlantationStorage.CurrExp + gainedExp) >= playerPlantationStorage.ExpToNextLevel : tempPlantGainedExp >= tempExpToNexLevel)
        {
            playerPlantationStorage.Level++;
            if (district.EndTime == null)
                playerStorage.Level++;
                
            tempPlantGainedExp = isManyLevel ? tempPlantGainedExp - tempExpToNexLevel : (playerPlantationStorage.CurrExp + gainedExp) - playerPlantationStorage.ExpToNextLevel;
            tempExpToNexLevel = isManyLevel ? tempExpToNexLevel * 2 : playerPlantationStorage.ExpToNextLevel * 2;
                
            if (tempPlantGainedExp >= tempExpToNexLevel)
            {
                AddLevels(district, playerPlantationStorage, gainedExp, dropsNotification, playerStorage, true, tempPlantGainedExp, tempExpToNexLevel);
            }
            else
            {
                playerPlantationStorage.CurrExp = tempPlantGainedExp;
                playerPlantationStorage.ExpToNextLevel = tempExpToNexLevel;
 
                playerStorage.GainedExperience += gainedExp;
                playerPlantationStorage.GainedExperience += gainedExp;

                dropsNotification.Add("Otrzymano " + gainedExp + "pkt doświadczenia");
            }
        }
        else
        {
            playerStorage.GainedExperience += gainedExp;
            playerPlantationStorage.GainedExperience += gainedExp;
            playerPlantationStorage.CurrExp += gainedExp;
                
            dropsNotification.Add("Otrzymano " + gainedExp + "pkt doświadczenia");
        }
    }

    public static async Task ProcessCollectPlantDrops(DriedFruit playerDriedFruit, Plant plant, List<string> dropsNotification, 
        Seed playerSeed, District district, PlantationStorage playerPlantationStorage, Random random, IIgnoreChangeService ignoreChangeService)
    {
        playerDriedFruit.OwnedAmount += plant.DriedFruitAmount;
        dropsNotification.Add("Otrzymano " + plant.DriedFruitAmount + 
                              GetMeasureUnitByType(typeof(DriedFruit)) + " suszu");

        var randomInt = random.Next(1, 100);
        if (randomInt <= plant.ChanceForSeed)
        {
            playerSeed.OwnedAmount += plant.SeedAmount;
            await ignoreChangeService.Add(playerSeed);
            dropsNotification.Add("Otrzymano " + plant.SeedAmount + GetMeasureUnitByType(typeof(Seed)) + " nasion");
        }
        else
        {
            dropsNotification.Add("Tym razem nie udało się zdobyć nasionek" + " Wylosowano " + randomInt + 
                                  ". Procent szansy " + plant.ChanceForSeed);
        }

        TokensOperator.UnlockTokenProfit(district, playerPlantationStorage, dropsNotification);
    }

    public static void AddGainedCurrency(Drop drop, CompleteQuest completeQuest)
    {
        if (drop.Prestige != null)
            completeQuest.GainedPrestige += (int) drop.Prestige;
        if (drop.Experience != null)
            completeQuest.GainedExp += (decimal) drop.Experience;
        if (drop.Gold != null)
            completeQuest.GainedGold += (decimal) drop.Gold;
        if (drop.QuestToken != null)
            completeQuest.GainedQuestTokens += (int) drop.QuestToken;
        if (drop.DealerToken != null)
            completeQuest.GainedDealerTokens += (int) drop.DealerToken;
        if (drop.BlackMarketToken != null)
            completeQuest.GainedBlackMarketTokens += (int) drop.BlackMarketToken;
        if (drop.DonToken != null)
            completeQuest.GainedDonTokens += (int) drop.DonToken;
        if (drop.UnlockToken != null)
            completeQuest.GainedUnlockTokens += (int) drop.UnlockToken;
        if (drop.Honor != null)
            completeQuest.GainedHonor += (int) drop.Honor;
    }

    public static void SetReceivedItemsMessages(CompleteQuest completeQuest)
    {
        var startMessage = "Otrzymano ";
        if (completeQuest.GainedPrestige > 0)
            completeQuest.DropsNotification.Add(startMessage +
                                                PlantationStorageFieldsHrNames.Prestige + ": " +
                                                completeQuest.GainedPrestige +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlantationStorageObservedFields.Prestige));

        if (completeQuest.GainedGold > 0)
            completeQuest.DropsNotification.Add(startMessage + completeQuest.GainedGold +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlantationStorageObservedFields.Gold));

        if (completeQuest.GainedQuestTokens > 0)
            completeQuest.DropsNotification.Add(startMessage +
                                                PlantationStorageFieldsHrNames.QuestToken +
                                                ": " + completeQuest.GainedQuestTokens +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlantationStorageObservedFields.QuestToken));

        if (completeQuest.GainedDealerTokens > 0)
            completeQuest.DropsNotification.Add(startMessage +
                                                PlantationStorageFieldsHrNames.DealerToken +
                                                ": " + completeQuest.GainedDealerTokens +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlantationStorageObservedFields.DealerToken));

        if (completeQuest.GainedBlackMarketTokens > 0)
            completeQuest.DropsNotification.Add(startMessage +
                                                PlantationStorageFieldsHrNames
                                                    .BlackMarketToken + ": " +
                                                completeQuest.GainedBlackMarketTokens +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlantationStorageObservedFields
                                                        .BlackMarketToken));

        if (completeQuest.GainedDonTokens > 0)
            completeQuest.DropsNotification.Add(startMessage +
                                                PlantationStorageFieldsHrNames.DonToken + ": " +
                                                completeQuest.GainedDonTokens +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlantationStorageObservedFields.DonToken));

        if (completeQuest.GainedUnlockTokens > 0)
            completeQuest.DropsNotification.Add(startMessage +
                                                PlantationStorageFieldsHrNames.UnlockToken +
                                                ": " + completeQuest.GainedUnlockTokens +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlantationStorageObservedFields.UnlockToken));

        if (completeQuest.GainedHonor > 0)
            completeQuest.DropsNotification.Add(startMessage +
                                                PlayerStorageFieldsHrNames.Honor + ": " +
                                                completeQuest.GainedHonor +
                                                GetMeasureUnitForCurrencyExchange(
                                                    PlayerStorageObservedFields.Honor));
    }

    public static bool CheckRequirements(CreatePlant createPlant, out string message)
    {
        message = "";
        var lampOk = false;
        var manureOk = false;
        var soilOk = false;
        var waterOk = false;
        var seedOk = false;
        var soilOwnedAmountOk = false;
        var soilClassOk = false;
        var lampAmountOk = false;
        var manureAmountOk = false;
        var soilAmountOk = false;
        var waterAmountOk = false;
        var seedAmountOk = false;
        var potAmountOk = false;
            
        var potCapacity = createPlant.Pot.Capacity;

        if (createPlant.Lamp.CapacityInPotRequirement <= potCapacity)
            lampOk = true;
        else
            message += "Lampa wymaga większej doniczki. ";

        if (createPlant.Manure.CapacityInPotRequirement <= potCapacity)
            manureOk = true;
        else
            message += "Nawóz wymaga większej doniczki. ";
            
        if (createPlant.Soil.CapacityInPotRequirement <= potCapacity)
            soilOk = true;
        else
            message += "Gleba wymaga większej doniczki. ";
            
        if (createPlant.Water.CapacityInPotRequirement <= potCapacity)
            waterOk = true;
        else
            message += "Woda wymaga większej doniczki. ";
            
        if (createPlant.Seed.CapacityInPotRequirement <= potCapacity)
            seedOk = true;
        else
            message += "Nasiono wymaga większej doniczki. ";
            
        if (createPlant.Soil.CapacityInPotRequirement <= potCapacity)
            soilOwnedAmountOk = true;
        else
            message += "Gleba wymaga większej doniczki. ";

        if (createPlant.Soil.SoilClass <= createPlant.Pot.MaxRangeOfSoilClass)
            soilClassOk = true;
        else
            message += "Klasa gleby jest za wysoka dla doniczki. ";
            
        if (createPlant.Lamp.OwnedAmount > 0)
            lampAmountOk = true;
        else
            message += "Brak wymaganej ilości lamp. ";

        if ((createPlant.Manure.OwnedAmount - createPlant.Seed.ManureConsumption) >= 0)
            manureAmountOk = true;
        else
            message += "Brak wymaganej ilości nawozu. ";

        if ((createPlant.Soil.OwnedAmount - createPlant.Pot.Capacity) >= 0)
            soilAmountOk = true;
        else
            message += "Brak wymaganej ilości gleby. ";

        if ((createPlant.Water.OwnedAmount - createPlant.Seed.WaterConsumption) >= 0)
            waterAmountOk = true;
        else
            message += "Brak wymaganej ilości wody. ";

        if ((createPlant.Seed.OwnedAmount - 1) >= 0)
            seedAmountOk = true;
        else
            message += "Brak wymaganej ilości nosion. " ;

        if ((createPlant.Pot.OwnedAmount - 1) >= 0)
            potAmountOk = true;
        else
            message += "Brak wymaganej ilości doniczek. ";

        return lampOk && manureOk && soilOk && waterOk && seedOk && soilOwnedAmountOk && soilClassOk && lampAmountOk 
               && manureAmountOk && soilAmountOk && waterAmountOk && seedAmountOk && potAmountOk;
    }
       
    public static bool AllRequirementsIsDone(CompleteQuest completeQuest)
    {
        var ret = true;
        var questProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(completeQuest.QuestInfoCreation
            .QuestsRequirementsProgress.First().RequirementsProgress);
        
        foreach (var reqProgress in questProgress)
        {
            var req = completeQuest.QuestInfoCreation.Requirements.Single(item => item.Id == reqProgress.Key);
            if (req.Amount == reqProgress.Value) continue;
            
            ret = false;
            break;
        }

        return ret;

    }
}