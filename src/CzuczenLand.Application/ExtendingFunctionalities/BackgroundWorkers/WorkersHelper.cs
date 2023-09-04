using System.Collections.Generic;
using System.Diagnostics;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers;

/// <summary>
/// Klasa pomocnicza dla worker'ów.
/// </summary>
public static class WorkersHelper
{
    /// <summary>
    /// Korekta timer'a dla workerów.
    /// Oblicza nowy czas cyklu pracy pracownika.
    /// </summary>
    /// <param name="periodTime">Domyślny czas cyklu pracy.</param>
    /// <param name="watch">Zegar mierzacy czas trwania operacji</param>
    /// <returns>Nowy czas cyklu zmniejszony o czas trwania operacji</returns>
    public static int CalculatePeriodTime(int periodTime, Stopwatch watch) =>
        periodTime - int.Parse((watch.ElapsedMilliseconds > periodTime ? periodTime : watch.ElapsedMilliseconds).ToString());
    
    /// <summary>
    /// Resetuje zadanie i jego postęp.
    /// </summary>
    /// <param name="quest">Zadanie do zresetowania</param>
    /// <param name="questRequirementsProgress">Postęp wymagań zadania</param>
    public static void ResetQuest(Quest quest, QuestRequirementsProgress questRequirementsProgress)
    {
        var newRequirementsProgress = new Dictionary<int, decimal>();
        var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questRequirementsProgress.RequirementsProgress);
            
        foreach (var reqProgress in requirementsProgress)
            newRequirementsProgress[reqProgress.Key] = 0;
            
        questRequirementsProgress.RequirementsProgress = JsonConvert.SerializeObject(newRequirementsProgress);
        
        quest.InProgress = false;
        quest.CurrentDuration = 0;
        quest.IsComplete = false;
        quest.IsAvailableInitially = true;
    }
}