using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MUNity.Schema.Simulation;
using MUNityCore.Controllers;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Hubs;
using MUNityCore.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityTest.SimulationTest
{
    [TestFixture]
    public class SimulationControllerTest
    {
        private MunityContext _context;

        private SimulationService _service;

        private Mock<IHubContext<SimulationHub, MUNity.Hubs.ITypedSimulationHub>> _mockedSimulationHub;

        private int simulationId;

        private string token;

        [OneTimeSetUp]
        public void Setup()
        {
            // Datenbank für den Test erzeugen und falls vorhanden erst einmal leeren und neu erstellen!
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite("Data Source=test_simulation.db");
            _context = new MunityContext(optionsBuilder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            this._service = new SimulationService(_context);
            this._mockedSimulationHub = new Mock<IHubContext<SimulationHub, MUNity.Hubs.ITypedSimulationHub>>();
        }

        [Test]
        [Order(0)]
        [Description("Should created a simulation that can be used for future tests.")]
        public void TestCreatingASimulation()
        {
            var controller = new SimulationController(_mockedSimulationHub.Object, _service);
            var request = new MUNity.Schema.Simulation.CreateSimulationRequest() 
            {
                Name=  "Test", 
                AdminPassword = "Password",
                UserDisplayName = "Peer"
            };
            var result = controller.CreateSimulation(request);
            Assert.NotNull(result);
            Assert.IsTrue(result.Result is OkObjectResult);
            var resultObject = result.Result as OkObjectResult;
            Assert.NotNull(resultObject);
            var createdResponse = resultObject.Value as CreateSimulationResponse;
            Assert.NotNull(createdResponse);
            Assert.NotNull(createdResponse.FirstUserToken);
            this.simulationId = createdResponse.SimulationId;
            this.token = createdResponse.FirstUserToken;
            System.Console.WriteLine($"Created a test simulation {simulationId} with token {this.token}");
        }

        [Test]
        [Order(1)]
        public void TestSimulationIsInLobbyList()
        {
            var controller = new SimulationController(_mockedSimulationHub.Object, _service);
            var response = controller.GetListOfSimulations();
            Assert.NotNull(response);
            Assert.IsTrue(response.Result is OkObjectResult, $"Expected the response to be of OkObjectResult but was: {response.Result.GetType()}");
            var okResult = response.Result as OkObjectResult;
            var content = okResult.Value as IEnumerable<SimulationListItemDto>;
            Assert.NotNull(content, "Expected a list of SimulationListItemDto...");
            Assert.IsTrue(content.Any(n => n.SimulationId == simulationId));
        }

        [Test]
        [Order(2)]
        public async Task TestGetSimulation()
        {
            var simulation = await GetTestSimulation();
            Assert.NotNull(simulation);
        }

        [Test]
        [Order(3)]
        public async Task TestCreateNewUser()
        {
            var isOwner = await this._service.IsTokenValidAndUserAdmin(simulationId, token);
            Assert.IsTrue(isOwner, "Current user should be owner");
            var startUsers = await GetTestUsersAdminDto();
            Assert.AreEqual(1, startUsers.Count);
            var controller = GetTestController();
            var isOwnerStill = await this._service.IsTokenValidAndUserAdmin(simulationId, token); 
            
            Assert.IsTrue(isOwnerStill, "Current user should be owner");

            // Main Action
            PrintUserTable();
            var result = await controller.CreateUser(token, simulationId);
            System.Console.WriteLine("------");
            PrintUserTable();
            var me = _context.SimulationUser.FirstOrDefault(n => n.Token == token);
            var isOwnerNow = await this._service.IsTokenValidAndUserAdmin(simulationId, token);
            
            Assert.IsTrue(isOwnerNow, "Current user should still be owner");
            var okObjetResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjetResult, $"Expected an okObjectResult for successful created a new user but got: {result.Result.GetType().Name}");
            var createdUser = okObjetResult.Value as SimulationUserAdminDto;
            Assert.NotNull(createdUser);
            var usersNow = await GetTestUsersAdminDto();
            Assert.NotNull(usersNow, "Error when getting the Users another time see logs!");
            Assert.AreEqual(2, usersNow.Count);
        }

        private void PrintUserTable()
        {
            var users = _context.SimulationUser.Include(n => n.Simulation).ToList();
            var text = users.Select(a => $"{a.SimulationUserId}\t{a.Token}\t{a.CanCreateRole}\t" + ((a.Simulation == null) ? "Keine Simulation" : a.Simulation.SimulationId));
            var fullText = string.Join("\n", text);
            System.Console.WriteLine(fullText);
        }

        private async Task<SimulationDto> GetTestSimulation()
        {
            var controller = new SimulationController(_mockedSimulationHub.Object, _service);
            var response = await controller.Simulation(token, simulationId);
            var objectResult = response.Result as OkObjectResult;
            return objectResult?.Value as SimulationDto;
        }

        private SimulationController GetTestController()
        {
            return new SimulationController(_mockedSimulationHub.Object, _service);
        }

        private async Task<List<SimulationUserAdminDto>> GetTestUsersAdminDto()
        {
            var controller = GetTestController();
            var response = await controller.GetUsersAsAdmin(token, simulationId);
            var okResult = response.Result as OkObjectResult;
            if (okResult == null)
            {
                System.Console.WriteLine($"Expected an OkObjectResult but got: {response.Result.GetType().Name} on simulation {this.simulationId} with token {this.token}");
                var allowedTokens = this._context.SimulationUser.Select(n => n.Token);
                var tokenServiceResponse = await this._service.IsTokenValidAndUserChairOrOwner(simulationId, token);
                var isChair = await this._service.IsTokenValidAndUserChair(simulationId, token);
                var isOwner = await this._service.IsTokenValidAndUserAdmin(simulationId, token);
                System.Console.WriteLine("Is Chair: " + isChair.ToString());
                System.Console.WriteLine("Is Owner: " + isOwner.ToString());
                System.Console.WriteLine("Token is chair or owner: " + tokenServiceResponse.ToString());
                System.Console.WriteLine("Registered Tokens: " + string.Join(", ",allowedTokens));
                return null;
            }
            var users = okResult.Value as List<SimulationUserAdminDto>;
            return users;
        }
    }
}