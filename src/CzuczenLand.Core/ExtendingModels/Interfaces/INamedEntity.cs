using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingModels.Interfaces;

public interface INamedEntity : IEntity<int>
{
    string Name { get; set; }
}