using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.DeleteConnected;

/// <summary>
/// Interfejs zawierający deklaracje metod do usuwania powiązanych encji.
/// </summary>
public interface IDeleter : ITransientDependency
{
    /// <summary>
    /// Usuwa powiązane encje związane z daną dzielnicą.
    /// </summary>
    /// <param name="entity">Rekord dzielnicy, dla której usuwane są powiązane encje.</param>
    Task DeleteConnected(District entity);
        
    /// <summary>
    /// Usuwa powiązane encje związane z daną nagrodą.
    /// </summary>
    /// <param name="entity">Rekord nagrody, dla której usuwane są powiązane encje.</param>
    Task DeleteConnected(Drop entity);
        
    /// <summary>
    /// Usuwa powiązane encje związane z danym wymaganiem.
    /// </summary>
    /// <param name="entity">Rekord wymagania, dla którego usuwane są powiązane encje.</param>
    Task DeleteConnected(Requirement entity);
       
    /// <summary>
    /// Usuwa powiązane encje związane z danym typem generowanym.
    /// </summary>
    /// <param name="entity">Rekord typu generowanego, dla którego usuwane są powiązane encje.</param>
    Task DeleteConnected(GeneratedType entity);
       
    /// <summary>
    /// Usuwa powiązane encje związane z danym magazynem plantacji.
    /// </summary>
    /// <param name="entity">Rekord magazynu plantacji, dla którego usuwane są powiązane encje.</param>
    Task DeleteConnected(PlantationStorage entity);
        
    /// <summary>
    /// Usuwa powiązane encje związane z danym magazynem gracza.
    /// </summary>
    /// <param name="entity">Rekord magazynu gracza, dla którego usuwane są powiązane encje.</param>
    Task DeleteConnected(PlayerStorage entity);
        
    /// <summary>
    /// Usuwa powiązane encje związane z danym zadaniem.
    /// </summary>
    /// <param name="entity">Rekord zadania, dla którego usuwane są powiązane encje.</param>
    Task DeleteConnected(Quest entity);
        
    /// <summary>
    /// Usuwa powiązane encje związane z danym użytkownikiem.
    /// </summary>
    /// <param name="entity">Rekord użytkownika, dla którego usuwane są powiązane encje.</param>
    Task DeleteConnected(User entity);
}