namespace CzuczenLand.Sessions.Dto;

/// <summary>
/// Klasa DTO przechowująca informacje o bieżącej sesji logowania.
/// </summary>
public class GetCurrentLoginInformationsOutput
{
    /// <summary>
    /// Informacje o użytkowniku.
    /// </summary>
    public UserLoginInfoDto User { get; set; }

    /// <summary>
    /// Informacje o dzierżawcy.
    /// </summary>
    public TenantLoginInfoDto Tenant { get; set; }
}