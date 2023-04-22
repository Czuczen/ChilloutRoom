using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket.Dto;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater;

public static class PlantationStateUpdater
{
    /// <summary>
    /// Aktualizacja wybranych produktów przy kreacji rośliny i produktów w zasobach
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static AnalysisState UpdateProductsState(this AnalysisState state)
    {
        if (!state.InitStatus ||
            state.PlayerStorage?.LastSelectedDistrictId != state.PlantationStorage.DistrictId || // jeśli nie ma go na dzielnicy z której to rekord został zaktualizowany nic nie robimy
            state.LastEntityPropertyChanges == null) return state;
            
        var product = (Product) state.Entity;
        var entityType = state.Entity.GetType();

        var obj = new UpdateProducts 
        {
            Id = product.Id, 
            OwnedAmountWithMeasureUnit = product.OwnedAmount.ToString("0.##") + PlantationManagerHelper.GetMeasureUnitByType(entityType), 
            OwnedAmount = product.OwnedAmount, 
            EntityName = entityType.Name, 
            RecordName = product.Name
        };
            
        state.InfoHub.Clients.User(state.UserId.ToString()).updateProduct(obj);

        return state;
    }
        
    /// <summary>
    /// Aktualizacja bonusów Wzmocnienie/Artefakt
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static AnalysisState UpdateBonusesState(this AnalysisState state)
    {
        if (!state.InitStatus ||
            state.PlayerStorage?.LastSelectedDistrictId != state.PlantationStorage.DistrictId || // jeśli nie ma go na dzielnicy z której to rekord został zaktualizowany nic nie robimy
            state.LastEntityPropertyChanges == null) return state;

        var bonus = (Bonus) state.Entity;
        var userIdAsString = state.PlantationStorage.UserId.ToString();
        var bonusUpdate = new BonusUpdate
        {
            BonusId = bonus.Id, IsArtifact = bonus.IsArtifact, IsActive = bonus.IsActive,
            RemainingActiveTime = bonus.RemainingActiveTime ?? 0
        };
            
        state.BonusHub.Clients.User(userIdAsString).bonusUpdate(bonusUpdate);   

        return state;
    }
        
    /// <summary>
    /// Aktualizacja magazynów
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static AnalysisState UpdateStorageState(this AnalysisState state)
    {
        if (!state.InitStatus || state.LastEntityPropertyChanges == null) return state;

        var entity = state.StorageEntity;
        var displayParser = state.DisplayParser;
        var entityType = state.Entity.GetType();
        var properties = entityType.GetProperties().ToList();

        var levelProp = properties.FirstOrDefault(item => item.Name == PlantationStorageObservedFields.Level);
        var gainedExperience = properties.FirstOrDefault(item => item.Name == PlantationStorageObservedFields.GainedExperience);
        var gold = properties.FirstOrDefault(item => item.Name == PlantationStorageObservedFields.Gold);
        
        var parsedLevel = (string) displayParser.Parse(levelProp, entity.Level);
        var parsedGainedExperience = (string) displayParser.Parse(gainedExperience, entity.GainedExperience);
        var parsedGold = (string) displayParser.Parse(gold, entity.Gold);
            
        if (state.Entity is PlantationStorage plantationStorage)
        {
            if (state.PlayerStorage.LastSelectedDistrictId != plantationStorage.DistrictId) return state;

            PlantationStateUpdaterHelper.SendItemsForNewLvl(state);
            var obj = PlantationStateUpdaterHelper.ProcessPlantationStorageUpdate(displayParser,
                plantationStorage, parsedLevel, parsedGainedExperience, parsedGold, state.ReceivedLevels);
            state.InfoHub.Clients.User(state.UserId.ToString()).updatePlantationState(obj);
        }
        else if (state.Entity is PlayerStorage playerStorage)
        {
            PlantationStateUpdaterHelper.SendItemsForNewLvl(state);
            
            var honor = properties.FirstOrDefault(item => item.Name == PlayerStorageObservedFields.Honor);
            var parsedHonor = (string) displayParser.Parse(honor, playerStorage.Honor);
            
            var obj = new PlayerStorageUpdate
            {
                Level = parsedLevel, 
                Honor = parsedHonor,
                GainedExperience = parsedGainedExperience,
                Gold = parsedGold
            };
                
            state.InfoHub.Clients.User(playerStorage.UserId.ToString()).updatePlayerState(obj);
        }

        return state;
    }

    /// <summary>
    /// Aktualizacja czarnego rynku
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static AnalysisState UpdateBlackMarketState(this AnalysisState state)
    {
        var entity = state.BlackMarketEntity;
        var blackMarketHub = state.BlackMarketHub;
        if (state.Action == UpdateActivityOperator.ActionCreated)
        {
            var blackMarketItem = state.DisplayParser
                .Parse(state.ObjectMapper.Map<List<BlackMarketItem>>(new List<BlackMarketTransaction> {entity}).Cast<object>().ToList()).Single();
                
            if (entity.SellerId != null)
            {
                var userConnections = state.OnlineClientManager.GetAllClients()
                    .Where(item => item.UserId == entity.SellerId).Select(item => item.ConnectionId).ToArray();

                blackMarketHub.Clients.User(entity.SellerId.ToString()).setIssuedTransaction(blackMarketItem);
                blackMarketHub.Clients.AllExcept(userConnections).setTransaction(blackMarketItem);    
            }
            else
                blackMarketHub.Clients.All.setTransaction(blackMarketItem);
        }

        if (state.Action == UpdateActivityOperator.ActionDeleted)
        {
            if (entity.SellerId != null)
            {
                var userConnections = state.OnlineClientManager.GetAllClients()
                    .Where(item => item.UserId == entity.SellerId).Select(item => item.ConnectionId).ToArray();
                        
                blackMarketHub.Clients.AllExcept(userConnections).removeTransaction(entity.Id, entity.ItemEntityName);

                var sellerMessageData = new BlackMarketTransactionStatus
                {
                    DistrictId = state.District.Id,
                    Status = BlackMarketStatuses.SuccessStatus,
                    Message = (entity.IsCanceled ? "Anulowano" : "Sprzedano") + ": " + 
                              entity.Quantity.ToString(CultureInfo.InvariantCulture).Replace(".", ",") + 
                              PlantationManagerHelper.GetMeasureUnitByEntityName(entity.ItemEntityName) +  " " + entity.ItemName + (entity.IsCanceled
                                  ? "" : ". Zysk: " + (entity.Quantity * entity.Price).ToString("0.##").Replace(".", ",") + "$")
                };

                blackMarketHub.Clients.User(entity.SellerId.ToString()).issue(sellerMessageData);
                blackMarketHub.Clients.User(entity.SellerId.ToString()).removeIssuedTransaction(entity.Id, entity.ItemEntityName);
            }
            else
                blackMarketHub.Clients.All.removeTransaction(entity.Id, entity.ItemEntityName);
        }

        return state;
    }
}