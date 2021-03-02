﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Models.Resolution;
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

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<string>> Public()
        {
            var resolutionId = await this._resolutionService.CreatePublicResolutionAsync("Neue Resolution");
            if (string.IsNullOrEmpty(resolutionId))
                return BadRequest();
            return Ok(resolutionId);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult IsUp()
        {
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Resolution>> Public(string id)
        {
            var resolution = await this._resolutionService.GetResolutionDtoAsync(id);
            if (resolution == null) return NotFound();
            return Ok(resolution);
        }

        ///// <summary>
        ///// Puts the user into the signalR Group for this document/resolution.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="id"></param>
        ///// <param name="connectionid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> SubscribeToResolution(string resolutionid, string connectionid)
        {
            //var resolution = _resolutionService.GetResolution(resolutionid);
            //if (resolution == null) return NotFound();

            //var infoModel = await _resolutionService.GetResolutionAuth(resolutionid);

            //var canRead = await CanUserReadResolution(resolutionid);
            //if (!canRead) return Forbid();

            await _hubContext.Groups.AddToGroupAsync(connectionid, resolutionid);

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}