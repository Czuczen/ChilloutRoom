using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.StoragesInfo;

[AutoMapFrom(typeof(PlantationStorage))]
public class PlantationStorageInfoViewModel
{
    public decimal TempCurrExp;
    public decimal TempExpToNextLevel;
    private decimal _gainedExperience;
    private decimal _gold; 
    private decimal _prestige;
    private int _unlockToken;
    private int _donToken;
    private int _questToken;
    private int _blackMarketToken;
    private int _dealerToken;
        
        
    public int UnlockedArtifactSlots { get; set; }

    public int UnlockedBuffsSlots { get; set; }
        
    public int MaxBuffsSlots { get; set; }
        
    public int MaxDailyQuestsCount { get; set; }
        
    public int MaxWeeklyQuestsCount { get; set; }
        
    public int UnlockedDailyQuestsCount { get; set; }
        
    public int UnlockedWeeklyQuestsCount { get; set; }
        
    public int MaxArtifactSlots { get; set; }
        
    public int ArtifactSlotsInUse { get; set; }
        
    public int BuffSlotsInUse { get; set; }
        
    public int Level { get; set; }
        
    public int StartedDailyQuestsCount { get; set; }
        
    public int StartedWeeklyQuestsCount { get; set; }
        
    public string GainedExperience 
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _gainedExperience);
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _gainedExperience = parsedValue;
            }   
        }
    }
        
    public string CurrExp
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, TempCurrExp);
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                TempCurrExp = parsedValue;
            }   
        }
    }
        
    public string ExpToNextLevel
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, TempExpToNextLevel);
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                TempExpToNextLevel = parsedValue;
            }   
        }
    }
        
    public string Gold
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _gold);
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _gold = parsedValue;
            }   
        }
    }

    public string Prestige
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _prestige);
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _prestige = parsedValue;
            }   
        }
    }
        
    public string UnlockToken
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _unlockToken);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _unlockToken = parsedValue;
            }   
        }
    }
        
    public string DonToken
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _donToken);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _donToken = parsedValue;
            }   
        }
    }
        
    public string QuestToken
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _questToken);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _questToken = parsedValue;
            }   
        }
    }
        
    public string DealerToken
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _dealerToken);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _dealerToken = parsedValue;
            }   
        }
    }
        
    public string BlackMarketToken
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _blackMarketToken);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _blackMarketToken = parsedValue;
            }   
        }
    }

    private IParserStrategy DisplayStrategy { get; } = new DisplayStrategy();
}