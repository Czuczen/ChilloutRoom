using System;
using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.BlackMarket.Dto;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities;

/// <summary>
/// Klasa operująca zyskiem i kosztami żetonów.
/// </summary>
public static class TokensOperator
{
    /// <summary>
    /// Stała reprezentująca odblokowanie miejsca na artefakt.
    /// </summary>
    private const string Artifact = "Artifact";
    
    /// <summary>
    /// Stała reprezentująca odblokowanie miejsca na wzmocnienie.
    /// </summary>
    private const string Buff = "Buff";
    
    /// <summary>
    /// Stała reprezentująca odblokowanie miejsca na misje dzienną.
    /// </summary>
    private const string Daily = "Daily";
    
    /// <summary>
    /// Stała reprezentująca odblokowanie miejsca na misje tygodniową.
    /// </summary>
    private const string Weekly = "Weekly";
    
    
    /// <summary>
    /// Instancja klasy Random do generowania losowych wartości.
    /// </summary>
    private static readonly Random Random = new();

    
    /// <summary>
    /// Oblicza i ustawia ilość zdobytych żetonów za ukończenie zadania.
    /// </summary>
    /// <param name="completeQuest">Dane o ukończonej misji.</param>
    public static void QuestTokenProfit(CompleteQuest completeQuest)
    {
        var randomInt = Random.Next(1, 100);
        if (randomInt > completeQuest.District.QuestTokenChanceFromCompleteQuest) return;
        
        switch (completeQuest.Quest.QuestType)
        {
            case DbQuestTypesNames.Achievement:
            case DbQuestTypesNames.Daily:
            case DbQuestTypesNames.Others:
                completeQuest.GainedQuestTokens +=
                    completeQuest.District.QuestTokenForAchievementDailyOthersAmount;
                break;
            case DbQuestTypesNames.Event:
                completeQuest.GainedQuestTokens += completeQuest.District.QuestTokenForEventAmount;
                break;
            case DbQuestTypesNames.Weekly:
                completeQuest.GainedQuestTokens += completeQuest.District.QuestTokenForWeeklyAmount;
                break;
        }
    }

    /// <summary>
    /// Oblicza i ustawia ilość wydanych żetonów za rozpoczęcie zadania typu wydarzenie.
    /// </summary>
    /// <param name="questHub">Kontekst huba zadań do komunikacji z klientem.</param>
    /// <param name="plantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="quest">Rozpoczynane zadanie gracza.</param>
    /// <returns>True, jeśli opłata żetonowa jest spełniona lub nie jest wymagana dla misji; False, jeśli brakuje żetonów.</returns>
    public static bool QuestTokenFee(IHubContext questHub, PlantationStorage plantationStorage, Quest quest)
    {
        var ret = true;
        if (quest.QuestType != DbQuestTypesNames.Event) return true;
        
        if (plantationStorage.QuestToken > 0)
            plantationStorage.QuestToken--;
        else
        {
            ret = false;
            questHub.Clients.User(plantationStorage.UserId.ToString()).notEnoughQuestToken(quest.Id, quest.QuestType,
                new List<string> {"Brak żetonów zadania"});
        }

        return ret;
    }
    
    /// <summary>
    /// Oblicza i ustawia ilość zdobytych żetonów za sprzedaż w strefie klienta. 
    /// </summary>
    /// <param name="district">Dzielnica na której dokonano sprzedaży.</param>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="itemAmount">Ilość sprzedanego suszu.</param>
    public static void DealerTokenProfit(District district, PlantationStorage playerPlantationStorage, decimal itemAmount)
    {
        var randomInt = Random.Next(1, 100);
        if (randomInt > district.DealerTokenChanceFromCustomerZone) return;
        
        switch (itemAmount)
        {
            case > 100:
                playerPlantationStorage.DealerToken += district.DealerTokenWithItemAmountMoreThan100;
                break;
            case > 10:
                playerPlantationStorage.DealerToken += district.DealerTokenWithItemAmountMoreThan10;
                break;
            default:
                playerPlantationStorage.DealerToken += district.DealerTokenWithItemAmountLessThan11;
                break;
        }
    }
    
    /// <summary>
    /// Oblicza i ustawia ilość wydanych żetonów za wystawienie transakcji na czarnym rynku
    /// </summary>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="status">Status wystawianej transakcji na czarnym rynku.</param>
    /// <returns>True, jeśli opłata żetonowa jest spełniona; False, jeśli brakuje żetonów.</returns>
    public static bool DealerTokenFee(PlantationStorage playerPlantationStorage, BlackMarketTransactionStatus status)
    {
        var ret = true;
        if (playerPlantationStorage.DealerToken > 0)
            playerPlantationStorage.DealerToken--;
        else
        {
            ret = false;
            status.Status = BlackMarketStatuses.DangerStatus;
            status.Message = "Brak żetonów dealera";
        }

        return ret;
    }
    
    /// <summary>
    /// Oblicza i ustawia ilość zdobytych żetonów za kupowanie i sprzedawanie na czarnym rynku
    /// </summary>
    /// <param name="district">Dzielnica na której dokonano transakcji.</param>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="cost">Koszt transakcji.</param>
    public static void BlackMarketTokenProfit(District district, PlantationStorage playerPlantationStorage, decimal cost)
    {
        var randomInt = Random.Next(1, 100);
        if (randomInt > district.BlackMarketTokenChanceForBlackMarketTransaction) return;
        
        switch (cost)
        {
            case > 100000:
                playerPlantationStorage.BlackMarketToken += district.BlackMarketTokenWithCostMoreThan100000;
                break;
            case > 10000:
                playerPlantationStorage.BlackMarketToken += district.BlackMarketTokenWithCostMoreThan10000;
                break;
            case > 5000:
                playerPlantationStorage.BlackMarketToken += district.BlackMarketTokenWithCostMoreThan5000;
                break;
            default:
                playerPlantationStorage.BlackMarketToken += district.BlackMarketTokenWithCostLessThan5001;
                break;
        }
    }
    
    /// <summary>
    /// Oblicza i ustawia ilość wydanych żetonów za niepłacenie haraczu na czarnym rynku za wystawienie transakcji.
    /// </summary>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="status">Status wystawianej transakcji na czarnym rynku.</param>
    /// <returns>True, jeśli opłata żetonowa jest spełniona; False, jeśli brakuje żetonów.</returns>
    public static bool BlackMarketTokenFee(PlantationStorage playerPlantationStorage, BlackMarketTransactionStatus status)
    {
        var ret = true;
        if (playerPlantationStorage.BlackMarketToken > 0)
            playerPlantationStorage.BlackMarketToken--;
        else
        {
            ret = false;
            status.Status = BlackMarketStatuses.DangerStatus;
            status.Message = "Brak żetonów czarnego rynku";
        }
        
        return ret;
    }
    
    /// <summary>
    /// Oblicza i ustawia ilość zdobytych żetonów za sprzedawanie na czarnym rynku będąc donem dzielnicy.
    /// </summary>
    /// <param name="isDon">Czy gracz jest donem dzielnicy.</param>
    /// <param name="district">Dzielnica na której dokonano transakcji.</param>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    public static void DonTokenProfit(bool isDon, District district, PlantationStorage playerPlantationStorage)
    {
        var randomInt = Random.Next(1, 100);
        if (isDon && randomInt <= district.DonTokenChanceFromSellBlackMarketTransaction)
            playerPlantationStorage.DonToken++;
    }
    
    /// <summary>
    /// Oblicza i ustawia ilość wydanych żetonów za kupienie transakcji opłaconej żetonem dona.
    /// Transakcja opłacona żetonem dona nie pobiera złota.
    /// </summary>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="status">Status kupowanej transakcji na czarnym rynku.</param>
    /// <returns>True, jeśli opłata żetonowa jest spełniona; False, jeśli brakuje żetonów.</returns>
    public static bool DonTokenFee(PlantationStorage playerPlantationStorage, BlackMarketTransactionStatus status)
    {
        var ret = true;
        if (playerPlantationStorage.DonToken > 0)
            playerPlantationStorage.DonToken--;
        else
        {
            ret = false;
            status.Status = BlackMarketStatuses.DangerStatus;
            status.Message = "Brak żetonów don'a";
        }
        
        return ret;
    }
    
    /// <summary>
    /// Oblicza i ustawia ilość zdobytych żetonów za zbieranie roślin.
    /// Szansa na zdobycie powinna być bardzo mała.
    /// </summary>
    /// <param name="district">Dzielnica, w której ma miejsce akcja zbierania rośliny.</param>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="dropsNotification">Lista powiadomień o zdobytych przedmiotach.</param>
    public static void UnlockTokenProfit(District district, PlantationStorage playerPlantationStorage, List<string> dropsNotification)
    {
        if (district.UnlockTokenChanceForCollectPlant <= 0) return;
        
        var randomDecimal = DecimalUtils.NextDecimal(0, 100);
        if (randomDecimal > district.UnlockTokenChanceForCollectPlant) return;

        playerPlantationStorage.UnlockToken++;
        dropsNotification.Add("Otrzymano token odblokowania");
    }

    /// <summary>
    /// Oblicza i ustawia ilość wydanych żetonów za odblokowanie miejsca na artefakt, wzmocnienie, zadania dzienne i tygodniowe.
    /// </summary>
    /// <param name="playerPlantationStorage">Magazyn plantacji gracza.</param>
    /// <param name="bonusActivation">Obiekt aktywacji bonusu z wynikiem akcji odblokowania.</param>
    /// <param name="unlockType">Typ odblokowania (np. "Artifact", "Buff", "Daily", "Weekly").</param>
    /// <returns>Zwraca true, jeśli odblokowanie było możliwe i udane, w przeciwnym razie false.</returns>
    public static bool UnlockTokenFee(PlantationStorage playerPlantationStorage, BonusActivation bonusActivation, string unlockType)
    {
        if (playerPlantationStorage.UnlockToken <= 0) return false;
        
        switch (unlockType)
        {
            case Artifact when playerPlantationStorage.UnlockedArtifactSlots < playerPlantationStorage.MaxArtifactSlots:
                if (playerPlantationStorage.UnlockedArtifactSlots == 0)
                {
                    playerPlantationStorage.UnlockedArtifactSlots++;
                    playerPlantationStorage.UnlockToken--;
                    bonusActivation.SuccessfulActivation = true;
                    bonusActivation.InfoMessage.Add("Odblokowano slot artefaktu. " + "Koszt: 1");   
                }
                else
                {
                    var tokensCost = playerPlantationStorage.UnlockedArtifactSlots *
                                     playerPlantationStorage.UnlockedArtifactSlots;
                    if (tokensCost <= playerPlantationStorage.UnlockToken)
                    {
                        playerPlantationStorage.UnlockToken -= tokensCost;
                        playerPlantationStorage.UnlockedArtifactSlots++;
                        bonusActivation.SuccessfulActivation = true;
                        bonusActivation.InfoMessage.Add("Odblokowano slot artefaktu. " + "Koszt: " + tokensCost);
                    }
                    else
                    {
                        bonusActivation.SuccessfulActivation = false;
                        bonusActivation.InfoMessage.Add("Odblokowanie nie udane. Koszt: " + tokensCost + " Posiadane: " + playerPlantationStorage.UnlockToken);
                    }
                }
                break;
            case Buff when playerPlantationStorage.UnlockedBuffsSlots < playerPlantationStorage.MaxBuffsSlots:
                if (playerPlantationStorage.UnlockedBuffsSlots == 0)
                {
                    playerPlantationStorage.UnlockedBuffsSlots++;
                    playerPlantationStorage.UnlockToken--;
                    bonusActivation.SuccessfulActivation = true;
                    bonusActivation.InfoMessage.Add("Odblokowano slot wzmocnienia. " + "Koszt: 1");
                }
                else
                {
                    var tokensCost = playerPlantationStorage.UnlockedBuffsSlots *
                                     playerPlantationStorage.UnlockedBuffsSlots;
                    if (tokensCost <= playerPlantationStorage.UnlockToken)
                    {
                        playerPlantationStorage.UnlockToken -= tokensCost;
                        playerPlantationStorage.UnlockedBuffsSlots++;
                        bonusActivation.SuccessfulActivation = true;
                        bonusActivation.InfoMessage.Add("Odblokowano slot wzmocnienia. " + "Koszt: " + tokensCost);
                    }
                    else
                    {
                        bonusActivation.SuccessfulActivation = false;
                        bonusActivation.InfoMessage.Add("Odblokowanie nie udane. Koszt: " + tokensCost + " Posiadane: " + playerPlantationStorage.UnlockToken);
                    }
                }
                break;
            case Daily when playerPlantationStorage.UnlockedDailyQuestsCount < playerPlantationStorage.MaxDailyQuestsCount:
                if (playerPlantationStorage.UnlockedDailyQuestsCount == 0)
                {
                    playerPlantationStorage.UnlockedDailyQuestsCount++;
                    playerPlantationStorage.UnlockToken--;
                    bonusActivation.SuccessfulActivation = true;
                    bonusActivation.InfoMessage.Add("Odblokowano slot zadania dziennego. " + "Koszt: 1");
                }
                else
                {
                    var tokensCost = playerPlantationStorage.UnlockedDailyQuestsCount *
                                     playerPlantationStorage.UnlockedDailyQuestsCount;
                    if (tokensCost <= playerPlantationStorage.UnlockToken)
                    {
                        playerPlantationStorage.UnlockToken -= tokensCost;
                        playerPlantationStorage.UnlockedDailyQuestsCount++;
                        bonusActivation.SuccessfulActivation = true;
                        bonusActivation.InfoMessage.Add("Odblokowano slot zadania dziennego. " + "Koszt: " + tokensCost);
                    }
                    else
                    {
                        bonusActivation.SuccessfulActivation = false;
                        bonusActivation.InfoMessage.Add("Odblokowanie nie udane. Koszt: " + tokensCost + " Posiadane: " + playerPlantationStorage.UnlockToken);
                    }
                }
                break;
            case Weekly when playerPlantationStorage.UnlockedWeeklyQuestsCount < playerPlantationStorage.MaxWeeklyQuestsCount:
                if (playerPlantationStorage.UnlockedWeeklyQuestsCount == 0)
                {
                    playerPlantationStorage.UnlockedWeeklyQuestsCount++;
                    playerPlantationStorage.UnlockToken--;
                    bonusActivation.SuccessfulActivation = true;
                    bonusActivation.InfoMessage.Add("Odblokowano slot zadania tygodniowego. " + "Koszt: 1");
                }
                else
                {
                    var tokensCost = playerPlantationStorage.UnlockedWeeklyQuestsCount *
                                     playerPlantationStorage.UnlockedWeeklyQuestsCount;
                    if (tokensCost <= playerPlantationStorage.UnlockToken)
                    {
                        playerPlantationStorage.UnlockToken -= tokensCost;
                        playerPlantationStorage.UnlockedWeeklyQuestsCount++;
                        bonusActivation.SuccessfulActivation = true;
                        bonusActivation.InfoMessage.Add("Odblokowano slot zadania tygodniowego. " + "Koszt: " + tokensCost);
                    }
                    else
                    {
                        bonusActivation.SuccessfulActivation = false;
                        bonusActivation.InfoMessage.Add("Odblokowanie nie udane. Koszt: " + tokensCost + " Posiadane: " + playerPlantationStorage.UnlockToken);
                    }
                }
                break;
            default:
                bonusActivation.SuccessfulActivation = false;
                bonusActivation.InfoMessage.Add("Odblokowanie nie udane");
                break;
        }

        return true;
    }
}
