using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage.Dto;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage;

/// <summary>
/// Klasa pomocnicza dla hub'a magazynu/zasobów
/// </summary>
public static class StorageHubHelper
{
    /// <summary>
    /// Stała przechowująca nazwę akcji "sprzedaj".
    /// </summary>
    public const string SellActionName = "sell";
    
    /// <summary>
    /// Stała przechowująca nazwę akcji "kup".
    /// </summary>
    public const string BuyActionName = "buy";
    
    
    /// <summary>
    /// Metoda przetwarzająca wymianę waluty.
    /// W zależności od podanej nazwy waluty oraz akcji (kupna/sprzedaży) oraz ilości dokonuje wymiany waluty.
    /// </summary>
    /// <param name="district">Dzielnica, w której dokonywana jest transakcja.</param>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="playerStorage">Magazyn przedmiotów gracza.</param>
    /// <param name="currencyName">Nazwa wymienianej waluty.</param>
    /// <param name="action">Typ akcji (kupna/sprzedaży).</param>
    /// <param name="amount">Ilość wymienianej waluty.</param>
    /// <param name="shopTransaction">Informacje o wykonywanej transakcji.</param>
    public static void ProcessCurrencyExchange(District district, PlantationStorage playerPlantationStorage, 
        PlayerStorage playerStorage, string currencyName, string action, decimal amount, ShopTransaction shopTransaction)
    {
        switch (currencyName)
        {
            case PlantationStorageObservedFields.Gold:
                switch (action)
                {
                    case BuyActionName:
                        var plantationCost = district.GoldExchangeRate * amount;
                        if (plantationCost <= playerPlantationStorage.Gold)
                        {
                            playerPlantationStorage.Gold -= plantationCost;
                            playerStorage.Gold += amount;
                            shopTransaction.SuccessfulTransaction = true;
                            SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, plantationCost,  new Product{ Name = "Złoto gracza"}, action, currencyName);
                        }
                        else
                            SetNotSuccessFullTransaction(shopTransaction, action);
                        break;
                    case SellActionName:
                        var playerCost = amount;
                        if (playerCost <= playerStorage.Gold)
                        {
                            playerStorage.Gold -= playerCost;
                            playerPlantationStorage.Gold += amount;
                            shopTransaction.SuccessfulTransaction = true;
                            SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, playerCost, new Product{ Name = "Złoto gracza"}, action, currencyName);
                        }
                        else
                            SetNotSuccessFullTransaction(shopTransaction, action);
                        break;
                }
                break;
            case PlantationStorageObservedFields.DonToken when action == SellActionName:
                if (amount <= playerPlantationStorage.DonToken)
                {
                    var profit = district.DonTokenExchangeRate * amount;
                    playerPlantationStorage.DonToken -= (int) amount;
                    playerPlantationStorage.Gold += profit;
                    shopTransaction.SuccessfulTransaction = true;
                    SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, profit, new Product{ Name = PlantationStorageFieldsHrNames.DonToken}, action, currencyName);
                }
                else
                    SetNotSuccessFullTransaction(shopTransaction, action);
                break;
            case PlantationStorageObservedFields.UnlockToken when action == SellActionName:
                if (amount <= playerPlantationStorage.UnlockToken)
                {
                    var profit = district.UnlockTokenExchangeRate * amount;
                    playerPlantationStorage.UnlockToken -= (int) amount;
                    playerPlantationStorage.Gold += profit;
                    shopTransaction.SuccessfulTransaction = true;
                    SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, profit, new Product{ Name = PlantationStorageFieldsHrNames.UnlockToken}, action, currencyName);
                }
                else
                    SetNotSuccessFullTransaction(shopTransaction, action);
                break;
            case PlantationStorageObservedFields.QuestToken when action == SellActionName:
                if (amount <= playerPlantationStorage.QuestToken)
                {
                    var profit = district.QuestTokenExchangeRate * amount;
                    playerPlantationStorage.QuestToken -= (int) amount;
                    playerPlantationStorage.Gold += profit;
                    shopTransaction.SuccessfulTransaction = true;
                    SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, profit, new Product{ Name = PlantationStorageFieldsHrNames.QuestToken}, action, currencyName);
                }
                else
                    SetNotSuccessFullTransaction(shopTransaction, action);
                break;
            case PlantationStorageObservedFields.DealerToken when action == SellActionName:
                if (amount <= playerPlantationStorage.DealerToken)
                {
                    var profit = district.DealerTokenExchangeRate * amount;
                    playerPlantationStorage.DealerToken -= (int) amount;
                    playerPlantationStorage.Gold += profit;
                    shopTransaction.SuccessfulTransaction = true;
                    SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, profit, new Product{ Name = PlantationStorageFieldsHrNames.DealerToken}, action, currencyName);
                }
                else
                    SetNotSuccessFullTransaction(shopTransaction, action);
                break;
            case PlantationStorageObservedFields.BlackMarketToken when action == SellActionName:
                if (amount <= playerPlantationStorage.BlackMarketToken)
                {
                    var profit = district.BlackMarketTokenExchangeRate * amount;
                    playerPlantationStorage.BlackMarketToken -= (int) amount;
                    playerPlantationStorage.Gold += profit;
                    shopTransaction.SuccessfulTransaction = true;
                    SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, profit, new Product{ Name = PlantationStorageFieldsHrNames.BlackMarketToken}, action, currencyName);
                }
                else
                    SetNotSuccessFullTransaction(shopTransaction, action);
                break;
            case PlantationStorageObservedFields.Prestige when action == SellActionName:
                if (amount <= playerPlantationStorage.Prestige)
                {
                    var profit = district.PrestigeExchangeRate * amount;
                    playerPlantationStorage.Prestige -= (int) amount;
                    playerPlantationStorage.Gold += profit;
                    shopTransaction.SuccessfulTransaction = true;
                    SetSuccessFullTransactionMessage<CurrencyExchange>(shopTransaction, amount, profit, new Product{ Name = PlantationStorageFieldsHrNames.Prestige}, action, currencyName);
                }
                else
                    SetNotSuccessFullTransaction(shopTransaction, action);
                break;
        }
    }

    /// <summary>
    /// Metoda statyczna odpowiedzialna za zakup produktu przez użytkownika.
    /// Wylicza cenę za zakupioną ilość produktu, sprawdza czy użytkownik ma wystarczającą ilość złota i aktualizuje ilość produktu oraz stan konta plantacji.
    /// </summary>
    /// <param name="shopTransaction">Obiekt transakcji sklepu.</param>
    /// <param name="action">Typ akcji (kupna/sprzedaży).</param>
    /// <param name="plantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="amount">Ilość produktu do zakupu.</param>
    /// <param name="product">Obiekt produktu.</param>
    /// <typeparam name="T">Typ produktu.</typeparam>
    public static void BuyAction<T>(ShopTransaction shopTransaction, string action, PlantationStorage plantationStorage, decimal amount, IStorageProduct product)
    {
        var buyPrice = (decimal) product.BuyPrice;
        var goldAmount = buyPrice * amount;

        if (goldAmount <= plantationStorage.Gold)
        {
            var newOwnedAmount = product.OwnedAmount + amount;
            product.OwnedAmount = newOwnedAmount;
            plantationStorage.Gold -= goldAmount;
            shopTransaction.SuccessfulTransaction = true;

            SetSuccessFullTransactionMessage<T>(shopTransaction, amount, goldAmount, product, action);
        }
        else
        {
            SetNotSuccessFullTransaction(shopTransaction, action);
        }
    }

    /// <summary>
    /// Metoda statyczna odpowiedzialna za sprzedaż produktu przez użytkownika.
    /// Wylicza cenę za sprzedaną ilość produktu, sprawdza czy użytkownik ma wystarczającą ilość produktu i aktualizuje ilość produktu oraz stan konta plantacji.
    /// </summary>
    /// <param name="shopTransaction">Obiekt transakcji sklepu.</param>
    /// <param name="action">Typ akcji (kupna/sprzedaży).</param>
    /// <param name="plantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="amount">Ilość produktu do sprzedaży.</param>
    /// <param name="product">Obiekt produktu.</param>
    /// <typeparam name="T">Typ produktu.</typeparam>
    public static void SellAction<T>(ShopTransaction shopTransaction, string action, PlantationStorage plantationStorage, decimal amount, 
        IStorageProduct product) 
    {
        var sellPrice = (decimal) product.SellPrice;
        var goldAmount = sellPrice * amount;
        var currOwnedAmount = product.OwnedAmount;
            
        if (currOwnedAmount >= amount)
        {
            var newOwnedAmount = currOwnedAmount - amount;
            product.OwnedAmount = newOwnedAmount;
            plantationStorage.Gold += goldAmount;
            shopTransaction.SuccessfulTransaction = true;

            SetSuccessFullTransactionMessage<T>(shopTransaction, amount, goldAmount, product, action);
        }
        else
        {
            SetNotSuccessFullTransaction(shopTransaction, action);
        }
    }

    /// <summary>
    /// Metoda prywatna odpowiedzialna za ustawienie komunikatu o powodzeniu transakcji.
    /// Tworzy komunikat informujący o rodzaju transakcji, ilości i jednostce sprzedanego/kupionego produktu, oraz wartości kosztu lub zysku.
    /// </summary>
    /// <param name="shopTransaction">Obiekt transakcji sklepu.</param>
    /// <param name="amount">Ilość produktu sprzedanego/kupionego</param>
    /// <param name="goldAmount">Wartość kosztu/zysku w złocie</param>
    /// <param name="obj">Obiekt sprzedanego/kupionego produktu</param>
    /// <param name="action">Typ akcji (kupna/sprzedaży).</param>
    /// <param name="currencyName">Nazwa waluty, w której została przeprowadzona wymiana.</param>
    /// <typeparam name="T">Typ produktu.</typeparam>
    private static void SetSuccessFullTransactionMessage<T>(ShopTransaction shopTransaction, decimal amount, decimal goldAmount, 
        INamedEntity obj, string action, string currencyName = "")
    {
        var measureUnit = string.IsNullOrWhiteSpace(currencyName)  
            ? PlantationManagerHelper.GetMeasureUnitByType(typeof(T))
            : PlantationManagerHelper.GetMeasureUnitForCurrencyExchange(currencyName);

        var sellOrBuy = action == BuyActionName ? "Kupiono " : "Sprzedano ";
        var expenseOrProfit = action == BuyActionName ? "Koszt: " : "Zysk: ";
            
        shopTransaction.InfoMessage.Add("Transakcja zakończona powodzeniem.");
        shopTransaction.InfoMessage.Add(sellOrBuy + amount + measureUnit + " - " + obj.Name);
        shopTransaction.InfoMessage.Add(expenseOrProfit + goldAmount.ToString("0.##") + "$");
    }

    /// <summary>
    /// Metoda prywatna odpowiedzialna za ustawienie komunikatu o niepowodzeniu transakcji.
    /// Tworzy komunikat informujący o rodzaju błędu - braku wystarczającej ilości produktu lub braku wystarczających środków finansowych, w zależności od rodzaju działania - sprzedaż/kupno.
    /// </summary>
    /// <param name="shopTransaction">Obiekt transakcji sklepu.</param>
    /// <param name="action">Typ akcji (kupna/sprzedaży).</param>
    private static void SetNotSuccessFullTransaction(ShopTransaction shopTransaction, string action)
    {
        shopTransaction.SuccessfulTransaction = false;
        shopTransaction.InfoMessage.Add( "Transakcja zakończona niepowodzeniem.");
        shopTransaction.InfoMessage.Add( action == BuyActionName ? "Za mało kasy." : "Niewystarczająca ilość");
    }
}