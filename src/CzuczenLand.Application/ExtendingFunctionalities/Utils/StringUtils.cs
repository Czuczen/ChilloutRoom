using System;
using System.Linq;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

/// <summary>
/// Klasa pomocnicza dla typu - string
/// </summary>
public static class StringUtils
{
    /// <summary>
    /// Statyczne pole zawierające instancję klasy Random.
    /// </summary>
    private static readonly Random Random = new();
        
    
    /// <summary>
    /// Metoda generująca losowy ciąg znaków o żądanej długości.
    /// </summary>
    /// <param name="length">Długość generowanego ciągu znaków.</param>
    /// <returns>Losowy ciąg znaków o żądanej długości.</returns>
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
        
    /// <summary>
    /// Metoda zwracająca fragment tekstu pomiędzy zadanymi znakami początkowymi i końcowymi.
    /// </summary>
    /// <param name="str">Tekst, w którym szukany jest fragment.</param>
    /// <param name="startChar">Znak początkowy fragmentu.</param>
    /// <param name="endChar">Znak końcowy fragmentu.</param>
    /// <returns>Fragment tekstu pomiędzy zadanymi znakami początkowymi i końcowymi.</returns>
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
        
    /// <summary>
    /// Metoda usuwająca określone znaki z podanego łańcucha znaków.
    /// </summary>
    /// <param name="strToChange">Łańcuch znaków, z którego mają być usunięte znaki.</param>
    /// <param name="chars">Tablica znaków do usunięcia.</param>
    /// <returns>Zwraca łańcuch znaków bez określonych znaków.</returns>
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

    /// <summary>
    /// Metoda konwertująca pierwszą literę w ciągu znaków na małą literę.
    /// </summary>
    /// <param name="str">Wejściowy ciąg znaków.</param>
    /// <returns>Ciąg znaków z pierwszą literą zamienioną na małą literę.</returns>
    public static string FirstCharToLowerCase(string str) => char.ToLowerInvariant(str[0]) + str.Substring(1);
}