using System;
using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.District.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia dzielnicy.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.District))]
public class DistrictCreateDto
{
    /// <summary>
    /// Nazwa dzielnicy.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.Name)]
    public string Name { get; set; }
        
    /// <summary>
    /// Czas rozpoczęcia aktywności dzielnicy.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.StartTime)]
    public DateTime StartTime { get; set; }
        
    /// <summary>
    /// Czas zakończenia aktywności dzielnicy (opcjonalny). Dla dzielnic ograniczonych czasowo nie nalicza się poziomu gracza i nie można na niej transferować złota gracza.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.EndTime)]
    public DateTime? EndTime { get; set; }
        
    /// <summary>
    /// Okres cykliczności, aktywności dzielnicy (opcjonalny).
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.CyclicTime)]
    public int? CyclicTime { get; set; }
        
    /// <summary>
    /// Wartość szybkości więdnięcia roślin.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.WiltSpeed)]
    public decimal WiltSpeed { get; set; }
        
    /// <summary>
    /// Wartość dzielnika szybkości wzrostu.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.GrowingSpeedDivider)]
    public decimal GrowingSpeedDivider { get; set; }
        
        
        
        
        
    /// <summary>
    /// Cena dziupli w dzielnicy.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.HollowPrice)]
    public decimal HollowPrice { get; set; }
        
    /// <summary>
    /// Początkowa ilość złota dla plantacji w dzielnicy.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.StartGold)]
    public decimal StartGold { get; set; }
        
    /// <summary>
    /// Początkowa ilość punktów do następnego poziomu dla plantacji w dzielnicy.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.StartExpToNextLevel)]
    public decimal StartExpToNextLevel { get; set; }
        
    /// <summary>
    /// Wymagany poziom gracza dla dzielnicy.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.PlayerLevelRequirement)]
    public int PlayerLevelRequirement { get; set; }
        
    /// <summary>
    /// Flaga określająca, czy dzielnica jest zdefiniowana.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.IsDefined)]
    public bool IsDefined { get; set; }
    
    
    
    
    /// <summary>
    /// Ilość honoru za zajęcie pierwszego miejsca w dzielnicy.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.HonorForTakingFirstPlace)]
    public int HonorForTakingFirstPlace { get; set; }
    
    /// <summary>
    /// Ilość honoru za zajęcie drugiego miejsca w dzielnicy.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.HonorForTakingSecondPlace)]
    public int HonorForTakingSecondPlace { get; set; }
    
    /// <summary>
    /// Ilość honoru za zajęcie trzeciego miejsca w dzielnicy.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.HonorForTakingThirdPlace)]
    public int HonorForTakingThirdPlace { get; set; }
    
    /// <summary>
    /// Ilość honoru za otrzymanie nagrody pocieszenia w dzielnicy.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.HonorConsolationPrize)]
    public int HonorConsolationPrize { get; set; }
    
    
        
        
    /// <summary>
    /// Maksymalna ilość slotów na wzmocnienia.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.MaxBuffsSlots)]
    public int MaxBuffsSlots { get; set; }
        
    /// <summary>
    /// Maksymalna ilość slotów na artefakty.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.MaxArtifactSlots)]
    public int MaxArtifactSlots { get; set; }
        
    /// <summary>
    /// Maksymalna ilość zadań dziennych.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.MaxDailyQuestsCount)]
    public int MaxDailyQuestsCount { get; set; }
        
    /// <summary>
    /// Maksymalna ilość zadań tygodniowych.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.MaxWeeklyQuestsCount)]
    public int MaxWeeklyQuestsCount { get; set; }
        
        
        
        
    /// <summary>
    /// Liczba odblokowanych slotów na wzmocnienia przy starcie.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.UnlockedBuffsSlotsOnStart)]
    public int UnlockedBuffsSlotsOnStart { get; set; }
        
    /// <summary>
    /// Liczba odblokowanych slotów na artefakty przy starcie.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.UnlockedArtifactSlotsOnStart)]
    public int UnlockedArtifactSlotsOnStart { get; set; }
        
    /// <summary>
    /// Liczba odblokowanych zadań dziennych na starcie.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.UnlockedDailyQuestsOnStart)]
    public int UnlockedDailyQuestsOnStart { get; set; }
        
    /// <summary>
    /// Liczba odblokowanych zadań tygodniowych na starcie.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.UnlockedWeeklyQuestsOnStart)]
    public int UnlockedWeeklyQuestsOnStart { get; set; }
        
        
        
        

    /// <summary>
    /// Współczynnik wymiany złota.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.GoldExchangeRate)]
    public int GoldExchangeRate { get; set; }
        
    /// <summary>
    /// Współczynnik wymiany prestiżu.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.PrestigeExchangeRate)]
    public int PrestigeExchangeRate { get; set; }

    /// <summary>
    /// Współczynnik wymiany żetonów zadań.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.QuestTokenExchangeRate)]
    public int QuestTokenExchangeRate { get; set; }
        
    /// <summary>
    /// Współczynnik wymiany żetonów dealera.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DealerTokenExchangeRate)]
    public int DealerTokenExchangeRate { get; set; }
        
    /// <summary>
    /// Współczynnik wymiany żetonów czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenExchangeRate)]
    public int BlackMarketTokenExchangeRate { get; set; }
        
    /// <summary>
    /// Współczynnik wymiany żetonów dona.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DonTokenExchangeRate)]
    public int DonTokenExchangeRate { get; set; }
        
    /// <summary>
    /// Współczynnik wymiany żetonów odblokowania.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.UnlockTokenExchangeRate)]
    public int UnlockTokenExchangeRate { get; set; }
        
        
        
        
        
    /// <summary>
    /// Szansa na zdobycie żetonów zadania po ukończeniu zadania.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.QuestTokenChanceFromCompleteQuest)]
    public int QuestTokenChanceFromCompleteQuest { get; set; }
        
    /// <summary>
    /// Szansa na zdobycie żetonów dealera ze strefy klienta.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DealerTokenChanceFromCustomerZone)]
    public int DealerTokenChanceFromCustomerZone { get; set; }
        
    /// <summary>
    /// Szansa na zdobycie żetonów czarnego rynku podczas transakcji.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenChanceForBlackMarketTransaction)]
    public int BlackMarketTokenChanceForBlackMarketTransaction { get; set; }
        
    /// <summary>
    /// Szansa na zdobycie żetonów dona ze sprzedaży na czarnym rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DonTokenChanceFromSellBlackMarketTransaction)]
    public int DonTokenChanceFromSellBlackMarketTransaction { get; set; }
        
    /// <summary>
    /// Szansa na zdobycie żetonów odblokowania podczas zbierania rośliny.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.UnlockTokenChanceForCollectPlant)]
    public decimal UnlockTokenChanceForCollectPlant { get; set; }
        
        
        
        
     
    /// <summary>
    /// Procent haraczu dona.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DonCharityPercentage)]
    public decimal DonCharityPercentage { get; set; }
        
    /// <summary>
    /// Prestiż wymagany do osiągnięcia rangi Dona.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.PrestigeToBecomeDon)]
    public int PrestigeToBecomeDon { get; set; }
        
    /// <summary>
    /// Zadania o tą ilość poziomów niższe od poziomu plantacji nie będą już dostępne.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.LessLevelDoNotTakeQuests)]
    public int LessLevelDoNotTakeQuests { get; set; }
        
    /// <summary>
    /// Szansa na wystąpienie specjalnej oferty w strefie klienta.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.ChanceForSpecialOfferInCustomerZone)]
    public int ChanceForSpecialOfferInCustomerZone { get; set; }
        
        
        
        
        
        
    /// <summary>
    /// Dolna granica ilości standardowej oferty w strefie klienta.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.StandardOfferQuantityLowerRangeInCustomerZone)]
    public int StandardOfferQuantityLowerRangeInCustomerZone { get; set; }
        
    /// <summary>
    /// Górna granica ilości standardowej oferty w strefie klienta.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.StandardOfferQuantityHighRangeInCustomerZone)]
    public int StandardOfferQuantityHighRangeInCustomerZone { get; set; }
        
    /// <summary>
    /// Dolna granica ilości specjalnej oferty w strefie klienta.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.SpecialOfferQuantityLowerRangeInCustomerZone)]
    public int SpecialOfferQuantityLowerRangeInCustomerZone { get; set; }
        
    /// <summary>
    /// Górna granica ilości specjalnej oferty w strefie klienta.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.SpecialOfferQuantityHighRangeInCustomerZone)]
    public int SpecialOfferQuantityHighRangeInCustomerZone { get; set; }
        
        
        
        
        
        
    /// <summary>
    /// Czas oferty w strefie klienta.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.OfferTimeInCustomerZone)]
    public int OfferTimeInCustomerZone { get; set; }
        
    /// <summary>
    /// Szansa na zapłacenie haraczu donowi przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.ChanceForPayDonTribute)]
    public int ChanceForPayDonTribute { get; set; }
        
    /// <summary>
    /// Maksymalna ilość transakcji na czarnym rynku dla pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.MaxBlackMarketTransactionsCount)]
    public int MaxBlackMarketTransactionsCount { get; set; }
        
    /// <summary>
    /// Szansa na dodanie kupna transakcji na czarnym rynku przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.ChanceForAddBuyBlackMarketTransaction)]
    public int ChanceForAddBuyBlackMarketTransaction { get; set; }
        
    /// <summary>
    /// Szansa na wystawienie nasion względem suszu przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.ChanceForIssueSeedAgainstDriedFruitInBlackMarket)]
    public int ChanceForIssueSeedAgainstDriedFruitInBlackMarket { get; set; }
        
    /// <summary>
    /// Szansa na dodanie transakcji na czarnym rynku przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.ChanceForAddIssueBlackMarketTransaction)]
    public int ChanceForAddIssueBlackMarketTransaction { get; set; }
        
    /// <summary>
    /// Czy transakcje użytkowników są kupowane przez pracownika czarnego rynku w pierwszej kolejności.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.BuyUsersTransactions)]
    public bool BuyUsersTransactions { get; set; }
    
    /// <summary>
    /// Ilość losowo wybranych transakcji do kupienia.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.NumberDrawnTransactionsToBuy)]
    public int NumberDrawnTransactionsToBuy { get; set; }
    
    /// <summary>
    /// Ilość losowo wybranych transakcji do wystawienia.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.NumberDrawnTransactionsToIssue)]
    public int NumberDrawnTransactionsToIssue { get; set; }
        
        
        
        
        
    /// <summary>
    /// Ilość wystawianego suszu przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForDriedFruit)]
    public int BlackMarketIssueQuantityForDriedFruit { get; set; }
        
    /// <summary>
    /// Ilość wystawianych lamp przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForLamp)]
    public int BlackMarketIssueQuantityForLamp { get; set; }
        
    /// <summary>
    /// Ilość wystawianego nawozu przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForManure)]
    public int BlackMarketIssueQuantityForManure { get; set; }
        
    /// <summary>
    /// Ilość wystawianych doniczek przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForPot)]
    public int BlackMarketIssueQuantityForPot { get; set; }
        
    /// <summary>
    /// Ilość wystawianych nasion przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForSeed)]
    public int BlackMarketIssueQuantityForSeed { get; set; }
        
          
    /// <summary>
    /// Ilość wystawianej gleby przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForSoil)]
    public int BlackMarketIssueQuantityForSoil { get; set; }
        
    /// <summary>
    /// Ilość wystawianej wody przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForWater)]
    public int BlackMarketIssueQuantityForWater { get; set; }
        
    /// <summary>
    /// Ilość wystawianych bonusów przez pracownika czarnego rynku.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForBonus)]
    public int BlackMarketIssueQuantityForBonus { get; set; }
        
        
        
        
        
        
    /// <summary>
    /// Ilość otrzymanych żetonów za zadanie typu osiągnięcie/dzienne/inne.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.QuestTokenForAchievementDailyOthersAmount)]
    public int QuestTokenForAchievementDailyOthersAmount { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych żetonów za zadanie typu wydarzenie.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.QuestTokenForEventAmount)]
    public int QuestTokenForEventAmount { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych żetonów za zadanie typu tygodniowe.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.QuestTokenForWeeklyAmount)]
    public int QuestTokenForWeeklyAmount { get; set; }
        
        
        
        
        
        
        
    /// <summary>
    /// Ilość otrzymanych żetonów dealera w strefie klienta przy sprzedanej ilości większej niż 100.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountMoreThan100)]
    public int DealerTokenWithItemAmountMoreThan100 { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych żetonów dealera w strefie klienta przy sprzedanej ilości większej niż 10.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountMoreThan10)]
    public int DealerTokenWithItemAmountMoreThan10 { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych żetonów dealera w strefie klienta przy sprzedanej ilości mniejszej niż 11.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountLessThan11)]
    public int DealerTokenWithItemAmountLessThan11 { get; set; }
        
        
        
        
        
        
        
        
    /// <summary>
    /// Ilość otrzymanych żetonów czarnego rynku przy wartości sprzedaży większej niż 100 000. 
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan100000)]
    public int BlackMarketTokenWithCostMoreThan100000 { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych żetonów czarnego rynku przy wartości sprzedaży większej niż 10 000.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan10000)]
    public int BlackMarketTokenWithCostMoreThan10000 { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych żetonów czarnego rynku przy wartości sprzedaży większej niż 5 000.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan5000)]
    public int BlackMarketTokenWithCostMoreThan5000 { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych żetonów czarnego rynku przy wartości sprzedaży mniejszej niż 5 001.
    /// </summary>
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostLessThan5001)]
    public int BlackMarketTokenWithCostLessThan5001 { get; set; }
        
        
        
        
    /// <summary>
    /// Identyfikator użytkownika (opiekuna dzielnicy).
    /// </summary>
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.User)]
    public long UserId { get; set; }
}