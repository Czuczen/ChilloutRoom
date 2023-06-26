using System;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Chat.Dto;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Chat;

/// <summary>
/// Hub SignalR obsługujący czat.
/// [AbpAuthorize] dawało wyjątek - Abp.Authorization.AbpAuthorizationException: Current user did not login to the application!
/// Nic nie psuł ale jednak wyskakiwał. W każdym hub'ie wyskakiwał. Użycie [AbpMvcAuthorize] rozwiązało problem.
/// </summary>
[AbpMvcAuthorize]
public class ChatHub : Hub, ITransientDependency
{
    /// <summary>
    /// Właściwość pozwalająca na uzyskanie dostępu do sesji Abp, która przechowuje informacje dotyczące aktualnie zalogowanego użytkownika.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public IAbpSession AbpSession { get; set; }
        
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }

    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    public ChatHub()
    {
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
    }

    /// <summary>
    /// Metoda wysyłająca wiadomość do wszystkich klientów.
    /// </summary>
    /// <param name="message">Treść wiadomości.</param>
    public void SendMessageToAll(string message)
    {
        try
        {
            var messageData = new ChatMessageData
            {
                UserName = Context.User.Identity.Name,
                Message = message
            };

            Clients.All.receiveMessage(messageData);
        }
        catch (Exception ex)
        {
            Logger.Error("Blad =======//========", ex);
            Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);
        }
    }
}