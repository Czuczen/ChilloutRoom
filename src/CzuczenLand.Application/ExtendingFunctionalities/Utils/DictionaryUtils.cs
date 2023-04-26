using System;
using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

/// <summary>
/// Klasa pomocnicza dla typu - dictionary
/// </summary>
public static class DictionaryUtils
{
    /// <summary>
    /// Sprawdza, czy dany klucz w słowniku istnieje oraz czy jego wartość nie jest null, DBNull ani pustym ciągiem znaków.
    /// </summary>
    /// <param name="dict">Słownik, w którym ma być przeszukanie.</param>
    /// <param name="key">Klucz, który ma być sprawdzony.</param>
    /// <returns>True, jeśli klucz istnieje i jego wartość nie jest null, DBNull ani pustym ciągiem znaków, w przeciwnym razie false.</returns>
    public static bool DictKeyExistAndHasValue(Dictionary<string, object> dict, string key)
    {
        var ret = dict != null && dict.ContainsKey(key) && dict[key] != null && DBNull.Value != dict[key] && string.Empty != dict[key].ToString();

        return ret;
    }
}