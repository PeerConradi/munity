using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Models.Resolution;
using MUNity.Schema.Simulation.Resolution;
using MUNitySchema.Schema.Simulation.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Resa
{
    [Route("api/Resa")]
    [ApiController]
    public class MainResaController : ControllerBase
    {
        Services.SqlResolutionService _resolutionService;

        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        public MainResaController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext, Services.SqlResolutionService resolutionService)
        {
            this._hubContext = hubContext;
            this._resolutionService = resolutionService;
        }

        /// <summary>
        /// Creates a new Public Resolution that can be edited by anyone that knows the ID.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<string>> Public()
        {
            var resolutionId = await this._resolutionService.CreatePublicResolutionAsync("Neue Resolution");
            if (string.IsNullOrEmpty(resolutionId))
                return BadRequest();
            return Ok(resolutionId);
        }

        /// <summary>
        /// Will return a status 200 OK when the resolution controller is reachable.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult IsUp()
        {
            return Ok();
        }

        /// <summary>
        /// Returns a public resolution with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Resolution>> Public(string id)
        {
            var resolution = await this._resolutionService.GetResolutionDtoAsync(id);
            if (resolution == null) return NotFound();
            return Ok(resolution);
        }

        /// <summary>
        /// Puts the user into the signalR Group for this document/resolution.
        /// The groupid is just the id of the resolution
        /// </summary>
        /// <param name="resolutionid">The id (normaly a guid) of the resolution.</param>
        /// <param name="connectionid">The connectionid given by SignalR</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> SubscribeToResolution(string resolutionid, string connectionid)
        {
            //var resolution = _resolutionService.GetResolution(resolutionid);
            //if (resolution == null) return NotFound();

            //var infoModel = await _resolutionService.GetResolutionAuth(resolutionid);

            //var canRead = await CanUserReadResolution(resolutionid);
            //if (!canRead) return BadRequest();

            await _hubContext.Groups.AddToGroupAsync(connectionid, resolutionid);

            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Returns an info element of the given resolutionid.
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<ResolutionSmallInfo> GetResolutionInfo(string resolutionId)
        {
            ResolutionSmallInfo info = this._resolutionService.GetResolutionInfo(resolutionId);
            if (info == null)
                return NotFound();
            return Ok(info);
        }

        /// <summary>
        /// Allows any user to create amendments for this resolution.
        /// </summary>
        /// <param name="simulationService"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> EnableOnlineAmendments([FromServices]Services.SimulationService simulationService,
            [FromBody]SimulationResolutionRequest body)
        {
            var allowed = await simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!allowed)
                return BadRequest();

            this._resolutionService.AllowOnlineAmendments(body.ResolutionId);

            return Ok();
        }

        /// <summary>
        /// Disables amendments for other users that know the id of the resolution.
        /// </summary>
        /// <param name="simulationService"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DisableOnlineAmendments([FromServices] Services.SimulationService simulationService,
            [FromBody] SimulationResolutionRequest body)
        {
            var allowed = await simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!allowed)
                return BadRequest();

            this._resolutionService.DisableOnlineAmendments(body.ResolutionId);

            return Ok();
        }

        /// <summary>
        /// Allows anyone to edit the resolution with the given id. If the simulation 
        /// is linked to a simulation this will allow anyone inside this simulation to change
        /// the resolution.
        /// </summary>
        /// <param name="simulationService"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> EnablePublicEdit([FromServices] Services.SimulationService simulationService,
            [FromBody] SimulationResolutionRequest body)
        {
            var allowed = await simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!allowed)
                return BadRequest();

            this._resolutionService.EnablePublicEdit(body.ResolutionId);

            return Ok();
        }

        /// <summary>
        /// Deactivates the public edit mode. If the resolution is linked to a simulation this 
        /// will only allow people with the role of Chairman or the owner to make changes to the
        /// resolution.
        /// </summary>
        /// <param name="simulationService"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> DisablePublicEdit([FromServices] Services.SimulationService simulationService,
            [FromBody] SimulationResolutionRequest body)
        {
            var allowed = await simulationService.IsTokenValidAndUserChairOrOwner(body);
            if (!allowed)
                return BadRequest();

            this._resolutionService.DisablePublicEdit(body.ResolutionId);

            return Ok();
        }
    }
}
