namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca zestaw sprawdzanych wartości.
/// </summary>
public class CheckValues
{
    /// <summary>
    /// Różnica między starą wartością aktualizowanej encji a nową dla typu int.
    /// </summary>
    public int? Int { get; set; }
        
    /// <summary>
    /// Różnica między starą wartością aktualizowanej encji a nową dla typu decimal.
    /// </summary>
    public decimal? Decimal { get; set; }
}