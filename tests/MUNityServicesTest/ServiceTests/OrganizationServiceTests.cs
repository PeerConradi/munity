using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Services.Test.ServiceTests;

[TestFixture(Author = "Aeuke Peer Conradi")]
public class OrganizationServiceTests
{
    private ServiceProvider provider;

    private const string TEST_ORGA_NAME = "Deutschle Model United Nations e.V.";

    private const string TEST_ORGA_SHORT = "DMUN e.V.";

    [OneTimeSetUp]
    public void Setup()
    {
        provider = TestHelpers.GetCompleteProvider();
    }

    [Test]
    [Order(0)]
    public void TestNameIsAvailable()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var result = service.IsNameAvailable(TEST_ORGA_NAME);
        Assert.IsTrue(result);
    }

    [Test]
    [Order(1)]
    public void TestShortIsAvailable()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var result = service.IsShortAvailable(TEST_ORGA_SHORT);
        Assert.IsTrue(result);
    }

    [Test]
    [Order(2)]
    public async Task TestCreateOrganization()
    {
        var orgaService = provider.GetRequiredService<OrganizationService>();
        var userService = provider.GetRequiredService<UserService>();
        var user = await userService.CreateUser("tester", "test@mail.com", "password");
        var organization = orgaService.CreateOrganization(TEST_ORGA_NAME, TEST_ORGA_SHORT, user);
        Assert.NotNull(organization);
    }

    [Test]
    [Order(3)]
    public void TestOrganizationIsInDatabase()
    {
        var context = provider.GetRequiredService<MunityContext>();
        Assert.AreEqual(1, context.Organizations.Count());
    }

    [Test]
    [Order(4)]
    public void TestNameIsNoLongerAvailable()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var result = service.IsNameAvailable(TEST_ORGA_NAME);
        Assert.IsFalse(result);
    }

    [Test]
    [Order(5)]
    public void TestShortIsNoLongerAvailable()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var result = service.IsShortAvailable(TEST_ORGA_SHORT);
        Assert.IsFalse(result);
    }

    [Test]
    [Order(6)]
    public void TestOrganizationHasEasyId()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var result = service.OrganizationWithIdExisits("dmunev");
        Assert.IsTrue(result);
    }

    [Test]
    [Order(7)]
    public void TestUserIsInOrganization()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var result = service.IsUsernameMemberOfOrganiation("tester", "dmunev");
        Assert.IsTrue(result);
    }

    [Test]
    [Order(8)]
    public void TestGetOrganizationTinyInfo()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var info = service.GetTinyInfo("dmunev");
        Assert.NotNull(info);
        Assert.AreEqual(TEST_ORGA_SHORT, info.Short);
        Assert.AreEqual(TEST_ORGA_NAME, info.Name);
        Assert.AreEqual("dmunev", info.OrganizationId);
    }

    [Test]
    [Order(9)]
    public void TestGetOrganizationConferenceInfo()
    {
        var service = provider.GetRequiredService<OrganizationService>();
        var info = service.GetOrgaConferenceInfo("dmunev");
        Assert.NotNull(info);
        Assert.AreEqual(0, info.ProjectCount);
        Assert.AreEqual(0, info.ConferenceCount);
    }
}
