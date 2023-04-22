using System;
using System.Linq;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

public static class StringUtils
{
    private static readonly Random Random = new();
        
    
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
        
    public static string CutString(string str, string startChar, string endChar)
    {
        var ret = "";
   
        if (!string.IsNullOrWhiteSpace(str))
        {
            var positionFrom = str.IndexOf(startChar, StringComparison.Ordinal) + startChar.Length;
            var positionTo = str.LastIndexOf(endChar, StringComparison.Ordinal);

            if (positionTo != -1)
            {
                ret = str.Substring(positionFrom, positionTo - positionFrom);
            }
        }

        return ret;
    }
        
    public static string RemoveCharacters(string strToChange, char[] chars)
    {
        var ret = "";
        if (!string.IsNullOrWhiteSpace(strToChange) && chars != null && chars.Length > 0)
        {
            foreach (var character in chars)
            {
                strToChange = strToChange.Replace(character.ToString(), string.Empty);
            }

            ret = strToChange;
        }
            
        return ret;
    }

    public static string FirstCharToLowerCase(string str) => char.ToLowerInvariant(str[0]) + str.Substring(1);
}