using Abp.AutoMapper;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.TicTacToe.Dto;

[AutoMapFrom(typeof(TicTacToeStorage))]
public class TicTacToeStartMatchData
{
    public string PlayerName { get; set; }

    public int GamesPlayed { get; set; }
        
    public int GamesWon { get; set; }
        
    public int GamesLost { get; set; }
        
    public int TiedGames { get; set; }
        
    public string Symbol { get; set; }
}