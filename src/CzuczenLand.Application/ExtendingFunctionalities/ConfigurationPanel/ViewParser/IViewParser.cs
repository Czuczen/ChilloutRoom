using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;

public interface IViewParser : ITransientDependency
{
    Task<List<object>> ParseObjectsValues(List<object> dtoObjList, List<PropertyInfo> properties);
}