namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Chat.Dto;

/// <summary>
/// Klasa przechowująca dane wiadomości czatu.
/// </summary>
public class ChatMessageData
{
    /// <summary>
    /// Nazwa użytkownika, który wysłał wiadomość.
    /// </summary>
    public string UserName { get; set; }
        
    /// <summary>
    /// Treść wiadomości.
    /// </summary>
    public string Message { get; set; }
}