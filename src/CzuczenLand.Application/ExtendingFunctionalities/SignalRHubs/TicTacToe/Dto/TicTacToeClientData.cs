namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.TicTacToe.Dto;

/// <summary>
/// Klasa reprezentująca dane klienta gry w kółko i krzyżyk.
/// </summary>
public class TicTacToeClientData
{
    /// <summary>
    /// Określa, czy klient chce zagrać z przeciwnikiem.
    /// </summary>
    public bool WantToPlayWithOpponent { get; set; }
        
    /// <summary>
    /// Określa, czy klient oczekuje na przeciwnika.
    /// </summary>
    public bool WaitForOpponent { get; set; }
        
    /// <summary>
    /// Nazwa grupy, do której należy klient.
    /// </summary>
    public string GroupName { get; set; }
        
    /// <summary>
    /// Symbol, którym gra klient.
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Identyfikator połączenia klienta.
    /// </summary>
    public string ConnectionId { get; set; }
        
    /// <summary>
    /// Identyfikator użytkownika przeciwnika.
    /// </summary>
    public long? OpponentUserId { get; set; }
        
    /// <summary>
    /// Identyfikator połączenia przeciwnika.
    /// </summary>
    public string OpponentConnectionId { get; set; }
        
    /// <summary>
    /// Określa, czy gra jest w toku.
    /// </summary>
    public bool GameInProgress { get; set; }
}