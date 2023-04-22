using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;

public class Plantation
{
    public int? SelectedDistrictId { get; set; }
        
    public List<Select2Data> S2DistrictsList { get; set; }
        
    public bool IsNewPlayer { get; set; }
        
    public bool TooLowLevel { get; set; }

    public PlayerStorage PlayerStorage { get; set; }
        
    public PlantationStorage PlantationStorage { get; set; }
        
    public District District { get; set; }
    
    public List<District> AvailableDistricts { get; set; }
    
    public Dictionary<int, string> WardensNames { get; set; }

    public List<Plant> Plants { get; set; }
        
    public bool MustBuyHollow { get; set; }
        
    public string DistrictLoadingMessage { get; set; }
        
    public decimal HollowPrice { get; set; }
        
    public List<Quest> FilteredQuests { get; set; }
        
    public List<DriedFruit> UserDriedFruits { get; set; }
    public List<Lamp> UserLamps { get; set; }
    public List<Manure> UserManures { get; set; }
    public List<Soil> UserSoils { get; set; }
    public List<Water> UserWaters { get; set; }
    public List<Seed> UserSeeds { get; set; }
    public List<Pot> UserPots { get; set; }
        
    public List<Bonus> UserBonuses { get; set; }

    public List<CurrencyExchange> CurrencyExchanges { get; } = new();

    public DonData DonData { get; set; } = new();

    public QuestInfoCreation QuestInfoCreation { get; set; } = new();
}