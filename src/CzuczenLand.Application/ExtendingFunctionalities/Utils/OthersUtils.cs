using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

/// <summary>
/// Klasa pomocnicza dla różnych narzędzi i funkcjonalności, które nie pasują do innych klas w aplikacji. 
/// </summary>
public static class OthersUtils
{
    /// <summary>
    /// Metoda służąca do tworzenia błędu.
    /// </summary>
    public static void CreateError()
    {
        var ret = new Dictionary<string, object>();
        var error = ret["field_name"];
    }
}