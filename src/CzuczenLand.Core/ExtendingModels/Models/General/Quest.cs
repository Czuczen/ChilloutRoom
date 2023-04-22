using System;
using Abp.Auditing;
using Abp.AutoMapper;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.General;

[Audited]
[AutoMapFrom(typeof(PartMessages))]
public class Quest : PartMessages, IPlantationGeneratedEntity
{
    public decimal? Duration { get; set; }
        
    public decimal CurrentDuration { get; set; }
        
    public bool IsAvailableInitially { get; set; }
        
    public bool IsRepetitive { get; set; }
        
    public bool IsComplete { get; set; }
        
    public bool WorkerSent { get; set; } // po to żeby nie wysyłał na frontend tych które już tam są. To samo zadanie wyświetlało się wielokrotnie.

    public int? CyclicTime { get; set; } // tylko zadania eventowe
        
    public int CompletedAmount { get; set; }
        
    public int? PlantationLevelRequirement { get; set; } // może być null ale tylko wtedy gdy zadanie to osiągnięcie
        
    public string QuestType { get; set; }
        
    public DateTime? StartTime { get; set; }
        
    public DateTime? EndTime { get; set; }
        
    public bool InProgress { get; set; }

    public int GeneratedTypeId { get; set; }
        
    public int? PlantationStorageId { get; set; }
        
    public int? PlayerStorageId { get; set; }
}