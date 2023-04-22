using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

[AutoMapFrom(typeof(Bonus))]
public class BonusInfoViewModel : ProductInfoViewModel
{
    private decimal? _artifactPutCost;
    private decimal? _artifactPullCost;
    private int? _activeTimePerUse;
    private decimal? _increaseDropChanceFromQuests;
        
        
    [DisplayName(BonusFieldsHrNames.Color)]
    public string Color { get; set; }

    [DisplayName(BonusFieldsHrNames.IsStackable)]
    public bool? IsStackable { get; set; }

    [DisplayName(BonusFieldsHrNames.IsArtifact)]
    public bool IsArtifact { get; set; }

    [DisplayName(BonusFieldsHrNames.ArtifactPutCost)]
    public string ArtifactPutCost
    {
        get => (_artifactPutCost ?? 0) + "$";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _artifactPutCost = parsedValue;    
            }
        }
    }
        
    [DisplayName(BonusFieldsHrNames.ArtifactPullCost)]
    public string ArtifactPullCost
    {
        get => (_artifactPullCost ?? 0) + "$";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _artifactPullCost = parsedValue;    
            }
        }
    }

    [DisplayName(BonusFieldsHrNames.ActiveTimePerUse)]
    public string ActiveTimePerUse
    {
        get => (_activeTimePerUse ?? 0) + "sec";
        set
        { 
            if (int.TryParse(value, out var parsedValue))
            {
                _activeTimePerUse = parsedValue;    
            }
        }
    }

    [DisplayName(BonusFieldsHrNames.IncreaseDropChanceFromQuests)]
    public string IncreaseDropChanceFromQuests
    {
        get => (_increaseDropChanceFromQuests ?? 0) + "%";
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                _increaseDropChanceFromQuests = parsedValue;    
            }
        }
    }
}