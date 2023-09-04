using System.Threading.Tasks;
using Abp.Application.Services;
using CzuczenLand.Sessions.Dto;

namespace CzuczenLand.Sessions;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi sesji.
/// </summary>
public interface ISessionAppService : IApplicationService
{
    /// <summary>
    /// Metoda do pobierania informacji o bieżącej sesji logowania.
    /// </summary>
    /// <returns>Obiekt zawierający informacje o bieżącej sesji logowania.</returns>
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}