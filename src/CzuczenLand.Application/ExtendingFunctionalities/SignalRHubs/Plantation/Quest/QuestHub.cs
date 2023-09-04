using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;

/// <summary>
/// Hub SignalR obsługujący zadania.
/// [AbpAuthorize] dawało wyjątek - Abp.Authorization.AbpAuthorizationException: Current user did not login to the application!
/// Nic nie psuł ale jednak wyskakiwał. W każdym hub'ie wyskakiwał. Użycie [AbpMvcAuthorize] rozwiązało problem.
/// </summary>
[AbpMvcAuthorize]
public class QuestHub : Hub, ITransientDependency
{
}