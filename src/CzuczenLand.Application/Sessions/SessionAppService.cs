using System.Threading.Tasks;
using Abp.Auditing;
using Abp.ObjectMapping;
using CzuczenLand.Sessions.Dto;

namespace CzuczenLand.Sessions;

/// <summary>
/// Serwis aplikacyjny do obsługi sesji.
/// </summary>
public class SessionAppService : CzuczenLandAppServiceBase, ISessionAppService
{
    /// <summary>
    /// Mapper obiektów.
    /// </summary>
    private readonly IObjectMapper _objectMapper;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="objectMapper">Mapper obiektów.</param>
    public SessionAppService(IObjectMapper objectMapper)
    {
        _objectMapper = objectMapper;
    }

    /// <summary>
    /// Metoda do pobierania informacji o bieżącej sesji logowania.
    /// </summary>
    /// <returns>Obiekt zawierający informacje o bieżącej sesji logowania.</returns>
    [DisableAuditing]
    public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
    {
        var output = new GetCurrentLoginInformationsOutput();

        if (AbpSession.UserId.HasValue)
        {
            output.User = _objectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
        }

        if (AbpSession.TenantId.HasValue)
        {
            output.Tenant = _objectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
        }

        return output;
    }
}