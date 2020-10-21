using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityCore.Util.Extenstions;
using MUNityCore.Models.Resolution;
using MUNityCore.Schema.Request;
using MUNityCore.DataHandlers.EntityFramework.Models;
using MUNityCore.Schema.Request.Resolution;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Services;

namespace MUNityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResolutionController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> _hubContext;

        private readonly IResolutionService _resolutionService;

        private readonly IAuthService _authService;

        public ResolutionController(IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> hubContext, 
            IResolutionService resolutionService,
                IAuthService authService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
            _authService = authService;
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResolutionV2> CreatePublic(string title)
        {
            if (User == null)
            {
                // This should be called when the User is not logged in
            }

            return await _resolutionService.CreatePublicResolution(title);
        }

        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        public async Task<ResolutionV2> UpdateProtectedResolution([FromBody] ResolutionV2 resolution)
        {
            var user = _authService.GetUserOfClaimPrincipal(User);
            // Check if user is allowed to update this document
            var canEdit = _authService.CanUserEditResolution(user, resolution);

            if (!canEdit)
                return null;

            // Update this document
            return await _resolutionService.SaveResolution(resolution);
        }

        /// <summary>
        /// Updates a Resolution that has Public Access. 
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [AllowAnonymous]
        public async Task<ActionResult<ResolutionV2>> UpdatePublicResolution([FromBody]ResolutionV2 resolution)
        {
            // Check if a resolution with this Id exists
            var res = await _resolutionService.GetResolution(resolution.ResolutionId);

            if (res == null)
                return NotFound("Resolution not found");

            // Check if the resolution is public
            var auth = await _resolutionService.GetResolutionAuth(resolution.ResolutionId);
            if (auth.AllowPublicEdit)
            {
                // Update this document
                var updatedDocument = await _resolutionService.SaveResolution(resolution);
                await _hubContext.Clients.Groups(updatedDocument.ResolutionId).ResolutionChanged(updatedDocument);
                return Ok(updatedDocument);
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                "Something went wrong when saving the resolution!");
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResolutionV2> GetPublic(string id,
            [FromServices] IResolutionService resolutionService)
        {
            return await resolutionService.GetResolution(id);
        }

        ///// <summary>
        ///// Puts the user into the signalR Group for this document.
        ///// </summary>
        ///// <param name="auth"></param>
        ///// <param name="id"></param>
        ///// <param name="connectionid"></param>
        ///// <param name="resolutionService"></param>
        ///// <param name="authService"></param>
        ///// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> SubscribeToResolution([FromHeader] string id,
            [FromHeader] string connectionid)
        {
            var resolution = _resolutionService.GetResolution(id);

            var infoModel = await _resolutionService.GetResolutionAuth(id);

            if (resolution != null)
            {
                if (infoModel.AllowPublicRead)
                {
                    await _hubContext.Groups.AddToGroupAsync(connectionid, id);
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    // TODO: Check when the Resoltion is private, if the user can subscribe (read)
                }
            }

            return StatusCode(StatusCodes.Status200OK);
        }



    }
}
