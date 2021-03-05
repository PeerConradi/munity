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
    [Route("api/Simulation/Status")]
    [ApiController]
    public class SimStatusController : ControllerBase, ISimulationController
    {
        private Services.SimulationService _simulationService;

        public IHubContext<SimulationHub, ITypedSimulationHub> HubContext { get; set; }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<SimulationStatusDto> Current(int simulationId)
        {
            var current = _simulationService.GetCurrentStatus(simulationId);
            if (current == null)
                return NotFound("No current Status found!");
            return Ok(current);
        }

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
