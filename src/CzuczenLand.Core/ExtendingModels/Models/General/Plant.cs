using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingModels.Models.General;

public class Plant : Entity<int>, IPlantationGeneratedEntity
{
    public string Name { get; set; }
        
    public string Description { get; set; }
    
    public double GrowingLevel { get; set; }

    public decimal WiltLevel { get; set; }
        
    public string ImageUrl { get; set; }
        
    public decimal GrowingSpeed { get; set; }

    public int ChanceForSeed { get; set; }
        
    public int TimeOfInsensitivity { get; set; }
        
    public decimal DriedFruitAmount { get; set; }
        
    public int SeedAmount { get; set; }
        
    public decimal GainedExp { get; set; }
        
    public int TimeRemaining { get; set; }
        
    public decimal GrowingSpeedDivider { get; set; }
        
    public decimal WiltSpeed { get; set; }
        
    public int SetsBaf { get; set; }

    public int LampId { get; set; }
        
    public int SoilId { get; set; }
        
    public int PotId { get; set; }
        
    public int SeedId { get; set; }
        
    public int WaterId { get; set; }
        
    public int ManureId { get; set; }

    public int GeneratedTypeId { get; set; }
        
    public int? PlantationStorageId { get; set; } // nie będzie nullable ale dla zgodności z interfejsem ustawione na nullable
}