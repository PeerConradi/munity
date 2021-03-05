using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Schema.Simulation;
using MUNity.Schema.Resolution;

namespace MUNityCore.Controllers.Resa
{
    [Route("api/Resolution/[controller]")]
    [ApiController]
    public class AmendmentConfigController : ControllerBase
    {
        private Services.IResolutionService _resolutionService;

        private Services.SimulationService _simulationService;

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<bool>> SimUserPostAmendments([FromHeader] string simsimtoken, string resolutionId)
        {
            var auth = await this._resolutionService.GetResolutionAuth(resolutionId);
            if (auth.AllowPublicEdit) return Ok(true);
            if (auth.AllowOnlineAmendments) return Ok(true);
            return Ok(false);
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> AllowAmendments([FromBody]SetResolutionOnlineAmendmentState state)
        {
            var isAllowed = await this._simulationService.IsTokenValidAndUserChairOrOwner(state);
            if (!isAllowed) return BadRequest();

            bool changed = this._resolutionService.SetOnlineAmendmentMode(state);
            if (!changed) return NotFound();
            return Ok();
        }


        public AmendmentConfigController(Services.IResolutionService resolutionService, Services.SimulationService simulationService)
        {
            this._resolutionService = resolutionService;
            this._simulationService = simulationService;
        }
    }
}
