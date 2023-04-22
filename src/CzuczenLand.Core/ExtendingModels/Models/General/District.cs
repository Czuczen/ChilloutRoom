using System;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingModels.Models.General;

public class District : Entity<int>, INamedEntity
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; } // dzielnice ograniczone czasowo nie naliczają poziomu gracza i nie można na nich transferować golda gracza 
    public int? CyclicTime { get; set; }
    public decimal WiltSpeed { get; set; }
    public decimal GrowingSpeedDivider { get; set; }
        
        
    public decimal HollowPrice { get; set; }
    public decimal StartGold { get; set; }
    public decimal StartExpToNextLevel { get; set; }
    public int PlayerLevelRequirement { get; set; }
    public bool IsDefined { get; set; }



    public int HonorForTakingFirstPlace { get; set; }
    public int HonorForTakingSecondPlace { get; set; }
    public int HonorForTakingThirdPlace { get; set; }
    public int HonorConsolationPrize { get; set; }
        
        
    public int MaxBuffsSlots { get; set; }
    public int MaxArtifactSlots { get; set; }
    public int MaxDailyQuestsCount { get; set; }
    public int MaxWeeklyQuestsCount { get; set; }
        
        
    public int UnlockedBuffsSlotsOnStart { get; set; }
    public int UnlockedArtifactSlotsOnStart { get; set; }
    public int UnlockedDailyQuestsOnStart { get; set; }
    public int UnlockedWeeklyQuestsOnStart { get; set; }
        

    public int GoldExchangeRate { get; set; }
    public int PrestigeExchangeRate { get; set; }
    public int QuestTokenExchangeRate { get; set; }
    public int DealerTokenExchangeRate { get; set; }
    public int BlackMarketTokenExchangeRate { get; set; }
    public int DonTokenExchangeRate { get; set; }
    public int UnlockTokenExchangeRate { get; set; }
        
        
    public int QuestTokenChanceFromCompleteQuest { get; set; }
    public int DealerTokenChanceFromCustomerZone { get; set; }
    public int BlackMarketTokenChanceForBlackMarketTransaction { get; set; }
    public int DonTokenChanceFromSellBlackMarketTransaction { get; set; }
    public decimal UnlockTokenChanceForCollectPlant { get; set; }
        
        
    public decimal DonCharityPercentage { get; set; }
    public int PrestigeToBecomeDon { get; set; }
    public int LessLevelDoNotTakeQuests { get; set; }
    public int ChanceForSpecialOfferInCustomerZone { get; set; }
        
        
    public int StandardOfferQuantityLowerRangeInCustomerZone { get; set; }
    public int StandardOfferQuantityHighRangeInCustomerZone { get; set; }
    public int SpecialOfferQuantityLowerRangeInCustomerZone { get; set; }
    public int SpecialOfferQuantityHighRangeInCustomerZone { get; set; }
        
        
    public int OfferTimeInCustomerZone { get; set; }
    public int ChanceForPayDonTribute { get; set; }
    public int MaxBlackMarketTransactionsCount { get; set; }
    public int ChanceForAddBuyBlackMarketTransaction { get; set; }
    public int ChanceForIssueSeedAgainstDriedFruitInBlackMarket { get; set; }
    public int ChanceForAddIssueBlackMarketTransaction { get; set; }
    public bool BuyUsersTransactions { get; set; }
    public int NumberDrawnTransactionsToBuy { get; set; }
    public int NumberDrawnTransactionsToIssue { get; set; }
    
    

    public int BlackMarketIssueQuantityForDriedFruit { get; set; }
    public int BlackMarketIssueQuantityForLamp { get; set; }
    public int BlackMarketIssueQuantityForManure { get; set; }
    public int BlackMarketIssueQuantityForPot { get; set; }
    public int BlackMarketIssueQuantityForSeed { get; set; }
    public int BlackMarketIssueQuantityForSoil { get; set; }
    public int BlackMarketIssueQuantityForWater { get; set; }
    public int BlackMarketIssueQuantityForBonus { get; set; }
        
        
    public int QuestTokenForAchievementDailyOthersAmount { get; set; }
    public int QuestTokenForEventAmount { get; set; }
    public int QuestTokenForWeeklyAmount { get; set; }
        
        
    public int DealerTokenWithItemAmountMoreThan100 { get; set; }
    public int DealerTokenWithItemAmountMoreThan10 { get; set; }
    public int DealerTokenWithItemAmountLessThan11 { get; set; }
        
        
    public int BlackMarketTokenWithCostMoreThan100000 { get; set; }
    public int BlackMarketTokenWithCostMoreThan10000 { get; set; }
    public int BlackMarketTokenWithCostMoreThan5000 { get; set; }
    public int BlackMarketTokenWithCostLessThan5001 { get; set; }
        
        
    public long UserId { get; set; }
}