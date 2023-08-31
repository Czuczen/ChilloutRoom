using System;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Db;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Edit;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;

/// <summary>
/// Klasa fabryki dostarczającej obiekty parsera zgodnie z określoną strategią parsowania.
/// </summary>
public static class ParserFactory
{
    /// <summary>
    /// Tworzy i zwraca obiekt parsera zgodnie z wybraną strategią parsowania.
    /// </summary>
    /// <param name="parseStrategy">Strategia parsowania.</param>
    /// <returns>Obiekt parsera.</returns>
    public static IParser GetParser(EnumUtils.ParseStrategies parseStrategy)
    {
        switch (parseStrategy)
        {
            case EnumUtils.ParseStrategies.Db:
                return new Parser(new DbStrategy());
            case EnumUtils.ParseStrategies.Display:
                return new Parser(new DisplayStrategy());
            case EnumUtils.ParseStrategies.Edit:
                return new Parser(new EditStrategy());
            default:
                throw new ArgumentOutOfRangeException(nameof(parseStrategy), parseStrategy, null);
        }
    }
}