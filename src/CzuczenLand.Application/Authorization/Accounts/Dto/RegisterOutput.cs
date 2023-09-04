namespace CzuczenLand.Authorization.Accounts.Dto;

/// <summary>
/// Klasa DTO reprezentująca wynik rejestracji użytkownika.
/// </summary>
public class RegisterOutput
{
    /// <summary>
    /// Określa, czy użytkownik może zalogować się po rejestracji.
    /// </summary>
    public bool CanLogin { get; set; }
}