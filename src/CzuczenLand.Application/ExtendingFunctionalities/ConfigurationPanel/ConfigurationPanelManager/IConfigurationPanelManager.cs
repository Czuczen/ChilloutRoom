using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.ObjectMapping;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager;

/// <summary>
/// Interfejs dla zarządzania panelem konfiguracyjnym.
/// </summary>
public interface IConfigurationPanelManager : ITransientDependency
{
    /// <summary>
    /// Formatuje logi zapisane w pliku do obiektu Logs.
    /// </summary>
    /// <returns>Obiekt Logs zawierający sformatowane logi.</returns>
    Logs FormatLogs();
        
    /// <summary>
    /// Rozpoczyna testy struktury.
    /// </summary>
    /// <param name="isAdmin">Czy użytkownik jest administratorem.</param>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Lista testów struktury.</returns>
    Task<List<StructureTest>> StartTests(bool isAdmin, long userId);
        
    /// <summary>
    /// Ładuje dane potrzebne do wygenerowania formularza edycji lub tworzenia obiektu.
    /// </summary>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="objectMapper">Interfejs do mapowania obiektów.</param>
    /// <param name="isAdmin">Czy użytkownik jest administratorem.</param>
    /// <param name="objectId">Identyfikator obiektu (jeśli edycja).</param>
    /// <param name="editManyAction">Akcja edycji wielu obiektów (jeśli nie puste).</param>
    /// <returns>Obiekt EditOrCreate zawierający informacje o edycji/tworzeniu obiektu.</returns>
    Task<EditOrCreate> EditOrCreateObject(string entity, IObjectMapper objectMapper, bool isAdmin, int? objectId = null,
        string editManyAction = null);

    /// <summary>
    /// Wyszukuje sugestie dla formularza encji, która ma możliwość ustawiania relacji Many-to-many.
    /// </summary>
    /// <param name="valueToSearch">Tekst do wyszukania.</param>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="generatedTypeId">Identyfikator typu generowanego.</param>
    /// <param name="districtId">Identyfikator dzielnicy.</param>
    /// <returns>Lista sugestii pasujących do wyszukiwanego tekstu.</returns>
    Task<List<object>> SearchSuggestions(string valueToSearch, string entity, int? generatedTypeId, int? districtId);
}