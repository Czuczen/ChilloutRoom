namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.TicTacToe.Dto;

public class TicTacToeClientData
{
    public bool WantToPlayWithOpponent { get; set; }
        
    public bool WaitForOpponent { get; set; }
        
    public string GroupName { get; set; }
        
    public string Symbol { get; set; }

    public string ConnectionId { get; set; }
        
    public long? OpponentUserId { get; set; }
        
    public string OpponentConnectionId { get; set; }
        
    public bool GameInProgress { get; set; }
}