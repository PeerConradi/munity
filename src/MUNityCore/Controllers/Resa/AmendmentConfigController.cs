using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Schema.Simulation;
using MUNity.Schema.Resolution;
using Microsoft.AspNetCore.Cors;

namespace MUNityCore.Controllers.Resa
{
    /// <summary>
    /// Controller to handle anything around amendments.
    /// </summary>
    [Route("api/Resolution/[controller]")]
    [ApiController]
    public class AmendmentConfigController : ControllerBase
    {
        private Services.IResolutionService _resolutionService;

        private Services.SimulationService _simulationService;

        /// <summary>
        /// Checks if the user with the given token inside the header.
        /// (Header Key: simsimtoken, Value: [TOKEN OF SIMULATION SLOT])
        /// is allowed to create a new amendment for the resolution document.
        /// </summary>
        /// <param name="simsimtoken"></param>
        /// <param name="resolutionId"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<bool>> SimUserPostAmendments([FromHeader] string simsimtoken, string resolutionId)
        {
            var auth = await this._resolutionService.GetResolutionAuth(resolutionId);
            if (auth.AllowPublicEdit) return Ok(true);
            if (auth.AllowOnlineAmendments) return Ok(true);
            return Ok(false);
        }

        /// <summary>
        /// Allows users inside a simulation to create new amendments of any type.
        /// Any Slot inside the simulation will be allowed to create amendments. You may want
        /// to handle different rights via the client. Currently there is no way you can only allow
        /// to add amendments to only one type of role.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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
