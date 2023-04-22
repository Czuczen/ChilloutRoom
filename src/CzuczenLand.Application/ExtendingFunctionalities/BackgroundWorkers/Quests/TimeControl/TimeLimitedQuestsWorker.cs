using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Quests.TimeControl.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;
using CzuczenLand.ExtendingModels.Models.General;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Quests.TimeControl;

public class TimeLimitedQuestsWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    private readonly IRepository<Quest, int> _questRepository;
    private readonly IRepository<PlantationStorage, int> _plantationStorageRepository;
    private readonly IRepository<PlayerStorage, int> _playerStorageRepository;
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    private readonly IQuestService _questService;
    private IHubContext _questHub;
    private const int PeriodTime = 1000; // 1s

    private List<QuestRequirementsProgress> AllQuestsRequirementsProgress { get; set; }
    
    private List<PlantationStorage> AllPlantationStorages { get; set; }
    
    private List<PlayerStorage> AllPlayerStorages { get; set; }
    
    private List<Quest> AllQuests { get; set; }
    
    private List<District> AllDistricts { get; set; }
    
    private List<GeneratedType> AllGeneratedTypes { get; set; }


    public TimeLimitedQuestsWorker(
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<Quest, int> questRepository,
        IRepository<PlantationStorage, int> plantationStorageRepository,
        IRepository<PlayerStorage, int> playerStorageRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        IQuestService questService,
        AbpTimer timer
    ) 
        : base(timer)
    {
        _districtRepository = districtRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _questRepository = questRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _playerStorageRepository = playerStorageRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _questService = questService;
        Timer.Period = PeriodTime;
    }
        
    /// <summary>
    /// Nie wychodziło równo co sekundę. Dlatego robimy korektę.
    /// Czasami jeszcze łapie poślizg 15 milisekund ale jak dla mnie jest to już wystarczające.
    /// </summary>
    protected override void DoWork()
    {
        var watch = Stopwatch.StartNew();
        ExecuteWork();
        watch.Stop();
        Timer.Period = WorkersHelper.CalculatePeriodTime(PeriodTime, watch);
    }

    /// <summary>
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje 
    /// </summary>
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        AllDistricts = _districtRepository.GetAllList();
        AllPlantationStorages = _plantationStorageRepository.GetAllList();
        AllGeneratedTypes = _generatedTypeRepository.GetAllList();
        AllPlayerStorages = _playerStorageRepository.GetAllList();
        AllQuests = _questRepository.GetAllList();
        AllQuestsRequirementsProgress = _questRequirementsProgressRepository.GetAllList();
        _questHub ??= GlobalHost.ConnectionManager.GetHubContext<QuestHub>();
            
        SetDuration();
        RemoveNotAvailableEventQuests();
        SetAvailableEventQuests();
        SetCyclicTime(); // musi być ostatnie
    }

    private void SetDuration()
    {
        var questsWithDuration = AllQuests.Where(item =>
            item.PlantationStorageId != null && item.InProgress && item.Duration != null && item.Duration > 0 && item.IsAvailableInitially);
            
        foreach (var quest in questsWithDuration)
        {
            try
            {
                var questPlantationStorage = AllPlantationStorages.Single(item => item.Id == (int) quest.PlantationStorageId); // PlantationStorageId nie będzie null. Na tym etapie musi być ustawione.
                var district = AllDistricts.Single(item => item.Id == questPlantationStorage.DistrictId);
                
                if (!district.IsDefined) continue;
                if (quest.PlantationLevelRequirement > questPlantationStorage.Level) continue;
                
                quest.CurrentDuration++;
                var playerStorage = AllPlayerStorages.Single(item => item.UserId == questPlantationStorage.UserId);
                if (questPlantationStorage.DistrictId != playerStorage.LastSelectedDistrictId) continue;
                
                var userId = questPlantationStorage.UserId;
                if (quest.Duration <= quest.CurrentDuration)
                {
                    var questProgress = AllQuestsRequirementsProgress.Single(item => item.QuestId == quest.Id);
                    WorkersHelper.ResetQuest(quest, questProgress);
                    _questService.SetStartValues(new List<Quest> {quest});
                    var obj = new SetDurationResponse
                    {
                        QuestId = quest.Id,
                        RemainingTime = "Pozostały czas: 0sek",
                        TimeUp = true
                    };
                        
                    _questHub.Clients.User(userId.ToString()).updateDuration(obj);
                }
                else
                {
                    var remainingTime = quest.Duration - quest.CurrentDuration;
                    var obj = new SetDurationResponse
                    {
                        QuestId = quest.Id,
                        RemainingTime = "Pozostały czas: " + remainingTime + "sek",
                        TimeUp = false
                    };
                        
                    _questHub.Clients.User(userId.ToString()).updateDuration(obj);
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException && ex.Message.Contains("Sequence contains no matching element")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        }
    }

    /// <summary>
    /// Zadanie cykliczne które ma ustawiony czas startu później niż końca nie będzie przedłużane. Takiego zadania nie da się też rozpocząć. 
    /// </summary>
    private void SetCyclicTime()
    {
        var currDateTime = Clock.Now;
        var eventQuestsAfterEvent = AllQuests.Where(item =>
            item.QuestType == DbQuestTypesNames.Event && item.StartTime < currDateTime &&
            item.EndTime < currDateTime && item.CyclicTime != null &&
            item.CyclicTime > 0).ToList();

        foreach (var eventQuest in eventQuestsAfterEvent)
        {
            try
            {
                var questGeneratedType = AllGeneratedTypes.Single(item => item.Id == eventQuest.GeneratedTypeId); // Po PlantationStorageId nie szukamy dzielnicy bo są tu też definicje zadań, które nie mają ustawionego PlantationStorageId
                var district = AllDistricts.Single(item => item.Id == questGeneratedType.DistrictId);
                if (!district.IsDefined) continue;

                var timeToAdd = TimeSpan.FromMinutes(double.Parse(eventQuest.CyclicTime.ToString()));
                eventQuest.StartTime += timeToAdd;
                eventQuest.EndTime += timeToAdd;   
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException && ex.Message.Contains("Sequence contains no matching element")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        }
    }

    private void SetAvailableEventQuests()
    {
        var currDateTime = Clock.Now;
        var playersEventQuests = AllQuests.Where(item =>
            item.QuestType == DbQuestTypesNames.Event && item.PlantationStorageId != null && item.IsAvailableInitially && 
            item.StartTime <= currDateTime && item.EndTime > currDateTime && !item.WorkerSent).ToList();

        foreach (var quest in playersEventQuests)
        {
            try
            {
                var playerPlantationStorage = AllPlantationStorages.Single(item => item.Id == (int) quest.PlantationStorageId); // PlantationStorageId nie będzie null. Na tym etapie musi być ustawione.
                var district = AllDistricts.Single(item => item.Id == playerPlantationStorage.DistrictId);
                
                if (!district.IsDefined) continue;
                if (quest.PlantationLevelRequirement > playerPlantationStorage.Level) continue;

                var wasCompleted = quest.IsComplete;
                var playerStorage = AllPlayerStorages.Single(item => item.UserId == playerPlantationStorage.UserId);
                if (wasCompleted)
                {
                    var questProgress = AllQuestsRequirementsProgress.Single(item => item.QuestId == quest.Id);
                    WorkersHelper.ResetQuest(quest, questProgress);
                }

                quest.WorkerSent = true;
                    
                if (!wasCompleted || playerPlantationStorage.DistrictId != playerStorage.LastSelectedDistrictId)
                    continue;
                    
                _questHub.Clients.User(playerPlantationStorage.UserId.ToString()).setAvailableEventQuest(quest.Id, quest.QuestType);
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException && ex.Message.Contains("Sequence contains no matching element")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        }
    }

    private void RemoveNotAvailableEventQuests()
    {
        var currDateTime = Clock.Now;
  
        var eventQuestsAfterEvent = AllQuests.Where(item =>
            item.QuestType == DbQuestTypesNames.Event && item.EndTime < currDateTime &&
            item.PlantationStorageId != null && item.IsAvailableInitially && item.WorkerSent).ToList();
            
        foreach (var eventQuest in eventQuestsAfterEvent)
        {
            try
            {
                var plantationStorage = AllPlantationStorages.Single(item => item.Id == (int) eventQuest.PlantationStorageId); // PlantationStorageId nie będzie null. Na tym etapie musi być ustawione.
                var district = AllDistricts.Single(item => item.Id == plantationStorage.DistrictId);
                
                if (!district.IsDefined) continue;
                if (eventQuest.PlantationLevelRequirement > plantationStorage.Level) continue;

                var isCyclic = eventQuest.CyclicTime != null && eventQuest.CyclicTime > 0;
                var playerStorage = AllPlayerStorages.Single(item => item.UserId == plantationStorage.UserId);
                
                eventQuest.WorkerSent = false;
                eventQuest.InProgress = false;
                    
                if ((!isCyclic && eventQuest.IsComplete) || plantationStorage.DistrictId != playerStorage.LastSelectedDistrictId)
                    continue;
                    
                if (!isCyclic && !eventQuest.IsComplete)
                    eventQuest.IsAvailableInitially = false;
                    
                _questHub.Clients.User(plantationStorage.UserId.ToString()).removeNotAvailableEventQuest(eventQuest.Id, eventQuest.QuestType, eventQuest.IsComplete);
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException && ex.Message.Contains("Sequence contains no matching element")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        }
    }
}