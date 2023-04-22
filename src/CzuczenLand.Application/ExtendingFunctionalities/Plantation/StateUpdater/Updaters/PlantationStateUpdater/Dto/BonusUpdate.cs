namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;

public class BonusUpdate
{
    public bool IsArtifact { get; set; }
        
    public int BonusId { get; set; }

    public int RemainingActiveTime { get; set; }
        
    public bool IsActive { get; set; }
}