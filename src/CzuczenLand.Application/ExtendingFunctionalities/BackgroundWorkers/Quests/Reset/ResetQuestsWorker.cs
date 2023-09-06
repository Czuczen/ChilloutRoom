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
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.Quests.Reset;

/// <summary>
/// Klasa wykonująca pracę w cyklach związaną z resetowaniem zadań.
/// </summary>
public class ResetQuestsWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
{
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium zadań.
    /// </summary>
    private readonly IRepository<Quest, int> _questRepository;
    
    /// <summary>
    /// Repozytorium magazynów plantacji.
    /// </summary>
    private readonly IRepository<PlantationStorage, int> _plantationStorageRepository;
    
    /// <summary>
    /// Repozytorium postępu wymagań zadań.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Serwis podstawowy obsługujący logikę biznesową związaną z zadaniami.
    /// </summary>
    private readonly IQuestService _questService;
    
    /// <summary>
    /// Kontekst Huba zadań.
    /// </summary>
    private IHubContext _questHub;
    
    /// <summary>
    /// Okres czasu (w milisekundach) między cyklami pracy.
    /// </summary>
    private const int PeriodTime = 60000; // 1min

    
    /// <summary>
    /// Aktualna data i godzina.
    /// </summary>
    private DateTime CurrDateTime { get; set; }
    
    /// <summary>
    /// Data pierwszego dnia tygodnia.
    /// </summary>
    private DateTime DateOfFirstDayWeek { get; set; }
    
    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="questRepository">Repozytorium zadań.</param>
    /// <param name="plantationStorageRepository">Repozytorium magazynów plantacji.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium postępu wymagań zadań.</param>
    /// <param name="questService">Serwis zadań.</param>
    /// <param name="timer">Timer do określania czasu cyklu pracy.</param>
    public ResetQuestsWorker(
        IRepository<District> districtRepository,
        IRepository<Quest, int> questRepository,
        IRepository<PlantationStorage, int> plantationStorageRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        IQuestService questService,
        AbpTimer timer
    ) 
        : base(timer)
    {
        _districtRepository = districtRepository;
        _questRepository = questRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _questService = questService;

        DateOfFirstDayWeek = DateTimeUtils.GetDateOfFirstDayWeek(Clock.Now);
        Timer.Period = PeriodTime;
    }
    
    /// <summary>
    /// Metoda wykonywana w każdym cyklu pracy pracownika.
    /// Nie wychodziło równo co sekundę. Dlatego robimy korektę.
    /// Czasami jeszcze łapie poślizg 15 milisekund ale jak dla mnie jest to już wystarczające.
    /// ASP.NET Boilerplate sam normalizuje godziny. Clock - https://aspnetboilerplate.com/Pages/Documents/Timing
    /// </summary>
    protected override void DoWork()
    {
        var watch = Stopwatch.StartNew();
        CurrDateTime = Clock.Now;
        
        if (CurrDateTime.Hour == 0 && CurrDateTime.Minute == 0)
            ExecuteWork();
            
        watch.Stop();
        Timer.Period = WorkersHelper.CalculatePeriodTime(PeriodTime, watch);
    }

    /// <summary>
    /// Resetuje ukończone zadania dzienne i tygodniowe.
    /// Musi mieć jednostkę pracy. Musi być virtual. Może być protected lub public. Inaczej nie aktualizuje zmian.
    /// Osobno po to, żeby Stopwatch zrobił prawidłowy pomiar bo na koniec metody jednostka pracy wykonuje swoje operacje 
    /// </summary>
    [UnitOfWork]
    protected virtual void ExecuteWork()
    {
        var weeklySubtractedDate = CurrDateTime - TimeSpan.FromDays(7);
        var weeklyLimitTimeHasPassed = weeklySubtractedDate >= DateOfFirstDayWeek;

        var questQuery = _questRepository.GetAll().Where(item =>
            item.IsComplete && (item.QuestType == DbQuestTypesNames.Daily ||
                                item.QuestType == DbQuestTypesNames.Weekly));

        var filteredQuestAndQuestProgress = _questRequirementsProgressRepository.GetAll()
            .Join(questQuery,
                progress => progress.QuestId,
                quest => quest.Id,
                (progress, quest) => new {progress, quest}).ToList();

        var allDistricts = _districtRepository.GetAllList();
        var allPlantationStorages = _plantationStorageRepository.GetAllList();

        var completedQuests = filteredQuestAndQuestProgress.Select(item => item.quest).ToList();
        var questsProgress = filteredQuestAndQuestProgress.Select(item => item.progress).ToList();
        
        _questHub ??= GlobalHost.ConnectionManager.GetHubContext<QuestHub>();
        
        if (completedQuests.Any())
        {
            foreach (var quest in completedQuests)
            {
                try
                {
                    var playerPlantationStorage = allPlantationStorages.Single(item => item.Id == quest.PlantationStorageId);
                    var district = allDistricts.Single(item => item.Id == playerPlantationStorage.DistrictId);
                    if (!district.IsDefined) continue;
                    
                    var questProgress = questsProgress.Single(item => item.QuestId == quest.Id);
                    switch (quest.QuestType)
                    {
                        case DbQuestTypesNames.Daily:
                            WorkersHelper.ResetQuest(quest, questProgress);
                            _questService.SetStartValues(new List<Quest> {quest});
                            _questHub.Clients.User(playerPlantationStorage.UserId.ToString()).setRestartedQuest(quest.Id, quest.QuestType);
                            break;
                        case DbQuestTypesNames.Weekly:
                            if (weeklyLimitTimeHasPassed)
                            {
                                WorkersHelper.ResetQuest(quest, questProgress);
                                _questService.SetStartValues(new List<Quest> {quest});
                                _questHub.Clients.User(playerPlantationStorage.UserId.ToString()).setRestartedQuest(quest.Id, quest.QuestType);
                            }
                            break;
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

        WorkersHelper.ResetStartedQuestsLimit(allPlantationStorages, allDistricts, weeklyLimitTimeHasPassed);

        if (weeklyLimitTimeHasPassed)
            DateOfFirstDayWeek = DateTimeUtils.GetDateOfFirstDayWeek(CurrDateTime);
    }
}