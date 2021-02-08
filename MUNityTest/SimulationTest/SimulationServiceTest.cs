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
            Assert.NotNull(simulation);
        }

        [Test]
        [Order(2)]
        public async Task TestGetSimulation()
        {
            var service = new SimulationService(_context);
            var simulation = await service.GetSimulation(this.simulationId);
            Assert.NotNull(simulation);
        }

        [Test]
        [Order(3)]
        public async Task TestGenerateInitalUser()
        {
            var service = new SimulationService(_context);
            var simulation = await service.GetSimulation(this.simulationId);
            var user = service.CreateModerator(simulation, "TestUser");
            Assert.NotNull(user);
        }

        [Test]
        [Order(4)]
        public void TestCanFindInitUser()
        {
            var service = new SimulationService(_context);
            var users = service.GetSimulationUsers(this.simulationId);
            var user = users.FirstOrDefault(n => n.DisplayName == "TestUser");
            Assert.NotNull(user);
        }

        [Test]
        [Order(5)]
        public async Task TestCreateNewUser()
        {
            var service = new SimulationService(_context);
            var simulation = await service.GetSimulation(simulationId);
            var user = service.CreateUser(simulation, "User1");
            Assert.NotNull(user);
        }

        [Test]
        [Order(6)]
        public void TestCreateARole()
        {
            var service = new SimulationService(_context);
            var role = service.AddDelegateRole(simulationId, "Germany", "de");
            Assert.NotNull(role);
        }

        [Test]
        [Order(7)]
        public async Task TestRoleExists()
        {
            var service = new SimulationService(_context);
            var roles = await service.GetSimulationRoles(simulationId);
            Assert.IsTrue(roles.Any(n => n.Name == "Germany"));
        }

        [Test]
        [Order(8)]
        public async Task TestAssignRoleToUser()
        {
            var service = new SimulationService(_context);
            var completeSimulation = await service.GetSimulationWithUsersAndRoles(simulationId);
            var user = completeSimulation.Users.FirstOrDefault(n => n.DisplayName == "User1");
            var role = completeSimulation.Roles.FirstOrDefault(n => n.Name == "Germany");
            var result = service.BecomeRole(completeSimulation, user, role);
            Assert.IsTrue(result);
        }

        [Test]
        [Order(9)]
        public async Task TestUserHasRole()
        {
            var service = new SimulationService(_context);
            var completeSimulation = await service.GetSimulationWithUsersAndRoles(simulationId);
            var user = completeSimulation.Users.FirstOrDefault(n => n.DisplayName == "User1");
            Assert.NotNull(user.Role);
        }

        [Test]
        [Order(10)]
        public async Task TestCreateDefaultPetitionTypes()
        {
            var service = new SimulationService(_context);
            var result = await service.CreateDefaultPetitionTypes();
            Assert.NotZero(result);
        }

        [Test]
        [Order(11)]
        public async Task AddPetitionTypeToSimulation()
        {
            var service = new SimulationService(_context);
            var petitionType = (await service.GetPetitionTypes()).First();
            Assert.NotNull(petitionType);
            var request = new MUNity.Schema.Simulation.Managment.AddPetitionTypeRequestBody()
            {
                PetitionTypeId = petitionType.PetitionTypeId,
                SimulationId = simulationId
            };
            await service.AddPetitionTypeToSimulation(request);
            var list = await service.GetSimulationPetitionTypes(simulationId);
            Assert.NotNull(list);
            Assert.IsTrue(list.Any());
        }
    }
}