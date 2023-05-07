using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Info;

/// <summary>
/// Hub SignalR obsługujący działania informacyjne.
/// Np. przedmioty na nowy poziom, zmiana definicji przez opiekuna, zmiana dzielnicy. 
/// </summary>
[AbpMvcAuthorize]
public class InfoHub : Hub, ITransientDependency
{
}