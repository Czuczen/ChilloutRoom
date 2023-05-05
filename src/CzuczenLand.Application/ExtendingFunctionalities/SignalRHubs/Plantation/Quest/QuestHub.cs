using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;

/// <summary>
/// Hub SignalR obsługujący zadania.
/// </summary>
[AbpMvcAuthorize]
public class QuestHub : Hub, ITransientDependency
{
}