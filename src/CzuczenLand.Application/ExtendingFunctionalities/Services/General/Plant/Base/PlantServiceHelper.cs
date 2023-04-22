using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;

public static class PlantServiceHelper
{
    public static decimal CalculateGrowingSpeed(Lamp lamp, Manure manure, Soil soil, Water water, Seed seed, Pot pot, decimal formBonuses)
    {
        var growingSpeed = lamp.IncreaseGrowingSpeed + manure.IncreaseGrowingSpeed + soil.IncreaseGrowingSpeed +
                           water.IncreaseGrowingSpeed + seed.IncreaseGrowingSpeed + pot.IncreaseGrowingSpeed + formBonuses;

        return growingSpeed;
    }

    public static int CalculateChanceForSeed(Lamp lamp, Manure manure, Soil soil, Water water, Seed seed, Pot pot, int formBonuses)
    {
        var chanceForDrop = lamp.IncreaseChanceForSeed + manure.IncreaseChanceForSeed + soil.IncreaseChanceForSeed +
                            water.IncreaseChanceForSeed + seed.IncreaseChanceForSeed + pot.IncreaseChanceForSeed + formBonuses;

        return chanceForDrop;
    }
        
    public static int CalculateTimeOfInsensitivity(Lamp lamp, Manure manure, Soil soil, Water water, Seed seed, Pot pot, int formBonuses)
    {
        var timeOfInsensitivity = lamp.IncreaseTimeOfInsensitivity + manure.IncreaseTimeOfInsensitivity + soil.IncreaseTimeOfInsensitivity +
                                  water.IncreaseTimeOfInsensitivity + seed.IncreaseTimeOfInsensitivity + pot.IncreaseTimeOfInsensitivity + formBonuses;

        return timeOfInsensitivity;
    }
        
    public static decimal CalculateDriedFruitAmount(Lamp lamp, Manure manure, Soil soil, Water water, Seed seed, Pot pot, decimal formBonuses)
    {
        var chanceForDrop = lamp.IncreaseDriedFruitAmount + manure.IncreaseDriedFruitAmount + soil.IncreaseDriedFruitAmount +
                            water.IncreaseDriedFruitAmount + seed.IncreaseDriedFruitAmount + pot.IncreaseDriedFruitAmount + formBonuses;

        return chanceForDrop;
    }
        
    public static int CalculateSeedAmount(Lamp lamp, Manure manure, Soil soil, Water water, Seed seed, Pot pot, int formBonuses)
    {
        var chanceForDrop = lamp.IncreaseSeedAmount + manure.IncreaseSeedAmount + soil.IncreaseSeedAmount +
                            water.IncreaseSeedAmount + seed.IncreaseSeedAmount + pot.IncreaseSeedAmount + formBonuses;

        return chanceForDrop;
    }
        
    public static decimal CalculateGainedExp(Lamp lamp, Manure manure, Soil soil, Water water, Seed seed, Pot pot, decimal formBonuses)
    {
        var chanceForDrop = lamp.IncreaseGainedExp + manure.IncreaseGainedExp + soil.IncreaseGainedExp +
                            water.IncreaseGainedExp + seed.IncreaseGainedExp + pot.IncreaseGainedExp + formBonuses;

        return chanceForDrop;
    }
        
    public static int CalculateSetsBuff(List<string> productsSetsNames, List<Bonus> activeBonuses, ExtendingModels.Models.General.District district)
    {
        var setsBaf = 1;
        var bonusSetsBuff = 1;
            
        var countedSetNames = new List<int>();
        foreach (var setName in productsSetsNames)
        {
            countedSetNames.Add(productsSetsNames.Where(currItem =>
                !string.IsNullOrWhiteSpace(currItem) &&
                string.Equals(currItem, setName, StringComparison.CurrentCultureIgnoreCase)).ToList().Count);
        }
            
        var oneSixElement = countedSetNames.Any(item => item == 6);
        var countOneSetElements = countedSetNames.Where(item => item >= 3 && item != 6).ToList().Count;
        var oneThreeElements = countOneSetElements >= 3 && countOneSetElements != 6;
        var twoThreeElements = countedSetNames.Where(item => item >= 3 && item != 6).ToList().Count == 6;

        if (oneSixElement)
            setsBaf = 4;

        if (oneThreeElements)
            setsBaf = 2;

        if (twoThreeElements)
            setsBaf = 3;

            
        var bonusesSetsNames = activeBonuses.Select(bonus => bonus.SetName).ToList();
        var countedBonusesSetsNames = new List<int>();
        foreach (var setName in bonusesSetsNames)
        {
            countedBonusesSetsNames.Add(bonusesSetsNames.Where(currItem =>
                !string.IsNullOrWhiteSpace(currItem) &&
                string.Equals(currItem, setName, StringComparison.CurrentCultureIgnoreCase)).ToList().Count);
        }

        var maxBonusesCount = district.MaxBuffsSlots + district.MaxArtifactSlots;
        var halfOfMaxBonusesCount = maxBonusesCount / 2;
            
        var oneMaxElementFromBonus = countedBonusesSetsNames.Any(item => item == maxBonusesCount);
        var countOneSetElementsFromBonus = countedBonusesSetsNames
            .Where(item => item >= halfOfMaxBonusesCount && item != maxBonusesCount).ToList().Count;
        var oneHalfElementsFromBonus = countOneSetElementsFromBonus >= halfOfMaxBonusesCount &&
                                       countOneSetElements != maxBonusesCount;
        var twoHalfElementsFromBonus =
            countedBonusesSetsNames.Where(item => item >= halfOfMaxBonusesCount && item != maxBonusesCount).ToList()
                .Count == maxBonusesCount;
            
        if (oneMaxElementFromBonus)
            bonusSetsBuff = 4;

        if (oneHalfElementsFromBonus)
            bonusSetsBuff = 2;

        if (twoHalfElementsFromBonus)
            bonusSetsBuff = 3;

            
        if (bonusSetsBuff > 1)
            setsBaf += bonusSetsBuff - 1;
            
        return setsBaf;
    }
    
    public static int CalculateTimeRemaining(decimal growingLevel, decimal growingSpeed, decimal growingSpeedDivider)
    {
        var remainingDistance = 100 - growingLevel;
        return (int) Math.Ceiling(remainingDistance / (growingSpeed / growingSpeedDivider));
    }

    public static void ProcessPlantGrowing(ExtendingModels.Models.General.Plant plant, decimal wiltSpeed, decimal growingSpeed)
    {
        if (plant.GrowingLevel >= 100)
        {
            if (plant.TimeOfInsensitivity == 0)
            {
                var wouldBeMore = plant.WiltLevel + wiltSpeed;
                if (wouldBeMore > 100)
                {
                    plant.WiltLevel = 100;
                }
                else
                {
                    plant.WiltLevel += wiltSpeed;
                }
            }
            else
            {
                plant.TimeOfInsensitivity--;
            }
        }
        else
        {
            var wouldBeMore = plant.GrowingLevel + (double) growingSpeed;
            if (wouldBeMore > 100)
            {
                plant.GrowingLevel = 100;
            }
            else
            {
                plant.GrowingLevel = wouldBeMore;
            }
        }
    }
    
    public static decimal CountSoilPenalty(Pot pot)
    {
        var soilPenaltyCount = decimal.Parse(((decimal) 20 / 100 * pot.Capacity).ToString("0.##").Replace(",", "."),
            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

        return soilPenaltyCount;
    }

    public static void CalculateAndSetWiltPenalty(ExtendingModels.Models.General.Plant plant)
    {
        if (plant.WiltLevel > 0)
        {
            plant.DriedFruitAmount -= decimal.Parse((plant.WiltLevel / 100 * plant.DriedFruitAmount)
                .ToString("0.##").Replace(",", "."),
                NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            plant.SeedAmount -= Convert.ToInt32(plant.WiltLevel) / 100 * plant.SeedAmount;

            plant.ChanceForSeed -= int.Parse((plant.WiltLevel / 100 * plant.ChanceForSeed).ToString("0"),
                NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            plant.GainedExp -= decimal.Parse((plant.WiltLevel / 100 * plant.GainedExp)
                .ToString("0.##").Replace(",", "."),
                NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }
    }

    public static async Task SetCreationCosts(CreatePlant createPlant, IIgnoreChangeService ignoreChangeService)
    {
        var lamp = createPlant.Lamp;
        var manure = createPlant.Manure;
        var seed = createPlant.Seed;
        var soil = createPlant.Soil;
        var pot = createPlant.Pot;
        var water = createPlant.Water;

        lamp.OwnedAmount--;
        lamp.InUseCount++;
        await ignoreChangeService.Add(lamp);
            
        manure.OwnedAmount -= seed.ManureConsumption;
        await ignoreChangeService.Add(manure);
            
        soil.OwnedAmount -= pot.Capacity;
        await ignoreChangeService.Add(soil);
            
        water.OwnedAmount -= seed.WaterConsumption;
        await ignoreChangeService.Add(water);

        seed.OwnedAmount--;
        await ignoreChangeService.Add(seed);
            
        pot.InUseCount++;
        pot.OwnedAmount--;
        await ignoreChangeService.Add(pot);
    }
}