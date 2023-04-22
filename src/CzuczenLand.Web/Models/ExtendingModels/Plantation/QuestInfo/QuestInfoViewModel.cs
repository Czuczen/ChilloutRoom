using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using Abp.Timing;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;

[AutoMapFrom(typeof(Quest))]
public class QuestInfoViewModel : PartMessagesInfoViewModel
{
    private bool? _allRequirementsIsDone;
        
        
    public decimal? Duration { get; set; }
        
    public decimal CurrentDuration { get; set; }
        
    public bool IsAvailableInitially { get; set; }

    public int? CyclicTime { get; set; }
        
    public int? PlantationLevelRequirement { get; set; }
        
    public string RequirementsProgress { get; set; }
        
    public string QuestType { get; set; }
        
    public DateTime? StartTime { get; set; }
        
    public DateTime? EndTime { get; set; }
        
    public bool InProgress { get; set; }
        
    public bool IsComplete { get; set; }

    public bool IsRepetitive { get; set; }
        

    private TimeSpan CyclicTimeAsTimeSpan => Clock.Now.AddMinutes(double.Parse(CyclicTime.ToString())) - Clock.Now;

    public string CyclicTimeAsString => $"{CyclicTimeAsTimeSpan.Days} dni {CyclicTimeAsTimeSpan.Hours} godzin {CyclicTimeAsTimeSpan.Minutes} minut";
        
    public DateTime? NewStartTime => StartTime + TimeSpan.FromMinutes(double.Parse(CyclicTime.ToString()));

    public DateTime? NewEndTime => EndTime + TimeSpan.FromMinutes(double.Parse(CyclicTime.ToString()));
        
    private Dictionary<int, decimal> _requirementsProgressDict;

    private Dictionary<int, decimal> RequirementsProgressDict => _requirementsProgressDict ??=
        JsonConvert.DeserializeObject<Dictionary<int, decimal>>(RequirementsProgress);
        
    public bool AllRequirementsIsDone
    {
        get
        {
            if (_allRequirementsIsDone != null) return (bool) _allRequirementsIsDone;

            var completedAmount = 0;
            foreach (var req in Requirements)
            {
                if (RequirementsProgressDict[req.Id] >= req.Amount)
                {
                    completedAmount++;
                }
            }

            _allRequirementsIsDone = completedAmount == Requirements.Count;
            return (bool) _allRequirementsIsDone;
        }
    }
        
    public readonly List<RequirementInfoViewModel> Requirements = new();

    public readonly List<DropInfoViewModel> Drops = new();
}