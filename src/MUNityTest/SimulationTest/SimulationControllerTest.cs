using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Managment;
using MUNityCore.Controllers;
using MUNityCore.Controllers.Simulation;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Hubs;
using MUNityCore.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityTest.SimulationTest
{
    [TestFixture(Author = "Peer Conradi", Category = "Simulation", 
        Description = "Testing the Simulation Controller Endpoints with an SQLite Database", 
        TestName = "SimulationControllerTest")]
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
        public void T00CreatingASimulation()
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
        public void T01SimulationIsInLobbyList()
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
        public async Task T02GetSimulation()
        {
            var simulation = await GetTestSimulation();
            Assert.NotNull(simulation);
        }

        [Test]
        [Order(3)]
        public async Task T03CreateNewUser()
        {
            var startUsers = await GetTestUsersAdminDto();
            Assert.AreEqual(1, startUsers.Count);
            var controller = new SimulationUserController(_mockedSimulationHub.Object, _service);
            var body = new SimulationRequest()
            {
                SimulationId = simulationId,
                Token = token
            };
            var result = await controller.CreateUser(body);
            var okObjetResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjetResult, $"Expected an okObjectResult for successful created a new user but got: {result.Result.GetType().Name}");
            var createdUser = okObjetResult.Value as SimulationUserAdminDto;
            Assert.NotNull(createdUser);
            var usersNow = await GetTestUsersAdminDto();
            Assert.NotNull(usersNow, "Error when getting the Users another time see logs!");
            Assert.AreEqual(2, usersNow.Count);
        }

        [Test]
        [Order(4)]
        public async Task T04GetRolesAreEmpty()
        {
            var roles = await GetTestSimulationRoles();
            Assert.NotNull(roles);
            Assert.IsFalse(roles.Any());
        }

        [Test]
        [Order(5)]
        public async Task T05CreateChairmanRole()
        {
            var request = new CreateRoleRequest()
            {
                Iso = "un",
                Name = "Vorsitzender",
                RoleType = RoleTypes.Chairman,
                SimulationId = simulationId,
                Token = token
            };
            var controller = new SimulationRolesController(_mockedSimulationHub.Object, _service);
            var response = await controller.CreateRole(request);
            Assert.NotNull(response);
            var objectResult = response.Result as OkObjectResult;
            Assert.NotNull(objectResult);
            var newRole = objectResult.Value as SimulationRoleDto;
            Assert.NotNull(newRole);
            Assert.AreEqual("un", newRole.Iso);
            Assert.AreEqual("Vorsitzender", newRole.Name);
            Assert.AreEqual(RoleTypes.Chairman, newRole.RoleType);

            var roles = await GetTestSimulationRoles();
            Assert.NotNull(roles);
            Assert.IsTrue(roles.Any(n => n.RoleType == RoleTypes.Chairman));
        }

        [Test]
        [Order(6)]
        public async Task T06CreateDelegateRole()
        {
            var request = new CreateRoleRequest()
            {
                Iso = "de",
                Name = "Deutschland",
                RoleType = RoleTypes.Delegate,
                SimulationId = simulationId,
                Token = token
            };
            var controller = new SimulationRolesController(_mockedSimulationHub.Object, _service);
            var response = await controller.CreateRole(request);
            Assert.NotNull(response);
            var objectResult = response.Result as OkObjectResult;
            Assert.NotNull(objectResult);
            var newRole = objectResult.Value as SimulationRoleDto;
            Assert.NotNull(newRole);
            Assert.AreEqual("de", newRole.Iso);
            Assert.AreEqual("Deutschland", newRole.Name);
            Assert.AreEqual(RoleTypes.Delegate, newRole.RoleType);

            var roles = await GetTestSimulationRoles();
            Assert.NotNull(roles);
            Assert.IsTrue(roles.Any(n => n.RoleType == RoleTypes.Delegate));
            Assert.AreEqual(2, roles.Count);
        }

        [Test]
        [Order(7)]
        public async Task T07SetOwnerRoleToChairman()
        {
            var users = await GetTestUsersAdminDto();
            var roles = await GetTestSimulationRoles();
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(2, roles.Count);

            var owner = users.FirstOrDefault(n => n.DisplayName == "Peer");
            var chairRole = roles.FirstOrDefault(n => n.RoleType == RoleTypes.Chairman);
            Assert.NotNull(owner);

            var controller = new SimulationRolesController(_mockedSimulationHub.Object, _service);
            var body = new SetUserSimulationRole()
            {
                RoleId = chairRole.SimulationRoleId,
                SimulationId = simulationId,
                Token = token,
                UserId = owner.SimulationUserId
            };

            var result = await controller.SetUserRole(body);
            Assert.IsTrue(result is OkResult);

            var regetUsers = await GetTestUsersAdminDto();
            var regetOwner = regetUsers.FirstOrDefault(n => n.DisplayName == "Peer");
            Assert.AreEqual(chairRole.SimulationRoleId, regetOwner.RoleId);
        }

        [Test]
        [Order(8)]
        public async Task T08SetOtherUserToDelegateRole()
        {
            var users = await GetTestUsersAdminDto();
            var roles = await GetTestSimulationRoles();
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(2, roles.Count);

            var delegateUser = users.FirstOrDefault(n => n.DisplayName != "Peer");
            var delegateRole = roles.FirstOrDefault(n => n.RoleType == RoleTypes.Delegate);
            Assert.NotNull(delegateUser);
            Assert.NotNull(delegateRole);

            var controller = new SimulationRolesController(_mockedSimulationHub.Object, _service);
            var body = new SetUserSimulationRole()
            {
                RoleId = delegateRole.SimulationRoleId,
                SimulationId = simulationId,
                Token = token,
                UserId = delegateUser.SimulationUserId
            };

            var result = await controller.SetUserRole(body);
            Assert.IsTrue(result is OkResult);

            var regetUsers = await GetTestUsersAdminDto();
            var regetDelegate = regetUsers.FirstOrDefault(n => n.DisplayName != "Peer");
            Assert.AreEqual(delegateRole.SimulationRoleId, regetDelegate.RoleId);
        }

        [Test]
        [Order(9)]
        public async Task T09CreatePetitionType()
        {
            var body = new CreatePetitionTypeRequest()
            {
                Category = "Persönlicher Antrag",
                Description = "Recht auf Information",
                Name = "Recht auf Information",
                Reference = "§1",
                Ruling = PetitionRulings.Chairs,
                SimulationId = simulationId,
                Token = token
            };
            var controller = new PetitionController(_mockedSimulationHub.Object, _service);
            var result = await controller.CreatePetitionType(body);
            Assert.NotNull(result, $"Expected a result but got nothing");
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult, $"Expected an okObjectResult but got {result.Result.GetType().Name}");
            var petition = okObjectResult.Value as PetitionTypeDto;
            Assert.NotNull(petition, $"Expected an PetitionTypeDto but got {okObjectResult.Value.GetType().Name}");
            Assert.AreEqual("Persönlicher Antrag", petition.Category);
            Assert.AreEqual("Recht auf Information", petition.Description);
            Assert.AreEqual("Recht auf Information", petition.Name);
            Assert.AreEqual("§1", petition.Reference);
            Assert.AreEqual(PetitionRulings.Chairs, petition.Ruling);

            var allPetitionTypes = await GetAllPetitionTypes();
            Assert.NotNull(allPetitionTypes, "Expected something from all petition types but got nothing.");
            Assert.IsTrue(allPetitionTypes.Any());
        }

        [Test]
        [Order(10)]
        public async Task T10AddPetitionTypeToSimulation()
        {
            var controller = new PetitionController(_mockedSimulationHub.Object, _service);
            var petitionTypes = await GetAllPetitionTypes();
            Assert.NotNull(petitionTypes);
            var petitionType = petitionTypes.FirstOrDefault();
            Assert.NotNull(petitionType);

            var body = new AddPetitionTypeRequestBody()
            {
                AllowChairs = false,
                AllowDelegates = true,
                AllowNgo = true,
                AllowSpectator = false,
                OrderIndex = 1,
                PetitionTypeId = petitionType.PetitionTypeId,
                SimulationId = simulationId,
                Token = token
            };

            var result = await controller.AddPetitionTypeToSimulation(body);
            Assert.IsTrue(result is OkResult);

            var getSimPetitionTypesResult = await controller.SimulationPetitionTypes(token, simulationId);
            Assert.NotNull(getSimPetitionTypesResult);
            var okObjectResult = getSimPetitionTypesResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult, $"Expected Sim Petition Types to be okObjectResult but was {getSimPetitionTypesResult.Result.GetType().Name}");
            var simPetitionTypes = okObjectResult.Value as List<PetitionTypeSimulationDto>;
            Assert.NotNull(simPetitionTypes);
            Assert.IsTrue(simPetitionTypes.Any());
        }

        [Test]
        [Order(11)]
        public async Task T11GetSlots()
        {
            var controller = GetTestController();
            var result = await controller.Slots(token, simulationId);
            Assert.NotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var slotList = okResult.Value as List<SimulationSlotDto>;
            Assert.NotNull(slotList);
            Assert.AreEqual(2, slotList.Count);
            Assert.AreEqual("Peer", slotList[0].DisplayName);
            Assert.AreEqual("Vorsitzender", slotList[0].RoleName);
        }

        [Test]
        [Order(12)]
        public async Task T12CreateThirdUserNoRole()
        {
            var startUsers = await GetTestUsersAdminDto();
            Assert.AreEqual(2, startUsers.Count);
            var controller = new SimulationUserController(_mockedSimulationHub.Object, _service);
            var body = new SimulationRequest()
            {
                SimulationId = simulationId,
                Token = token
            };
            var result = await controller.CreateUser(body);
            var okObjetResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjetResult, $"Expected an okObjectResult for successful created a new user but got: {result.Result.GetType().Name}");
            var createdUser = okObjetResult.Value as SimulationUserAdminDto;
            Assert.NotNull(createdUser);
            var usersNow = await GetTestUsersAdminDto();
            Assert.NotNull(usersNow, "Error when getting the Users another time see logs!");
            Assert.AreEqual(3, usersNow.Count);
        }

        [Test]
        [Order(13)]
        [Description("This testcase contains a user slot that has not selected a role")]
        public async Task T13GetSlotsWithEmptyRole()
        {
            var controller = GetTestController();
            var result = await controller.Slots(token, simulationId);
            Assert.NotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            var slotList = okResult.Value as List<SimulationSlotDto>;
            Assert.NotNull(slotList);
            Assert.AreEqual(3, slotList.Count);
            Assert.IsTrue(string.IsNullOrEmpty(slotList[2].DisplayName));
            Assert.IsTrue(string.IsNullOrEmpty(slotList[2].RoleName));
            Assert.AreEqual(-2, slotList[2].RoleId);
        }

        private async Task<List<SimulationRoleDto>> GetTestSimulationRoles()
        {
            var controller = new SimulationRolesController(_mockedSimulationHub.Object, _service);
            var result = await controller.GetSimulationRoles(token, simulationId);
            var okObjectResult = result.Result as OkObjectResult;
            if (okObjectResult == null) return null;
            return okObjectResult.Value as List<SimulationRoleDto>;
        }

        private async Task<List<PetitionTypeDto>> GetAllPetitionTypes()
        {
            var controller = new PetitionController(_mockedSimulationHub.Object, _service);
            var result = await controller.AllPetitionTypes();
            var okObjectResult = result.Result as OkObjectResult;
            if (okObjectResult == null) return null;
            return okObjectResult.Value as List<PetitionTypeDto>;
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
                return null;
            }
            var users = okResult.Value as List<SimulationUserAdminDto>;
            return users;
        }
    }
}