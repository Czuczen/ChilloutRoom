using System;
using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;

/// <summary>
/// Klasa pomocnicza dla testów struktury definicji opiekunów dzielnic.
/// </summary>
public static class StructureTestsHelper
{
    /// <summary>
    /// Sprawdza istnienie produktów użytkownika.
    /// </summary>
    /// <param name="structureTest">Test struktury do którego dodawane są wyniki testu.</param>
    /// <param name="objects">Lista obiektów INamedEntity reprezentujących produkty użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <param name="district">Dzielnica.</param>
    /// <param name="generatedType">Typ generowanego obiektu.</param>
    /// <param name="definitionExist">Flaga określająca czy istnieje definicja dla typu.</param>
    public static void CheckUserProductsExistence(StructureTest structureTest, List<INamedEntity> objects, string userName, District district, GeneratedType generatedType, bool definitionExist)
    {
        var subTest = new SubTest();
        if (objects != null && objects.Count > 0)
        {
            if (objects.Count > 1)
            {
                var defsMessage = objects.Aggregate("</br>Nadmiarowe rekordy:", (current, obj) => current + "</br>" + obj.Name);
                subTest.Description = "Użytkownik posiada nadmiarowe rekordy. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name + defsMessage;
                subTest.Status = EnumUtils.StructureTestsStatuses.Error;
            }
            else
            {
                subTest.Description = "Użytkownik posiada jeden rekord. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name;
                subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
        }
        else if (definitionExist)
        {
            subTest.Description = "Użytkownik nie posiada rekordu a definicja dla typu istnieje. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name;
            subTest.Status = EnumUtils.StructureTestsStatuses.Error;
        }
        else
        {
            subTest.Description = "Użytkownik nie posiada rekordu a definicja dla typu nie istnieje. " + "Użytkownik - " + userName + ". Dzielnica - " + district.Name + ". Id typu generowanego: " + generatedType.Id + ". Typ generowany - " + generatedType.Name;
            subTest.Status = EnumUtils.StructureTestsStatuses.Warn;
        }

        structureTest.SubTests.Add(subTest);
    }

    /// <summary>
    /// Ustawia wynik błędu w teście struktury.
    /// </summary>
    /// <param name="structureTest">Test struktury do którego dodawane są wyniki błędu.</param>
    /// <param name="ex">Wyjątek.</param>
    public static void SetErrorInTest(StructureTest structureTest, Exception ex)
    {
        var subTest = new SubTest
        {
            Description = "Coś poszło nie tak. " + ex,
            Status = EnumUtils.StructureTestsStatuses.Error
        };

        structureTest.SubTests.Add(subTest);
    }

    /// <summary>
    /// Sprawdza istnienie powiązanego obiektu z rośliną.
    /// </summary>
    /// <typeparam name="T">Typ encji połączonej z rośliną.</typeparam>
    /// <param name="structureTest">Test struktury do którego dodawane są wyniki testu.</param>
    /// <param name="connectedObj">Obiekt połączony z rośliną.</param>
    /// <param name="plant">Roślina.</param>
    public static void CheckPlantConnectedEntity<T>(StructureTest structureTest, INamedEntity connectedObj, Plant plant)
    {
        var subTest = new SubTest();
        var type = typeof(T);
        var plantName = plant.Name;

        if (SelectListLoaderHelper.EntitiesNamesDbToHr.ContainsKey(type.Name))
        {
            var hrEntityName = SelectListLoaderHelper.EntitiesNamesDbToHr[type.Name];
                    
            if (connectedObj != null)
            {
                subTest.Description = "Połączony rekord encji istnieje. " + "Roślina - " + plantName + ". Encja - " + hrEntityName + ". Nazwa rekordu - " + connectedObj.Name + ". Id rekordu: " + connectedObj.Id + ". Id rośliny: " + plant.Id;
                subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
            }
            else
            {
                subTest.Description = "Połączony rekord encji nie istnieje. " + "Roślina - " + plantName + ". Encja - " + hrEntityName + ". Id rośliny: " + plant.Id;
                subTest.Status = EnumUtils.StructureTestsStatuses.Error;
            }
        }
        else
        {
            subTest.Description = "Nie znaleziono wyświetlanej nazwy encji. " + "Nazwa encji w bazie danych - " + type.Name;
            subTest.Status = EnumUtils.StructureTestsStatuses.Error;
        }

        structureTest.SubTests.Add(subTest);
    }
    
    /// <summary>
    /// Pobiera kolor statusu testu struktury.
    /// </summary>
    /// <param name="status">Status testu struktury.</param>
    /// <returns>Kolor w formacie szesnastkowym lub eng.</returns>
    public static string GetStatusColor(EnumUtils.StructureTestsStatuses status)
    {
        var ret= status == EnumUtils.StructureTestsStatuses.Ok ? "limegreen" :
            status == EnumUtils.StructureTestsStatuses.Warn ? "#FFCC00" : "red";
            
        return ret;
    }

    /// <summary>
    /// Pobiera tekstowy opis statusu testu struktury.
    /// </summary>
    /// <param name="status">Status testu struktury.</param>
    /// <returns>Opis statusu.</returns>
    public static string GetStatusText(EnumUtils.StructureTestsStatuses status)
    {
        var ret = status == EnumUtils.StructureTestsStatuses.Ok ? "Ok" :
            status == EnumUtils.StructureTestsStatuses.Warn ? "Uwaga" : "Błąd";

        return ret;
    }
        
    /// <summary>
    /// Sprawdza istnienie typu generowanego na produkcie.
    /// </summary>
    /// <param name="structureTest">Test struktury do którego dodawane są wyniki testu.</param>
    /// <param name="objects">Lista obiektów IPlantationGeneratedEntity reprezentujących produkty.</param>
    /// <param name="generatedTypes">Lista typów generowanych.</param>
    public static void TypeOnProductExist(StructureTest structureTest, List<IPlantationGeneratedEntity> objects, List<GeneratedType> generatedTypes)
    {
        if (objects != null)
        {
            foreach (var obj in objects)
            {
                var subTest = new SubTest();
                    
                var generatedType = generatedTypes.SingleOrDefault(item => item.Id == obj.GeneratedTypeId);
                if (generatedType != null)
                {
                    subTest.Description = "Typ zdefiniowany na produkcie istnieje. " + "Produkt - " + obj.Name + ". Id produktu: " + obj.Id + ". Typ generowany - " + generatedType.Name + ". Id typu generowanego: " + generatedType.Id;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Ok;
                }
                else
                {
                    subTest.Description = "Typ zdefiniowany na produkcie nie istnieje. " + "Produkt - " + obj.Name + ". Id produktu: " + obj.Id;
                    subTest.Status = EnumUtils.StructureTestsStatuses.Error;
                }
                    
                structureTest.SubTests.Add(subTest);
            }
        }
    }
}
