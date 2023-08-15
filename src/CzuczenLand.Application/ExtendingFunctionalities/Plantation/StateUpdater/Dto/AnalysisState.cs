using System.Collections.Generic;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.ObjectMapping;
using Abp.RealTime;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Display;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Shared;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

/// <summary>
/// Klasa zbierająca wszystkie dane potrzebne do analizy zmiany na danej encji.
/// </summary>
public class AnalysisState
{
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    private long? _userId;
        

    /// <summary>
    /// Dostępne zadania użytkownika.
    /// </summary>
    public List<Quest> AvailableQuests { get; set; }
        
    /// <summary>
    /// Dostępne zadania połączone ze swoimi wymaganiami.
    /// </summary>
    public List<QuestWithRequirements> QuestsWithRequirements { get; set; }
        
    /// <summary>
    /// Pole ustawiane jeśli aktualizowana encja jest magazynem.
    /// </summary>
    public PartStorage StorageEntity { get; set; }
        
    /// <summary>
    /// Pole ustawiane jeśli aktualizowana encja jest generowana użytkownikom na podstawie definicji.
    /// </summary>
    public IPlantationGeneratedEntity GeneratedEntity { get; set; }

    /// <summary>
    /// Aktualizowana encja, która będzie poddana analizie pod kątem dokonanej zmiany.
    /// </summary>
    public object Entity => (object) GeneratedEntity ?? StorageEntity;

    /// <summary>
    /// Magazyn plantacji użytkownika do którego należy aktualizowana encja.
    /// </summary>
    public PlantationStorage PlantationStorage { get; set; }

    /// <summary>
    /// Status inicjalizacji stanu analizy przez buildera.
    /// </summary>
    public bool InitStatus { get; set; }
        
    /// <summary>
    /// Magazyn gracza do którego należy aktualizowana encja.
    /// </summary>
    public PlayerStorage PlayerStorage { get; set; }
        
    /// <summary>
    /// Kontekst huba dla zadań.
    /// </summary>
    public IHubContext QuestHub { get; set; }
        
    /// <summary>
    /// Kontekst huba dla informacji.
    /// </summary>
    public IHubContext InfoHub { get; set; }
        
    /// <summary>
    /// Kontekst huba dla czarnego rynku.
    /// </summary>
    public IHubContext BlackMarketHub { get; set; }
        
    /// <summary>
    /// Kontekst huba dla bonusów.
    /// </summary>
    public IHubContext BonusHub { get; set; }
        
    /// <summary>
    /// Kolekcja zmian właściwości encji.
    /// </summary>
    public ICollection<EntityPropertyChange> LastEntityPropertyChanges { get; set; }
        
    /// <summary>
    /// Pole ustawiane jeśli encja jest transakcją czarnego rynku i jest tworzona lub usuwana.
    /// </summary>
    public BlackMarketTransaction BlackMarketEntity { get; set; }
        
    /// <summary>
    /// Określa, czy użytkownik jest opiekunem dzielnicy.
    /// </summary>
    public bool IsDistrictWarden { get; set; }
        
    /// <summary>
    /// Akcja tworzenia lub usuwania roślin i transakcji czarnego rynku.
    /// </summary>
    public string Action { get; set; }
        
    /// <summary>
    /// Manager klientów online hubów signalr.
    /// </summary>
    public IOnlineClientManager OnlineClientManager { get; set; }
        
    /// <summary>
    /// Mapper obiektów.
    /// </summary>
    public IObjectMapper ObjectMapper { get; set; }
        
    /// <summary>
    /// Lista aktualnie dostępnych dzielnic po zwiększeniu poziomu gracza w formacie dla biblioteki Select2.
    /// </summary>
    public List<Select2Data> S2DistrictsList { get; set; }
        
    /// <summary>
    /// Listy przedmiotów dostępnych na nowo zdobytym poziomie.
    /// </summary>
    public CurrLvlItems ReceivedLvlItems { get; set; }
        
    /// <summary>
    /// Dzielnica z którą jest powiązana aktualizowana encja.
    /// </summary>
    public District District { get; set; }
        
    /// <summary>
    /// Liczba otrzymanych poziomów.
    /// </summary>
    public int ReceivedLevels { get; set; }

    /// <summary>
    /// Repozytorium postępu wymagań zadania.
    /// </summary>
    public IRepository<QuestRequirementsProgress> QuestRequirementsProgressRepository { get; set; }

    /// <summary>
    /// Nieukończone zadania powiązane z użytkownikiem do którego należy aktualizowana encja. 
    /// </summary>
    public List<Quest> Quests { get; set; }
        
    /// <summary>
    /// Lista postępu wymagań zadań.
    /// </summary>
    public List<QuestRequirementsProgress> QuestRequirementsProgress { get; set; }
        
    /// <summary>
    /// Informacja o ignorowaniu zmian.
    /// </summary>
    public IgnoreChange IgnoreChange { get; set; }
        
    /// <summary>
    /// Lista wymagań.
    /// </summary>
    public List<Requirement> Requirements { get; set; }

    /// <summary>
    /// Lista wszystkich dzielnic.
    /// </summary>
    public List<District> AllDistricts { get; set; }

    /// <summary>
    /// Lista magazynów plantacji użytkownika.
    /// </summary>
    public List<PlantationStorage> PlantationStorages { get; set; }

    /// <summary>
    /// Parser do wyświetlania danych.
    /// </summary>
    public IParser DisplayParser { get; set; }
        
    /// <summary>
    /// Parser do bazy danych.
    /// </summary>
    public IParser DbParser { get; set; }
        
    /// <summary>
    /// Strategia parsera do wyświetlania danych.
    /// </summary>
    public DisplayStrategy DisplayParserStrategy { get; set; }
        
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public long UserId
    {
        get => _userId ??= PlantationStorage.UserId;
        set => _userId = value;
    }

    /// <summary>
    /// Określa, czy wymagane jest zignorowanie zmiany.
    /// </summary>
    public bool NeedIgnoreChange
    {
        get
        {
            if (Entity is not Product) return false;
            return IgnoreChange != null;
        }
    }
    
    /// <summary>
    /// Lista dzielnic dostępnych po uzyskaniu nowego poziomu/ów.
    /// </summary>
    public List<District> NewDistricts { get; } = new();
}