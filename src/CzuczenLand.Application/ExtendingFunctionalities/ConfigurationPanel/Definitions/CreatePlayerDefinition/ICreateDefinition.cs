using System.Threading.Tasks;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;

public interface ICreateDefinition<in TCreateDto>
    where TCreateDto : class
{
    Task Create(TCreateDto entity);
}