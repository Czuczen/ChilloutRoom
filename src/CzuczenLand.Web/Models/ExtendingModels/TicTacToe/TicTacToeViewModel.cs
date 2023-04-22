using Abp.AutoMapper;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.Web.Models.ExtendingModels.TicTacToe;

[AutoMapFrom(typeof(TicTacToeStorage))]
public class TicTacToeViewModel
{
    public int GamesPlayed { get; set; }
        
    public int GamesWon { get; set; }
        
    public int GamesLost { get; set; }
        
    public int TiedGames { get; set; }
}