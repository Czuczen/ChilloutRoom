using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Update;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;

[AutoMapTo(typeof(ExtendingModels.Models.General.PlantationStorage))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.PlantationStorage))]
public class PlantationStorageUpdateDto : PartStorageUpdateDto
{
    [DisplayName(PlantationStorageFieldsHrNames.CurrExp)]
    public decimal CurrExp { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.ExpToNextLevel)]
    public decimal ExpToNextLevel { get; set; }
        
        
        
        
    
    [DisplayName(PlantationStorageFieldsHrNames.QuestToken)]
    public int QuestToken { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.DealerToken)]
    public int DealerToken { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.BlackMarketToken)]
    public int BlackMarketToken { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.DonToken)]
    public int DonToken { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.UnlockToken)]
    public int UnlockToken { get; set; }

    [DisplayName(PlantationStorageFieldsHrNames.Prestige)]
    public int Prestige { get; set; }


        
        
    [DisplayName(PlantationStorageFieldsHrNames.StartedDailyQuestsCount)]
    public int StartedDailyQuestsCount { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.StartedWeeklyQuestsCount)]
    public int StartedWeeklyQuestsCount { get; set; }
        
        
        
    [DisplayName(PlantationStorageFieldsHrNames.MaxBuffsSlots)]
    public int MaxBuffsSlots { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.MaxArtifactSlots)]
    public int MaxArtifactSlots { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.MaxDailyQuestsCount)]
    public int MaxDailyQuestsCount { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.MaxWeeklyQuestsCount)]
    public int MaxWeeklyQuestsCount { get; set; }
        
        
        
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedBuffsSlots)]
    public int UnlockedBuffsSlots { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedArtifactSlots)]
    public int UnlockedArtifactSlots { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedDailyQuestsCount)]
    public int UnlockedDailyQuestsCount { get; set; }
        
    [DisplayName(PlantationStorageFieldsHrNames.UnlockedWeeklyQuestsCount)]
    public int UnlockedWeeklyQuestsCount { get; set; }
}