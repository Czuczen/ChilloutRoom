using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Plant.Base;

/// <summary>
/// Interfejs serwisu obsługującego rośliny.
/// </summary>
public interface IPlantService : ITransientDependency
{
    /// <summary>
    /// Tworzy roślinę gracza.
    /// </summary>
    /// <param name="createPlant">Obiekt zawierający informacje do utworzenia nowej rośliny.</param>
    /// <param name="district">Dzielnica, w której roślina jest tworzona.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <param name="plantationStorageId">Identyfikator magazynu plantacji.</param>
    /// <param name="activeBonuses">Aktywne bonusy.</param>
    /// <param name="generatedType">Typ generowany nasiona dla rośliny.</param>
    /// <returns>Identyfikator nowo utworzonej rośliny.</returns>
    Task<int> CreatePlayerPlant(CreatePlant createPlant, ExtendingModels.Models.General.District district, string userName,
        int plantationStorageId, List<Bonus> activeBonuses, ExtendingModels.Models.General.GeneratedType generatedType);
        
    /// <summary>
    /// Usuwa rośliny powiązane z usuniętym produktem.
    /// </summary>
    /// <param name="productId">Identyfikator produktu.</param>
    /// <param name="entityName">Nazwa encji produktu.</param>
    Task DeleteConnectedPlantsToProduct(int productId, string entityName);

    /// <summary>
    /// Usuwa roślinę i aktualizuje powiązane produkty.
    /// Jeśli dzielnica fast się skończyła a użytkownik miał jakieś rośliny to dodawały się rekordy ignorowania
    /// ale z racji że już nie było magazynu plantacji state updater builder nie usuwał ignorowania. Dlatego potrzebna jest flaga needIgnoreChange.
    /// </summary>
    /// <param name="fieldToCompare">Pole do porównania.</param>
    /// <param name="value">Wartość pola.</param>
    /// <param name="needIgnoreChange">Flaga określająca potrzebę ignorowania zmian.</param>
    Task DeletePlantAndSetUseCountConnectedProducts(string fieldToCompare, int value, bool needIgnoreChange = true);
}