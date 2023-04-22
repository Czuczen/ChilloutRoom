namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;

public class UpdateProducts
{
    public int Id { get; set; }
        
    public string OwnedAmountWithMeasureUnit { get; set; }
        
    public decimal OwnedAmount { get; set; }
        
    public string EntityName { get; set; }
        
    public string RecordName { get; set; }
}