using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Consts;

public static class PlantationStorageObservedFields
{
    public const string Level = "Level";
    public const string GainedExperience = "GainedExperience";
    public const string CurrExp = "CurrExp";
    public const string ExpToNextLevel = "ExpToNextLevel";
    public const string StartedDailyQuestsCount = "StartedDailyQuestsCount";
    public const string StartedWeeklyQuestsCount = "StartedWeeklyQuestsCount";
    public const string MaxDailyQuestsCount = "MaxDailyQuestsCount";
    public const string MaxWeeklyQuestsCount = "MaxWeeklyQuestsCount";
    public const string Gold = "Gold";
    public const string QuestToken = "QuestToken";
    public const string DealerToken = "DealerToken";
    public const string BlackMarketToken = "BlackMarketToken";
    public const string Prestige = "Prestige";
        
    public const string UnlockedDailyQuestsCount = "UnlockedDailyQuestsCount";
    public const string UnlockedWeeklyQuestsCount = "UnlockedWeeklyQuestsCount";
    public const string UnlockedArtifactSlots = "UnlockedArtifactSlots";
    public const string UnlockedBuffsSlots = "UnlockedBuffsSlots";
    public const string ArtifactSlotsInUse = "ArtifactSlotsInUse";
    public const string BuffSlotsInUse = "BuffSlotsInUse";
    public const string MaxArtifactSlots = "MaxArtifactSlots";
    public const string MaxBuffsSlots = "MaxBuffsSlots";
    public const string DonToken = "DonToken";
    public const string UnlockToken = "UnlockToken";

    public static readonly List<string> CurrencyExchanges = new()
    {
        Gold,
        Prestige,
        DealerToken,
        QuestToken,
        BlackMarketToken,
        DonToken,
        UnlockToken
    };
}