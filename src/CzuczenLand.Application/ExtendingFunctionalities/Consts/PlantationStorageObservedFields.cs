using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Consts;

/// <summary>
/// Klasa przechowująca nazwy obserwowanych pól w magazynie plantacji.
/// </summary>
public static class PlantationStorageObservedFields
{
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z poziomem plantacji.
    /// </summary>
    public const string Level = "Level";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z uzyskanym doświadczeniem na plantacji.
    /// </summary>
    public const string GainedExperience = "GainedExperience";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z aktualnym doświadczeniem na plantacji.
    /// </summary>
    public const string CurrExp = "CurrExp";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z wymaganym doświadczeniem do następnego poziomu.
    /// </summary>
    public const string ExpToNextLevel = "ExpToNextLevel";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z liczbą rozpoczętych dziennych zadań na plantacji.
    /// </summary>
    public const string StartedDailyQuestsCount = "StartedDailyQuestsCount";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z liczbą rozpoczętych tygodniowych zadań na plantacji.
    /// </summary>
    public const string StartedWeeklyQuestsCount = "StartedWeeklyQuestsCount";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z maksymalną liczbą dziennych zadań na plantacji.
    /// </summary>
    public const string MaxDailyQuestsCount = "MaxDailyQuestsCount";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z maksymalną liczbą tygodniowych zadań na plantacji.
    /// </summary>
    public const string MaxWeeklyQuestsCount = "MaxWeeklyQuestsCount";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z ilością złota w magazynie plantacji.
    /// </summary>
    public const string Gold = "Gold";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z żetonem zadania.
    /// </summary>
    public const string QuestToken = "QuestToken";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z żetonem dealera.
    /// </summary>
    public const string DealerToken = "DealerToken";

    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z żetonem czarnego rynku.
    /// </summary>
    public const string BlackMarketToken = "BlackMarketToken";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z prestiżem plantacji.
    /// </summary>
    public const string Prestige = "Prestige";
        
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z odblokowaną liczbą dziennych zadań na plantacji.
    /// </summary>
    public const string UnlockedDailyQuestsCount = "UnlockedDailyQuestsCount";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z odblokowaną liczbą tygodniowych zadań na plantacji.
    /// </summary>
    public const string UnlockedWeeklyQuestsCount = "UnlockedWeeklyQuestsCount";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z odblokowaną liczbą slotów na artefakty.
    /// </summary>
    public const string UnlockedArtifactSlots = "UnlockedArtifactSlots";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z odblokowaną liczbą slotów na wzmocnienia.
    /// </summary>
    public const string UnlockedBuffsSlots = "UnlockedBuffsSlots";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z wykorzystywaną liczbą slotów na artefakty.
    /// </summary>
    public const string ArtifactSlotsInUse = "ArtifactSlotsInUse";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z wykorzystywaną liczbą slotów na wzmocnienia.
    /// </summary>
    public const string BuffSlotsInUse = "BuffSlotsInUse";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z maksymalną liczbą slotów na artefakty.
    /// </summary>
    public const string MaxArtifactSlots = "MaxArtifactSlots";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z maksymalną liczbą slotów na wzmocnienia.
    /// </summary>
    public const string MaxBuffsSlots = "MaxBuffsSlots";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z żetonem dona.
    /// </summary>
    public const string DonToken = "DonToken";
    
    /// <summary>
    /// Pole reprezentujące nazwę pola związanego z żetonem odblokowania.
    /// </summary>
    public const string UnlockToken = "UnlockToken";

    /// <summary>
    /// Lista przechowująca nazwy pól, które reprezentują waluty w magazynie plantacji.
    /// </summary>
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