using System.Collections.Generic;
using System.Linq;
using Abp.Timing;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.EntityAnalysisStateBuilder;

public static class EntityAnalysisStateBuilderHelper
{
    public static void SetPlayerStorageAnalysisState(AnalysisState state, PlayerStorage playerStorage)
    {
        var userId = playerStorage.UserId;
        state.PlayerStorage = playerStorage;
        
        if (state.ReceivedLevels > 0)
            GetReceivedLvlDistricts(state);
                        
        state.UserId = userId;

        var availableQuests = new List<Quest>(); 
        foreach (var plantationStorage in state.PlantationStorages)
        {
            availableQuests.AddRange(state.Quests.Where(item => item.QuestType != DbQuestTypesNames.Achievement &&
                                                                item.PlantationLevelRequirement <= plantationStorage.Level && !item.IsComplete));
        }

        var achievements = state.Quests.Where(item => item.QuestType == DbQuestTypesNames.Achievement).ToList();
        availableQuests.AddRange(achievements);

        state.AvailableQuests = availableQuests;
        state.QuestsWithRequirements = GetQuestsWithRequirements(state);
    }
        
    public static List<Quest> GetAvailableQuests(AnalysisState state)
    {
        var plantationStorage = state.PlantationStorage;
        var ret = state.Quests.Where(item =>
            item.QuestType != DbQuestTypesNames.Achievement &&
            item.PlantationLevelRequirement <= plantationStorage.Level).ToList();

        var achievements = state.Quests.Where(item =>
            item.QuestType == DbQuestTypesNames.Achievement).ToList();
        
        ret.AddRange(achievements);

        return ret;
    }

    public static List<QuestWithRequirements> GetQuestsWithRequirements(AnalysisState state)
    {
        var ret = new List<QuestWithRequirements>();
        if (state.AvailableQuests != null && state.AvailableQuests.Any())
        {
            foreach (var quest in state.AvailableQuests)
            {
                var questRequirementsProgress = state.QuestRequirementsProgress.Single(item => item.QuestId == quest.Id);
                var questWithConnections = new QuestWithRequirements{Quest = quest, QuestRequirementsProgress = questRequirementsProgress};

                foreach (var reqProgress in questWithConnections.RequirementsProgress)
                    questWithConnections.Requirements.Add(state.Requirements.Single(item => item.Id == reqProgress.Key));

                ret.Add(questWithConnections);
            }
        }

        return ret;
    }

    private static void GetReceivedLvlDistricts(AnalysisState state)
    {
        if (!state.IsDistrictWarden)
        {
            var oldLvl = state.StorageEntity.Level - state.ReceivedLevels;
            for (var i = 0; i < state.ReceivedLevels; i++)
            {
                var lvl = oldLvl + 1;
                state.NewDistricts.AddRange(state.AllDistricts.Where(item =>
                    item.PlayerLevelRequirement == lvl && (item.EndTime == null || item.EndTime > Clock.Now)));
            }
            
            if (state.NewDistricts.Count > 0)
                state.S2DistrictsList = GenerateS2DistrictsList(state);
        }
    }
        
    private static List<Select2Data> GenerateS2DistrictsList(AnalysisState state)
    {
        var storage = state.PlayerStorage;
        var playerDistricts = new List<District>();
        
        foreach (var plantationStorage in state.PlantationStorages)
        {
            var district = state.AllDistricts.SingleOrDefault(currDistrict =>
                currDistrict.Id == plantationStorage.DistrictId &&
                currDistrict.PlayerLevelRequirement <= storage.Level);
            if (district != null)
                playerDistricts.Add(district);
        }

        var dateTimeNow = Clock.Now;
        var hasWardenDistrict = state.AllDistricts.SingleOrDefault(item => item.UserId == storage.UserId);
        
        var availableDistricts = hasWardenDistrict != null
            ? new List<District> {hasWardenDistrict}
            : state.AllDistricts.Where(item => (item.StartTime <= dateTimeNow && item.IsDefined &&
                                                item.PlayerLevelRequirement <= storage.Level &&
                                                item.EndTime == null)
                                               ||
                                               (item.StartTime <= dateTimeNow && item.IsDefined &&
                                                item.PlayerLevelRequirement <= storage.Level &&
                                                item.EndTime != null &&
                                                item.EndTime > dateTimeNow
                                               )).ToList();
            
        return NewPlayerGeneratorHelper.GetS2DistrictsList(availableDistricts, playerDistricts);
    }
    
    public static int GetReceivedLevels(AnalysisState state)
    {
        var lastPropertiesChanges = state.LastEntityPropertyChanges;
        var properties = state.Entity.GetType().GetProperties();
            
        if (lastPropertiesChanges == null || !lastPropertiesChanges.Any()) return 0;
        
        var lvlPropertyChange = lastPropertiesChanges.SingleOrDefault(item => item.PropertyName == PlantationStorageObservedFields.Level);
        
        if (lvlPropertyChange?.OriginalValue == null) return 0;
        
        var prop = properties.SingleOrDefault(item => item.Name == lvlPropertyChange.PropertyName);
        var dbParser = state.DbParser;
        var parsedOldValue = (int) dbParser.Parse(prop, lvlPropertyChange.OriginalValue);
        var parsedNewValue = (int) dbParser.Parse(prop, lvlPropertyChange.NewValue);

        return parsedNewValue - parsedOldValue;
    }
}