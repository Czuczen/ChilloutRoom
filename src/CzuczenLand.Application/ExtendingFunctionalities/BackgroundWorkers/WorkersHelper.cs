using System.Collections.Generic;
using System.Diagnostics;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers;

public static class WorkersHelper
{
    /// <summary>
    /// Korekta timer'a dla workerów
    /// </summary>
    /// <param name="periodTime"></param>
    /// <param name="watch"></param>
    /// <returns></returns>
    public static int CalculatePeriodTime(int periodTime, Stopwatch watch) =>
        periodTime - int.Parse((watch.ElapsedMilliseconds > periodTime ? periodTime : watch.ElapsedMilliseconds).ToString());
    
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