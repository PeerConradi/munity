using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Simulation
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimStatusController : ControllerBase
    {
        private Services.SimulationService _simulationService;

        [HttpGet]
        [Route("[action]")]
        public ActionResult<SimulationStatusDto> GetCurrent(int simulationId)
        {
            var current = _simulationService.GetCurrentStatus(simulationId);
            if (current == null)
                return NotFound("No current Status found!");
            return Ok(current);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> SetCurrentStatus([FromBody]SetSimulationStatusDto body)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChair(body);
            if (!isAllowed) return Forbid();
            var createdStatus = this._simulationService.SetStatus(body);
            return Ok();
        }

        public SimStatusController(Services.SimulationService simulationService)
        {
            _simulationService = simulationService;
        }
    }
}
