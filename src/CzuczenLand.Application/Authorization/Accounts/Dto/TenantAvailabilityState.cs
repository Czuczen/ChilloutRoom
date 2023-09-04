namespace CzuczenLand.Authorization.Accounts.Dto;

/// <summary>
/// Stan dostępności dzierżawy.
/// </summary>
public enum TenantAvailabilityState
{
    /// <summary>
    /// Dzierżawa jest dostępna.
    /// </summary>
    Available = 1,
    
    /// <summary>
    /// Dzierżawa jest nieaktywna.
    /// </summary>
    InActive,
    
    /// <summary>
    /// Dzierżawa nie została znaleziona.
    /// </summary>
    NotFound
}