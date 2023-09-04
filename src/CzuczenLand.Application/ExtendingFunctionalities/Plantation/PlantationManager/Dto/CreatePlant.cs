using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

/// <summary>
/// Reprezentuje proces tworzenia nowej rośliny.
/// </summary>
public class CreatePlant
{
    /// <summary>
    /// Komunikat wyświetlany w przypadku niespełnionych wymagań.
    /// </summary>
    public string RequirementsIsNotOkMessage { get; set; }
        
    /// <summary>
    /// Informacja o spełnieniu wymagań.
    /// </summary>
    public bool RequirementsIsOk { get; set; }
        
    /// <summary>
    /// Reprezentacja nowej rośliny.
    /// </summary>
    public Plant Plant { get; set; }
        
    /// <summary>
    /// Reprezentacja użytej lampy.
    /// </summary>
    public Lamp Lamp { get; set; }
        
    /// <summary>
    /// Reprezentacja użytego nawozu.
    /// </summary>
    public Manure Manure { get; set; }
        
    /// <summary>
    /// Reprezentacja użytej gleby.
    /// </summary>
    public Soil Soil { get; set; }
        
    /// <summary>
    /// Reprezentacja użytej wody.
    /// </summary>
    public Water Water { get; set; }
        
    /// <summary>
    /// Reprezentacja użytego nasiona.
    /// </summary>
    public Seed Seed { get; set; }
        
    /// <summary>
    /// Reprezentacja użytej doniczki.
    /// </summary>
    public Pot Pot { get; set; }
}