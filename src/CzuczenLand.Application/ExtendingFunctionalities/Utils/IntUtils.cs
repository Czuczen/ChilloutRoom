using System;
using System.Linq;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

/// <summary>
/// Klasa pomocnicza dla typu - int
/// </summary>
public static class IntUtils
{
    /// <summary>
    /// Statyczne pole zawierające instancję klasy Random.
    /// </summary>
    private static readonly Random Random = new();

    
    /// <summary>
    /// Metoda generuje losową liczbę całkowitą z przedziału od 0 do podanej wartości.
    /// </summary>
    /// <param name="length">Maksymalna wartość losowanej liczby całkowitej.</param>
    /// <returns>Wygenerowana losowa liczba całkowita.</returns>
    public static int RandomInt(int length)
    {
        var currInt = Random.Next(length);

        return currInt;
    }
        
    /// <summary>
    /// Metoda służy do zamiany wartości typu string na nullable int.
    /// </summary>
    /// <param name="s">Wartość string, która ma być skonwertowana na nullable int.</param>
    /// <returns>Zwraca wartość nullable int, jeśli konwersja jest możliwa. W przeciwnym wypadku zwraca null.</returns>
    public static int? ToNullableInt(this string s)
    {
        if (int.TryParse(s, out var i)) 
            return i;
        
        return null;
    }

    /// <summary>
    /// Metoda sprawdza, czy wartość podana jako obiekt jest liczbą całkowitą.
    /// </summary>
    /// <param name="number">Obiekt do sprawdzenia.</param>
    /// <param name="decimalPlaces">Liczba miejsc po przecinku. Domyślnie ustawiona na null.</param>
    /// <returns>Zwraca true, jeśli wartość jest liczbą całkowitą, w przeciwnym wypadku false.</returns>
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
