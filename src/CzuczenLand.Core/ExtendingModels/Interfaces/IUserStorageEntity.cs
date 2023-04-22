namespace CzuczenLand.ExtendingModels.Interfaces;

public interface IUserStorageEntity : INamedEntity 
{
    long UserId { get; set; }
}