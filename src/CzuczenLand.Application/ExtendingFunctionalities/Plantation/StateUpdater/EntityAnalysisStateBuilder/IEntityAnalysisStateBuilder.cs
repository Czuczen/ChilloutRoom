using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.EntityAnalysisStateBuilder;

public interface IEntityAnalysisStateBuilder : ISingletonDependency
{
    AnalysisState Build(EnumUtils.AnalysisBuildTypes type, object entity, string action = "",
        EnumUtils.BlackMarketPages? page = null);
}