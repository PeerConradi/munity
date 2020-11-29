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
            if (User == null)
            {
                // This should be called when the User is not logged in
            }

            return await _resolutionService.CreatePublicResolution(title);
        }

        /// <summary>
        /// Update a resolution that is not opened to the public, you need to give a valid
        /// token for this operation. The Controller will then check if the user is allowed
        /// to change the resolution and update it.
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
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
        /// Updates a Resolution that has Public Access. It will also inform every connection that is listening
        /// (subscribed) to the WebSocket for this resolution.
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

        /// <summary>
        /// Updates a preamble paragraph inside the given resolution.
        /// note that this will also submit the changes to all clients that
        /// are listening on the resasocket by subscribing to this resolution.
        /// Will update the Text and the notices.
        /// Will return Status 200 if the changes are submitted.
        /// Will return Status 404 if the resolution or preamble paragraph cannot be found.
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [AllowAnonymous]
        public async Task<ActionResult> UpdatePublicResolutionPreambleParagraph(string resolutionId,
            [FromBody] PreambleParagraph paragraph)
        {
            var resolution = await this._resolutionService.GetResolution(resolutionId);
            if (resolution == null) return NotFound("Resolution with the given id not found!");
            if (resolution.Preamble == null) return NotFound("Resolution has no preamble section");
            if (resolution.Preamble.Paragraphs == null || resolution.Preamble.Paragraphs.Count == 0)
                return NotFound("Resolution has no preamble paragraphs.");
            var para = resolution.Preamble
                .Paragraphs.FirstOrDefault(n => 
                    n.PreambleParagraphId == paragraph.PreambleParagraphId);
            if (para == null) return NotFound("Paragraph not found");
            para.Text = paragraph.Text;
            para.Notices = paragraph.Notices;
            await this._resolutionService.SaveResolution(resolution);
            await this._hubContext.Clients.Group(resolutionId).PreambleParagraphChanged(resolutionId, para);
            return Ok();
        }

        /// <summary>
        /// Updates an operative paragraph inside a resolution. Will change the Text and Notices to the ones inside the
        /// given Model.
        /// This Call will then inform all clients that have Subscribed to the resolution inside the
        /// resasocket.
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [AllowAnonymous]
        public async Task<ActionResult> UpdatePublicResolutionOperativeParagraph(string resolutionId,
            [FromBody] OperativeParagraph paragraph)
        {
            var resolution = await this._resolutionService.GetResolution(resolutionId);
            if (resolution == null) return NotFound("Resolution with the given id not found!");
            if (resolution.OperativeSection == null) return NotFound("Resolution has no operative section");
            if (resolution.OperativeSection.Paragraphs == null || resolution.OperativeSection.Paragraphs.Count == 0)
                return NotFound("Resolution has no Operative paragraphs.");
            var para = resolution.OperativeSection
                .Paragraphs.FirstOrDefault(n =>
                    n.OperativeParagraphId == paragraph.OperativeParagraphId);
            if (para == null) return NotFound("Paragraph not found");
            para.Text = paragraph.Text;
            para.Notices = paragraph.Notices;
            await this._resolutionService.SaveResolution(resolution);
            await this._hubContext.Clients.Group(resolutionId).OperativeParagraphChanged(resolutionId, para);
            return Ok();
        }

        /// <summary>
        /// Returns a public resolution if its found.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resolutionService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResolutionV2> GetPublic(string id)
        {
            return await _resolutionService.GetResolution(id);
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
