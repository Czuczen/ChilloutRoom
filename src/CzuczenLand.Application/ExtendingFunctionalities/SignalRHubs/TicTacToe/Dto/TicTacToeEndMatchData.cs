using Abp.AutoMapper;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.TicTacToe.Dto;

/// <summary>
/// Klasa reprezentująca dane końcowe rozgrywki w kółko i krzyżyk
/// </summary>
[AutoMapFrom(typeof(TicTacToeStorage))]
public class TicTacToeEndMatchData
{
    /// <summary>
    /// Liczba rozegranych gier
    /// </summary>
    public int GamesPlayed { get; set; }
        
    /// <summary>
    /// Liczba wygranych gier
    /// </summary>
    public int GamesWon { get; set; }
        
    /// <summary>
    /// Liczba przegranych gier
    /// </summary>
    public int GamesLost { get; set; }
        
    /// <summary>
    /// Liczba zremisowanych gier
    /// </summary>
    public int TiedGames { get; set; }
}