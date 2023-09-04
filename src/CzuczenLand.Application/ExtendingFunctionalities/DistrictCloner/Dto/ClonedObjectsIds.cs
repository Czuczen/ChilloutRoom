using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

/// <summary>
/// Klasa przechowująca listy identyfikatorów sklonowanych obiektów.
/// </summary>
public class ClonedObjectsIds
{
    /// <summary>
    /// Lista identyfikatorów dzielnic.
    /// </summary>
    public List<int> DistrictIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów typów generowanych.
    /// </summary>
    public List<int> GeneratedTypeIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów nagród.
    /// </summary>
    public List<int> DropIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów wymagań.
    /// </summary>
    public List<int> RequirementIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów suszów.
    /// </summary>
    public List<int> DriedFruitIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów lamp.
    /// </summary>
    public List<int> LampIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów nawozów.
    /// </summary>
    public List<int> ManureIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów donic.
    /// </summary>
    public List<int> PotIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów nasion.
    /// </summary>
    public List<int> SeedIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów gleb.
    /// </summary>
    public List<int> SoilIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów wód.
    /// </summary>
    public List<int> WaterIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów bonusów.
    /// </summary>
    public List<int> BonusIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów zadań.
    /// </summary>
    public List<int> QuestIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów postępu wymagań zadań.
    /// </summary>
    public List<int> QuestRequirementsProgressIds { get; set; }
    
    /// <summary>
    /// Lista identyfikatorów relacji nagród do zadań.
    /// </summary>
    public List<int> DropQuestIds { get; set; }
}