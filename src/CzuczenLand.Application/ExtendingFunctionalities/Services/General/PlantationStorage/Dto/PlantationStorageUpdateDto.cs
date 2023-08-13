using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w magazynie plantacji.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.PlantationStorage))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.PlantationStorage))]
public class PlantationStorageUpdateDto : PartStorageUpdateDto
{
    /// <summary>
    /// Aktualne doświadczenie plantacji.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.CurrExp)]
    public decimal CurrExp { get; set; }
        
    /// <summary>
    /// Doświadczenie potrzebne do osiągnięcia następnego poziomu.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.ExpToNextLevel)]
    public decimal ExpToNextLevel { get; set; }
        
        
        
        
    
    /// <summary>
    /// Liczba żetonów misji.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.QuestToken)]
    public int QuestToken { get; set; }
        
    /// <summary>
    /// Liczba żetonów dealera.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.DealerToken)]
    public int DealerToken { get; set; }
        
    /// <summary>
    /// Liczba żetonów czarnego rynku.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.BlackMarketToken)]
    public int BlackMarketToken { get; set; }
        
    /// <summary>
    /// Liczba żetonów dona.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.DonToken)]
    public int DonToken { get; set; }
        
    /// <summary>
    /// Liczba żetonów odblokowania.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.UnlockToken)]
    public int UnlockToken { get; set; }

    /// <summary>
    /// Liczba prestiżu plantacji.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.Prestige)]
    public int Prestige { get; set; }


        
        
    /// <summary>
    /// Liczba rozpoczętych dziennych zadań.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.StartedDailyQuestsCount)]
    public int StartedDailyQuestsCount { get; set; }
        
    /// <summary>
    /// Liczba rozpoczętych tygodniowych zadań.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.StartedWeeklyQuestsCount)]
    public int StartedWeeklyQuestsCount { get; set; }
        
        
        
    /// <summary>
    /// Maksymalna liczba slotów na wzmocnienia.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.MaxBuffsSlots)]
    public int MaxBuffsSlots { get; set; }
        
    /// <summary>
    /// Maksymalna liczba slotów na artefakty.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.MaxArtifactSlots)]
    public int MaxArtifactSlots { get; set; }
        
    /// <summary>
    /// Maksymalna liczba dziennych zadań.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.MaxDailyQuestsCount)]
    public int MaxDailyQuestsCount { get; set; }
        
    /// <summary>
    /// Maksymalna liczba tygodniowych zadań.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.MaxWeeklyQuestsCount)]
    public int MaxWeeklyQuestsCount { get; set; }
        
        
        
    /// <summary>
    /// Liczba odblokowanych slotów na wzmocnienia.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedBuffsSlots)]
    public int UnlockedBuffsSlots { get; set; }
        
    /// <summary>
    /// Liczba odblokowanych slotów na artefakty.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedArtifactSlots)]
    public int UnlockedArtifactSlots { get; set; }
        
    /// <summary>
    /// Liczba odblokowanych dziennych zadań.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedDailyQuestsCount)]
    public int UnlockedDailyQuestsCount { get; set; }
        
    /// <summary>
    /// Liczba odblokowanych tygodniowych zadań.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedWeeklyQuestsCount)]
    public int UnlockedWeeklyQuestsCount { get; set; }
}