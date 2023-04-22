using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage.Dto;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage;

public static class StorageHubHelper
{
    public const string SellActionName = "sell";
    public const string BuyActionName = "buy";
        
    
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

    private static void SetNotSuccessFullTransaction(ShopTransaction shopTransaction, string action)
    {
        shopTransaction.SuccessfulTransaction = false;
        shopTransaction.InfoMessage.Add( "Transakcja zakończona niepowodzeniem.");
        shopTransaction.InfoMessage.Add( action == BuyActionName ? "Za mało kasy." : "Niewystarczająca ilość");
    }
}