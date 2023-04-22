using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;

[AbpMvcAuthorize]
public class QuestHub : Hub, ITransientDependency
{
}