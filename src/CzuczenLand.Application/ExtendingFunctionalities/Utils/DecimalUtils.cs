using System;

namespace CzuczenLand.ExtendingFunctionalities.Utils;

public static class DecimalUtils
{
    private static readonly Random Random = new();

    
    private static int NextInt32()
    {
        var firstBits = Random.Next(0, 1 << 4) << 28;
        var lastBits = Random.Next(0, 1 << 28);
        return firstBits | lastBits;
    }

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
    
    public static decimal NextDecimal(decimal minValue, decimal maxValue)
    {
        var nextDecimalSample = NextDecimalSample();
        return maxValue * nextDecimalSample + minValue * (1 - nextDecimalSample);
    }
}