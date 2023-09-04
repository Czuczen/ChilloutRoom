namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca aktualizację informacji produktów.
/// </summary>
public class UpdateProducts
{
    /// <summary>
    /// Identyfikator produktu.
    /// </summary>
    public int Id { get; set; }
        
    /// <summary>
    /// Posiadana ilość produktu wraz z jednostką miary.
    /// </summary>
    public string OwnedAmountWithMeasureUnit { get; set; }
        
    /// <summary>
    /// Posiadana ilość produktu.
    /// </summary>
    public decimal OwnedAmount { get; set; }
        
    /// <summary>
    /// Nazwa encji produktu.
    /// </summary>
    public string EntityName { get; set; }
        
    /// <summary>
    /// Nazwa rekordu produktu.
    /// </summary>
    public string RecordName { get; set; }
}