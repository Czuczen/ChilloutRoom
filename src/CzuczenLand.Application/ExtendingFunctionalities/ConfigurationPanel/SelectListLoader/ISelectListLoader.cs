using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Dependency;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;

public interface ISelectListLoader : ITransientDependency
{
    Task<Dictionary<string, object>> GetSelectFieldsOptions(List<PropertyInfo> properties, List<object> values,
        string entity, bool isEdit, bool isAdmin);
}