using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

/// <summary>
/// Klasa reprezentująca kontekst dzielnicy.
/// Zawiera wszystkie obiekty które składają się na w pełni działającą dzielnicę.
/// </summary>
public class DistrictContext
{
    /// <summary>
    /// Nazwa opiekuna dzielnicy.
    /// </summary>
    public string WardenName { get; set; }

    /// <summary>
    /// Hasło opiekuna dzielnicy.
    /// </summary>
    public string WardenPassword { get; set; }
    
    /// <summary>
    /// Obiekt reprezentujący dzielnicę.
    /// </summary>
    public District District { get; set; }
    
    /// <summary>
    /// Lista typów generowanych dzielnicy.
    /// </summary>
    public List<GeneratedType> GeneratedTypes { get; set; }
    
    /// <summary>
    /// Lista wymagań.
    /// </summary>
    public List<Requirement> Requirements { get; set; }
    
    /// <summary>
    /// Lista nagród.
    /// </summary>
    public List<Drop> Drops { get; set; }
    
    /// <summary>
    /// Lista suszów.
    /// </summary>
    public List<DriedFruit> DriedFruits { get; set; }
    
    /// <summary>
    /// Lista lamp.
    /// </summary>
    public List<Lamp> Lamps { get; set; }
    
    /// <summary>
    /// Lista nawozów.
    /// </summary>
    public List<Manure> Manures { get; set; }
    
    /// <summary>
    /// Lista donic.
    /// </summary>
    public List<Pot> Pots { get; set; }
    
    /// <summary>
    /// Lista nasion.
    /// </summary>
    public List<Seed> Seeds { get; set; }
    
    /// <summary>
    /// Lista gleb.
    /// </summary>
    public List<Soil> Soils { get; set; }
    
    /// <summary>
    /// Lista wód.
    /// </summary>
    public List<Water> Waters { get; set; }
    
    /// <summary>
    /// Lista bonusów.
    /// </summary>
    public List<Bonus> Bonuses { get; set; }
    
    /// <summary>
    /// Lista zadań.
    /// </summary>
    public List<Quest> Quests { get; set; }
    
    /// <summary>
    /// Lista postępów wymagań zadań.
    /// </summary>
    public List<QuestRequirementsProgress> QuestsRequirementsProgress { get; set; }
    
    /// <summary>
    /// Lista relacji nagród do zadań.
    /// </summary>
    public List<DropQuest> DropsQuests { get; set; }
}
