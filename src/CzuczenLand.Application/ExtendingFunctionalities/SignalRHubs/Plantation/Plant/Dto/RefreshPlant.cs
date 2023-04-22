namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Plant.Dto;

public class RefreshPlant
{
    public int PlantId { get; set; }
        
    public int TimeRemaining { get; set; }
        
    public int TimeOfInsensitivity { get; set; }
        
    public decimal GrowingLevel { get; set; }
        
    public decimal WiltLevel { get; set; }
}