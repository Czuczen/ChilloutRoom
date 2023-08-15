using System;
using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Comparers;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Conditions;
using CzuczenLand.ExtendingFunctionalities.General;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.QuestsStateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.QuestsStateUpdater;

/// <summary>
/// Klasa pomocnicza dla aktualizacji stanu zadań u gracza.
/// </summary>
public static class QuestsStateUpdaterHelper
{
    /// <summary>
    /// Sprawdza, czy wymaganie nie dotyczy aktualizowanej encji.
    /// </summary>
    /// <param name="req">Wymaganie do sprawdzenia.</param>
    /// <param name="state">Stan analizy, w którym ma być sprawdzane wymaganie.</param>
    /// <returns>Zwraca true, jeśli wymaganie nie dotyczy aktualizowanej encji; w przeciwnym razie false.</returns>
    private static bool NotCurrEntityReq(Requirement req, AnalysisState state)
    {
        var ret = true;
        if (state.GeneratedEntity != null)
        {
            if (!string.IsNullOrWhiteSpace(req.CustomEntityName)) // generowane typy połączone - plant, seed, dired fruit
            {
                ret = req.GeneratedTypeId != state.GeneratedEntity.GeneratedTypeId ||
                      req.CustomEntityName != state.Entity.GetType().Name;
            }
            else // zwykły generowany typ
            {
                ret = req.GeneratedTypeId != state.GeneratedEntity.GeneratedTypeId;
            }
        }
        else if (state.StorageEntity != null)
        {
            ret = req.CustomEntityName != state.Entity.GetType().Name;
        }
            
        return ret;
    }

    /// <summary>
    /// Sprawdza, czy wymaganie wymaga aktualizacji.
    /// </summary>
    /// <param name="req">Wymaganie do sprawdzenia.</param>
    /// <param name="quest">Zadanie, do którego przynależy wymaganie.</param>
    /// <param name="state">Stan analizy, w którym ma być sprawdzane wymaganie.</param>
    /// <param name="outCheckValues">Wyjściowe wartości sprawdzenia do dalszego przetwarzania.</param>
    /// <returns>Zwraca true, jeśli wymaganie wymaga aktualizacji; w przeciwnym razie false.</returns>
    public static bool ReqNeedUpdate(Requirement req, Quest quest, AnalysisState state, out CheckValues outCheckValues)
    {
        outCheckValues = null;
        
        if (NotCurrEntityReq(req, state)) return false;
        if (!AnyImportantDifference(req, state, out var checkValues)) return false;
        
        outCheckValues = checkValues;
            
        // żeby zadania odnoszące się do posiadania lub dostarczenia były aktualizowane, bez względu na to czy są rozpoczęte czy też nie
        var needUpdate = false;
        if (req.Condition == DbRequirementConditionsNames.Possession || req.Condition == DbRequirementConditionsNames.Deliver)
            needUpdate = true;
        else if (quest.InProgress)
            needUpdate = true;

        return needUpdate;
    }

    /// <summary>
    /// Przetwarza aktualizację dla wartości typu int w zależności od warunku wymagania.
    /// </summary>
    /// <param name="state">Stan analizy.</param>
    /// <param name="value">Wartość, która ma zostać zaktualizowana.</param>
    /// <param name="questAnalysis">Analiza zadania, do którego ma być dodana aktualizacja.</param>
    /// <param name="questWithRequirements">Zadanie wraz z wymaganiami.</param>
    /// <param name="req">Wymaganie, które ma być zaktualizowane.</param>
    public static void ProcessIntUpdate(AnalysisState state, int value, QuestWithRequirements questAnalysis, QuestWithRequirements questWithRequirements, Requirement req)
    {
        switch (req.Condition)
        {
            case DbRequirementConditionsNames.Possession:
            case DbRequirementConditionsNames.Deliver:
            {
                UpdateQuestReq(state, questAnalysis, questWithRequirements, req, value,
                    EnumUtils.AnalysisTypes.ProductOrStorage);
                break;
            }
            case DbRequirementConditionsNames.Increase when value > 0:
            {
                UpdateQuestReq(state, questAnalysis, questWithRequirements, req, value,
                    EnumUtils.AnalysisTypes.ProductOrStorage);
                break;
            }
            case DbRequirementConditionsNames.Decrease when value < 0:
            {
                UpdateQuestReq(state, questAnalysis, questWithRequirements, req, value,
                    EnumUtils.AnalysisTypes.ProductOrStorage);
                break;
            }
        }
    }

    /// <summary>
    /// Przetwarza aktualizację dla wartości typu decimal w zależności od warunku wymagania.
    /// </summary>
    /// <param name="value">Wartość, która ma zostać zaktualizowana.</param>
    /// <param name="questAnalysis">Analiza zadania, do którego ma być dodana aktualizacja.</param>
    /// <param name="questWithRequirements">Zadanie wraz z wymaganiami.</param>
    /// <param name="req">Wymaganie, które ma być zaktualizowane.</param>
    /// <param name="state">Stan analizy.</param>
    public static void ProcessDecimalUpdate(decimal value, QuestWithRequirements questAnalysis,
        QuestWithRequirements questWithRequirements, Requirement req, AnalysisState state)
    {
        switch (req.Condition)
        {
            case DbRequirementConditionsNames.Deliver:
            case DbRequirementConditionsNames.Possession:
            {
                UpdateQuestReq(state, questAnalysis, questWithRequirements, req, value,
                    EnumUtils.AnalysisTypes.ProductOrStorage);
                break;
            }
            case DbRequirementConditionsNames.Increase when value > 0:
            {
                if (state.NeedIgnoreChange) return;
                UpdateQuestReq(state, questAnalysis, questWithRequirements, req, value,
                    EnumUtils.AnalysisTypes.ProductOrStorage);
                break;
            }
            case DbRequirementConditionsNames.Decrease when value < 0:
            {
                if (state.NeedIgnoreChange) return;
                UpdateQuestReq(state, questAnalysis, questWithRequirements, req, value,
                    EnumUtils.AnalysisTypes.ProductOrStorage);
                break;
            }
        }
    }
        
    /// <summary>
    /// Przetwarza aktualizacje związane z roślinami na podstawie akcji.
    /// </summary>
    /// <param name="action">Akcja do przetworzenia.</param>
    /// <param name="state">Stan analizy.</param>
    /// <param name="entity">Roślina, dla której ma być dokonana aktualizacja.</param>
    /// <param name="questWithRequirements">Zadanie wraz z wymaganiami.</param>
    /// <param name="questAnalysis">Analiza zadania, do którego ma być dodana aktualizacja.</param>
    public static void ProcessPlantByAction(string action, AnalysisState state, Plant entity, QuestWithRequirements questWithRequirements, 
        QuestWithRequirements questAnalysis)
    {
        var requirements = questWithRequirements.Requirements.Where(item =>
            item.GeneratedTypeId == entity.GeneratedTypeId && item.CustomEntityName == state.Entity.GetType().Name);
        foreach (var req in requirements)
        {
            switch (action)
            {
                case UpdateActivityOperator.ActionDeleted when req.Comparer == DbComparers.CollectPlant ||
                                                               req.Comparer == DbComparers.RemovePlant:
                    ProcessDeletedPlantUpdate(state, entity, questAnalysis, questWithRequirements, req);
                    break;
                case UpdateActivityOperator.ActionCreated when req.Comparer == DbComparers.CreatePlant:
                    UpdateQuestReq(state, questAnalysis, questWithRequirements, req, 1,
                        EnumUtils.AnalysisTypes.Plant);
                    break;
            }
        }
    }

    /// <summary>
    /// Przetwarza aktualizację dla usuniętej rośliny.
    /// </summary>
    /// <param name="state">Stan analizy.</param>
    /// <param name="entity">Roślina, dla której ma być dokonana aktualizacja.</param>
    /// <param name="questAnalysis">Analiza zadania, do którego ma być dodana aktualizacja.</param>
    /// <param name="questWithRequirements">Zadanie wraz z wymaganiami.</param>
    /// <param name="req">Wymaganie, które ma być zaktualizowane.</param>
    private static void ProcessDeletedPlantUpdate(AnalysisState state, Plant entity, QuestWithRequirements questAnalysis,
        QuestWithRequirements questWithRequirements, Requirement req)
    {
        if (req.Condition == DbRequirementConditionsNames.Increase && 
            req.Comparer == DbComparers.CollectPlant && 
            entity.GrowingLevel >= 100 && entity.WiltLevel < 100)
        {
            UpdateQuestReq(state, questAnalysis, questWithRequirements, req, 1,
                EnumUtils.AnalysisTypes.Plant);
        }

        if (req.Condition == DbRequirementConditionsNames.Decrease && 
            req.Comparer == DbComparers.RemovePlant 
            && entity.WiltLevel >= 100)
        {
            UpdateQuestReq(state, questAnalysis, questWithRequirements, req, 1,
                EnumUtils.AnalysisTypes.Plant);
        }
    }

    /// <summary>
    /// Przetwarza aktualizację związaną z transakcją na czarnym rynku.
    /// Sprzedaż na czarnym rynku odbywa się, gdy rekord jest usuwany, a nie kiedy jest wystawiana transakcja. Kiedy transakcja jest wystawiana, zmiana w ilości produktu w polu "ownedAmount" jest ignorowana.
    /// Sprzedaż, zarówno zwykła jak i na czarnym rynku, jest zaliczana tylko wtedy, gdy rekord transakcji czarnego rynku zostaje usunięty. Wtedy do pola "ownedAmount" produktu dodawana jest odpowiednia ilość.
    /// Jeśli transakcja gracza jest sprzedana, ponieważ wartość "ownedAmount" produktu została zmieniona podczas wystawiania i zignorowana, to teraz zadania zarówno "sprzedaj na czarnym rynku", jak i zwykła sprzedaż są aktualizowane przez tę metodę, ponieważ reaguje ona na usunięcie rekordu transakcji.   
    /// Zwykłe kupowanie jest obsługiwane przez "UpdateProductOrStorageQuestsState", ponieważ nie ma tam opcji "ignoreChange" przy zmianie wartości. Kupowanie dodaje ilość do pola "ownedAmount" produktu i jest to wykrywane przez system.
    /// </summary>
    /// <param name="entity">Transakcja na Czarnym Rynku.</param>
    /// <param name="questWithRequirements">Zadanie wraz z wymaganiami.</param>
    /// <param name="questAnalysis">Analiza zadania, do którego ma być dodana aktualizacja.</param>
    /// <param name="state">Stan analizy.</param>
    /// <param name="req">Wymaganie, które ma być zaktualizowane.</param>
    public static void ProcessBlackMarketUpdate(BlackMarketTransaction entity, QuestWithRequirements questWithRequirements, 
        QuestWithRequirements questAnalysis, AnalysisState state, Requirement req)
    {
        if (req.Comparer == DbComparers.BuyOnBlackMarket && req.Condition == DbRequirementConditionsNames.Increase && entity.BuyerId == state.UserId)
        {
            UpdateQuestReq(state, questAnalysis, questWithRequirements, req, entity.Quantity, EnumUtils.AnalysisTypes.BlackMarket);
        }
            
        if (req.Comparer == DbComparers.SellOnBlackMarket && req.Condition == DbRequirementConditionsNames.Decrease && entity.SellerId == state.UserId)
        {
            UpdateQuestReq(state, questAnalysis, questWithRequirements, req, entity.Quantity, EnumUtils.AnalysisTypes.BlackMarket);
        }
        
        // zwykłe sprzedaj
        if (req.Comparer == DbComparers.OwnedAmount && req.Condition == DbRequirementConditionsNames.Decrease && entity.SellerId == state.UserId)  
        {
            UpdateQuestReq(state, questAnalysis, questWithRequirements, req, entity.Quantity, EnumUtils.AnalysisTypes.BlackMarket);
        }
    }

    /// <summary>
    /// Sprawdza, czy zaszła istotna zmiana dla aktualizowanej encji.
    /// </summary>
    /// <param name="req">Wymaganie do sprawdzenia.</param>
    /// <param name="state">Stan analizy, w którym ma być dokonywane sprawdzanie.</param>
    /// <param name="checkValues">Wartości sprawdzania dla dalszej obróbki.</param>
    /// <returns>Zwraca true, jeśli istnieje istotna różnica; w przeciwnym razie false.</returns>
    private static bool AnyImportantDifference(Requirement req, AnalysisState state, out CheckValues checkValues)
    {
        var ret = false;
        checkValues = new CheckValues();
        var entity = state.Entity;
        if (state.LastEntityPropertyChanges == null) return false;

        var dbParser = state.DbParser;
        var propChange = state.LastEntityPropertyChanges.FirstOrDefault(item => item.PropertyName == req.Comparer);
        if (propChange == null) return false;

        var entityType = entity.GetType();
        var prop = entityType.GetProperties().ToList()
            .SingleOrDefault(item => item.Name == propChange.PropertyName);
        if (prop == null) return false;

        if (req.Condition == DbRequirementConditionsNames.Possession
            || req.Condition == DbRequirementConditionsNames.Deliver)
        {
            var currValue = entityType.GetProperty(req.Comparer)?.GetValue(entity);
            var parsedValue = dbParser.Parse(prop, currValue);
            switch (parsedValue)
            {
                case int parsedIntValue:
                    checkValues.Int = parsedIntValue;
                    break;
                case decimal parsedDecimalValue:
                    checkValues.Decimal = parsedDecimalValue;
                    break;
            }

            ret = true;
        }
        else
        {
            var newValue = propChange.NewValue;
            var oldValue = !string.IsNullOrWhiteSpace(propChange.OriginalValue)
                ? propChange.OriginalValue
                : "0";

            var parsedOldValue = dbParser.Parse(prop, oldValue);
            var parsedNewValue = dbParser.Parse(prop, newValue);

            switch (parsedOldValue)
            {
                case int parsedIntOldValue when parsedNewValue is int parsedIntNewValue:
                    checkValues.Int = parsedIntNewValue - parsedIntOldValue;
                    break;
                case decimal parsedDecimalOldValue when parsedNewValue is decimal parsedDecimalNewValue:
                    checkValues.Decimal = parsedDecimalNewValue - parsedDecimalOldValue;
                    break;
            }

            if ((checkValues.Int != null && checkValues.Int != 0) || (checkValues.Decimal != null && checkValues.Decimal != 0))
                ret = true;
        }
            
        return ret;
    }

    /// <summary>
    /// Wysyła aktualizacje zadań do klienta poprzez huba.
    /// </summary>
    /// <param name="analysis">Lista analiz zadań do aktualizacji.</param>
    /// <param name="state">Stan analizy.</param>
    public static void SendUpdateQuests(List<QuestWithRequirements> analysis, AnalysisState state)
    {
        if (!analysis.Any()) return;
            
        var data = GenerateUpdateModel(analysis, state.QuestsWithRequirements);
        state.QuestHub.Clients.User(state.UserId.ToString()).updateQuests(data);
    }

    /// <summary>
    /// Generuje model aktualizacji zadań.
    /// </summary>
    /// <param name="updatedQuests">Zaktualizowane analizy zadań.</param>
    /// <param name="allQuestsWithAllRequirements">Lista wszystkich zadań wraz z wymaganiami.</param>
    /// <returns>Lista modeli do aktualizacji zadań.</returns>
    private static List<UpdateQuest> GenerateUpdateModel(List<QuestWithRequirements> updatedQuests, List<QuestWithRequirements> allQuestsWithAllRequirements)
    {
        var updatingList = new List<UpdateQuest>();
        foreach (var questWithUpdatedRequirements in updatedQuests)
        {
            var updatingQuest = new UpdateQuest
            {
                QuestId = questWithUpdatedRequirements.Quest.Id,
                InProgress = questWithUpdatedRequirements.Quest.InProgress
            };
                
            var requirementsProgressDict = allQuestsWithAllRequirements.Single(item => item.Quest.Id == questWithUpdatedRequirements.Quest.Id).RequirementsProgress;
            foreach (var req in questWithUpdatedRequirements.Requirements)
            {
                var updatingReq = new UpdateRequirement
                {
                    RequirementId = req.Id,
                    RequirementProgressText = requirementsProgressDict[req.Id] + "/" + req.Amount,
                    RequirementProgressPercentage = (requirementsProgressDict[req.Id] / req.Amount * 100 + "%").Replace(",", ".") 
                };
                    
                updatingQuest.UpdatingRequirements.Add(updatingReq);
            }

            updatingQuest.QuestIsComplete = allQuestsWithAllRequirements
                .Single(item => item.Quest.Id == questWithUpdatedRequirements.Quest.Id).Requirements
                .All(item => requirementsProgressDict[item.Id] >= item.Amount);
                
            updatingList.Add(updatingQuest);
        }

        return updatingList;
    }

    /// <summary>
    /// Dodaje analizę zadania do listy analiz, jeśli analiza istnieje.
    /// </summary>
    /// <param name="analysis">Lista analiz zadań do uaktualnienia.</param>
    /// <param name="questAnalysis">Analiza zadania do dodania.</param>
    public static void AddQuestAnalysisIfNeeded(List<QuestWithRequirements> analysis, QuestWithRequirements questAnalysis)
    {
        if (questAnalysis.Quest == null) return;
        analysis.Add(questAnalysis);
    }

    /// <summary>
    /// Aktualizuje wymaganie zadania w analizie.
    /// </summary>
    /// <param name="state">Stan analizy.</param>
    /// <param name="questAnalysis">Analiza zadania do aktualizacji.</param>
    /// <param name="questWithRequirements">Zadanie wraz z wymaganiami.</param>
    /// <param name="req">Wymaganie do aktualizacji.</param>
    /// <param name="value">Wartość, o którą ma być zaktualizowane wymaganie.</param>
    /// <param name="analysisType">Typ analizy.</param>
    private static void UpdateQuestReq(AnalysisState state, QuestWithRequirements questAnalysis, QuestWithRequirements questWithRequirements, Requirement req, 
        decimal value, EnumUtils.AnalysisTypes analysisType)
    {
        var quest = questWithRequirements.Quest;
        var requirementsProgress = questWithRequirements.RequirementsProgress;
        var currAmountRequirement = requirementsProgress[req.Id];
            
        if (req.Condition != DbRequirementConditionsNames.Deliver && req.Condition != DbRequirementConditionsNames.Possession &&
            currAmountRequirement == req.Amount) return;
            
        questAnalysis.Quest = quest;
        questAnalysis.Requirements.Add(req);
            
        switch (analysisType)
        {
            case EnumUtils.AnalysisTypes.ProductOrStorage:
                switch (req.Condition)
                {
                    case DbRequirementConditionsNames.Deliver:
                    case DbRequirementConditionsNames.Possession:
                        requirementsProgress[req.Id] = value > req.Amount ? req.Amount : value;
                        break;
                    case DbRequirementConditionsNames.Increase:
                        var increaseCurrAmount = currAmountRequirement + value;
                        requirementsProgress[req.Id] = increaseCurrAmount > req.Amount ? req.Amount : increaseCurrAmount;
                        break;
                    case DbRequirementConditionsNames.Decrease:
                        var decreaseCurrAmount = currAmountRequirement + value * -1;
                        requirementsProgress[req.Id] = decreaseCurrAmount > req.Amount ? req.Amount : decreaseCurrAmount;
                        break;
                }
                break;
            case EnumUtils.AnalysisTypes.Plant:
                requirementsProgress[req.Id] += value; 
                break;
            case EnumUtils.AnalysisTypes.BlackMarket:
                var currAmount = currAmountRequirement + value;
                requirementsProgress[req.Id] = currAmount > req.Amount ? req.Amount : currAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(analysisType), analysisType, null);
        }

        questWithRequirements.QuestRequirementsProgress.RequirementsProgress = JsonConvert.SerializeObject(requirementsProgress, new DecimalFormatConverter());
        state.QuestRequirementsProgressRepository.Update(questWithRequirements.QuestRequirementsProgress);
    }
}