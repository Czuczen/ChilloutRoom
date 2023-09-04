using System;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket;

/// <summary>
/// Klasa pomocnicza dla pracownika czarnego rynku.
/// </summary>
public static class BlackMarketWorkerHelper
{
    /// <summary>
    /// Generator liczb losowych.
    /// </summary>
    private static readonly Random Random = new();


    /// <summary>
    /// Oblicza ilość wystawianego produktu na czarnym rynku.
    /// </summary>
    /// <param name="entityName">Nazwa encji produktu.</param>
    /// <param name="district">Dzielnica dla której wystawiana jest transakcja.</param>
    /// <returns>Ilość produktu do wystawienia.</returns>
    public static decimal CalculateQuantity(string entityName, District district)
    {
        switch (entityName)
        {
            case EntitiesDbNames.DriedFruit:
                return Random.Next(1, district.BlackMarketIssueQuantityForDriedFruit);
            case EntitiesDbNames.Lamp:
                return Random.Next(1, district.BlackMarketIssueQuantityForLamp);
            case EntitiesDbNames.Manure:
                return Random.Next(1, district.BlackMarketIssueQuantityForManure);
            case EntitiesDbNames.Pot:
                return Random.Next(1, district.BlackMarketIssueQuantityForPot);
            case EntitiesDbNames.Seed:
                return Random.Next(1, district.BlackMarketIssueQuantityForSeed);
            case EntitiesDbNames.Soil:
                return Random.Next(1, district.BlackMarketIssueQuantityForSoil);
            case EntitiesDbNames.Water:
                return Random.Next(1, district.BlackMarketIssueQuantityForWater);
            case EntitiesDbNames.Bonus:
                return Random.Next(1, district.BlackMarketIssueQuantityForBonus);
            default:
                throw new ArgumentOutOfRangeException(entityName, district, null);
        }
    }
}