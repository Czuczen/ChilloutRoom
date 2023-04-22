using System;
using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation;

[AutoMapFrom(typeof(District))]
public class DistrictViewModel
{
    private DateTime _startTime;
    private DateTime? _endTime;
    private int? _cyclicTime;
    private decimal _wiltSpeed;
    private decimal _growingSpeedDivider;
    private decimal _hollowPrice;
    private decimal _startGold;
    private decimal _startExpToNextLevel;
    private int _playerLevelRequirement;
    private bool _isDefined;
    private int _honorForTakingFirstPlace;
    private int _honorForTakingSecondPlace;
    private int _honorForTakingThirdPlace;
    private int _honorConsolationPrize;
    private int _maxBuffsSlots;
    private int _maxArtifactSlots;
    private int _maxDailyQuestsCount;
    private int _maxWeeklyQuestsCount;
    private int _unlockedBuffsSlotsOnStart;
    private int _unlockedArtifactSlotsOnStart;
    private int _unlockedDailyQuestsOnStart;
    private int _unlockedWeeklyQuestsOnStart;
    private int _goldExchangeRate;
    private int _prestigeExchangeRate;
    private int _questTokenExchangeRate;
    private int _dealerTokenExchangeRate;
    private int _blackMarketTokenExchangeRate;
    private int _donTokenExchangeRate;
    private int _unlockTokenExchangeRate;
    private int _questTokenChanceFromCompleteQuest;
    private int _dealerTokenChanceFromCustomerZone;
    private int _blackMarketTokenChanceForBlackMarketTransaction;
    private int _donTokenChanceFromSellBlackMarketTransaction;
    private decimal _unlockTokenChanceForCollectPlant;
    private decimal _donCharityPercentage;
    private int _prestigeToBecomeDon;
    private int _lessLevelDoNotTakeQuests;
    private int _chanceForSpecialOfferInCustomerZone;
    private int _standardOfferQuantityLowerRangeInCustomerZone;
    private int _standardOfferQuantityHighRangeInCustomerZone;
    private int _specialOfferQuantityLowerRangeInCustomerZone;
    private int _specialOfferQuantityHighRangeInCustomerZone;
    private int _offerTimeInCustomerZone;
    private int _chanceForPayDonTribute;
    private int _maxBlackMarketTransactionsCount;
    private int _chanceForBuyBlackMarketTransaction;
    private int _chanceForIssueSeedAgainstDriedFruitInBlackMarket;
    private int _chanceForIssueBlackMarketTransaction;
    private bool _buyUsersTransactions;
    private int _numberDrawnTransactionsToBuy;
    private int _numberDrawnTransactionsToIssue;
    private int _blackMarketIssueQuantityForDriedFruit;
    private int _blackMarketIssueQuantityForLamp;
    private int _blackMarketIssueQuantityForManure;
    private int _blackMarketIssueQuantityForPot;
    private int _blackMarketIssueQuantityForSeed;
    private int _blackMarketIssueQuantityForSoil;
    private int _blackMarketIssueQuantityForWater;
    private int _blackMarketIssueQuantityForBonus;
    private int _questTokenForAchievementDailyOthersAmount;
    private int _questTokenForEventAmount;
    private int _questTokenForWeeklyAmount;
    private int _dealerTokenWithItemAmountMoreThan100;
    private int _dealerTokenWithItemAmountMoreThan10;
    private int _dealerTokenWithItemAmountLessThan11;
    private int _blackMarketTokenWithCostMoreThan100000;
    private int _blackMarketTokenWithCostMoreThan10000;
    private int _blackMarketTokenWithCostMoreThan5000;
    private int _blackMarketTokenWithCostLessThan5001;
        
        
        
    [DisplayName(DistrictFieldsHrNames.Name)]
    public string Name { get; set; }

    [DisplayName(DistrictFieldsHrNames.WardenName)]
    public string WardenName { get; set; }

        
        
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.StartTime)]
    public string StartTime
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.DateTime, _startTime);
        set
        {
            if (DateTime.TryParse(value, out var parsedValue))
            {
                _startTime = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.EndTime)]
    public string EndTime
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.DateTime, _endTime);
        set
        {
            if (DateTime.TryParse(value, out var parsedValue))
            {
                _endTime = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.CyclicTime)]
    public string CyclicTime
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _cyclicTime) + (_cyclicTime != null ? "min" : "");
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _cyclicTime = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.WiltSpeed)]
    public string WiltSpeed
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _wiltSpeed) + "/sec";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _wiltSpeed = parsedValue;
            }
        }
    }

        
    [DisplayName(DistrictFieldsHrNames.GrowingSpeedDivider)]
    public string GrowingSpeedDivider
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _growingSpeedDivider);
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _growingSpeedDivider = parsedValue;
            }
        }
    }
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.HollowPrice)]
    public string HollowPrice
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _hollowPrice) + "$";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _hollowPrice = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.StartGold)]
    public string StartGold
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _startGold) + "$";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _startGold = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.StartExpToNextLevel)]
    public string StartExpToNextLevel
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _startExpToNextLevel) + "pkt";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _startExpToNextLevel = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.PlayerLevelRequirement)]
    public string PlayerLevelRequirement
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _playerLevelRequirement);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _playerLevelRequirement = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.IsDefined)]
    public string IsDefined
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Boolean, _isDefined);
        set
        {
            if (bool.TryParse(value, out var parsedValue))
            {
                _isDefined = parsedValue;
            }
        }
    }
        


        
    [DisplayName(DistrictFieldsHrNames.HonorForTakingFirstPlace)]
    public string HonorForTakingFirstPlace
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _honorForTakingFirstPlace);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _honorForTakingFirstPlace = parsedValue;
            }
        }
    }
    
    [DisplayName(DistrictFieldsHrNames.HonorForTakingSecondPlace)]
    public string HonorForTakingSecondPlace
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _honorForTakingSecondPlace);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _honorForTakingSecondPlace = parsedValue;
            }
        }
    }
    
    [DisplayName(DistrictFieldsHrNames.HonorForTakingThirdPlace)]
    public string HonorForTakingThirdPlace
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _honorForTakingThirdPlace);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _honorForTakingThirdPlace = parsedValue;
            }
        }
    }
    
    [DisplayName(DistrictFieldsHrNames.HonorConsolationPrize)]
    public string HonorConsolationPrize
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _honorConsolationPrize);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _honorConsolationPrize = parsedValue;
            }
        }
    }
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.MaxBuffsSlots)]
    public string MaxBuffsSlots
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _maxBuffsSlots);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _maxBuffsSlots = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.MaxArtifactSlots)]
    public string MaxArtifactSlots
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _maxArtifactSlots);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _maxArtifactSlots = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.MaxDailyQuestsCount)]
    public string MaxDailyQuestsCount
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _maxDailyQuestsCount);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _maxDailyQuestsCount = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.MaxWeeklyQuestsCount)]
    public string MaxWeeklyQuestsCount
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _maxWeeklyQuestsCount);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _maxWeeklyQuestsCount = parsedValue;
            }
        }
    }
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.UnlockedBuffsSlotsOnStart)]
    public string UnlockedBuffsSlotsOnStart
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _unlockedBuffsSlotsOnStart);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _unlockedBuffsSlotsOnStart = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.UnlockedArtifactSlotsOnStart)]
    public string UnlockedArtifactSlotsOnStart
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _unlockedArtifactSlotsOnStart);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _unlockedArtifactSlotsOnStart = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.UnlockedDailyQuestsOnStart)]
    public string UnlockedDailyQuestsOnStart
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _unlockedDailyQuestsOnStart);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _unlockedDailyQuestsOnStart = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.UnlockedWeeklyQuestsOnStart)]
    public string UnlockedWeeklyQuestsOnStart
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _unlockedWeeklyQuestsOnStart);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _unlockedWeeklyQuestsOnStart = parsedValue;
            }
        }
    }
        
        
        
        

    [DisplayName(DistrictFieldsHrNames.GoldExchangeRate)]
    public string GoldExchangeRate
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _goldExchangeRate);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _goldExchangeRate = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.PrestigeExchangeRate)]
    public string PrestigeExchangeRate
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _prestigeExchangeRate);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _prestigeExchangeRate = parsedValue;
            }
        }
    }

    [DisplayName(DistrictFieldsHrNames.QuestTokenExchangeRate)]
    public string QuestTokenExchangeRate
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _questTokenExchangeRate);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _questTokenExchangeRate = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenExchangeRate)]
    public string DealerTokenExchangeRate
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _dealerTokenExchangeRate);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _dealerTokenExchangeRate = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenExchangeRate)]
    public string BlackMarketTokenExchangeRate
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketTokenExchangeRate);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketTokenExchangeRate = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.DonTokenExchangeRate)]
    public string DonTokenExchangeRate
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _donTokenExchangeRate);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _donTokenExchangeRate = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.UnlockTokenExchangeRate)]
    public string UnlockTokenExchangeRate
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _unlockTokenExchangeRate);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _unlockTokenExchangeRate = parsedValue;
            }
        }
    }
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenChanceFromCompleteQuest)]
    public string QuestTokenChanceFromCompleteQuest
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _questTokenChanceFromCompleteQuest) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _questTokenChanceFromCompleteQuest = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenChanceFromCustomerZone)]
    public string DealerTokenChanceFromCustomerZone
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _dealerTokenChanceFromCustomerZone) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _dealerTokenChanceFromCustomerZone = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenChanceForBlackMarketTransaction)]
    public string BlackMarketTokenChanceForBlackMarketTransaction
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketTokenChanceForBlackMarketTransaction) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketTokenChanceForBlackMarketTransaction = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.DonTokenChanceFromSellBlackMarketTransaction)]
    public string DonTokenChanceFromSellBlackMarketTransaction
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _donTokenChanceFromSellBlackMarketTransaction) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _donTokenChanceFromSellBlackMarketTransaction = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.UnlockTokenChanceForCollectPlant)]
    public string UnlockTokenChanceForCollectPlant
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _unlockTokenChanceForCollectPlant) + "%";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _unlockTokenChanceForCollectPlant = parsedValue;
            }
        }
    }
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.DonCharityPercentage)]
    public string DonCharityPercentage
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _donCharityPercentage) + "%";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _donCharityPercentage = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.PrestigeToBecomeDon)]
    public string PrestigeToBecomeDon
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _prestigeToBecomeDon) + "pkt";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _prestigeToBecomeDon = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.LessLevelDoNotTakeQuests)]
    public string LessLevelDoNotTakeQuests
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _lessLevelDoNotTakeQuests);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _lessLevelDoNotTakeQuests = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForSpecialOfferInCustomerZone)]
    public string ChanceForSpecialOfferInCustomerZone
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _chanceForSpecialOfferInCustomerZone) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _chanceForSpecialOfferInCustomerZone = parsedValue;
            }
        }
    }
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.StandardOfferQuantityLowerRangeInCustomerZone)]
    public string StandardOfferQuantityLowerRangeInCustomerZone
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _standardOfferQuantityLowerRangeInCustomerZone);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _standardOfferQuantityLowerRangeInCustomerZone = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.StandardOfferQuantityHighRangeInCustomerZone)]
    public string StandardOfferQuantityHighRangeInCustomerZone
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _standardOfferQuantityHighRangeInCustomerZone);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _standardOfferQuantityHighRangeInCustomerZone = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.SpecialOfferQuantityLowerRangeInCustomerZone)]
    public string SpecialOfferQuantityLowerRangeInCustomerZone
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _specialOfferQuantityLowerRangeInCustomerZone);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _specialOfferQuantityLowerRangeInCustomerZone = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.SpecialOfferQuantityHighRangeInCustomerZone)]
    public string SpecialOfferQuantityHighRangeInCustomerZone
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _specialOfferQuantityHighRangeInCustomerZone);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _specialOfferQuantityHighRangeInCustomerZone = parsedValue;
            }
        }
    }
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.OfferTimeInCustomerZone)]
    public string OfferTimeInCustomerZone
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _offerTimeInCustomerZone) + "ms";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _offerTimeInCustomerZone = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForPayDonTribute)]
    public string ChanceForPayDonTribute
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _chanceForPayDonTribute) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _chanceForPayDonTribute = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.MaxBlackMarketTransactionsCount)]
    public string MaxBlackMarketTransactionsCount
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _maxBlackMarketTransactionsCount);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _maxBlackMarketTransactionsCount = parsedValue;
            }
        }
    }
        
        
    [DisplayName(DistrictFieldsHrNames.ChanceForAddBuyBlackMarketTransaction)]
    public string ChanceForAddBuyBlackMarketTransaction
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _chanceForBuyBlackMarketTransaction) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _chanceForBuyBlackMarketTransaction = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForIssueSeedAgainstDriedFruitInBlackMarket)]
    public string ChanceForIssueSeedAgainstDriedFruitInBlackMarket
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _chanceForIssueSeedAgainstDriedFruitInBlackMarket) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _chanceForIssueSeedAgainstDriedFruitInBlackMarket = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.ChanceForAddIssueBlackMarketTransaction)]
    public string ChanceForAddIssueBlackMarketTransaction
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _chanceForIssueBlackMarketTransaction) + "%";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _chanceForIssueBlackMarketTransaction = parsedValue;
            }
        }
    }
    
    [DisplayName(DistrictFieldsHrNames.BuyUsersTransactions)]
    public string BuyUsersTransactions
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Boolean, _buyUsersTransactions);
        set
        {
            if (bool.TryParse(value, out var parsedValue))
            {
                _buyUsersTransactions = parsedValue;
            }
        }
    }

    [DisplayName(DistrictFieldsHrNames.NumberDrawnTransactionsToBuy)]
    public string NumberDrawnTransactionsToBuy
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _numberDrawnTransactionsToBuy) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _numberDrawnTransactionsToBuy = parsedValue;
            }
        }
    }

    [DisplayName(DistrictFieldsHrNames.NumberDrawnTransactionsToIssue)]
    public string NumberDrawnTransactionsToIssue
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _numberDrawnTransactionsToIssue) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _numberDrawnTransactionsToIssue = parsedValue;
            }
        }
    }
        
        
        
    
        
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForDriedFruit)]
    public string BlackMarketIssueQuantityForDriedFruit
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForDriedFruit);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForDriedFruit = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForLamp)]
    public string BlackMarketIssueQuantityForLamp
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForLamp);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForLamp = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForManure)]
    public string BlackMarketIssueQuantityForManure
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForManure);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForManure = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForPot)]
    public string BlackMarketIssueQuantityForPot
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForPot);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForPot = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForSeed)]
    public string BlackMarketIssueQuantityForSeed
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForSeed);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForSeed = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForSoil)]
    public string BlackMarketIssueQuantityForSoil
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForSoil);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForSoil = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForWater)]
    public string BlackMarketIssueQuantityForWater
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForWater);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForWater = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketIssueQuantityForBonus)]
    public string BlackMarketIssueQuantityForBonus
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketIssueQuantityForBonus);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketIssueQuantityForBonus = parsedValue;
            }
        }
    }
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenForAchievementDailyOthersAmount)]
    public string QuestTokenForAchievementDailyOthersAmount
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _questTokenForAchievementDailyOthersAmount) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _questTokenForAchievementDailyOthersAmount = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenForEventAmount)]
    public string QuestTokenForEventAmount
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _questTokenForEventAmount) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _questTokenForEventAmount = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.QuestTokenForWeeklyAmount)]
    public string QuestTokenForWeeklyAmount
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _questTokenForWeeklyAmount) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _questTokenForWeeklyAmount = parsedValue;
            }
        }
    }
        
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountMoreThan100)]
    public string DealerTokenWithItemAmountMoreThan100
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _dealerTokenWithItemAmountMoreThan100) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _dealerTokenWithItemAmountMoreThan100 = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountMoreThan10)]
    public string DealerTokenWithItemAmountMoreThan10
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _dealerTokenWithItemAmountMoreThan10) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _dealerTokenWithItemAmountMoreThan10 = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.DealerTokenWithItemAmountLessThan11)]
    public string DealerTokenWithItemAmountLessThan11
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _dealerTokenWithItemAmountLessThan11) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _dealerTokenWithItemAmountLessThan11 = parsedValue;
            }
        }
    }
        
        
        
        
        
        
        
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan100000)]
    public string BlackMarketTokenWithCostMoreThan100000
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketTokenWithCostMoreThan100000) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketTokenWithCostMoreThan100000 = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan10000)]
    public string BlackMarketTokenWithCostMoreThan10000
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketTokenWithCostMoreThan10000) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketTokenWithCostMoreThan10000 = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostMoreThan5000)]
    public string BlackMarketTokenWithCostMoreThan5000
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketTokenWithCostMoreThan5000) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketTokenWithCostMoreThan5000 = parsedValue;
            }
        }
    }
        
    [DisplayName(DistrictFieldsHrNames.BlackMarketTokenWithCostLessThan5001)]
    public string BlackMarketTokenWithCostLessThan5001
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketTokenWithCostLessThan5001) + "szt.";
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketTokenWithCostLessThan5001 = parsedValue;
            }
        }
    }
        
        
    private IParserStrategy DisplayStrategy { get; } = new DisplayStrategy();
}