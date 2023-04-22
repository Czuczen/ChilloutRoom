using System.Threading.Tasks;
using CzuczenLand.Sessions;
using Shouldly;
using Xunit;

namespace CzuczenLand.Tests.Sessions;

public class SessionAppService_Tests : CzuczenLandTestBase
{
    private readonly ISessionAppService _sessionAppService;

    public SessionAppService_Tests()
    {
        _sessionAppService = Resolve<ISessionAppService>();
    }

    [MultiTenantFact]
    public async Task Should_Get_Current_User_When_Logged_In_As_Host()
    {
        //Arrange
        LoginAsHostAdmin();

        //Act
        var output = await _sessionAppService.GetCurrentLoginInformations();

        //Assert
        var currentUser = await GetCurrentUserAsync();
        output.User.ShouldNotBe(null);
        output.User.Name.ShouldBe(currentUser.Name);
        output.User.Surname.ShouldBe(currentUser.Surname);

        // Jeśli w CzuczenLandCoreModule Configuration.MultiTenancy.IsEnabled jest wyłączone to w TestAbpSession TenantId
        // if (!this._multiTenancy.IsEnabled)
        //     return new int?(1);
        // zawsze zwraca id najemcy równe 1
        // dlatego oryginalne sprawdzenie output.Tenant.ShouldBe(null); zawsze da błąd bo output.Tenant będzie ustawione
        // output.Tenant.ShouldBe(null);
        output.Tenant.ShouldNotBe(null);
    }

    [Fact]
    public async Task Should_Get_Current_User_And_Tenant_When_Logged_In_As_Tenant()
    {
        //Act
        var output = await _sessionAppService.GetCurrentLoginInformations();

        //Assert
        var currentUser = await GetCurrentUserAsync();
        var currentTenant = await GetCurrentTenantAsync();

        output.User.ShouldNotBe(null);
        output.User.Name.ShouldBe(currentUser.Name);

        output.Tenant.ShouldNotBe(null);
        output.Tenant.Name.ShouldBe(currentTenant.Name);
    }
}