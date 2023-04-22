using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Info;

[AbpMvcAuthorize]
public class InfoHub : Hub, ITransientDependency
{
}