using System.Threading.Tasks;
using Abp.Application.Services;
using CzuczenLand.Authorization.Accounts.Dto;

namespace CzuczenLand.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}