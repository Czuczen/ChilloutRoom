using System.Collections.Generic;
using System.Threading.Tasks;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;

public interface IUpdateDefinition<in TUpdateDto>
    where TUpdateDto : class
{
    Task Update(TUpdateDto entity);

    Task Update(object updatedFields, List<int> ids);
}