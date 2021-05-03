using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Hubs;
using MUNityCore.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Schema.Simulation;
using MUNityCore.Extensions.CastExtensions;
using Microsoft.AspNetCore.Cors;

namespace MUNityCore.Controllers.Simulation
{
    /// <summary>
    /// Controller to handle roles of a simulation.
    /// </summary>
    [ApiController]
    [Route("api/Simulation/Roles")]
    public class SimulationRolesController : ControllerBase, ISimulationController
    {
        public IHubContext<SimulationHub, ITypedSimulationHub> HubContext { get; set; }

        private Services.SimulationService _simulationService;

        public SimulationRolesController(IHubContext<SimulationHub, ITypedSimulationHub> hubContext, Services.SimulationService simulationService)
        {
            this.HubContext = hubContext;
            this._simulationService = simulationService;
        }

        /// <summary>
        /// Creates a new role for a simulation.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<SimulationRoleDto>> CreateRole([FromBody]CreateRoleRequest body)
        {
            var isAllowed = await _simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed) return BadRequest();

            Models.Simulation.SimulationRole role = _simulationService.CreateRole(body);
            var roles = await _simulationService.GetSimulationRolesAsync(body.SimulationId);
            _ = GetSimulationHub(body)?.RolesChanged(new RolesChangedEventArgs(body.SimulationId, roles.Select(n => n.ToSimulationRoleItem())));
            return Ok(role.ToSimulationRoleItem());
        }

        /// <summary>
        /// Returns all the roles of a simulation.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<SimulationRoleDto>> GetSimulationRoles([FromHeader] string simsimtoken, int id)
        {
            var isAllowed = this._simulationService.IsTokenValid(id, simsimtoken);
            if (!isAllowed) 
                return Forbid();
            var roles = this._simulationService.GetSimulationRoles(id);
            var models = roles.Select(n => n.ToSimulationRoleDto()).ToList();
            return Ok(models);
        }

        /// <summary>
        /// Changes the role of a user. Note that only the owner (first user of the simulation) or users
        /// that have a role with the type Chairman are allowed to perform this action.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> SetUserRole([FromBody]SetUserSimulationRole body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!isAllowed) return BadRequest();

            bool success = this._simulationService.SetUserRole(body);
            if (!success) return NotFound("Role or user not found!");

            GetSimulationHub(body)?.UserRoleChanged(new UserRoleChangedEventArgs(body.SimulationId, body.UserId, body.RoleId));
            return Ok();   
        }

        private ITypedSimulationHub GetSimulationHub(SimulationRequest request)
        {
            return this.HubContext?.Clients?.Group($"sim_{request.SimulationId}");
        }
    }
}
