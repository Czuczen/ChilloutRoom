namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Quests.TimeControl.Dto;

public class SetDurationResponse
{
    public int QuestId { get; set; }
        
    public string RemainingTime { get; set; }

    public bool TimeUp { get; set; }
}