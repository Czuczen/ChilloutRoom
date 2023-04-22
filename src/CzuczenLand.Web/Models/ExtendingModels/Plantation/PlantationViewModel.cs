using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.GameStorage;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.StoragesInfo;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation;

public class PlantationViewModel
{
    public long UserId { get; set; }
        
    public DonData DonData { get; set; }

    public int? SelectedDistrictId { get; set; }
        
    public List<Select2Data> S2DistrictsList { get; set; }
        
    public bool IsNewPlayer { get; set; }
        
    public PlayerStorageInfoViewModel PlayerStorageInfoViewModel { get; set; }
        
    public PlantationStorageInfoViewModel PlantationStorageInfoViewModel { get; set; }

    public List<PlantViewModel> PlantsInfo { get; set; }
        
    public bool TooLowLevel { get; set; }

    public bool MustBuyHollow { get; set; }
        
    public decimal HollowPrice { get; set; }
        
    public string DistrictLoadingMessage { get; set; }
        
    public QuestsViewModel QuestsViewModel { get; set; }
        
    public StorageViewModel StorageViewModel { get; set; }

    public bool HasPlantation => PlantationStorageInfoViewModel != null;
        
    public decimal? CurrExpPercent => PlantationStorageInfoViewModel?.TempCurrExp / PlantationStorageInfoViewModel?.TempExpToNextLevel * 100;
        
    public string CurrExpWidthPercent => CurrExpPercent?.ToString("0.##").Replace(",", ".") + "%;";
        
    public List<BonusViewModel> Bonuses { get; set; }

    public List<BonusViewModel> Buffs => Bonuses.Where(item => !item.IsArtifact).ToList();
        
    public List<BonusViewModel> Artefacts => Bonuses.Where(item => item.IsArtifact).ToList();
}