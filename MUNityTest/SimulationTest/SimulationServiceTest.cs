using System;
using Microsoft.EntityFrameworkCore.Internal;
using MUNityCore.Models.Simulation;
using NUnit.Framework;
using MUNityCore.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;

namespace MUNityTest.SimulationTest
{
    [TestFixture]
    public class SimulationServiceTest
    {
        private int simulationId;

        private MunityContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_simulation.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("Should be able to create an instance of this service")]
        [Order(0)]
        public void TestCreateServiceInstance()
        {
            var instance = new SimulationService(_context);
            Assert.NotNull(instance);
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("It should be possible to create a new simulation")]
        [Order(1)]
        public void TestCreateSimulation()
        {
            var service = new SimulationService(_context);

            var simulation = service.CreateSimulation("Test Committee", "Password");
            this.simulationId = simulation.SimulationId;
            Console.WriteLine($"Created Simulation with id: {simulationId}");
            Assert.NotNull(simulation);
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("It should have created an owner role that can be used by the user for administration")]
        [Order(2)]
        public void TestOwnerRoleExists()
        {
            var service = new SimulationService(_context);
            var roles = service.GetSimulationsRoles(this.simulationId);
            Assert.NotNull(roles);
            var ownerRoleExists = roles.Any(n => n.RoleType == SimulationRole.RoleTypes.Moderator);
            Assert.NotNull(ownerRoleExists);
            Assert.IsTrue(ownerRoleExists);
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("You should be able to join the lobby with a custom display name and then become the owner")]
        [Order(3)]
        public async Task TestBecomeOwner()
        {
            var service = new SimulationService(_context);

            var simulation = await service.GetSimulation(this.simulationId);

            var ownerRole = service.GetSimulationsRoles(this.simulationId)
                .FirstOrDefault(n => n.RoleType == SimulationRole.RoleTypes.Moderator);
            Assert.NotNull(ownerRole);

            var joinedUser = service.JoinSimulation(simulation, "Joe Mama");
            Assert.NotNull(joinedUser);

            var result = service.BecomeRole(simulation, joinedUser, ownerRole);
            Assert.IsTrue(result);

        }

        [Test]
        [Author("Peer Conradi")]
        [Description("The next user should be unable to become the owner and get stuck inside the lobby")]
        [Order(4)]
        public async Task TestNextUserBecomesOwner()
        {
            var service = new SimulationService(_context);

            var simulation = await service.GetSimulation(this.simulationId);

            var ownerRole = service.GetSimulationsRoles(this.simulationId)
                .FirstOrDefault(n => n.RoleType == SimulationRole.RoleTypes.Moderator);
            Assert.NotNull(ownerRole);

            var joinedUser = service.JoinSimulation(simulation, "Joe Papa");
            Assert.NotNull(joinedUser);

            var result = service.BecomeRole(simulation, joinedUser, ownerRole);
            Assert.IsFalse(result);

        }

        [Test]
        [Author("Peer Conradi")]
        [Description("Should be able to create a new Chairman Role.")]
        [Order(5)]
        public void TestCreateChairmanRole()
        {
            var service = new SimulationService(_context);
            var chairmanRole = service.AddChairmanRole(this.simulationId, 3, "Chairman");
            Assert.NotNull(chairmanRole);
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("Should be able to get that Chairman Role")]
        [Order(6)]
        public void TestGetChairmanRole()
        {
            var service = new SimulationService(_context);
            var roles = service.GetSimulationsRoles(this.simulationId);
            Assert.NotNull(roles);
            Assert.IsTrue(roles.Any(n => n.RoleType == SimulationRole.RoleTypes.Chairman));
        }

        [Test]
        [Author("Peer Conradi")]
        [Description("Should be able to join the game lobby")]
        [Order(7)]
        public void TestJoinLobby()
        {
            var service = new SimulationService(_context);
            var user = service.JoinSimulation(simulationId, "User Two");
            var lobby = service.GetSimulationUsers(simulationId);
            var lobbyUsers = lobby.ToList();
            Assert.AreEqual(3, lobby.Count());
        }
    }
}