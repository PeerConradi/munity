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
        public ActionResult<SimulationResponse> GetSimulation([FromHeader]string simsimtoken, int id)
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
        public ActionResult<SimulationUserSetup> GetUsersAsAdmin([FromHeader]string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null || user.CanCreateRole == false) return Forbid();
            var users = this._simulationService.GetSimulationUsers(id);
            users.Include(n => n.Role)
                .Include(n => n.HubConnections)
                .Include(n => n.Simulation);
            var result = users.Select(n => n.AsUserSetup());
            return Ok(result);
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
                .Include(n => n.Simulation);
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
        public async Task<ActionResult> ApplyPreset([FromHeader]string simsimtoken, int simulationId, string presetId)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(simulationId);
            if (simulation == null) return NotFound();
            if (!simulation.Users.Any(n => n.Token == simsimtoken && n.CanCreateRole)) return Forbid();
            var preset = this._simulationService.Presets.FirstOrDefault(n => n.Id == presetId);
            if (preset == null) return NotFound();
            this._simulationService.ApplyPreset(simulation, preset);
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

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> MakeRequest([FromHeader]string simsimtoken, int simulationId, string request)
        {
            var user = this._simulationService.GetSimulationUser(simulationId, simsimtoken);
            if (user == null) return Forbid();
            await this._hubContext.Clients.Group($"sim_{simulationId}").UserRequest(simulationId, user.SimulationUserId, request);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> AcceptRequest([FromHeader]string simsimtoken, int simulationId, int userId, string request)
        {
            var user = this._simulationService.GetSimulationUser(simulationId, simsimtoken);
            if (!user.CanCreateRole) return Forbid();
            await this._hubContext.Clients.Group($"sim_{simulationId}").UserRequestAccepted(simulationId, userId, request);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteRequest([FromHeader]string simsimtoken, int simulationId, int userId, string request)
        {
            var user = this._simulationService.GetSimulationUser(simulationId, simsimtoken);
            if (userId != user.SimulationUserId && !user.CanCreateRole) return Forbid();
            await this._hubContext.Clients.Group($"sim_{simulationId}").UserRequestDeleted(simulationId, userId, request);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimulationTokenResponse>> JoinSimulation(int id, [FromBody]JoinAuthenticate request)
        {
            if (string.IsNullOrWhiteSpace(request.DisplayName)) return BadRequest();
            var simulation = await this._simulationService.GetSimulation(id);
            if (simulation == null) return NotFound();
            if (simulation.Users.Any(n => n.DisplayName == request.DisplayName)) return BadRequest();
            if (!string.IsNullOrEmpty(simulation.Password) && simulation.Password != request.Password)
                return Forbid();
            var user = this._simulationService.JoinSimulation(simulation, request.DisplayName);
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