using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

public class CurrLvlItems
{
    public List<DriedFruit> DriedFruits { get; } = new();
        
    public List<Lamp> Lamps { get; } = new();
        
    public List<Manure> Manures { get; } = new();
        
    public List<Pot> Pots { get; } = new();
        
    public List<Seed> Seeds { get; } = new();
        
    public List<Soil> Soils { get; } = new();
        
    public List<Water> Waters { get; } = new();
        
    public List<Quest> Quests { get; } = new();
        
    public List<Bonus> Bonuses { get; } = new();
}