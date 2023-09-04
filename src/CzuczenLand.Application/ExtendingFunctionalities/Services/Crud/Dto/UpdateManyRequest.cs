using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

/// <summary>
/// Klasa reprezentująca żądanie aktualizacji wielu rekordów.
/// </summary>
public class UpdateManyRequest
{
    /// <summary>
    /// Obiekt zawierający pola do zaktualizowania.
    /// </summary>
    public object FieldsToUpdate { get; set; }
        
    /// <summary>
    /// Lista identyfikatorów rekordów do zaktualizowania.
    /// </summary>
    public List<int> Ids { get; set; }
}