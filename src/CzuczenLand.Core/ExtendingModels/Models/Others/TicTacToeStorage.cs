using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingModels.Models.Others;

public class TicTacToeStorage : Entity<int>
{
    public string PlayerName { get; set; }

    public int GamesPlayed { get; set; }
        
    public int GamesWon { get; set; }
        
    public int GamesLost { get; set; }
        
    public int TiedGames { get; set; }
    
    public long UserId { get; set; }
}
