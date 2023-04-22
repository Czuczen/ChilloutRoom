using System;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.District.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.General.District))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.District))]
public class DistrictUpdateDto : EntityDto<int>
{
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.Name)]
    public string Name { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.StartTime)]
    public DateTime StartTime { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.EndTime)]
    public DateTime? EndTime { get; set; } // dzielnice ograniczone czasowo nie naliczają poziomu gracza i nie można na nich transferować golda gracza
        
    [DisplayName(DistrictFieldsHrNames.CyclicTime)]
    public int? CyclicTime { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.WiltSpeed)]
    public decimal WiltSpeed { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.GrowingSpeedDivider)]
    public decimal GrowingSpeedDivider { get; set; }
        
        
        
        
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.HollowPrice)]
    public decimal HollowPrice { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.StartGold)]
    public decimal StartGold { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.StartExpToNextLevel)]
    public decimal StartExpToNextLevel { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.PlayerLevelRequirement)]
    public int PlayerLevelRequirement { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.IsDefined)]
    public bool IsDefined { get; set; }
        

    

    [DisplayName(DistrictFieldsHrNames.HonorForTakingFirstPlace)]
    public int HonorForTakingFirstPlace { get; set; }
    
    [DisplayName(DistrictFieldsHrNames.HonorForTakingSecondPlace)]
    public int HonorForTakingSecondPlace { get; set; }
    
    [DisplayName(DistrictFieldsHrNames.HonorForTakingThirdPlace)]
    public int HonorForTakingThirdPlace { get; set; }
    
    [DisplayName(DistrictFieldsHrNames.HonorConsolationPrize)]
    public int HonorConsolationPrize { get; set; }
    

        
    
    [DisplayName(DistrictFieldsHrNames.MaxBuffsSlots)]
    public int MaxBuffsSlots { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.MaxArtifactSlots)]
    public int MaxArtifactSlots { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.MaxDailyQuestsCount)]
    public int MaxDailyQuestsCount { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.MaxWeeklyQuestsCount)]
    public int MaxWeeklyQuestsCount { get; set; }
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.UnlockedBuffsSlotsOnStart)]
    public int UnlockedBuffsSlotsOnStart { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.UnlockedArtifactSlotsOnStart)]
    public int UnlockedArtifactSlotsOnStart { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.UnlockedDailyQuestsOnStart)]
    public int UnlockedDailyQuestsOnStart { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.UnlockedWeeklyQuestsOnStart)]
    public int UnlockedWeeklyQuestsOnStart { get; set; }
        
        
        
        

    [DisplayName(DistrictFieldsHrNames.GoldExchangeRate)]
    public int GoldExchangeRate { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.PrestigeExchangeRate)]
    public int PrestigeExchangeRate { get; set; }

    [DisplayName(DistrictFieldsHrNames.QuestTokenExchangeRate)]
    public int QuestTokenExchangeRate { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenExchangeRate)]
    public int DealerTokenExchangeRate { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenExchangeRate)]
    public int BlackMarketTokenExchangeRate { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.DonTokenExchangeRate)]
    public int DonTokenExchangeRate { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.UnlockTokenExchangeRate)]
    public int UnlockTokenExchangeRate { get; set; }
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenChanceFromCompleteQuest)]
    public int QuestTokenChanceFromCompleteQuest { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenChanceFromCustomerZone)]
    public int DealerTokenChanceFromCustomerZone { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenChanceForBlackMarketTransaction)]
    public int BlackMarketTokenChanceForBlackMarketTransaction { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.DonTokenChanceFromSellBlackMarketTransaction)]
    public int DonTokenChanceFromSellBlackMarketTransaction { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.UnlockTokenChanceForCollectPlant)]
    public decimal UnlockTokenChanceForCollectPlant { get; set; }
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.DonCharityPercentage)]
    public decimal DonCharityPercentage { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.PrestigeToBecomeDon)]
    public int PrestigeToBecomeDon { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.LessLevelDoNotTakeQuests)]
    public int LessLevelDoNotTakeQuests { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForSpecialOfferInCustomerZone)]
    public int ChanceForSpecialOfferInCustomerZone { get; set; }
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.StandardOfferQuantityLowerRangeInCustomerZone)]
    public int StandardOfferQuantityLowerRangeInCustomerZone { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.StandardOfferQuantityHighRangeInCustomerZone)]
    public int StandardOfferQuantityHighRangeInCustomerZone { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.SpecialOfferQuantityLowerRangeInCustomerZone)]
    public int SpecialOfferQuantityLowerRangeInCustomerZone { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.SpecialOfferQuantityHighRangeInCustomerZone)]
    public int SpecialOfferQuantityHighRangeInCustomerZone { get; set; }
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.OfferTimeInCustomerZone)]
    public int OfferTimeInCustomerZone { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForPayDonTribute)]
    public int ChanceForPayDonTribute { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.MaxBlackMarketTransactionsCount)]
    public int MaxBlackMarketTransactionsCount { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForAddBuyBlackMarketTransaction)]
    public int ChanceForAddBuyBlackMarketTransaction { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForIssueSeedAgainstDriedFruitInBlackMarket)]
    public int ChanceForIssueSeedAgainstDriedFruitInBlackMarket { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForAddIssueBlackMarketTransaction)]
    public int ChanceForAddIssueBlackMarketTransaction { get; set; }
        
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.BuyUsersTransactions)]
    public bool BuyUsersTransactions { get; set; }
    
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.NumberDrawnTransactionsToBuy)]
    public int NumberDrawnTransactionsToBuy { get; set; }
    
    [FieldIsRequired]
    [DisplayName(DistrictFieldsHrNames.NumberDrawnTransactionsToIssue)]
    public int NumberDrawnTransactionsToIssue { get; set; }
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForDriedFruit)]
    public int BlackMarketIssueQuantityForDriedFruit { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForLamp)]
    public int BlackMarketIssueQuantityForLamp { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForManure)]
    public int BlackMarketIssueQuantityForManure { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForPot)]
    public int BlackMarketIssueQuantityForPot { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForSeed)]
    public int BlackMarketIssueQuantityForSeed { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForSoil)]
    public int BlackMarketIssueQuantityForSoil { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForWater)]
    public int BlackMarketIssueQuantityForWater { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForBonus)]
    public int BlackMarketIssueQuantityForBonus { get; set; }
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenForAchievementDailyOthersAmount)]
    public int QuestTokenForAchievementDailyOthersAmount { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenForEventAmount)]
    public int QuestTokenForEventAmount { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenForWeeklyAmount)]
    public int QuestTokenForWeeklyAmount { get; set; }
        
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountMoreThan100)]
    public int DealerTokenWithItemAmountMoreThan100 { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountMoreThan10)]
    public int DealerTokenWithItemAmountMoreThan10 { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountLessThan11)]
    public int DealerTokenWithItemAmountLessThan11 { get; set; }
        
        
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan100000)]
    public int BlackMarketTokenWithCostMoreThan100000 { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan10000)]
    public int BlackMarketTokenWithCostMoreThan10000 { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan5000)]
    public int BlackMarketTokenWithCostMoreThan5000 { get; set; }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostLessThan5001)]
    public int BlackMarketTokenWithCostLessThan5001 { get; set; }
        
        
        
        
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.User)]
    public long UserId { get; set; }
}