using CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation;

public class PlantViewModel
{
    public LampInfoViewModel LampInfoViewModel { get; set; }

    public ManureInfoViewModel ManureInfoViewModel { get; set; }
        
    public PlantInfoViewModel PlantInfoViewModel { get; set; }
        
    public PotInfoViewModel PotInfoViewModel { get; set; }
        
    public SeedInfoViewModel SeedInfoViewModel { get; set; }
        
    public SoilInfoViewModel SoilInfoViewModel { get; set; }
        
    public WaterInfoViewModel WaterInfoViewModel { get; set; }
        
    public string SetSameDescriptionLikeSeed
    {
        get
        {
            SeedInfoViewModel.Description = "Taki sam jak roślinki";
            return "";
        }
    }
        
    public string WiltAsPercentage => PlantInfoViewModel.WiltLevel.ToString("0.00").Replace(",", ".") + "%";
    public string WiltAsDisplayDecimal => PlantInfoViewModel.WiltLevel.ToString("0.00").Replace(",", ".");
    public string WiltBarIsHidden => PlantInfoViewModel.WiltLevel == 0 ? "visibility-hidden" : "";
    public string WiltBarIsActive => PlantInfoViewModel.WiltLevel == 100 ? "" : "active";
    public string WiltBarText => WiltAsPercentage.Replace(".", ",");
    private string GrowingLevelAsPercentage => PlantInfoViewModel.GrowingLevel.ToString("0.00").Replace(",", ".") + "%";
    private string RestOfGrowingLevel => (100 - PlantInfoViewModel.WiltLevel).ToString("0.00").Replace(",", ".") + "%";
    public string GrowingBarFill => PlantInfoViewModel.GrowingLevel > 100 - PlantInfoViewModel.WiltLevel ? RestOfGrowingLevel : GrowingLevelAsPercentage;
    public string GrowingBarIsActive => PlantInfoViewModel.GrowingLevel == 100 ? "" : "active";
    public string GrowingLevelAsDisplayDecimal => PlantInfoViewModel.GrowingLevel.ToString("0.00").Replace(",", ".");
    public string GrowingBarText => (PlantInfoViewModel.WiltLevel == 100 ? "" : GrowingLevelAsPercentage).Replace(".", ",");
    public string RemoveBtnIsShowed => PlantInfoViewModel.WiltLevel == 100 ? "d-block" : "d-none";
    public string CollectBtnIsShowed => PlantInfoViewModel.GrowingLevel == 100 && PlantInfoViewModel.WiltLevel != 100 ? "d-block" : "d-none";
}