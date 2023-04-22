using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.GameStorage;

public class StorageViewModel
{
    private decimal _plantationGold;

    
    public List<CurrencyExchangeViewModel> CurrencyExchanges { get; set; }
        
    public decimal PlayerGold { get; set; }
        
    public string PlantationGold
    {
        get => (string) DisplayStrategy.ParseValue(EnumUtils.PropTypes.Decimal, _plantationGold);
        set
        {
            if (decimal.TryParse(value.Replace(".", ","), out var parsedValue))
            {
                _plantationGold = parsedValue;
            }   
        }
    }

    public List<StorageProductViewModel> DriedFruits { get; } = new();
    public List<StorageProductViewModel> Lamps { get; } = new();
    public List<StorageProductViewModel> Manures { get; } = new();
    public List<StorageProductViewModel> Pots { get; } = new();
    public List<StorageProductViewModel> Seeds { get; } = new();
    public List<StorageProductViewModel> Soils { get; } = new();
    public List<StorageProductViewModel> Waters { get; } = new();
    public List<StorageProductViewModel> Bonuses { get; } = new();


    public List<StorageProductViewModel> ShopDriedFruits => DriedFruits.Where(item => item.IsShopItem).ToList();
    public List<StorageProductViewModel> ShopLamps => Lamps.Where(item => item.IsShopItem).ToList();
    public List<StorageProductViewModel> ShopManures => Manures.Where(item => item.IsShopItem).ToList();
    public List<StorageProductViewModel> ShopPots => Pots.Where(item => item.IsShopItem).ToList();
    public List<StorageProductViewModel> ShopSeeds => Seeds.Where(item => item.IsShopItem).ToList();
    public List<StorageProductViewModel> ShopSoils => Soils.Where(item => item.IsShopItem).ToList();
    public List<StorageProductViewModel> ShopWaters => Waters.Where(item => item.IsShopItem).ToList();
    public List<StorageProductViewModel> ShopBuffs => Bonuses.Where(item => item.IsShopItem && !item.IsArtifact).ToList();
    public List<StorageProductViewModel> ShopArtifacts => Bonuses.Where(item => item.IsShopItem && item.IsArtifact).ToList();
        
        
    public List<StorageProductViewModel> NotShopDriedFruits => DriedFruits.Where(item => !item.IsShopItem).ToList();
    public List<StorageProductViewModel> NotShopLamps => Lamps.Where(item => !item.IsShopItem).ToList();
    public List<StorageProductViewModel> NotShopManures => Manures.Where(item => !item.IsShopItem).ToList();
    public List<StorageProductViewModel> NotShopPots => Pots.Where(item => !item.IsShopItem).ToList();
    public List<StorageProductViewModel> NotShopSeeds => Seeds.Where(item => !item.IsShopItem).ToList();
    public List<StorageProductViewModel> NotShopSoils => Soils.Where(item => !item.IsShopItem).ToList();
    public List<StorageProductViewModel> NotShopWaters => Waters.Where(item => !item.IsShopItem).ToList();
    public List<StorageProductViewModel> NoShopBuffs => Bonuses.Where(item => !item.IsShopItem && !item.IsArtifact).ToList();
    public List<StorageProductViewModel> NoShopArtifacts => Bonuses.Where(item => !item.IsShopItem && item.IsArtifact).ToList();
        
        
    public List<QuestInfoViewModel> QuestsInfo { get; } = new();
    private IParserStrategy DisplayStrategy { get; } = new DisplayStrategy();

    public List<DistrictViewModel> Districts { get; } = new();
}