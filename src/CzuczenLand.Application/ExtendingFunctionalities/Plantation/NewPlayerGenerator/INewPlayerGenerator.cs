using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator;

public interface INewPlayerGenerator : ITransientDependency
{
    Task<Plantation> GetOrInitPlayerResources(bool heWantPayForHollow, PlayerStorage playerStorage, User user);
}