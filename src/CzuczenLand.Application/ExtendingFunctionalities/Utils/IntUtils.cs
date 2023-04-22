using System;
using System.Linq;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

public static class IntUtils
{
    private static readonly Random Random = new();

    
    public static int RandomInt(int length)
    {
        var currInt = Random.Next(length);

        return currInt;
    }
        
    public static int? ToNullableInt(this string s)
    {
        if (int.TryParse(s, out var i)) 
            return i;
        
        return null;
    }

    public static bool IsInt(object number, int? decimalPlaces = null)
    {
        bool isInt;
        var splinted = number.ToString().Split(',');

        if (splinted.Length == 1)
            isInt = true;
        else
        {
            var charsAfterComma = decimalPlaces != null ? splinted[1].Substring(0, (int) decimalPlaces) : splinted[1];  
            isInt = charsAfterComma.First().ToString() == "0" && charsAfterComma.Replace("0", "") == "";
        }

        return isInt;
    }
}
