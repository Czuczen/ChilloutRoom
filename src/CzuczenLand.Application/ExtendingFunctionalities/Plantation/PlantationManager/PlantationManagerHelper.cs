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

/// <summary>
/// Pomocnicza klasa do zarządzania plantacjami.
/// </summary>
public static class PlantationManagerHelper
{
    /// <summary>
    /// Pobiera czytelną nazwę dla podanej właściwości.
    /// </summary>
    /// <param name="currProp">Informacje o bieżącej właściwości.</param>
    /// <returns>Czytelna nazwa właściwości lub null, jeśli nie znaleziono.</returns>
    public static string GetHrPropName(PropertyInfo currProp)
    {
        var propName = currProp?.CustomAttributes?.SingleOrDefault(currAttr =>
                currAttr?.AttributeType?.FullName?.Contains("DisplayNameAttribute") ?? false)?.ConstructorArguments
            .First().Value.ToString();

        return propName;
    }
    
    /// <summary>
    /// Pobiera listę czytelnych nazw właściwości dla podanego typu.
    /// </summary>
    /// <param name="type">Typ, dla którego mają być pobrane właściwości HR.</param>
    /// <returns>Lista nazw właściwości HR lub pusta lista, jeśli nie znaleziono.</returns>
    public static List<string> GetHrPropList(Type type)
    {
        var props = type?.GetProperties().Select(currProp => currProp.CustomAttributes?.SingleOrDefault(currAttr => 
                currAttr?.AttributeType?.FullName?.Contains("DisplayNameAttribute") ?? false)?.ConstructorArguments.First().Value.ToString())
            .Select(currPropName => currPropName ?? "").ToList();

        return props;
    }
        
    /// <summary>
    /// Pobiera listę nazw wszystkich właściwości dla podanego typu.
    /// </summary>
    /// <param name="type">Typ, dla którego mają być pobrane nazwy właściwości.</param>
    /// <returns>Lista nazw wszystkich właściwości.</returns>
    public static List<string> GetPropList(Type type)
    {
        var props = type?.GetProperties().Select(currProp => currProp.Name).ToList();
            
        return props;
    }

    /// <summary>
    /// Określa minimalną jednostkę zmiany dla wartości pola formularza.
    /// </summary>
    /// <param name="itemEntityName">Nazwa encji.</param>
    /// <returns>Wartość minimalna kroku.</returns>
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

    /// <summary>
    /// Pobiera jednostkę miary na podstawie typu encji.
    /// </summary>
    /// <param name="type">Typ encji.</param>
    /// <returns>Jednostka miary jako ciąg znaków.</returns>
    public static string GetMeasureUnitByType(Type type)
    {
        return GetMeasureUnitByEntityName(type?.Name);
    }
    
    /// <summary>
    /// Pobiera jednostkę miary na podstawie nazwy encji.
    /// </summary>
    /// <param name="entityName">Nazwa encji.</param>
    /// <returns>Jednostka miary jako ciąg znaków.</returns>
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
        
    /// <summary>
    /// Pobiera jednostkę miary dla wymiany walut na podstawie nazwy waluty.
    /// </summary>
    /// <param name="currencyName">Nazwa waluty.</param>
    /// <returns>Jednostka miary jako ciąg znaków.</returns>
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
        
    /// <summary>
    /// Generuje listę elementów do sprzedaży na czarnym rynku na podstawie listy produktów.
    /// </summary>
    /// <typeparam name="TEntity">Typ produktu dziedziczący po klasie Product.</typeparam>
    /// <param name="items">Lista produktów.</param>
    /// <returns>Lista elementów do sprzedaży na czarnym rynku.</returns>
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
    /// Sprawdza limity i dostępność zadania.
    /// Zadania które ma ustawiony czas startu później niż końca nie da się rozpocząć.
    /// </summary>
    /// <param name="quest">Zadanie do sprawdzenia.</param>
    /// <param name="plantationStorage">Magazyn plantacji.</param>
    /// <param name="questHub">Kontekst huba dla zadań.</param>
    /// <returns>True, jeśli limit został przekroczony; w przeciwnym razie false.</returns>
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
        
    /// <summary>
    /// Ustawia wymianę walut.
    /// </summary>
    /// <param name="plantation">Informacje o plantacji.</param>
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

    /// <summary>
    /// Tworzy nową instancję wymiany waluty.
    /// </summary>
    /// <param name="name">Nazwa waluty.</param>
    /// <param name="description">Opis wymiany waluty.</param>
    /// <param name="exchangeRate">Kurs wymiany..</param>
    /// <param name="plantationStorageId">Identyfikator magazynu plantacji.</param>
    /// <param name="ownedAmount">Ilość posiadanej waluty.</param>
    /// <param name="sellPrice">Cena sprzedaży waluty.</param>
    /// <param name="buyPrice">Cena kupna waluty.</param>
    /// <param name="isOnlyToPlantationGoldTransfer">Określa, czy możliwa jest tylko wymiana na złoto plantacji.</param>
    /// <returns>Nowa instancja wymiany waluty.</returns>
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

    /// <summary>
    /// Dodaje poziomy gracza oraz plantacji w zależności od zdobytego doświadczenia.
    /// </summary>
    /// <param name="district">Dzielnica, w której rozgrywka ma miejsce.</param>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="gainedExp">Zdobyte doświadczenie.</param>
    /// <param name="dropsNotification">Powiadomienia o zdobytych nagrodach.</param>
    /// <param name="playerStorage">Magazyn gracza.</param>
    /// <param name="isManyLevel">Określa, czy zdobyto wiele poziomów naraz. Używane w rekurencji do określania czy mamy doczynienia z kolejnym wykonaniem.</param>
    /// <param name="tempPlantGainedExp">Tymczasowe zdobyte doświadczenie dla plantacji. Używane w rekurencji do przechowywania zdobytego doświadczenia pomniejszonego o popszedni poziom.</param>
    /// <param name="tempExpToNexLevel">Tymczasowy próg do następnego poziomu dla plantacji. Używane w rekurencji do przechowywania wymaganego doświadczenia na kolejny poziom.</param>
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

    /// <summary>
    /// Przetwarza zebrane plony z rośliny na plantacji gracza, dodając susz i potencjalnie nasiona.
    /// </summary>
    /// <param name="playerDriedFruit">Susz gracza powiązany z rośliną.</param>
    /// <param name="plant">Roślina, z której zebrane są plony.</param>
    /// <param name="dropsNotification">Powiadomienia o zdobytych przedmiotach.</param>
    /// <param name="playerSeed">Nasiono gracza powiązane z rośliną.</param>
    /// <param name="district">Dzielnica, w której rozgrywka ma miejsce.</param>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="random">Generator liczb losowych.</param>
    /// <param name="ignoreChangeService">Serwis do ignorowania zmian.</param>
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

    /// <summary>
    /// Dodaje zdobyte waluty z nagrody do informacji o ukończonym zadaniu.
    /// </summary>
    /// <param name="drop">Nagroda.</param>
    /// <param name="completeQuest">Informacje o ukończonym zadaniu.</param>
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

    /// <summary>
    /// Ustawia wiadomości dotyczące otrzymanych przedmiotów na podstawie informacji o ukończonym zadaniu.
    /// </summary>
    /// <param name="completeQuest">Informacje o ukończonym zadaniu.</param>
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

    /// <summary>
    /// Sprawdza spełnienie wymagań do stworzenia rośliny.
    /// </summary>
    /// <param name="createPlant">Dane dotyczące tworzonej rośliny.</param>
    /// <param name="message">Wiadomość z ewentualnymi brakującymi wymaganiami.</param>
    /// <returns>True, jeśli wymagania zostały spełnione, w przeciwnym razie False.</returns>
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

    /// <summary>
    /// Sprawdza, czy wszystkie wymagania ukończenia zadania zostały spełnione.
    /// </summary>
    /// <param name="completeQuest">Informacje o ukończonym zadaniu.</param>
    /// <returns>True, jeśli wszystkie wymagania zostały spełnione, w przeciwnym razie False.</returns>
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