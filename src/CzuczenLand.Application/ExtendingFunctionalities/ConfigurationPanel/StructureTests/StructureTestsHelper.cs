using System;
using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;

public static class StructureTestsHelper
{
    public static void CheckUserProductsExistence(StructureTest structureTest, List<INamedEntity> objects, string userName, District district, GeneratedType generatedType, bool definitionExist)
    {
        var minorTest = new MinorTest();
        if (objects != null && objects.Count > 0)
        {
            if (objects.Count > 1)
            {
                var defsMessage = objects.Aggregate("</br>Nadmiarowe definicje:", (current, obj) => current + "</br>" + obj.Name);
                minorTest.Description = "Użytkownik posiada nadmiarowe definicje. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name + defsMessage;
                minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
            }
            else
            {
                minorTest.Description = "Użytkownik posiada jedną definicje. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name;
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
        }
        else if (definitionExist)
        {
            minorTest.Description = "Użytkownik nie posiada definicji a definicja dla typu istnieje. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name;
            minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
        }
        else
        {
            minorTest.Description = "Użytkownik nie posiada definicji. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name;
            minorTest.Status = EnumUtils.StructureTestsStatuses.Warn;
        }

        structureTest.MinorTests.Add(minorTest);
    }

    public static void SetErrorInTest(StructureTest structureTest, Exception ex)
    {
        var minorTest = new MinorTest
        {
            Description = "Coś poszło nie tak. " + ex,
            Status = EnumUtils.StructureTestsStatuses.Error
        };

        structureTest.MinorTests.Add(minorTest);
    }

    public static void CheckPlantConnectedEntity<T>(StructureTest structureTest, INamedEntity connectedObj, Plant plant)
    {
        var minorTest = new MinorTest();
        var type = typeof(T);
        var plantName = plant.Name;

        if (SelectListLoaderHelper.EntitiesNamesDbToHr.ContainsKey(type.Name))
        {
            var hrEntityName = SelectListLoaderHelper.EntitiesNamesDbToHr[type.Name];
                    
            if (connectedObj != null)
            {
                minorTest.Description = "Połączony rekord encji istnieje. " + "Roślina - " + plantName + ". Encja - " + hrEntityName + ". Nazwa rekordu - " + connectedObj.Name + ". Id rekordu: " + connectedObj.Id + ". Id rośliny: " + plant.Id;
                minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
            else
            {
                minorTest.Description = "Połączony rekord encji nie istnieje. " + "Roślina - " + plantName + ". Encja - " + hrEntityName + ". Id rośliny: " + plant.Id;
                minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
            }
        }
        else
        {
            minorTest.Description = "Nie znaleziono wyświetlanej nazwy encji. " + "Nazwa encji w bazie danych - " + type.Name;
            minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
        }

        structureTest.MinorTests.Add(minorTest);
    }

    public static string GetStatusColor(EnumUtils.StructureTestsStatuses status)
    {
        var ret= status == EnumUtils.StructureTestsStatuses.Ok ? "limegreen" :
            status == EnumUtils.StructureTestsStatuses.Warn ? "#FFCC00" : "red";
            
        return ret;
    }

    public static string GetStatusText(EnumUtils.StructureTestsStatuses status)
    {
        var ret = status == EnumUtils.StructureTestsStatuses.Ok ? "Ok" :
            status == EnumUtils.StructureTestsStatuses.Warn ? "Uwaga" : "Błąd";

        return ret;
    }
        
    public static void TypeOnProductExist(StructureTest structureTest, List<IPlantationGeneratedEntity> objects, List<GeneratedType> generatedTypes)
    {
        if (objects != null)
        {
            foreach (var obj in objects)
            {
                var minorTest = new MinorTest();
                    
                var generatedType = generatedTypes.SingleOrDefault(item => item.Id == obj.GeneratedTypeId);
                if (generatedType != null)
                {
                    minorTest.Description = "Typ zdefiniowany na produkcie istnieje. " + "Produkt - " + obj.Name + ". Id produktu: " + obj.Id + ". Typ generowany - " + generatedType.Name + ". Id typu generowanego: " + generatedType.Id;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    minorTest.Description = "Typ zdefiniowany na produkcie nie istnieje. " + "Produkt - " + obj.Name + ". Id produktu: " + obj.Id;
                    minorTest.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                    
                structureTest.MinorTests.Add(minorTest);
            }
        }
    }
}
