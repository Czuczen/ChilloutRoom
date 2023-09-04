using System;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

/// <summary>
/// Klasa pomocnicza dla typu - decimal 
/// </summary>
public static class DecimalUtils
{
    /// <summary>
    /// Statyczne pole zawierające instancję klasy Random.
    /// </summary>
    private static readonly Random Random = new();


    /// <summary>
    /// Generuje losową liczbę typu Int.
    /// </summary>
    /// <returns>Losowa liczba typu Int.</returns>
    private static int NextInt32()
    {
        var firstBits = Random.Next(0, 1 << 4) << 28;
        var lastBits = Random.Next(0, 1 << 28);
        return firstBits | lastBits;
    }

    /// <summary>
    /// Generuje losową liczbę typu decimal.
    /// </summary>
    /// <returns>Losowa liczba typu decimal.</returns>
    public static decimal NextDecimal()
    {
        var scale = (byte) Random.Next(29);
        var sign = Random.Next(2) == 1;
        return new decimal(NextInt32(), 
            NextInt32(),
            NextInt32(),
            sign,
            scale);
    }

    /// <summary>
    /// Generuje losową liczbę typu decimal z użyciem próbkowania Monte Carlo.
    /// </summary>
    /// <returns>Losowa liczba typu decimal.</returns>
    public static decimal NextDecimalSample()
    {
        var sample = 1m;
        while (sample >= 1)
        {
            var a = NextInt32();
            var b = NextInt32();
            var c = Random.Next(542101087);
            sample = new Decimal(a, b, c, false, 28);
        }
            
        return sample;
    }
    
    /// <summary>
    /// Generuje losową liczbę typu decimal z zakresu określonego przez podane wartości minimalne i maksymalne.
    /// </summary>
    /// <param name="minValue">Minimalna wartość, jaką może przyjąć losowana liczba.</param>
    /// <param name="maxValue">Maksymalna wartość, jaką może przyjąć losowana liczba.</param>
    /// <returns>Losowa liczba typu decimal z zakresu określonego przez podane wartości minimalne i maksymalne.</returns>
    public static decimal NextDecimal(decimal minValue, decimal maxValue)
    {
        var nextDecimalSample = NextDecimalSample();
        return maxValue * nextDecimalSample + minValue * (1 - nextDecimalSample);
    }
}