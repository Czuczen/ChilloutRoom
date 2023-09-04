using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Conditions;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.CustomEntity;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;

/// <summary>
/// Klasa pomocnicza do przetwarzania zależności pól dla testów struktury.
/// </summary>
public static class FieldDependenciesTestsHelper
{
    /// <summary>
    /// Przetwarza zależność ilości posiadanych w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista dostępnych typów generowanych.</param>
    public static void ProcessRequirementOwnedAmountComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId != null)
        {
            var reqGeneratedType = generatedTypes.FirstOrDefault(item => item.Id == req.GeneratedTypeId);
            if (reqGeneratedType != null)
            {
                var conditionTest = new MinorTest();
                var customEntityTest = new MinorTest();
                var amountTest = new MinorTest();
                var districtTest = new MinorTest();

                if (SelectListLoaderHelper.RequirementConditionsNamesDbToHr.Any(item => item.Key == req.Condition))
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                else
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                
                if (reqGeneratedType.EntityName == EntitiesDbNames.Quest)
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    generatedTypeTest.Description = "Nie można wymagać posiadania ilości zadań. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    generatedTypeTest.Description = "Typ generowany jest prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                }
                
                if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName != DbCustomEntityNames.Plant && req.CustomEntityName is DbCustomEntityNames.Seed or DbCustomEntityNames.DriedFruit or DbCustomEntityNames.Plant && reqGeneratedType.EntityName == EntitiesDbNames.Seed)
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(req.CustomEntityName) && reqGeneratedType.EntityName is not DbCustomEntityNames.Seed or DbCustomEntityNames.DriedFruit or DbCustomEntityNames.Plant)
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                        customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                    }
                    else
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                    }
                }

                var isInt = IntUtils.IsInt(req.Amount);
                if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is not DbCustomEntityNames.DriedFruit 
                    && ((!isInt && reqGeneratedType.EntityName is EntitiesDbNames.Lamp or EntitiesDbNames.Pot or EntitiesDbNames.Seed or EntitiesDbNames.Bonus) 
                    || (!isInt && !string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.Seed)))
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer + ". Encja niestandardowa wymagania - " + req.CustomEntityName;
                }
                else
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer + ". Encja niestandardowa wymagania - " + req.CustomEntityName;
                }
                
                if (reqGeneratedType.DistrictId == req.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                
                structureTest.MinorTests.Add(conditionTest);
                structureTest.MinorTests.Add(customEntityTest);
                structureTest.MinorTests.Add(amountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeTest.Description = "Typ generowany nie istnieje. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
            }
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany jest wymagany. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
        }
        
        structureTest.MinorTests.Add(generatedTypeTest);
    }
    
    /// <summary>
    /// Przetwarza zależność poziomu w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementLevelComparer(Requirement req, StructureTest structureTest)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId == null)
        {
            var conditionTest = new MinorTest();
            var customEntityTest = new MinorTest();
            var amountTest = new MinorTest();

            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            generatedTypeTest.Description = "Typ generowany nie ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;

            if (req.Condition is DbRequirementConditionsNames.Increase or DbRequirementConditionsNames.Possession)
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }
            else
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }

            if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.PlantationStorage or DbCustomEntityNames.PlayerStorage)
            {
                customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;   
                }
            }

            var isInt = IntUtils.IsInt(req.Amount);
            if (!isInt)
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }
            else
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }

            structureTest.MinorTests.Add(conditionTest);
            structureTest.MinorTests.Add(customEntityTest);
            structureTest.MinorTests.Add(amountTest);
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany nie powinien być ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
        }
        
        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność zdobytego doświadczenia w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementGainedExperienceComparer(Requirement req, StructureTest structureTest)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId == null)
        {
            var conditionTest = new MinorTest();
            var customEntityTest = new MinorTest();
            
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            generatedTypeTest.Description = "Typ generowany nie ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
            
            if (req.Condition is DbRequirementConditionsNames.Increase or DbRequirementConditionsNames.Possession)
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }
            else
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }

            if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.PlantationStorage or DbCustomEntityNames.PlayerStorage)
            {
                customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;   
                }
            }

            structureTest.MinorTests.Add(conditionTest);
            structureTest.MinorTests.Add(customEntityTest);
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany nie powinien być ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność złota w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementGoldComparer(Requirement req, StructureTest structureTest)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId == null)
        {
            var conditionTest = new MinorTest();
            var customEntityTest = new MinorTest();
            
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            generatedTypeTest.Description = "Typ generowany nie ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
            
            if (SelectListLoaderHelper.RequirementConditionsNamesDbToHr.Any(item => item.Key == req.Condition))
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }
            else
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }

            if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.PlantationStorage or DbCustomEntityNames.PlayerStorage)
            {
                customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;   
                }
            }
            
            structureTest.MinorTests.Add(conditionTest);
            structureTest.MinorTests.Add(customEntityTest);
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany nie powinien być ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność prestiżu w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementPrestigeComparer(Requirement req, StructureTest structureTest)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId == null)
        {
            var conditionTest = new MinorTest();
            var customEntityTest = new MinorTest();
            var amountTest = new MinorTest();
            
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            generatedTypeTest.Description = "Typ generowany nie ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;

            if (SelectListLoaderHelper.RequirementConditionsNamesDbToHr.Any(item => item.Key == req.Condition))
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }
            else
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }

            if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.PlantationStorage)
            {
                customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;   
                }
            }
            
            var isInt = IntUtils.IsInt(req.Amount);
            if (!isInt)
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }
            else
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }
            
            structureTest.MinorTests.Add(conditionTest);
            structureTest.MinorTests.Add(customEntityTest);
            structureTest.MinorTests.Add(amountTest);
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany nie powinien być ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność ukończonej ilości w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista generowanych typów.</param>
    public static void ProcessRequirementCompletedAmountComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId != null)
        {
            var reqGeneratedType = generatedTypes.FirstOrDefault(item => item.Id == req.GeneratedTypeId);
            if (reqGeneratedType != null)
            {
                var conditionTest = new MinorTest();
                var customEntityTest = new MinorTest();
                var amountTest = new MinorTest();
                var districtTest = new MinorTest();
                
                if (req.Condition is DbRequirementConditionsNames.Increase or DbRequirementConditionsNames.Possession)
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                else
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                
                if (reqGeneratedType.EntityName != EntitiesDbNames.Quest)
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    generatedTypeTest.Description = "Ilość ukończonych jest przeznaczona tylko dla zadania. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    generatedTypeTest.Description = "Typ generowany jest prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                }
                
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }

                var isInt = IntUtils.IsInt(req.Amount);
                if (!isInt)
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                
                if (reqGeneratedType.DistrictId == req.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                
                structureTest.MinorTests.Add(conditionTest);
                structureTest.MinorTests.Add(customEntityTest);
                structureTest.MinorTests.Add(amountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeTest.Description = "Typ generowany nie istnieje. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
            }
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany jest wymagany. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność użycia w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista generowanych typów.</param>
    public static void ProcessRequirementUsagesComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId != null)
        {
            var reqGeneratedType = generatedTypes.FirstOrDefault(item => item.Id == req.GeneratedTypeId);
            if (reqGeneratedType != null)
            {
                var conditionTest = new MinorTest();
                var customEntityTest = new MinorTest();
                var amountTest = new MinorTest();
                var districtTest = new MinorTest();
                
                if (req.Condition is DbRequirementConditionsNames.Increase or DbRequirementConditionsNames.Possession)
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                else
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                
                if (reqGeneratedType.EntityName != EntitiesDbNames.Bonus)
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    generatedTypeTest.Description = "Użycia są przeznaczone tylko dla bonusu. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    generatedTypeTest.Description = "Typ generowany jest prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                }
                
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }

                var isInt = IntUtils.IsInt(req.Amount);
                if (!isInt)
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                
                if (reqGeneratedType.DistrictId == req.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                
                structureTest.MinorTests.Add(conditionTest);
                structureTest.MinorTests.Add(customEntityTest);
                structureTest.MinorTests.Add(amountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeTest.Description = "Typ generowany nie istnieje. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
            }
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany jest wymagany. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
        }
        
        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność zbierania rośliny w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista generowanych typów.</param>
    public static void ProcessRequirementCollectPlantComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId != null)
        {
            var reqGeneratedType = generatedTypes.FirstOrDefault(item => item.Id == req.GeneratedTypeId);
            if (reqGeneratedType != null)
            {
                var conditionTest = new MinorTest();
                var customEntityTest = new MinorTest();
                var amountTest = new MinorTest();
                var districtTest = new MinorTest();

                if (req.Condition is DbRequirementConditionsNames.Increase)
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                else
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                
                if (reqGeneratedType.EntityName != EntitiesDbNames.Seed)
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    generatedTypeTest.Description = "Zbierz roślinę jest przeznaczone tylko dla typu nasiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    generatedTypeTest.Description = "Typ generowany jest prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                }
                
                if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.Plant && reqGeneratedType.EntityName == EntitiesDbNames.Seed)
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                    }
                    else
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                    }
                }

                var isInt = IntUtils.IsInt(req.Amount);
                if (!isInt)
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                
                if (reqGeneratedType.DistrictId == req.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                
                structureTest.MinorTests.Add(conditionTest);
                structureTest.MinorTests.Add(customEntityTest);
                structureTest.MinorTests.Add(amountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeTest.Description = "Typ generowany nie istnieje. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
            }
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany jest wymagany. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność usuwania rośliny w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista generowanych typów.</param>
    public static void ProcessRequirementRemovePlantComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId != null)
        {
            var reqGeneratedType = generatedTypes.FirstOrDefault(item => item.Id == req.GeneratedTypeId);
            if (reqGeneratedType != null)
            {
                var conditionTest = new MinorTest();
                var customEntityTest = new MinorTest();
                var amountTest = new MinorTest();
                var districtTest = new MinorTest();
                
                if (req.Condition is DbRequirementConditionsNames.Decrease)
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                else
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                
                if (reqGeneratedType.EntityName != EntitiesDbNames.Seed)
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    generatedTypeTest.Description = "Zbierz roślinę jest przeznaczone tylko dla typu nasiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    generatedTypeTest.Description = "Typ generowany jest prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                }

                if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.Plant && reqGeneratedType.EntityName == EntitiesDbNames.Seed)
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                    }
                    else
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                    }
                }

                var isInt = IntUtils.IsInt(req.Amount);
                if (!isInt)
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                
                if (reqGeneratedType.DistrictId == req.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                
                structureTest.MinorTests.Add(conditionTest);
                structureTest.MinorTests.Add(customEntityTest);
                structureTest.MinorTests.Add(amountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeTest.Description = "Typ generowany nie istnieje. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
            }
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany jest wymagany. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność tworzenia rośliny w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista generowanych typów.</param>
    public static void ProcessRequirementCreatePlantComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        ProcessRequirementCollectPlantComparer(req, structureTest, generatedTypes);
    }

    /// <summary>
    /// Przetwarza zależność sprzedaży na czarnym rynku w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista generowanych typów.</param>
    public static void ProcessRequirementSellOnBlackMarketComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId != null)
        {
            var reqGeneratedType = generatedTypes.FirstOrDefault(item => item.Id == req.GeneratedTypeId);
            if (reqGeneratedType != null)
            {
                var conditionTest = new MinorTest();
                var customEntityTest = new MinorTest();
                var amountTest = new MinorTest();
                var districtTest = new MinorTest();
                
                if (req.Condition is DbRequirementConditionsNames.Decrease)
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                else
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                
                if (reqGeneratedType.EntityName == EntitiesDbNames.Quest)
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    generatedTypeTest.Description = "Nie można wystawiać zadań na czarnym rynku. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    generatedTypeTest.Description = "Typ generowany jest prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                }
                
                if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.Seed or DbCustomEntityNames.DriedFruit && reqGeneratedType.EntityName == EntitiesDbNames.Seed)
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(req.CustomEntityName) && reqGeneratedType.EntityName is not DbCustomEntityNames.Seed or DbCustomEntityNames.DriedFruit)
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                        customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                    }
                    else
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                    }
                }

                var isInt = IntUtils.IsInt(req.Amount);
                if (!isInt && reqGeneratedType.EntityName is EntitiesDbNames.Lamp or EntitiesDbNames.Pot or EntitiesDbNames.Seed or EntitiesDbNames.Bonus)
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                
                if (reqGeneratedType.DistrictId == req.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                
                structureTest.MinorTests.Add(conditionTest);
                structureTest.MinorTests.Add(customEntityTest);
                structureTest.MinorTests.Add(amountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeTest.Description = "Typ generowany nie istnieje. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
            }
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany jest wymagany. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
        }
        
        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność zakupu na czarnym rynku w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista generowanych typów.</param>
    public static void ProcessRequirementBuyOnBlackMarketComparer(Requirement req, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId != null)
        {
            var reqGeneratedType = generatedTypes.FirstOrDefault(item => item.Id == req.GeneratedTypeId);
            if (reqGeneratedType != null)
            {
                var conditionTest = new MinorTest();
                var customEntityTest = new MinorTest();
                var amountTest = new MinorTest();
                var districtTest = new MinorTest();
                
                if (req.Condition is DbRequirementConditionsNames.Increase)
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                else
                {
                    conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
                }
                
                if (reqGeneratedType.EntityName == EntitiesDbNames.Quest)
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    generatedTypeTest.Description = "Nie można wystawiać zadań na czarnym rynku. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    generatedTypeTest.Description = "Typ generowany jest prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                }
                
                if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.Seed or DbCustomEntityNames.DriedFruit && reqGeneratedType.EntityName == EntitiesDbNames.Seed)
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(req.CustomEntityName) && reqGeneratedType.EntityName is not DbCustomEntityNames.Seed or DbCustomEntityNames.DriedFruit)
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                        customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;
                    }
                    else
                    {
                        customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Porównanie - " + req.Comparer;   
                    }
                }

                var isInt = IntUtils.IsInt(req.Amount);
                if (!isInt && reqGeneratedType.EntityName is EntitiesDbNames.Lamp or EntitiesDbNames.Pot or EntitiesDbNames.Seed or EntitiesDbNames.Bonus)
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Nazwa encji typu generowanego - " + reqGeneratedType.EntityName + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
                }
                
                if (reqGeneratedType.DistrictId == req.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id dzielnicy dla wymagania: " + req.DistrictId + ". Typ generowany - " + reqGeneratedType.Name + ". Id typu generowanego: " + reqGeneratedType.Id + ". Id dzielnicy dla typu generowanego: " + reqGeneratedType.DistrictId + ". Porównanie - " + req.Comparer;
                }
                
                structureTest.MinorTests.Add(conditionTest);
                structureTest.MinorTests.Add(customEntityTest);
                structureTest.MinorTests.Add(amountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeTest.Description = "Typ generowany nie istnieje. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
            }
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany jest wymagany. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność żetonu zadania w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementQuestTokenComparer(Requirement req, StructureTest structureTest)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId == null)
        {
            var conditionTest = new MinorTest();
            var customEntityTest = new MinorTest();
            var amountTest = new MinorTest();
            
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            generatedTypeTest.Description = "Typ generowany nie ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
            
            if (SelectListLoaderHelper.RequirementConditionsNamesDbToHr.Any(item => item.Key == req.Condition))
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }
            else
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }

            if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.PlantationStorage)
            {
                customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;   
                }
            }
            
            var isInt = IntUtils.IsInt(req.Amount);
            if (!isInt)
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }
            else
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }
            
            structureTest.MinorTests.Add(conditionTest);
            structureTest.MinorTests.Add(customEntityTest);
            structureTest.MinorTests.Add(amountTest);
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany nie powinien być ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależność żetonu dealera w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementDealerTokenComparer(Requirement req, StructureTest structureTest)
    {
        ProcessRequirementQuestTokenComparer(req, structureTest);
    }

    /// <summary>
    /// Przetwarza zależność żetonu czarnego rynku w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementBlackMarketTokenComparer(Requirement req, StructureTest structureTest)
    {
        ProcessRequirementQuestTokenComparer(req, structureTest);
    }

    /// <summary>
    /// Przetwarza zależność żetonu Dona w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementDonTokenComparer(Requirement req, StructureTest structureTest)
    {
        ProcessRequirementQuestTokenComparer(req, structureTest);
    }

    /// <summary>
    /// Przetwarza zależność żetonu odblokowania w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementUnlockTokenComparer(Requirement req, StructureTest structureTest)
    {
        ProcessRequirementQuestTokenComparer(req, structureTest);
    }

    /// <summary>
    /// Przetwarza zależność honoru w testach wymagań.
    /// </summary>
    /// <param name="req">Wymaganie, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessRequirementHonorComparer(Requirement req, StructureTest structureTest)
    {
        var generatedTypeTest = new MinorTest();

        if (req.GeneratedTypeId == null)
        {
            var conditionTest = new MinorTest();
            var customEntityTest = new MinorTest();
            var amountTest = new MinorTest();
            
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            generatedTypeTest.Description = "Typ generowany nie ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
            
            if (SelectListLoaderHelper.RequirementConditionsNamesDbToHr.Any(item => item.Key == req.Condition))
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                conditionTest.Description = "Warunek prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }
            else
            {
                conditionTest.Status = EnumUtils.StructureTestsStatuses.Error;
                conditionTest.Description = "Warunek nie prawidłowy. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer + ". Warunek - " + req.Condition;
            }

            if (!string.IsNullOrWhiteSpace(req.CustomEntityName) && req.CustomEntityName is DbCustomEntityNames.PlayerStorage)
            {
                customEntityTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                customEntityTest.Description = "Encja niestandardowa prawidłowa. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(req.CustomEntityName))
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa nie ustawiona. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Porównanie - " + req.Comparer;
                }
                else
                {
                    customEntityTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    customEntityTest.Description = "Encja niestandardowa ustawiona błędnie. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Encja niestandardowa wymagania - " + req.CustomEntityName + ". Porównanie - " + req.Comparer;   
                }
            }
            
            var isInt = IntUtils.IsInt(req.Amount);
            if (!isInt)
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                amountTest.Description = "Ilość ustawiona nie prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }
            else
            {
                amountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                amountTest.Description = "Ilość ustawiona prawidłowo. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Ilość na wymaganiu: " + req.Amount + ". Porównanie - " + req.Comparer;
            }

            structureTest.MinorTests.Add(conditionTest);
            structureTest.MinorTests.Add(customEntityTest);
            structureTest.MinorTests.Add(amountTest);
        }
        else
        {
            generatedTypeTest.Status = EnumUtils.StructureTestsStatuses.Error;
            generatedTypeTest.Description = "Typ generowany nie powinien być ustawiony. Wymaganie - " + req.Name + ". Id wymagania: " + req.Id + ". Id typu generowanego: " + req.GeneratedTypeId + ". Porównanie - " + req.Comparer;
        }

        structureTest.MinorTests.Add(generatedTypeTest);
    }

    /// <summary>
    /// Przetwarza zależności pól nagrody w testach struktury.
    /// </summary>
    /// <param name="fieldsHeaveValues">Słownik określający, czy pola mają wartości.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="drop">Nagroda, dla której sprawdzane są zależności.</param>
    /// <param name="generatedTypes">Lista dostępnych typów generowanych.</param>
    public static void ProcessDropFieldDependencies(Dictionary<string, bool> fieldsHeaveValues, StructureTest structureTest, Drop drop, List<GeneratedType> generatedTypes)
    {
        if (!fieldsHeaveValues.Any(item => item.Value))
        {
            var dropNotHeaveAnyReward = new MinorTest();
            dropNotHeaveAnyReward.Status = EnumUtils.StructureTestsStatuses.Error;
            dropNotHeaveAnyReward.Description = "Żadna nagroda nie została ustawiona" + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            structureTest.MinorTests.Add(dropNotHeaveAnyReward);
            return;
        }

        if (fieldsHeaveValues[EntitiesDbNames.GeneratedType])
        {
            var generatedTypeSetTest = new MinorTest();
            var generatedTypeExistTest = new MinorTest();

            var generatedType = generatedTypes.FirstOrDefault(item => item.Id == drop.GeneratedTypeId);
            if (generatedType != null)
            {
                var itemAmountTest = new MinorTest();
                var districtTest = new MinorTest();
                
                generatedTypeExistTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                generatedTypeExistTest.Description = "Typ generowany nagrody istnieje. Typ generowany - " + generatedType.Name + ". Id typu generowanego: " + generatedType.Id + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;

                var isInt = false;
                if (drop.ItemAmount != null)
                    isInt = IntUtils.IsInt(drop.ItemAmount);

                if (drop.ItemAmount == null)
                {
                    itemAmountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    itemAmountTest.Description = "Ilość przedmiotu nie jest ustawiona" + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
                }
                else if (generatedType.EntityName == EntitiesDbNames.Quest)
                {
                    if (!isInt)
                    {
                        itemAmountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        itemAmountTest.Description = "Ilość przedmiotu nie jest liczbą całkowitą. Ilość: " + drop.ItemAmount + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
                    }
                    else if (drop.ItemAmount != 1)
                    {
                        itemAmountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        itemAmountTest.Description = "Ilość przedmiotu dla zadania musi wynosić 1. Ilość: " + drop.ItemAmount + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
                    }
                    else
                    {
                        itemAmountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                        itemAmountTest.Description = "Ilość ustawiona prawidłowo. " + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
                    }
                }
                else
                {
                    if (!isInt && generatedType.EntityName is EntitiesDbNames.Lamp or EntitiesDbNames.Pot or EntitiesDbNames.Seed or EntitiesDbNames.Bonus)
                    {
                        itemAmountTest.Status = EnumUtils.StructureTestsStatuses.Error;
                        itemAmountTest.Description = "Ilość przedmiotu nie jest liczbą całkowitą. Ilość: " + drop.ItemAmount + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
                    }
                    else
                    {
                        itemAmountTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                        itemAmountTest.Description = "Ilość ustawiona prawidłowo. " + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;   
                    }
                }

                if (generatedType.DistrictId == drop.DistrictId)
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                    districtTest.Description = "Dzielnica ustawiona prawidłowo" + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id + ". Typ generowany - " + generatedType.Name + ". Id typu generowanego: " + generatedType.Id + ". Id dzielnicy typu generowanego: " + generatedType.DistrictId + ". Id dzielnicy nagrody: " + drop.DistrictId;
                }
                else
                {
                    districtTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    districtTest.Description = "Dzielnica ustawiona nie prawidłowo" + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id + ". Typ generowany - " + generatedType.Name + ". Id typu generowanego: " + generatedType.Id + ". Id dzielnicy typu generowanego: " + generatedType.DistrictId + ". Id dzielnicy nagrody: " + drop.DistrictId;
                }
                
                structureTest.MinorTests.Add(itemAmountTest);
                structureTest.MinorTests.Add(districtTest);
            }
            else
            {
                generatedTypeExistTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeExistTest.Description = "Typ generowany nagrody nie istnieje. " + ". Id typu generowanego: " + drop.GeneratedTypeId + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }

            if (fieldsHeaveValues.Any(item => item.Value && item.Key != EntitiesDbNames.GeneratedType))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != EntitiesDbNames.GeneratedType)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                generatedTypeSetTest.Status = EnumUtils.StructureTestsStatuses.Error;
                generatedTypeSetTest.Description = "Typ generowany ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id + ". Id typu gnerowanego: " + drop.GeneratedTypeId;
            }
            else
            {
                generatedTypeSetTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                generatedTypeSetTest.Description = "Typ generowany ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id + ". Id typu gnerowanego: " + drop.GeneratedTypeId;
            }

            structureTest.MinorTests.Add(generatedTypeSetTest);
            structureTest.MinorTests.Add(generatedTypeExistTest);
        }
        else if (drop.ItemAmount != null)
        {
            var itemAmountIsSet = new MinorTest();
            itemAmountIsSet.Status = EnumUtils.StructureTestsStatuses.Error;
            itemAmountIsSet.Description = "Ilość przedmiotu ustawiona" + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id + ". Ilość przedmiotu: " + drop.ItemAmount;
            structureTest.MinorTests.Add(itemAmountIsSet);
        }

        if (fieldsHeaveValues[PlantationStorageObservedFields.Gold])
        {
            var goldTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.Gold))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.Gold)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                goldTest.Status = EnumUtils.StructureTestsStatuses.Error;
                goldTest.Description = "Złoto ustawione. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                goldTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                goldTest.Description = "Złoto ustawione. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(goldTest);
        }
        
        if (fieldsHeaveValues[PlantationStorageObservedFields.Prestige])
        {
            var prestigeTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.Prestige))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.Prestige)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                prestigeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                prestigeTest.Description = "Prestiż ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                prestigeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                prestigeTest.Description = "Prestiż ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(prestigeTest);
        }
        
        if (fieldsHeaveValues[PlantationStorageObservedFields.QuestToken])
        {
            var questTokenTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.QuestToken))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.QuestToken)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                questTokenTest.Status = EnumUtils.StructureTestsStatuses.Error;
                questTokenTest.Description = "Żeton zadania ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                questTokenTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                questTokenTest.Description = "Żeton zadania ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(questTokenTest);
        }
        
        if (fieldsHeaveValues[PlantationStorageObservedFields.DealerToken])
        {
            var dealerTokenTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.DealerToken))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.DealerToken)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                dealerTokenTest.Status = EnumUtils.StructureTestsStatuses.Error;
                dealerTokenTest.Description = "Żeton dealera ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                dealerTokenTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                dealerTokenTest.Description = "Żeton dealera ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(dealerTokenTest);
        }
        
        if (fieldsHeaveValues[PlantationStorageObservedFields.BlackMarketToken])
        {
            var blackMarketTokenTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.BlackMarketToken))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.BlackMarketToken)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                blackMarketTokenTest.Status = EnumUtils.StructureTestsStatuses.Error;
                blackMarketTokenTest.Description = "Żeton czarnego rynku ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                blackMarketTokenTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                blackMarketTokenTest.Description = "Żeton czarnego rynku ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(blackMarketTokenTest);
        }
        
        if (fieldsHeaveValues[PlantationStorageObservedFields.DonToken])
        {
            var donTokenTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.DonToken))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.DonToken)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                donTokenTest.Status = EnumUtils.StructureTestsStatuses.Error;
                donTokenTest.Description = "Żeton don'a ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                donTokenTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                donTokenTest.Description = "Żeton don'a ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(donTokenTest);
        }
        
        if (fieldsHeaveValues[PlantationStorageObservedFields.UnlockToken])
        {
            var unlockTokenTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.UnlockToken))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.UnlockToken)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                unlockTokenTest.Status = EnumUtils.StructureTestsStatuses.Error;
                unlockTokenTest.Description = "Żeton odblokowania ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                unlockTokenTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                unlockTokenTest.Description = "Żeton odblokowania ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(unlockTokenTest);
        }
        
        if (fieldsHeaveValues[PlayerStorageObservedFields.Honor])
        {
            var honorTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlayerStorageObservedFields.Honor))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlayerStorageObservedFields.Honor)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                honorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                honorTest.Description = "Honor ustawiony. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                honorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                honorTest.Description = "Honor ustawiony. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(honorTest);
        }
        
        if (fieldsHeaveValues[PlantationStorageObservedFields.GainedExperience])
        {
            var gainedExperienceTest = new MinorTest();
            
            if (fieldsHeaveValues.Any(item => item.Value && item.Key != PlantationStorageObservedFields.GainedExperience))
            {
                var notAllowedFields = fieldsHeaveValues
                    .Where(item => item.Value && item.Key != PlantationStorageObservedFields.GainedExperience)
                    .Select(item => item.Key);
                var fieldsNames = string.Join(", ", notAllowedFields.Select(item => item));
                gainedExperienceTest.Status = EnumUtils.StructureTestsStatuses.Error;
                gainedExperienceTest.Description = "Doświadczenie ustawione. Inne wartości ustawione - " + fieldsNames + ". Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            else
            {
                gainedExperienceTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                gainedExperienceTest.Description = "Doświadczenie ustawione. Inne wartości nie ustawione." + " Nagroda - " + drop.Name + ". Id nagrody: " + drop.Id;
            }
            
            structureTest.MinorTests.Add(gainedExperienceTest);
        }
    }

    /// <summary>
    /// Przetwarza zależności pola ilości posiadanych produktów w testach struktury.
    /// </summary>
    /// <param name="product">Produkt, dla którego sprawdzane są zależności.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    /// <param name="generatedTypes">Lista dostępnych typów generowanych.</param>
    public static void ProcessProductOwnedAmountFieldDependencies(Product product, StructureTest structureTest, List<GeneratedType> generatedTypes)
    {
        var minorTest1 = new MinorTest();
        
        var generatedType = generatedTypes.FirstOrDefault(item => item.Id == product.GeneratedTypeId);
        if (generatedType != null)
        {
            minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
            minorTest1.Description = "Typ generowany produktu istnieje. Typ generowany - " + generatedType.Name + ". Id typu generowanego: " + generatedType.Id + ". Produkt - " + product.Name + ". Id produktu: " + product.Id;

            var minorTest2 = new MinorTest();
            var isInt = IntUtils.IsInt(product.OwnedAmount);
            
            if (!isInt &&  generatedType.EntityName is EntitiesDbNames.Lamp or EntitiesDbNames.Pot or EntitiesDbNames.Seed or EntitiesDbNames.Bonus)
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest2.Description = "Posiadana ilość nie jest liczbą całkowitą. Ilość: " + product.OwnedAmount + ". Produkt - " + product.Name + ". Id produktu: " + product.Id;
            }
            else
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest2.Description = "Posiadana ilość jest prawidłowa. Ilość: " + product.OwnedAmount + ". Produkt - " + product.Name + ". Id produktu: " + product.Id;
            }
            
            structureTest.MinorTests.Add(minorTest2);
        }
        else
        {
            minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
            minorTest1.Description = "Typ generowany produktu nie istnieje. " + "Id typu generowanego: " + product.GeneratedTypeId + ". Produkt - " + product.Name + ". Id produktu: " + product.Id;
        }

        structureTest.MinorTests.Add(minorTest1);
    }

    /// <summary>
    /// Przetwarza zależności pól cen produktu w testach struktury.
    /// </summary>
    /// <param name="product">Produkt, dla którego sprawdzane są zależności cen.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessProductPricesForShopItemFieldDependencies(Product product, StructureTest structureTest)
    {
        var minorTest = new MinorTest();
        
        if (product.IsShopItem)
        {
            if (product.SellPrice != null && product.BuyPrice != null)
            {
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest.Description = "Produkt jest dostępny w sklepie. Cena kupna i sprzedaży ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
            }
            else
            {
                if (product.SellPrice == null && product.BuyPrice != null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Produkt jest dostępny w sklepie. Cena kupna ustawiona ale cena sprzedaży nie jest ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
                else if (product.SellPrice != null && product.BuyPrice == null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Produkt jest dostępny w sklepie. Cena sprzedaży ustawiona ale cena kupna nie jest ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Produkt jest dostępny w sklepie. Cena kupna i sprzedaży nie ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
            }
        }
        else
        {
            if (product.SellPrice == null && product.BuyPrice == null)
            {
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest.Description = "Produkt nie jest dostępny w sklepie. Cena kupna i sprzedaży nie ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
            }
            else
            {
                if (product.SellPrice == null && product.BuyPrice != null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Produkt nie jest dostępny w sklepie. Cena sprzedaży nie ustawiona ale cena kupna jest ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
                else if (product.SellPrice != null && product.BuyPrice == null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Produkt nie jest dostępny w sklepie. Cena kupna nie ustawiona ale cena sprzedaży jest ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = "Produkt nie jest dostępny w sklepie. Cena kupna i sprzedaży ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
            }
        }
        
        structureTest.MinorTests.Add(minorTest);
    }

    /// <summary>
    /// Przetwarza zależności pól cen produktu czarnego rynku w testach struktury.
    /// </summary>
    /// <param name="product">Produkt, dla którego sprawdzane są zależności cen.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessProductPricesForBlackMarketFieldDependencies(Product product, StructureTest structureTest)
    {
        var minorTest = new MinorTest();

        if (product.IsBlackMarketWorkerItem || product.PlayerCanSellInBlackMarket)
        {
            var startMessage = "Produkt jest dostępny dla " +
                               (product.IsBlackMarketWorkerItem && product.PlayerCanSellInBlackMarket
                                   ? "pracownika czarnego rynku i do sprzedaży przez gracza w czarnym rynku. "
                                   : product.IsBlackMarketWorkerItem
                                       ? "pracownika czarnego rynku. "
                                       : "gracza do sprzedaży w czarnym rynku. ");

            if (product.BlackMarketMinSellPrice != null && product.BlackMarketMaxSellPrice != null)
            {
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest.Description = startMessage + "Cena minimalna i maksymalna ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;   
            }
            else
            {
                if (product.BlackMarketMinSellPrice == null && product.BlackMarketMaxSellPrice != null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = startMessage + "Cena maksymalna ustawiona ale cena minimalna nie ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;   
                }
                else if (product.BlackMarketMinSellPrice != null && product.BlackMarketMaxSellPrice == null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = startMessage + "Cena minimalna ustawiona ale cena maksymalna nie ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = startMessage + "Cena minimalna i maksymalna nie ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
            }
        }
        else
        {
            var startMessage = "Produkt nie jest dostępny dla pracownika czarnego rynku i do sprzedaży przez gracza w czarnym rynku. ";
            if (product.BlackMarketMinSellPrice == null && product.BlackMarketMaxSellPrice == null)
            {
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest.Description = startMessage + "Cena minimalna i maksymalna nie ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;   
            }
            else
            {
                if (product.BlackMarketMinSellPrice == null && product.BlackMarketMaxSellPrice != null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = startMessage + "Cena minimalna nie ustawiona ale cena maksymalna ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;   
                }
                else if (product.BlackMarketMinSellPrice != null && product.BlackMarketMaxSellPrice == null)
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = startMessage + "Cena maksymalna nie ustawiona ale cena minimalna ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
                else
                {
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                    minorTest.Description = startMessage + "Cena minimalna i maksymalna ustawiona. Produkt - " + product.Name + ". Id produktu: " + product.Id;
                }
            }
        }
        
        structureTest.MinorTests.Add(minorTest);
    }

    /// <summary>
    /// Przetwarza zależności pól dla suszu w testach struktury.
    /// </summary>
    /// <param name="driedFruit">Susz, dla którego sprawdzane są zależności pól.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessDriedFruitFieldDependencies(DriedFruit driedFruit, StructureTest structureTest)
    {
        var minorTest1 = new MinorTest();
        var minorTest2 = new MinorTest();
        
        if (driedFruit.AvailableInCustomerZone)
        {
            if (driedFruit.OfferChance != null)
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest1.Description = "Szansa na złożenie oferty ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }
            else
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest1.Description = "Szansa na złożenie oferty nie ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }

            if (driedFruit.CustomerZonePrice != null)
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest2.Description = "Cena sprzedaży w strefie klienta ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }
            else
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest2.Description = "Cena sprzedaży w strefie klienta nie ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }
        }
        else
        {
            if (driedFruit.OfferChance == null)
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest1.Description = "Szansa na złożenie oferty nie ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }
            else
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest1.Description = "Szansa na złożenie oferty ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }

            if (driedFruit.CustomerZonePrice == null)
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest2.Description = "Cena sprzedaży w strefie klienta nie ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }
            else
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest2.Description = "Cena sprzedaży w strefie klienta ustawiona. Susz - " + driedFruit.Name + ". Id suszu: " + driedFruit.Id;
            }
        }

        structureTest.MinorTests.Add(minorTest1);
        structureTest.MinorTests.Add(minorTest2);
    }

    /// <summary>
    /// Przetwarza zależności pól dla bonusu w testach struktury.
    /// </summary>
    /// <param name="bonus">Bonus, dla którego sprawdzane są zależności pól.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessBonusFieldDependencies(Bonus bonus, StructureTest structureTest)
    {
        var minorTest1 = new MinorTest();
        var minorTest2 = new MinorTest();
        var minorTest3 = new MinorTest();
        var minorTest4 = new MinorTest();

        if (bonus.IsArtifact)
        {
            var startMessage = "Bonus jest artefaktem. ";
            if (bonus.ArtifactPullCost != null)
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest1.Description = startMessage + "Koszt wyciągnięcia artefaktu ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest1.Description = startMessage + "Koszt wyciągnięcia artefaktu nie ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }

            if (bonus.ArtifactPutCost != null)
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest2.Description = startMessage + "Koszt włożenia artefaktu ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest2.Description = startMessage + "Koszt włożenia artefaktu nie ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }

            if (bonus.ActiveTimePerUse == null)
            {
                minorTest3.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest3.Description = startMessage + "Czas aktywności wzmocnienia po użyciu nie ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest3.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest3.Description = startMessage + "Czas aktywności wzmocnienia po użyciu ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }

            if (bonus.IsStackable == null)
            {
                minorTest4.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest4.Description = startMessage + "Stakowanie czasu wzmocnienia nie ustawione. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest4.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest4.Description = startMessage + "Stakowanie czasu wzmocnienia ustawione. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
        }
        else
        {
            var startMessage = "Bonus jest wzmocnieniem. ";
            if (bonus.ArtifactPullCost == null)
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest1.Description = startMessage + "Koszt wyciągnięcia artefaktu nie ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest1.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest1.Description = startMessage + "Koszt wyciągnięcia artefaktu ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }

            if (bonus.ArtifactPutCost == null)
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest2.Description = startMessage + "Koszt włożenia artefaktu nie ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest2.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest2.Description = startMessage + "Koszt włożenia artefaktu ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }

            if (bonus.ActiveTimePerUse != null)
            {
                minorTest3.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest3.Description = startMessage + "Czas aktywności wzmocnienia po użyciu ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest3.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest3.Description = startMessage + "Czas aktywności wzmocnienia po użyciu nie ustawiony. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }

            if (bonus.IsStackable != null)
            {
                minorTest4.Status = EnumUtils.StructureTestsStatuses.Ok;
                minorTest4.Description = startMessage + "Stakowanie czasu wzmocnienia ustawione. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
            else
            {
                minorTest4.Status = EnumUtils.StructureTestsStatuses.Error;
                minorTest4.Description = startMessage + "Stakowanie czasu wzmocnienia nie ustawione. Bonus - " + bonus.Name + ". Id bonusu: " + bonus.Id;
            }
        }

        structureTest.MinorTests.Add(minorTest1);
        structureTest.MinorTests.Add(minorTest2);
        structureTest.MinorTests.Add(minorTest3);
        structureTest.MinorTests.Add(minorTest4);
    }

    /// <summary>
    /// Przetwarza zależności pól dla zadania w testach struktury.
    /// </summary>
    /// <param name="quest">Zadanie, dla którego sprawdzane są zależności pól.</param>
    /// <param name="structureTest">Test struktury, do którego dodawane są wyniki testu.</param>
    public static void ProcessQuestFieldDependencies(Quest quest, StructureTest structureTest)
    {
        var levelRequirementTest = new MinorTest();
        var timesTest = new MinorTest();
        var cyclicTimeTest = new MinorTest();

        if (quest.QuestType == DbQuestTypesNames.Achievement)
        {
            if (quest.PlantationLevelRequirement != null)
            {
                levelRequirementTest.Status = EnumUtils.StructureTestsStatuses.Error;
                levelRequirementTest.Description = "Wymagany poziom plantacji nie powinien być ustawiony dla zadania typu osiągnięcie." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
            else
            {
                levelRequirementTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                levelRequirementTest.Description = "Wymagany poziom plantacji nie ustawiony." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;                        
            }
        }
        else
        {
            if (quest.PlantationLevelRequirement == null)
            {
                levelRequirementTest.Status = EnumUtils.StructureTestsStatuses.Error;
                levelRequirementTest.Description = "Wymagany poziom plantacji nie ustawiony ." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
            else
            {
                levelRequirementTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                levelRequirementTest.Description = "Wymagany poziom plantacji ustawiony." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;                        
            }
        }

        if (quest.QuestType == DbQuestTypesNames.Event)
        {
            if (quest.StartTime == null || quest.EndTime == null)
            {
                timesTest.Status = EnumUtils.StructureTestsStatuses.Error;
                timesTest.Description = "Zadanie typu wydarzenie musi mieć ustawiony czas startu i końca." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
            else
            {
                timesTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                timesTest.Description = "Zadanie posiada ustawiony czas startu i końca." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }

            if (quest.CyclicTime != null)
            {
                cyclicTimeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                cyclicTimeTest.Description = "Zadanie posiada ustawiony czas cykliczności." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
            else
            {
                cyclicTimeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                cyclicTimeTest.Description = "Zadanie nie posiada ustawionego czasu cykliczności." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
        }
        else
        {
            if (quest.StartTime != null || quest.EndTime != null)
            {
                timesTest.Status = EnumUtils.StructureTestsStatuses.Error;
                timesTest.Description = "Zadanie typu " + SelectListLoaderHelper.QuestTypesNamesDbToHr[quest.QuestType] + " nie może mieć ustawionego czasu startu i końca." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
            else
            {
                timesTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                timesTest.Description = "Zadanie nie posiada ustawionego czasu startu i końca." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }

            if (quest.CyclicTime == null)
            {
                cyclicTimeTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                cyclicTimeTest.Description = "Zadanie nie posiada ustawionego czasu cykliczności." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
            else
            {
                cyclicTimeTest.Status = EnumUtils.StructureTestsStatuses.Error;
                cyclicTimeTest.Description = "Zadanie posiada ustawiony czas cykliczności." + " Zadanie - " + quest.Name + ". Id zadania: " + quest.Id;
            }
        }

        structureTest.MinorTests.Add(levelRequirementTest);
        structureTest.MinorTests.Add(timesTest);
        structureTest.MinorTests.Add(cyclicTimeTest);
    }
}
