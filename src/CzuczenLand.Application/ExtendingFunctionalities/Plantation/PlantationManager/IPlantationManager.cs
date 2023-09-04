using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.ObjectMapping;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager;

/// <summary>
/// Interfejs definiujący operacje związane z zarządzaniem plantacjami.
/// </summary>
public interface IPlantationManager : ITransientDependency
{
    /// <summary>
    /// Pobiera informacje o plantacji dla określonego użytkownika.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="districtId">Identyfikator dzielnicy (opcjonalnie).</param>
    /// <param name="heWantPayForHollow">Czy gracz chce zapłacić za dziuple.</param>
    /// <returns>Informacje o plantacji.</returns>
    Task<Plantation> GetPlantation(long userId, int? districtId, bool heWantPayForHollow);

    /// <summary>
    /// Tworzy obiekt czarnego rynku dla danego gracza.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="objectMapper">Mapper obiektów.</param>
    /// <returns>Obiekt reprezentujący czarny rynek.</returns>
    Task<BlackMarket> CreatePlayerBlackMarket(long userId, IObjectMapper objectMapper);

    /// <summary>
    /// Pobiera dostępne produkty gracza dla danej encji.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="valueToSearch">Wartość do wyszukania (opcjonalnie).</param>
    /// <returns>Lista dostępnych produktów gracza dla danej encji.</returns>
    Task<List<object>> GetAvailablePlayerProducts(long userId, string entity, string valueToSearch);

    /// <summary>
    /// Tworzy roślinę dla danego gracza.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <param name="plantData">Dane dotyczące tworzonej rośliny.</param>
    /// <returns>Obiekt reprezentujący wynik tworzenia rośliny.</returns>
    Task<CreatePlant> CreatePlayerPlant(long userId, string userName, PlantData plantData);

    /// <summary>
    /// Reprezentuje zebranie rośliny gracza na podstawie identyfikatora.
    /// </summary>
    /// <param name="id">Identyfikator rośliny do zebrania.</param>
    /// <returns>Lista powiadomień akcji zbierania rośliny.</returns>
    Task<List<string>> CollectPlayerPlant(int id);
        
    /// <summary>
    /// Usuwa roślinę gracza na podstawie identyfikatora.
    /// </summary>
    /// <param name="id">Identyfikator rośliny do usunięcia.</param>
    /// <returns>Lista powiadomień akcji usuwania rośliny.</returns>
    Task<List<string>> RemovePlayerPlant(int id);

    /// <summary>
    /// Przetwarza ukończone zadanie użytkownika.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <param name="questId">Identyfikator ukończonego zadania.</param>
    /// <param name="objectMapper">Mapper obiektów.</param>
    /// <returns>Model z danymi ukończonego zadania.</returns>
    Task<CompleteQuest> ProcessCompletedQuest(long userId, int questId, IObjectMapper objectMapper);

    /// <summary>
    /// Tworzy model informacji o zadaniach.
    /// </summary>
    /// <param name="quest">Zadanie dla którego tworzony jest model informacji.</param>
    /// <returns>Model informacji o zadaniach.</returns>
    Task<QuestInfoCreation> CreateQuestInfoCreationModel(Quest quest);
}