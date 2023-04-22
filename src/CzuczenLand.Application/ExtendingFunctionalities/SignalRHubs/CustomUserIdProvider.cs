using Abp.Dependency;
using Abp.Runtime.Session;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs;

/// <summary>
/// ITransientDependency dla AbpSession. Inaczej nie ma id
/// </summary>
public class CustomUserIdProvider : IUserIdProvider, ITransientDependency
{
    public IAbpSession AbpSession { get; set; }
        
    
    public CustomUserIdProvider()
    {
        AbpSession = NullAbpSession.Instance;
    }
        
    public string GetUserId(IRequest request) => AbpSession?.GetUserId().ToString();
}