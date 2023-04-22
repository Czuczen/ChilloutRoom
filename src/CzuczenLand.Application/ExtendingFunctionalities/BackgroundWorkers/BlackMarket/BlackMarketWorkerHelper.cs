using System;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.BlackMarket;

public static class BlackMarketWorkerHelper
{
    private static readonly Random Random = new();


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