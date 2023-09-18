using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

    /// <summary>
    /// Resetuje limit rozpoczętych zadań dla plantacji.
    /// </summary>
    /// <param name="allPlantationStorages">Lista wszystkich magazynów plantacji</param>
    /// <param name="allDistricts">Lista wszystkich dzielnic</param>
    /// <param name="weeklyLimitTimeHasPassed">Czy minął czas tygodniowego limitu</param>
    public static void ResetStartedQuestsLimit(List<PlantationStorage> allPlantationStorages, List<District> allDistricts, bool weeklyLimitTimeHasPassed)
    {
        if (!allDistricts.Any() || !allPlantationStorages.Any()) return;
        
        foreach (var plantationStorage in allPlantationStorages)
        {
            var district = allDistricts.Single(item => item.Id == plantationStorage.DistrictId);
            if (!district.IsDefined) continue;
            
            plantationStorage.StartedDailyQuestsCount = 0;
            if (weeklyLimitTimeHasPassed)
                plantationStorage.StartedWeeklyQuestsCount = 0;
        }
    }
}