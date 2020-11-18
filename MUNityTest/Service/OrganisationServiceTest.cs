using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Models.Organization;
using MUNityCore.Services;
using NUnit.Framework;

namespace MUNityTest.Service
{
    [TestFixture]
    public class OrganisationServiceTest
    {
        private MunCoreContext _context;
        private Organization _organization;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Setup Test Environment");
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunCoreContext>();
            optionsBuilder.UseSqlite("Data Source=test_organisation.db");
            _context = new MunCoreContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Order(1)]
        [Author("Peer Conradi")]
        [Description("First Test Creates an Organization that can create projects etc.")]
        public async Task TestCreateAndGetOrganisation()
        {
            var service = new OrganisationService(_context);
            var result = service.CreateOrganisation("Deutsche Model United Nations e.V.", "dmun e.V.");
            var dbResult = await service.GetOrganisation(result.OrganizationId);
            Assert.AreEqual("Deutsche Model United Nations e.V.", dbResult.OrganizationName);
            _organization = dbResult;
        }

        [Test]
        [Order(2)]
        [Author("Peer Conradi")]
        [Description("Test adding a role to the organization.")]
        public async Task TestAddOrganisationRole()
        {
            var service = new OrganisationService(_context);
            var role = service.AddOrganisationRole(_organization, "Vorstand", true);
            Assert.NotNull(role);
            var reloadOrga = await service.GetOrganisation(_organization.OrganizationId);
            Assert.NotNull(reloadOrga);
            Assert.AreEqual(1, reloadOrga.Roles.Count);
            var roles = service.GetOrganisationRoles(_organization.OrganizationId);
            Assert.AreEqual(1, roles.Count());
        }

        [Test]
        [Order(3)]
        [Author("Peer Conradi")]
        [Description("Test to add a Member to the organization")]
        public async Task TestAddUserToOrganisation()
        {
            var service = new OrganisationService(_context);
            var role = service.GetOrganisationRoles(_organization.OrganizationId).FirstOrDefault();
            Assert.NotNull(role);
            var userService = new UserService(_context);
            var user = userService.CreateUser("test", "Max", "Mustermann", "123456", "test@mail.com", new DateTime(1990, 1, 1));
            var result = service.AddUserToOrganisation(user, _organization, role);
            Assert.NotNull(result);
            var reloadOrga = await service.GetOrganisation(_organization.OrganizationId);
            Assert.AreEqual(1, reloadOrga.Member.Count);
        }
    }
}