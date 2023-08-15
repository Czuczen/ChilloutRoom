using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.EntityAnalysisStateBuilder;

/// <summary>
/// Interfejs budowniczego stanu analizy encji.
/// </summary>
public interface IEntityAnalysisStateBuilder : ISingletonDependency
{
    /// <summary>
    /// Buduje stan analizy dla określonego typu działania na encji.
    /// </summary>
    /// <param name="type">Typ budowania analizy.</param>
    /// <param name="entity">Encja, na której ma zostać wykonana analiza.</param>
    /// <param name="action">Działanie wykonywane na encji.</param>
    /// <param name="page">Strona transakcji czarnego rynku (dotyczy tylko typu BlackMarket).</param>
    /// <returns>Stan analizy dla określonego typu działania na encji.</returns>
    AnalysisState Build(EnumUtils.AnalysisBuildTypes type, object entity, string action = "",
        EnumUtils.BlackMarketPages? page = null);
}