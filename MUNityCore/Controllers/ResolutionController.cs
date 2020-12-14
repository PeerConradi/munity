using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityCore.Util.Extensions;
using MUNityCore.Models.Resolution;
using MUNityCore.Schema.Request;
using MUNityCore.Schema.Request.Resolution;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Services;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The ResolutionController offers endpoints to work with resolution documents.
    /// </summary>
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
        public ActionResult IsUp()
        {
            return Ok();
        }

        /// <summary>
        /// Create a public accessable resolution.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResolutionV2> CreatePublic(string title)
        {
            return await _resolutionService.CreatePublicResolution(title);
        }

        /// <summary>
        /// Returns a resolution with the given Id if the user is allowed to read the resolution or it is public.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResolutionV2>> GetResolution(string id)
        {
            if (!await CanUserReadResolution(id))
                return Forbid();

            return await this._resolutionService.GetResolution(id);
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> ResolutionExists(string id)
        {
            return await this._resolutionService.ResolutionExists(id);
        }

        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> CanEditResolution(string id)
        {
            return await CanUserEditResolution(id);
        }

        /// <summary>
        /// Updates a resolution if the user is logged in.
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [AllowAnonymous]
        public async Task<ActionResult<ResolutionV2>> UpdateResolution([FromBody]ResolutionV2 resolution)
        {
            if (!await CanUserEditResolution(resolution.ResolutionId))
                return Forbid();

            var updatedDocument = await _resolutionService.SaveResolution(resolution);
            await _hubContext.Clients.Groups(updatedDocument.ResolutionId).ResolutionChanged(updatedDocument);
            return Ok(updatedDocument);
        }

        [Route("[action]")]
        [HttpPatch]
        [AllowAnonymous]
        public async Task<ActionResult> UpdatePreambleParagraph(string resolutionId, string tan, [FromBody] PreambleParagraph paragraph)
        {
            if (!await CanUserEditResolution(resolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(resolutionId);
            if (resolution == null) return NotFound("Resolution with the given id not found!");
            var result = await this._resolutionService.UpdatePreambleParagraph(resolution, paragraph);
            
            if (result)
            {
                await this._hubContext.Clients.Group(resolutionId).PreambleParagraphChanged(resolutionId, paragraph, tan);
                return Ok();
            }
            return Problem();
        }

        [Route("[action]")]
        [HttpPatch]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateOperativeParagraph(string resolutionId, string tan, [FromBody]OperativeParagraph paragraph)
        {
            if (!await CanUserEditResolution(resolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(resolutionId);
            if (resolution == null) return NotFound("Resolution with the given id not found!");
            var result = await this._resolutionService.UpdateOperativeParagraph(resolution, paragraph);

            if (result)
            {
                await this._hubContext.Clients.Group(resolutionId).OperativeParagraphChanged(resolutionId, paragraph, tan);
                return Ok();
            }
            return Problem();
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
        public async Task<IActionResult> SubscribeToResolution(string resolutionid,string connectionid)
        {
            var resolution = _resolutionService.GetResolution(resolutionid);

            var infoModel = await _resolutionService.GetResolutionAuth(resolutionid);

            if (resolution != null)
            {
                if (infoModel.AllowPublicRead)
                {
                    await _hubContext.Groups.AddToGroupAsync(connectionid, resolutionid);
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    // TODO: Check when the Resoltion is private, if the user can subscribe (read)
                }
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        private async Task<bool> CanUserEditResolution(string id)
        {
            var resolutionAuth = await this._resolutionService.GetResolutionAuth(id);
            if (resolutionAuth == null) return false;
            if (resolutionAuth.AllowPublicEdit) return true;
            // The resolution is not public and no User was found. So the user is not allowed to edit this document!
            if (User == null) return false;
            var user = this._authService.GetUserOfClaimPrincipal(User);
            if (user == null) return false;
            return resolutionAuth.Users.Any(n => n.User.MunityUserId == user.MunityUserId && n.CanWrite);
        }

        private async Task<bool> CanUserReadResolution(string id)
        {
            var resolutionAuth = await this._resolutionService.GetResolutionAuth(id);
            if (resolutionAuth == null) return false;
            if (resolutionAuth.AllowPublicRead) return true;
            // The resolution is not public and no User was found. So the user is not allowed to edit this document!
            if (User == null) return false;
            var user = this._authService.GetUserOfClaimPrincipal(User);
            if (user == null) return false;
            return resolutionAuth.Users.Any(n => n.User.MunityUserId == user.MunityUserId && n.CanRead);
        }
    }
}
