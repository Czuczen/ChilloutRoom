using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca przedmioty na nowo zdobyte poziomy.
/// Tworzone przez buildera do analizy.
/// </summary>
public class CurrLvlItems
{
    /// <summary>
    /// Lista suszów.
    /// </summary>
    public List<DriedFruit> DriedFruits { get; } = new();
        
    /// <summary>
    /// Lista lamp.
    /// </summary>
    public List<Lamp> Lamps { get; } = new();
        
    /// <summary>
    /// Lista nawozów.
    /// </summary>
    public List<Manure> Manures { get; } = new();
        
    /// <summary>
    /// Lista donic.
    /// </summary>
    public List<Pot> Pots { get; } = new();
        
    /// <summary>
    /// Lista nasion.
    /// </summary>
    public List<Seed> Seeds { get; } = new();
        
    /// <summary>
    /// Lista gleb.
    /// </summary>
    public List<Soil> Soils { get; } = new();
        
    /// <summary>
    /// Lista wód.
    /// </summary>
    public List<Water> Waters { get; } = new();
        
    /// <summary>
    /// Lista zadań.
    /// </summary>
    public List<Quest> Quests { get; } = new();
        
    /// <summary>
    /// Lista bonusów.
    /// </summary>
    public List<Bonus> Bonuses { get; } = new();
}