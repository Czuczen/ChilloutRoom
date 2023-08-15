namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

/// <summary>
/// Reprezentuje dane wysłane od klienta dotyczące utworzenia rośliny.
/// </summary>
public class PlantData
{
    /// <summary>
    /// Identyfikator lampy.
    /// </summary>
    public int Lamp { get; set; }
    
    /// <summary>
    /// Identyfikator nawozu.
    /// </summary>
    public int Manure { get; set; }
    
    /// <summary>
    /// Identyfikator gleby.
    /// </summary>
    public int Soil { get; set; }
    
    /// <summary>
    /// Identyfikator wody.
    /// </summary>
    public int Water { get; set; }
    
    /// <summary>
    /// Identyfikator nasiona.
    /// </summary>
    public int Seed { get; set; }
    
    /// <summary>
    /// Identyfikator doniczki.
    /// </summary>
    public int Pot { get; set; }
}