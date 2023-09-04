using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;

/// <summary>
/// Reprezentuje DTO dla magazynu plantacji.
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.General.PlantationStorage))]
public class PlantationStorageDto : PartStorageDto
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
    /// Liczba używanych slotów na wzmocnienia.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.BuffSlotsInUse)]
    public int BuffSlotsInUse { get; set; }
        
    /// <summary>
    /// Liczba używanych slotów na artefakty.
    /// </summary>
    [DisplayName(PlantationStorageFieldsHrNames.ArtifactSlotsInUse)]
    public int ArtifactSlotsInUse { get; set; }
        
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
        
        
        
    /// <summary>
    /// Identyfikator dzielnicy, do której przypisany jest magazyn plantacji.
    /// </summary>
    [DisplayName(EntitiesHrNames.District), Display(GroupName = EntitiesDbNames.District)]
    public int DistrictId { get; set; }
}