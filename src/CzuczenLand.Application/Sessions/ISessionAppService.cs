using System.Threading.Tasks;
using Abp.Application.Services;
using CzuczenLand.Sessions.Dto;

namespace CzuczenLand.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}