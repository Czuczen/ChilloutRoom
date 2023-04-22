using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.QuestsStateUpdater;

public static class QuestsStateUpdater
{
    public static void UpdateProductOrStorageQuestsState(this AnalysisState state)
    {
        if (!state.InitStatus || state.LastEntityPropertyChanges == null) return;
            
        var analysis = new List<QuestWithRequirements>();
        foreach (var questWithRequirements in state.QuestsWithRequirements)
        {
            var questAnalysis = new QuestWithRequirements();
            foreach (var req in questWithRequirements.Requirements)
            {
                if (!QuestsStateUpdaterHelper.ReqNeedUpdate(req, questWithRequirements.Quest, state, out var checkValues)) continue;

                if (checkValues.Int != null)
                    QuestsStateUpdaterHelper.ProcessIntUpdate(state, (int) checkValues.Int, questAnalysis, questWithRequirements, req);
                    
                if (checkValues.Decimal != null)
                    QuestsStateUpdaterHelper.ProcessDecimalUpdate((decimal) checkValues.Decimal, questAnalysis, questWithRequirements, req, state);
            }

            QuestsStateUpdaterHelper.AddQuestAnalysisIfNeeded(analysis, questAnalysis);
        }

        if (state.Entity is PlayerStorage || state.PlayerStorage.LastSelectedDistrictId == state.District.Id)
            QuestsStateUpdaterHelper.SendUpdateQuests(analysis, state);
    }

    public static void UpdatePlantQuestsState(this AnalysisState state)
    {
        if (!state.InitStatus) return;
        
        var entity = (Plant) state.GeneratedEntity;
        var analysis = new List<QuestWithRequirements>();

        foreach (var inProgressQuestWithRequirements in state.QuestsWithRequirements.Where(item => item.Quest.InProgress))
        {
            var questAnalysis = new QuestWithRequirements();
            QuestsStateUpdaterHelper.ProcessPlantByAction(state.Action, state, entity, inProgressQuestWithRequirements, questAnalysis);
            QuestsStateUpdaterHelper.AddQuestAnalysisIfNeeded(analysis, questAnalysis);
        }
            
        if (state.PlayerStorage.LastSelectedDistrictId == state.District.Id)
            QuestsStateUpdaterHelper.SendUpdateQuests(analysis, state);
    }

    public static void UpdateBlackMarketQuestsState(this AnalysisState state)
    {
        var entity = state.BlackMarketEntity;
        if (!state.InitStatus || entity.IsCanceled) return;
            
        var analysis = new List<QuestWithRequirements>();
        foreach (var inProgressQuestWithRequirements in state.QuestsWithRequirements.Where(item => item.Quest.InProgress))
        {
            var analysisQuest = new QuestWithRequirements();
            var requirementsProgress = inProgressQuestWithRequirements.RequirementsProgress;
            var requirements = inProgressQuestWithRequirements.Requirements.Where(item =>
                (item.GeneratedTypeId == entity.GeneratedTypeId && string.IsNullOrWhiteSpace(item.CustomEntityName) 
                                                                && requirementsProgress[item.Id] < item.Amount)
                ||
                (item.GeneratedTypeId == entity.GeneratedTypeId && !string.IsNullOrWhiteSpace(item.CustomEntityName)
                                                                && item.CustomEntityName == entity.ItemEntityName && requirementsProgress[item.Id] < item.Amount));
                
            foreach (var req in requirements)
            {
                QuestsStateUpdaterHelper.ProcessBlackMarketUpdate(entity, inProgressQuestWithRequirements, analysisQuest, state, req);
            }

            QuestsStateUpdaterHelper.AddQuestAnalysisIfNeeded(analysis, analysisQuest);
        }
            
        if (state.PlayerStorage.LastSelectedDistrictId == state.District.Id)
            QuestsStateUpdaterHelper.SendUpdateQuests(analysis, state);
    }
}