using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using MUNityCore.Util.Extensions;
using MUNityCore.Models.Organization;
using MUNityCore.Models.Simulation;
using MUNityCore.Schema.Request.Simulation;
using MUNityCore.Schema.Response.Simulation;
using MUNityCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The Controller for Simulations (online committees) etc.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SimulationController : ControllerBase
    {

        private readonly IHubContext<Hubs.SimulationHub, Hubs.ITypedSimulationHub> _hubContext;

        private readonly SimulationService _simulationService;

        public SimulationController(IHubContext<Hubs.SimulationHub, Hubs.ITypedSimulationHub> hubContext,
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
            return Ok("alpha-0.0.1");
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<SimulationListItem>> GetListOfSimulations()
        {
            var result = this._simulationService.GetSimulations().Select(n => (SimulationListItem)n);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimSimResponse> GetSimulation([FromHeader]string simsimtoken, int id)
        {
            var simulation = this._simulationService.GetSImulationWithHubsUsersAndRoles(id);
            if (simulation == null) return NotFound();
            var users = this._simulationService.GetSimulationUsers(id);
            if (!users.Any(n => n.Token == simsimtoken)) return Forbid();
            return Ok((SimSimResponse)simulation);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SimulationRoleItem>>> GetSimulationRoles([FromHeader]string simsimtoken, int id)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(id);
            if (simulation == null) return NotFound();
            if (simulation.Users.All(n => n.Token != simsimtoken)) return Forbid();
            var roles = simulation.Roles.Select(n => new SimulationRoleItem(n, simulation.Users.Where(a => a.Role == n)));
            return Ok(roles);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimulationAuthSchema> GetSimulationAuth([FromHeader]string simsimtoken, int id)
        {
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null) return NotFound();
            return Ok((SimulationAuthSchema)user);
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
                simulation.Phase = (Simulation.GamePhases)phase;
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
        public async Task<ActionResult<SimSimTokenResponse>> JoinSimulation(int id, [FromBody]JoinAuthenticate request)
        {
            if (string.IsNullOrWhiteSpace(request.DisplayName)) return BadRequest();
            var simulation = await this._simulationService.GetSimulation(id);
            if (simulation == null) return NotFound();
            if (simulation.Users.Any(n => n.DisplayName == request.DisplayName)) return BadRequest();
            if (!string.IsNullOrEmpty(simulation.Password) && simulation.Password != request.Password)
                return Forbid();
            var user = this._simulationService.JoinSimulation(simulation, request.DisplayName);
            return Ok((SimSimTokenResponse)user);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<SimSimTokenResponse> CreateSimulation([FromBody]SimulationRequests.CreateSimulation request)
        {
            var result = this._simulationService.CreateSimulation(request.Name, request.Password, request.UserDisplayName, request.AdminPassword);
            var admin = result.Users.First();
            var response = new SimSimTokenResponse()
            {
                Name = result.Name,
                Token = admin.Token,
                SimulationId = result.SimulationId,
                Pin = admin.Pin
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Subscribe([FromHeader]string simsimtoken, int id, string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
                return BadRequest();
            var user = this._simulationService.GetSimulationUser(id, simsimtoken);
            if (user == null) return Forbid();
            await this._hubContext.Groups.AddToGroupAsync(connectionId, $"sim_{id}");
            var mdl = new SimulationHubConnection()
            {
                User = user,
                ConnectionId = connectionId,
                CreationDate = DateTime.Now
            };
            user.HubConnections.Add(mdl);
            this._simulationService.SaveDbChanges();
            await this._hubContext.Clients.Group($"sim_{id}").UserConnected(id, user);
            return Ok(true);
        }

    }
}