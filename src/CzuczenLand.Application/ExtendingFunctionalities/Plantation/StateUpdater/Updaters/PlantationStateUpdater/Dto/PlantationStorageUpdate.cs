namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca aktualizację informacji magazynu plantacji.
/// </summary>
public class PlantationStorageUpdate
{
    /// <summary>
    /// Poziom magazynu plantacji.
    /// </summary>
    public string Level { get; set; }
        
    /// <summary>
    /// Uzyskane doświadczenie.
    /// </summary>
    public string GainedExperience { get; set; }
        
    /// <summary>
    /// Ilość złota.
    /// </summary>
    public string Gold { get; set; }
        
    /// <summary>
    /// Przetworzone obecne doświadczenie.
    /// </summary>
    public string ParsedCurrExp { get; set; }
        
    /// <summary>
    /// Przetworzone doświadczenie do następnego poziomu.
    /// </summary>
    public string ParsedExpToNextLevel { get; set; }
        
    /// <summary>
    /// Punkty prestiżu.
    /// </summary>
    public string Prestige { get; set; }
        
    /// <summary>
    /// Ilość zadań dziennych w toku.
    /// </summary>
    public string DailyQuestsInProgressCount { get; set; }
        
    /// <summary>
    /// Ilość zadań tygodniowych w toku.
    /// </summary>
    public string WeeklyQuestsInProgressCount { get; set; }
        
    /// <summary>
    /// Ilość otrzymanych poziomów.
    /// </summary>
    public int ReceivedLevels { get; set; }
        
    /// <summary>
    /// Doświadczenie do następnego poziomu.
    /// </summary>
    public decimal ExpToNextLevel { get; set; }
        
    /// <summary>
    /// Obecne doświadczenie.
    /// </summary>
    public decimal CurrExp { get; set; }
        
    /// <summary>
    /// Tokeny odblokowania.
    /// </summary>
    public string UnlockToken { get; set; }
        
    /// <summary>
    /// Tokeny dona.
    /// </summary>
    public string DonToken { get; set; }
        
    /// <summary>
    /// Tokeny zadania.
    /// </summary>
    public string QuestToken { get; set; }
        
    /// <summary>
    /// Tokeny dealera.
    /// </summary>
    public string DealerToken { get; set; }
        
    /// <summary>
    /// Tokeny czarnego rynku.
    /// </summary>
    public string BlackMarketToken { get; set; }
        
    /// <summary>
    /// Identyfikator magazynu plantacji.
    /// </summary>
    public int PlantationStorageId { get; set; }
        
    /// <summary>
    /// Maksymalna ilość zadań tygodniowych.
    /// </summary>
    public string MaxWeeklyQuestsCount { get; set; }
    
    /// <summary>
    /// Maksymalna ilość zadań dziennych.
    /// </summary>
    public string MaxDailyQuestsCount { get; set; }
        
    /// <summary>
    /// Miejsca na artefakty.
    /// </summary>
    public string ArtifactsPlaces { get; set; }
        
    /// <summary>
    /// Miejsca na wzmocnienia.
    /// </summary>
    public string BuffsPlaces { get; set; }
        
    /// <summary>
    /// Określa, czy przycisk odblokowania wzmocnienia jest widoczny.
    /// </summary>
    public bool UnlockBuffBtnVisible { get; set; }
        
    /// <summary>
    /// Określa, czy przycisk odblokowania artefaktu jest widoczny.
    /// </summary>
    public bool UnlockArtifactBtnVisible { get; set; }
        
    /// <summary>
    /// Miejsca na zadania tygodniowe.
    /// </summary>
    public string WeeklyQuestsPlaces { get; set; }
        
    /// <summary>
    /// Miejsca na zadania dzienne.
    /// </summary>
    public string DailyQuestsPlaces { get; set; }
        
    /// <summary>
    /// Odblokowane zadania dzienne.
    /// </summary>
    public string UnlockedDailyQuestsCount { get; set; }
        
    /// <summary>
    /// Odblokowane zadania tygodniowe.
    /// </summary>
    public string UnlockedWeeklyQuestsCount { get; set; }
}