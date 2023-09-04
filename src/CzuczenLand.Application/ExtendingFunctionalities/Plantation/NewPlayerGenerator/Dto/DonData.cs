namespace CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;

/// <summary>
/// Reprezentuje dane dotyczące dona dzielnicy.
/// </summary>
public class DonData
{
    /// <summary>
    /// Określa, czy dana dzielnica ma dona.
    /// </summary>
    public bool WeHaveDon { get; set; }
        
    /// <summary>
    /// Nazwa dona/gracza.
    /// </summary>
    public string DonName { get; set; }
        
    /// <summary>
    /// Procent haraczu dona za wystawianie na czarnym rynku.
    /// </summary>
    public decimal DonCharityPercentage { get; set; }
        
    /// <summary>
    /// Identyfikator dona/gracza.
    /// </summary>
    public long DonId { get; set; }
        
    /// <summary>
    /// Identyfikator dzielnicy na której gracz jest donem.
    /// </summary>
    public int DistrictId { get; set; }
}