using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.StoragesInfo;

[AutoMapFrom(typeof(PlayerStorage))]
public class PlayerStorageInfoViewModel
{
    private decimal _gainedExperience;
    private decimal _gold; 
    private int _honor;
        
    public int Level { get; set; }
        
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
        
    public string Honor
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Int, _honor);
        set
        {
            if (int.TryParse(value, out var parsedValue))
            {
                _honor = parsedValue;
            }   
        }
    }

    private IParserStrategy DisplayStrategy { get; } = new DisplayStrategy();
}