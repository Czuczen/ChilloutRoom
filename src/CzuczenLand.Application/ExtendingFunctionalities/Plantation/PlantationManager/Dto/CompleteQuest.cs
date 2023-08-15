using System.Collections.Generic;
using Abp.ObjectMapping;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Strategies.Db;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

/// <summary>
/// Reprezentuje zakończenie zadania.
/// Gromadzi dane potrzebne do przetworzenia ukończonego zadania.
/// </summary>
public class CompleteQuest
{
    /// <summary>
    /// Ilość zdobytego doświadczenia.
    /// </summary>
    public decimal GainedExp = 0;
    
    /// <summary>
    /// Ilość zdobytego złota.
    /// </summary>
    public decimal GainedGold = 0;
    
    /// <summary>
    /// Ilość zdobytego prestiżu.
    /// </summary>
    public int GainedPrestige = 0;
    
    /// <summary>
    /// Ilość zdobytych żetonów misji.
    /// </summary>
    public int GainedQuestTokens  = 0;
    
    /// <summary>
    /// Ilość zdobytych żetonów dealera.
    /// </summary>
    public int GainedDealerTokens  = 0;
    
    /// <summary>
    /// Ilość zdobytych żetonów czarnego rynku.
    /// </summary>
    public int GainedBlackMarketTokens  = 0;
    
    /// <summary>
    /// Ilość zdobytego honoru.
    /// </summary>
    public int GainedHonor  = 0;
        
    
    /// <summary>
    /// Ukończone zadanie.
    /// </summary>
    public Quest Quest { get; set; }
        
    /// <summary>
    /// Magazyn gracza.
    /// </summary>
    public PlayerStorage PlayerStorage { get; set; }
        
    /// <summary>
    /// Magazyn plantacji.
    /// </summary>
    public PlantationStorage PlantationStorage { get; set; }
        
    /// <summary>
    /// Definicja ukończonego zadania.
    /// </summary>
    public Quest QuestDefinition { get; set; }

    /// <summary>
    /// Lista powiązań z nagrodami.
    /// </summary>
    public List<DropQuest> Connections { get; set; }
        
    /// <summary>
    /// Mapuje obiekty.
    /// </summary>
    public IObjectMapper ObjectMapper { get; set; }

    /// <summary>
    /// Strategia parsowania do bazy danych.
    /// </summary>
    public DbStrategy ParserDbStrategy { get; set; }
        
    /// <summary>
    /// Parser do bazy danych.
    /// </summary>
    public IParser DbParser { get; set; }
        
    /// <summary>
    /// Lista typów generowanych dzielnicy.
    /// </summary>
    public List<GeneratedType> GeneratedTypes { get; set; }
        
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public long UserId { get; set; }
        
    /// <summary>
    /// Dzielnica na której ukończono zadanie.
    /// </summary>
    public District District { get; set; }
        
    /// <summary>
    /// Ilość zdobytych żetonów Dona.
    /// </summary>
    public int GainedDonTokens { get; set; }
        
    /// <summary>
    /// Ilość zdobytych żetonów odblokowania.
    /// </summary>
    public int GainedUnlockTokens { get; set; }
        
    /// <summary>
    /// Status inicjalizacji.
    /// </summary>
    public bool InitStatus { get; set; }

    /// <summary>
    /// Zbiór danych do utworzenia informacji o zadaniu.
    /// </summary>
    public QuestInfoCreation QuestInfoCreation { get; set; }

    /// <summary>
    /// Postępy w wymaganiach zadania.
    /// </summary>
    public QuestRequirementsProgress QuestRequirementsProgress { get; set; }
        
    /// <summary>
    /// Powiadomienia o zdobytych nagrodach.
    /// </summary>
    public List<string> DropsNotification { get; } = new();
        
    /// <summary>
    /// Lista powiązanych nagród.
    /// </summary>
    public List<Drop> ConnectedDrops { get; } = new();
}