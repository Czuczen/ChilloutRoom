using System;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Chat.Dto;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Chat;

/// <summary>
/// [UnitOfWork] - Jednostka pracy wymaga ITransientDependency
/// IOnlineClientManager - wymaga ITransientDependency
/// AbpSession i Logger - wymaga ITransientDependency
/// [AbpAuthorize] dawało wyjątek - Abp.Authorization.AbpAuthorizationException: Current user did not login to the application!
/// Nic nie psuł ale jednak wyskakiwał. W każdym hub'ie wyskakiwał. Użycie [AbpMvcAuthorize] rozwiązało problem.
/// </summary>
[AbpMvcAuthorize]
public class ChatHub : Hub, ITransientDependency
{
    public IAbpSession AbpSession { get; set; }
        
    public ILogger Logger { get; set; }

    
    public ChatHub()
    {
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
    }

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