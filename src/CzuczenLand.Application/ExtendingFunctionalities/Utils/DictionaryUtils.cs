using System;
using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

public static class DictionaryUtils
{
    public static bool DictKeyExistAndHasValue(Dictionary<string, object> dict, string key)
    {
        var ret = dict != null && dict.ContainsKey(key) && dict[key] != null && DBNull.Value != dict[key] && string.Empty != dict[key].ToString();

        return ret;
    }
}