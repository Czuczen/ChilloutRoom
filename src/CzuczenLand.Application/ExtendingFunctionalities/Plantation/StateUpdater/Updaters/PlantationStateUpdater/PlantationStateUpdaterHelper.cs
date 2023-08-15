using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater;

/// <summary>
/// Pomocnicza klasa do aktualizacji stanu plantacji.
/// </summary>
public static class PlantationStateUpdaterHelper
{
    /// <summary>
    /// Przetwarza aktualizację stanu zasobów plantacji.
    /// </summary>
    /// <param name="displayParser">Parser wyświetlania.</param>
    /// <param name="plantationStorage">Magazyn plantacji.</param>
    /// <param name="parsedLevel">Sparsowany poziom.</param>
    /// <param name="parsedGainedExperience">Sparsowane zdobyte doświadczenie.</param>
    /// <param name="parsedGold">Sparsowane złoto.</param>
    /// <param name="receivedLevels">Otrzymane poziomy.</param>
    /// <returns>Aktualizowany stan zasobów plantacji.</returns>
    public static PlantationStorageUpdate ProcessPlantationStorageUpdate(IParser displayParser,
        PlantationStorage plantationStorage, string parsedLevel, string parsedGainedExperience, 
        string parsedGold, int receivedLevels)
    {
        var parsedPlantationStorage = (Dictionary<string, object>) displayParser.Parse(new List<object>{plantationStorage}).First();
            
        var buffsPlaces = parsedPlantationStorage[PlantationStorageObservedFields.BuffSlotsInUse] + "/" + 
                          parsedPlantationStorage[PlantationStorageObservedFields.UnlockedBuffsSlots] + "/" +
                          parsedPlantationStorage[PlantationStorageObservedFields.MaxBuffsSlots];
        var artifactsPlaces = parsedPlantationStorage[PlantationStorageObservedFields.ArtifactSlotsInUse] + "/" + 
                              parsedPlantationStorage[PlantationStorageObservedFields.UnlockedArtifactSlots] + "/" + 
                              parsedPlantationStorage[PlantationStorageObservedFields.MaxArtifactSlots];
            
        var dailyQuestsPlaces = parsedPlantationStorage[PlantationStorageObservedFields.StartedDailyQuestsCount] + "/" + 
                                parsedPlantationStorage[PlantationStorageObservedFields.UnlockedDailyQuestsCount] + "/" +
                                parsedPlantationStorage[PlantationStorageObservedFields.MaxDailyQuestsCount];
        var weeklyQuestsPlaces = parsedPlantationStorage[PlantationStorageObservedFields.StartedWeeklyQuestsCount] + "/" + 
                                 parsedPlantationStorage[PlantationStorageObservedFields.UnlockedWeeklyQuestsCount] + "/" + 
                                 parsedPlantationStorage[PlantationStorageObservedFields.MaxWeeklyQuestsCount];

        return new PlantationStorageUpdate
        {
            PlantationStorageId = plantationStorage.Id,
            Level = parsedLevel, 
            GainedExperience = parsedGainedExperience, 
            Gold = parsedGold, 
            ReceivedLevels = receivedLevels,
            ExpToNextLevel = plantationStorage.ExpToNextLevel,
            CurrExp = plantationStorage.CurrExp,
            ParsedCurrExp = (string) parsedPlantationStorage[PlantationStorageObservedFields.CurrExp], 
            ParsedExpToNextLevel = (string) parsedPlantationStorage[PlantationStorageObservedFields.ExpToNextLevel],
            UnlockToken = (string) parsedPlantationStorage[PlantationStorageObservedFields.UnlockToken],
            DonToken = (string) parsedPlantationStorage[PlantationStorageObservedFields.DonToken],
            QuestToken = (string) parsedPlantationStorage[PlantationStorageObservedFields.QuestToken], 
            DealerToken = (string) parsedPlantationStorage[PlantationStorageObservedFields.DealerToken], 
            BlackMarketToken = (string) parsedPlantationStorage[PlantationStorageObservedFields.BlackMarketToken],
            Prestige = (string) parsedPlantationStorage[PlantationStorageObservedFields.Prestige], 
            DailyQuestsInProgressCount = (string) parsedPlantationStorage[PlantationStorageObservedFields.StartedDailyQuestsCount], 
            WeeklyQuestsInProgressCount = (string) parsedPlantationStorage[PlantationStorageObservedFields.StartedWeeklyQuestsCount], 
            MaxDailyQuestsCount = (string) parsedPlantationStorage[PlantationStorageObservedFields.MaxDailyQuestsCount], 
            MaxWeeklyQuestsCount = (string) parsedPlantationStorage[PlantationStorageObservedFields.MaxWeeklyQuestsCount],
            ArtifactsPlaces = artifactsPlaces,
            BuffsPlaces = buffsPlaces,
            UnlockBuffBtnVisible = !(plantationStorage.UnlockedBuffsSlots >= plantationStorage.MaxBuffsSlots) ,
            UnlockArtifactBtnVisible = !(plantationStorage.UnlockedArtifactSlots >= plantationStorage.MaxArtifactSlots),
            DailyQuestsPlaces = dailyQuestsPlaces,
            WeeklyQuestsPlaces = weeklyQuestsPlaces,
            UnlockedDailyQuestsCount = (string) parsedPlantationStorage[PlantationStorageObservedFields.UnlockedDailyQuestsCount],
            UnlockedWeeklyQuestsCount = (string) parsedPlantationStorage[PlantationStorageObservedFields.UnlockedWeeklyQuestsCount]
        };
    }

    /// <summary>
    /// Generuje listę nazw nowych przedmiotów dla zdobytych poziomów.
    /// </summary>
    /// <param name="currLvlItems">Obiekt zawierający przedmioty dla zdobytych poziomów.</param>
    /// <returns>Lista nazw nowych przedmiotów.</returns>
    private static List<string> GenerateNamesForNewLvlItems(CurrLvlItems currLvlItems)
    {
        var ret = new List<string>();
        var currLvlDriedFruitsNames = currLvlItems.DriedFruits.Select(entity => "Dostępny nowy susz - " + entity.Name).ToList();
        var currLvlLampsNames = currLvlItems.Lamps.Select(entity => "Dostępna nowa lampa - " + entity.Name).ToList();
        var currLvlManuresNames = currLvlItems.Manures.Select(entity => "Dostępny nowy nawóz - " + entity.Name).ToList();
        var currLvlPotsNames = currLvlItems.Pots.Select(entity => "Dostępna nowa doniczka - " + entity.Name).ToList();
        var currLvlSeedsNames = currLvlItems.Seeds.Select(entity => "Dostępne nowe nasiono - " + entity.Name).ToList();
        var currLvlSoilsNames = currLvlItems.Soils.Select(entity => "Dostępna nowa gleba - " + entity.Name).ToList();
        var currLvlWatersNames = currLvlItems.Waters.Select(entity => "Dostępna nowa woda - " + entity.Name).ToList();
        var currLvlBonusesNames = currLvlItems.Bonuses.Select(entity =>
            (entity.IsArtifact ? "Dostępny nowy artefakt - " : "Dostępne nowe wzmocnienie - ") + entity.Name).ToList();
        var currLvlQuestsNames = currLvlItems.Quests.Select(entity => "Dostępne nowe zadanie - " + SelectListLoaderHelper.QuestTypesNamesDbToHr[entity.QuestType] + " - " + entity.Name).ToList();

        ret.AddRange(currLvlDriedFruitsNames);
        ret.AddRange(currLvlLampsNames);
        ret.AddRange(currLvlManuresNames);
        ret.AddRange(currLvlPotsNames);
        ret.AddRange(currLvlSeedsNames);
        ret.AddRange(currLvlSoilsNames);
        ret.AddRange(currLvlWatersNames);
        ret.AddRange(currLvlBonusesNames);
        ret.AddRange(currLvlQuestsNames);

        return ret;
    }
    
    /// <summary>
    /// Wysyła użytkownikowi informacje o nowych przedmiotach.
    /// </summary>
    /// <param name="state">Stan analizy zawierający informacje o nowym poziomie.</param>
    public static void SendItemsForNewLvl(AnalysisState state)
    {
        var receivedLevels = state.ReceivedLevels;
        if (receivedLevels <= 0) return;

        var storage = state.StorageEntity;
        if (storage is PlantationStorage plantationStorage)
        {
            var allItemsNames = GenerateNamesForNewLvlItems(state.ReceivedLvlItems);
            if (!allItemsNames.Any()) return;
                
            var currLvlQuestsData = state.ReceivedLvlItems.Quests.Select(entity => new LvlQuestsData
            {
                QuestId = entity.Id,
                QuestType = entity.QuestType
            }).ToList();
                
            var responseObj = new ItemsForNewLvls
            {
                ItemsNames = allItemsNames, 
                QuestsData = currLvlQuestsData, 
                S2DistrictsList = null
            };
                
            state.InfoHub.Clients.User(plantationStorage.UserId.ToString()).itemsForNewLvls(responseObj);
        }
        else if (storage is PlayerStorage playerStorage)
        {
            if (!state.NewDistricts.Any()) return;
            
            var parser = state.DisplayParserStrategy;
            var currLvlDistrictsNames = state.NewDistricts.Select(entity =>
                "Dostępna nowa dzielnica - " + entity.Name +
                ". Start: " +
                parser.ParseValue(EnumUtils.PropTypes.DateTime, entity.StartTime) + " Koniec: " +
                parser.ParseValue(EnumUtils.PropTypes.DateTime, entity.EndTime)
            ).ToList();
                
            var responseObj = new ItemsForNewLvls
            {
                ItemsNames = currLvlDistrictsNames,
                QuestsData = null, 
                S2DistrictsList = state.S2DistrictsList
            };
                
            state.InfoHub.Clients.User(playerStorage.UserId.ToString()).itemsForNewLvls(responseObj);
        }
    }
}