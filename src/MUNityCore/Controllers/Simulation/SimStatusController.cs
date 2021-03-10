using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Hubs;
using MUNity.Schema.Simulation;
using MUNityCore.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Simulation
{
    /// <summary>
    /// Controller to handle the status of a simulation.
    /// </summary>
    [Route("api/Simulation/Status")]
    [ApiController]
    public class SimStatusController : ControllerBase, ISimulationController
    {
        private Services.SimulationService _simulationService;

        public IHubContext<SimulationHub, ITypedSimulationHub> HubContext { get; set; }

        /// <summary>
        /// Returns the current status of the simulation with the given id.
        /// </summary>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SimulationStatusDto> Current(int simulationId)
        {
            var current = _simulationService.GetCurrentStatus(simulationId);
            if (current == null)
                return NotFound("No current Status found!");
            return Ok(current);
        }

        /// <summary>
        /// Sets the current Status of the simulation and notifies the socket with "StatusChanged".
        /// <see cref="SimulationStatusDto"/>
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> CurrentStatus([FromBody]SetSimulationStatusDto body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChair(body);
            if (!isAllowed) return BadRequest();
            var createdStatus = this._simulationService.SetStatus(body);

            var newStatusSocketMessage = new SimulationStatusDto()
            {
                SimulationStatusId = createdStatus.SimulationStatusId,
                StatusText = createdStatus.StatusText,
                StatusTime = createdStatus.StatusTime
            };
            _ = GetSimulationHub(body)?.StatusChanged(newStatusSocketMessage).ConfigureAwait(false);
            return Ok();
        }

        private ITypedSimulationHub GetSimulationHub(SimulationRequest request)
        {
            return this.HubContext?.Clients?.Group($"sim_{request.SimulationId}");
        }

        public SimStatusController(Services.SimulationService simulationService, IHubContext<SimulationHub, ITypedSimulationHub> hubContext)
        {
            _simulationService = simulationService;
            HubContext = hubContext;
        }
    }
}
