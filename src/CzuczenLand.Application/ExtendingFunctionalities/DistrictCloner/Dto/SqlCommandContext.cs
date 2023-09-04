namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

/// <summary>
/// Klasa reprezentująca kontekst polecenia SqlCommand.
/// </summary>
public class SqlCommandContext
{
    /// <summary>
    /// Zapytanie SQL.
    /// </summary>
    public string Query { get; set; }
        
    /// <summary>
    /// Parametry SQL.
    /// </summary>
    public object[] SqlParameters { get; set; } 
}