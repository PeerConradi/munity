using System;
using Microsoft.EntityFrameworkCore.Internal;
using MUNityCore.Models.Simulation;
using NUnit.Framework;
using MUNityCore.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.EntityFramework;
using MUNity.Schema.Simulation;

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

        bool skipTestsDependingOnDMUNFile = false;
        [Test]
        [Order(10)]
        public void TestLoadingPreset()
        {
            var path = AppContext.BaseDirectory + "assets/templates/petitions/DMUN.csv";
            var fileExists = System.IO.File.Exists(path);
            if (fileExists)
            {
                var service = new SimulationService(_context);
                var template = service.LoadSimulationPetitionTemplate(path, "DMUN");
                Assert.NotNull(template);
                Assert.AreEqual("DMUN", template.TemplateName);
                Assert.IsTrue(template.Entries.Any());
                template.Entries.ForEach(n => Console.WriteLine(n.Name));
                Assert.AreEqual(15, template.Entries.Count);
                service.ApplyPetitionTemplateToSimulation(template, simulationId);
                var saved = service.GetPetitionTypesOfSimulation(simulationId);
                Assert.NotNull(saved);
                Assert.IsTrue(saved.Any());
                Assert.AreEqual(15, saved.Count);
            }
            else
            {
                Assert.Ignore("The DMUN2.csv was not found that is needed for this test.");
                skipTestsDependingOnDMUNFile = true;
            }
        }

        [Test]
        [Order(11)]
        public async Task TestCreateAgendaItem()
        {
            if (skipTestsDependingOnDMUNFile)
            {
                Assert.Ignore("Ignored because the DMUN File was not found!");
                return;
            }

            var service = new SimulationService(_context);
            var dto = new CreateAgendaItemDto()
            {
                Name = "Test",
                Description = "Description",
                SimulationId = simulationId
            };
            var result = await service.CreateAgendaItem(dto);
            Assert.NotNull(result);
            var recall = await service.GetAgendaItems(simulationId);
            Assert.IsTrue(recall.Any());
            var recallWithPetitions = await service.GetAgendaItemsAndPetitionsDto(simulationId);
            Assert.IsTrue(recallWithPetitions.Any());
        }

        [Test]
        [Order(12)]
        public async Task TestMakePetition()
        {
            if (skipTestsDependingOnDMUNFile)
            {
                Assert.Ignore("Ignored because the DMUN File was not found!");
                return;
            }

            var service = new SimulationService(_context);
            var petitionTypes = await service.GetPetitionTypes();
            if (!petitionTypes.Any())
            {
                Assert.Ignore("No petition types, test skipped, someone should fix this testcase!");
                return;
            }
            var user = await service.GetSimulationUsers(simulationId).FirstOrDefaultAsync();
            var agendaItems = await service.GetAgendaItems(simulationId);

            Assert.IsTrue(petitionTypes.Any());
            Assert.NotNull(user);
            Assert.IsTrue(agendaItems.Any());

            var dto = new CreatePetitionRequest()
            {
                PetitionDate = DateTime.Now,
                PetitionTypeId = petitionTypes.First().PetitionTypeId,
                PetitionUserId = user.SimulationUserId,
                SimulationId = simulationId,                                    // SimulationId should be optional
                Status = MUNitySchema.Models.Simulation.EPetitionStates.Unkown,
                TargetAgendaItemId = agendaItems.First().AgendaItemId,
                Text = "More info",
            };

            var created = service.SubmitPetition(dto);
            Assert.NotNull(created);
            var recall = await service.GetAgendaItemsAndPetitionsDto(simulationId);
            Assert.NotNull(recall);
            Assert.IsTrue(recall.Any());
            Assert.IsTrue(recall.First().Petitions.Any());
        }

    }
}