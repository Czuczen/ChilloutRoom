using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;

/// <summary>
/// Interfejs usługi podstawowej obsługującej zadania.
/// </summary>
public interface IQuestService : ITransientDependency
{
    /// <summary>
    /// Ustawia zależności zadania i wartości początkowe.
    /// </summary>
    /// <param name="entityId">Identyfikator zadania będącego definicją.</param>
    /// <param name="entitiesIds">Słownik zawierający nazwę encji i przypisane do niej identyfikatory.</param>
    Task SetQuestDependencies(int? entityId, Dictionary<string, List<int>> entitiesIds);
        
    /// <summary>
    /// Aktualizuje zależności zadania.
    /// </summary>
    /// <param name="entityId">Identyfikator zadania będącego definicją.</param>
    /// <param name="entitiesIds">Słownik zawierający nazwę encji i przypisane do niej identyfikatory.</param>
    Task UpdateQuestDependencies(int? entityId, Dictionary<string, List<int>> entitiesIds);

    /// <summary>
    /// Ustawia asynchronicznie wartości początkowe dla zadań gracza.
    /// </summary>
    /// <param name="playerRecords">Lista zadań gracza.</param>
    Task SetStartValuesAsync(List<ExtendingModels.Models.General.Quest> playerRecords);
        
    /// <summary>
    /// Ustawia synchronicznie wartości początkowe dla zadań gracza.
    /// </summary>
    /// <param name="playerRecords">Lista zadań gracza.</param>
    void SetStartValues(List<ExtendingModels.Models.General.Quest> playerRecords);

    /// <summary>
    /// Tworzy postęp wymagań zadań dla gracza na podstawie zadań będących definicją.
    /// </summary>
    /// <param name="questsDefinitions">Lista definicji zadań.</param>
    /// <param name="playerRecords">Lista zadań gracza.</param>
    Task CreatePlayerQuestsRequirementsProgress(List<ExtendingModels.Models.General.Quest> questsDefinitions,
        List<ExtendingModels.Models.General.Quest> playerRecords);
}