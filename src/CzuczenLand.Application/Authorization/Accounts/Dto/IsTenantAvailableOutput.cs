namespace CzuczenLand.Authorization.Accounts.Dto;

/// <summary>
/// Klasa DTO reprezentująca wynik sprawdzania dostępności dzierżawy.
/// </summary>
public class IsTenantAvailableOutput
{
    /// <summary>
    /// Stan dostępności dzierżawy.
    /// </summary>
    public TenantAvailabilityState State { get; set; }

    /// <summary>
    /// Identyfikator dzierżawy.
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// Konstruktor domyślny klasy IsTenantAvailableOutput.
    /// </summary>
    public IsTenantAvailableOutput()
    {

    }

    /// <summary>
    /// Konstruktor klasy IsTenantAvailableOutput ustawiający jej właściwości.
    /// </summary>
    /// <param name="state">Stan dostępności dzierżawy.</param>
    /// <param name="tenantId">Identyfikator dzierżawy (opcjonalny).</param>
    public IsTenantAvailableOutput(TenantAvailabilityState state, int? tenantId = null)
    {
        State = state;
        TenantId = tenantId;
    }
}