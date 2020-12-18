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
        public async Task<ActionResult<SimSimResponse>> GetSimulation([FromHeader]string simsimtoken, int id)
        {
            var simulation = await this._simulationService.GetSimulationWithUsersAndRoles(id);
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


        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<SimSimTokenResponse>> JoinSimulation([FromHeader]string password, [FromHeader]string displayName, int id)
        {
            if (string.IsNullOrWhiteSpace(displayName)) return BadRequest();
            var simulation = await this._simulationService.GetSimulation(id);
            if (simulation == null) return NotFound();
            if (simulation.Users.Any(n => n.DisplayName == displayName)) return BadRequest();
            if (!string.IsNullOrEmpty(simulation.Password) && simulation.Password != password)
                return Forbid();
            var user = this._simulationService.JoinSimulation(simulation, displayName);
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

            return Ok(true);
        }

    }
}