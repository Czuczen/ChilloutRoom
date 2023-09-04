using Abp.Dependency;
using Abp.Runtime.Session;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs;

/// <summary>
/// Udostępnia identyfikator użytkownika (id) z obiektu AbpSession.
/// Przy użyciu interfejsu ITransientDependency rejestrujemy klasę do iniekcji zależności.
/// Dzięki iniekcji zależności obiekt AbpSession zostanie wstrzyknięty poprzez wstrzykiwanie właściwości. 
/// </summary>
public class CustomUserIdProvider : IUserIdProvider, ITransientDependency
{
    /// <summary>
    /// Właściwość pozwalająca na uzyskanie dostępu do sesji Abp, która przechowuje informacje dotyczące aktualnie zalogowanego użytkownika.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public IAbpSession AbpSession { get; set; }

    /// <summary>
    /// Konstruktor, który ustawia właściwość AbpSession.
    /// </summary>
    public CustomUserIdProvider()
    {
        AbpSession = NullAbpSession.Instance;
    }
        
    /// <summary>
    /// Metoda, która zwraca identyfikator aktualnie zalogowanego użytkownika w formie stringa.
    /// Wykorzystuje właściwość AbpSession do pobrania identyfikatora użytkownika.
    /// Jeśli nie ma zalogowanego użytkownika, zwraca null.
    /// </summary>
    /// <param name="request">Obiekt IRequest, który reprezentuje żądanie.</param>
    /// <returns>Identyfikator aktualnie zalogowanego użytkownika w formie stringa lub null, jeśli nie ma zalogowanego użytkownika.</returns>
    public string GetUserId(IRequest request) => AbpSession?.GetUserId().ToString();
}