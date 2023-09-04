using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator;

/// <summary>
/// Interfejs definiujący generator danych gracza dla wybranej dzielnicy.
/// </summary>
public interface INewPlayerGenerator : ITransientDependency
{
    /// <summary>
    /// Inicjuje lub pobiera dane gracza dla wybranej dzielnicy i przypisuje je do obiektu plantacji.
    /// </summary>
    /// <param name="heWantPayForHollow">Czy gracz chce płacić za dziuple.</param>
    /// <param name="playerStorage">Dane przechowujące informacje o graczu (magazyn gracza).</param>
    /// <param name="user">Użytkownik wchodzący na dzielnicę.</param>
    /// <returns>Obiekt reprezentujący plantację z zasobami gracza.</returns>
    Task<Plantation> GetOrInitPlayerResources(bool heWantPayForHollow, PlayerStorage playerStorage, User user);
}