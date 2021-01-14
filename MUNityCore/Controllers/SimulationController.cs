using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityCore.Models.Simulation;
using MUNityCore.Services;
using Microsoft.AspNetCore.Authorization;
using MUNity.Schema.Simulation;
using MUNityCore.Extensions.CastExtensions;
using Microsoft.EntityFrameworkCore;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The Controller for Simulations (online committees) etc.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationController : ControllerBase
    {

        private readonly IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> _hubContext;

        private readonly SimulationService _simulationService;

        public SimulationController(IHubContext<Hubs.SimulationHub, MUNity.Hubs.ITypedSimulationHub> hubContext,
            SimulationService simulationService)
        {
            this._hubContext = hubContext;
            this._simulationService = simulationService;
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<string> IsOnline()
        {
            return Ok("alpha-0.0.2");
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<SimulationListItem>> GetListOfSimulations()
        {
            var result = this._simulationService.GetSimulations().Select(n => n);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationResponse> GetSimulation([FromHeader] string simsimtoken, int id)
        {
            var simulation = this._simulationService.GetSImulationWithHubsUsersAndRoles(id);
            if (simulation == null) return NotFound();
            var users = this._simulationService.GetSimulationUsers(id);
            if (!users.Any(n => n.Token == simsimtoken)) return Forbid();
            return Ok(simulation.AsResponse());
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<string> GetListOfSpeakersId([FromHeader] string simsimtoken, int simulationId)
        {
            var currentUser = this._simulationService.GetSimulationUser(simulationId, simsimtoken);
            if (currentUser == null) return null;
            return this._simulationService.GetSpeakerlistIdOfSimulation(simulationId);
        }

        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult InitVoting([FromBody]object model)
        {
            // Create Model
            // Token
            // SimulationId
            // Name
            // AllowAbstention

            // Send Model to all sockets
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult Vote([FromBody]object vote)
        {
            // SImulationId
            // Token
            // VoteValue (option)

            // Send vote to all sockets
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<string> GetResolutionId([FromHeader] string simsimtoken, int simulationId)
        {
            var currentUser = this._simulationService.GetSimulationUser(simulationId, simsimtoken);
            if (currentUser == null) return null;
            return this._simulationService.GetSpeakerlistIdOfSimulation(simulationId);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<MUNity.Models.ListOfSpeakers.ListOfSpeakers> InitListOfSpeakers([FromHeader] string simsimtoken, int simulationId)
        {
            var currentUser = this._simulationService.GetSimulationUserWithRole(simulationId, simsimtoken);
            if (currentUser == null || (!currentUser.CanCreateRole && currentUser.Role.RoleType != SimulationRole.RoleTypes.Chairman)) return Forbid();
            // TODO: Send Socket information.
            return Ok(this._simulationService.InitListOfSpeakers(simulationId));
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationUserSetup>> CreateUser([FromHeader]string simsimtoken, int id)
        {
            var currentUser = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (currentUser.CanCreateRole == false) return Forbid();
            var simulation = await this._simulationService.GetSimulation(id);
            if (simulation == null) return NotFound();
            var newUser = this._simulationService.CreateUser(simulation, "");
            return Ok(newUser.AsUserSetup());
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationUserSetup> GetUsersAsAdmin([FromHeader]string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null || user.CanCreateRole == false) return Forbid();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.AsUserSetup()).ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<bool> IsUserOnline([FromHeader]string simsimtoken, int simulationId, int userId)
        {
            var user = this._simulationService.GetSimulationUser(simulationId, simsimtoken);
            if (user == null || user.CanCreateRole == false) return Forbid();
            return Ok(this._simulationService.UserOnline(simulationId, userId));
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationUserItem> GetUsersDefault([FromHeader]string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null) return Forbid();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation).ToList();
            var result = users.Select(n => n.AsUserItem());
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SimulationRoleItem>>> GetSimulationRoles([FromHeader]string simsimtoken, int id)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(id);
            if (simulation == null) return NotFound();
            if (simulation.Users.All(n => n.Token != simsimtoken)) return Forbid();
            var roles = simulation.Roles.Select(n => n.AsRoleItem());
            return Ok(roles);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationAuthSchema> GetSimulationAuth([FromHeader]string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null) return NotFound();
            return Ok(user.AsAuthSchema());
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> PickRole([FromHeader]string simsimtoken, int simulationId, int roleId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            var user = simulation.Users.FirstOrDefault(n => n.Token == simsimtoken);
            if (user == null) return NotFound();
            if (!user.CanSelectRole) return Forbid();
            var role = simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            if (role == null) return NotFound();
            this._simulationService.BecomeRole(simulation, user, role);
            await this._hubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(simulationId, user.SimulationUserId, roleId);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> SetUserRole([FromHeader]string simsimtoken, int simulationId, int userId, int roleId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            var user = simulation.Users.FirstOrDefault(n => n.Token == simsimtoken);
            if (user == null || user.CanCreateRole == false) return Forbid();
            var targetUser = simulation.Users.FirstOrDefault(n => n.SimulationUserId == userId);
            if (targetUser == null) return NotFound();
            if (roleId == -2)
            {
                this._simulationService.BecomeRole(simulation, targetUser, null);
                await this._hubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(simulationId, targetUser.SimulationUserId, roleId);
                return Ok();
            }
            var targetRole = simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            if (targetRole == null) return NotFound();

            this._simulationService.BecomeRole(simulation, targetUser, targetRole);
            await this._hubContext.Clients.Group($"sim_{simulationId}").UserRoleChanged(simulationId, targetUser.SimulationUserId, roleId);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyPreset([FromHeader]string simsimtoken, int simulationId, string presetId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole)) return Forbid();
            var preset = this._simulationService.Presets.FirstOrDefault(n => n.Id == presetId);
            if (preset == null) return NotFound();
            this._simulationService.ApplyPreset(simulation, preset);
            await this._hubContext.Clients.Group($"sim_{simulationId}").RolesChanged(simulationId, simulation.Roles.Select(n => n.AsRoleItem()));
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Models.Simulation.Presets.ISimulationPreset>> GetPresets()
        {
            return Ok(this._simulationService.Presets);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> SetPhase([FromHeader]string simsimtoken, int simulationId, int phase)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole))
                return Forbid();
            try
            {
                simulation.Phase = (SimulationEnums.GamePhases)phase;
                this._simulationService.SaveDbChanges();
                await this._hubContext.Clients.Group($"sim_{simulationId}").PhaseChanged(simulationId, simulation.Phase);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> MakePetition([FromBody]Petition petition)
        {
            var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
            if (user == null) return Forbid();
            petition.PetitionUserId = user.SimulationUserId;
            await this._hubContext.Clients.Group($"sim_{petition.SimulationId}").UserPetition(petition);
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> AcceptPetition([FromBody]Petition petition)
        {
            var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
            if (!user.CanCreateRole) return Forbid();
            await this._hubContext.Clients.Group($"sim_{petition.SimulationId}").UserPetitionAccepted(petition);
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> DeletePetition([FromBody] Petition petition)
        {
            var user = this._simulationService.GetSimulationUser(petition.SimulationId, petition.Token);
            if (petition.PetitionUserId != user.SimulationUserId && !user.CanCreateRole) return Forbid();
            await this._hubContext.Clients.Group($"sim_{petition.SimulationId}").UserPetitionDeleted(petition);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationTokenResponse>> JoinSimulation([FromBody]JoinAuthenticate request)
        {
            if (string.IsNullOrWhiteSpace(request.DisplayName)) return BadRequest();
            var simulation = await this._simulationService.GetSimulation(request.SimulationId);
            if (simulation == null) return NotFound();
            var user = this._simulationService.GetSimulationUserByPublicId(request.SimulationId, request.UserId);
            if (user == null) return NotFound();
            //if (simulation.Users.Any(n => n.DisplayName == request.DisplayName)) return BadRequest();
            if (user.Password != request.Password)
                return Forbid();

            if (!string.IsNullOrEmpty(request.DisplayName))
            {
                user.DisplayName = request.DisplayName;
                this._simulationService.SaveDbChanges();
            }
            if (string.IsNullOrEmpty(user.Token))
            {
                user.Token = Util.Tools.IdGenerator.RandomString(20);
                this._simulationService.SaveDbChanges();
            } 

            return Ok(user.AsTokenResponse());
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<CreateSimulationResponse> CreateSimulation([FromBody]CreateSimulationRequest request)
        {
            var result = this._simulationService.CreateSimulation(request.Name, request.AdminPassword);
            var moderator = this._simulationService.CreateModerator(result, request.UserDisplayName ?? "");
            var admin = result.Users.First();
            var response = new CreateSimulationResponse()
            {
                FirstUserId = moderator.PublicUserId,
                FirstUserPassword = moderator.Password,
                FirstUserToken = moderator.Token,
                SimulationId = result.SimulationId,
                SimulationName = result.Name,
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Subscribe([FromBody]SubscribeSimulation body)
        {
            if (string.IsNullOrEmpty(body.ConnectionId))
                return BadRequest();
            var user = this._simulationService.GetSimulationUser(body.SimulationId, body.Token);
            if (user == null) return Forbid();
            await this._hubContext.Groups.AddToGroupAsync(body.ConnectionId, $"sim_{body.SimulationId}");
            var mdl = new SimulationHubConnection()
            {
                User = user,
                ConnectionId = body.ConnectionId,
                CreationDate = DateTime.Now
            };
            user.HubConnections.Add(mdl);
            this._simulationService.SaveDbChanges();
            await this._hubContext.Clients.Group($"sim_{body.SimulationId}").UserConnected(body.SimulationId, user.AsUserItem());
            return Ok(true);
        }

    }
}