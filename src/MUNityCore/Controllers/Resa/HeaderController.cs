using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Models.Resolution.EventArguments;
using MUNityCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Resa
{
    [Route("api/Resolution")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly IResolutionService _resolutionService;

        private readonly IAuthService _authService;

        public HeaderController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            IResolutionService resolutionService,
                IAuthService authService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
            _authService = authService;
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> UpdateHeaderName([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(body.ResolutionId);
            if (resolution.ResolutionId == null) return NotFound();
            if (body.Text == null) return BadRequest();
            if (body.Text == resolution.Header.Name) return Ok();

            resolution.Header.Name = body.Text;
            _ = _resolutionService.SaveResolution(resolution).ConfigureAwait(false);
            _ = this._hubContext.Clients.Group(body.ResolutionId).HeaderNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> UpdateHeaderFullName([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(body.ResolutionId);
            if (resolution.ResolutionId == null) return NotFound();
            if (body.Text == null) return BadRequest();
            if (body.Text == resolution.Header.FullName) return Ok();

            resolution.Header.FullName = body.Text;
            _ = _resolutionService.SaveResolution(resolution).ConfigureAwait(false);
            _ = this._hubContext.Clients.Group(body.ResolutionId).HeaderFullNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> UpdateHeaderTopic([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(body.ResolutionId);

            if (resolution.ResolutionId == null) return NotFound();
            if (body.Text == null) return BadRequest();
            if (body.Text == resolution.Header.Topic) return Ok();

            resolution.Header.Topic = body.Text;
            _ = _resolutionService.SaveResolution(resolution).ConfigureAwait(false);
            _ = this._hubContext.Clients.Group(body.ResolutionId).HeaderTopicChanged(body).ConfigureAwait(false);
            await this._resolutionService.SetNameInDb(body.ResolutionId, body.Text);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> UpdateHeaderAgendaItem([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(body.ResolutionId);
            if (resolution.ResolutionId == null) return NotFound();
            if (body.Text == null) return BadRequest();
            if (body.Text == resolution.Header.AgendaItem) return Ok();

            resolution.Header.AgendaItem = body.Text;
            _ = _resolutionService.SaveResolution(resolution).ConfigureAwait(false);
            _ = this._hubContext.Clients.Group(body.ResolutionId).HeaderAgendaItemChanged(body).ConfigureAwait(false);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> UpdateHeaderSession([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(body.ResolutionId);
            if (resolution.ResolutionId == null) return NotFound();
            if (body.Text == null) return BadRequest();
            if (body.Text == resolution.Header.Session) return Ok();

            resolution.Header.Session = body.Text;
            _ = _resolutionService.SaveResolution(resolution).ConfigureAwait(false);
            _ = this._hubContext.Clients.Group(body.ResolutionId).HeaderSessionChanged(body).ConfigureAwait(false);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> UpdateHeaderSubmitterName([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(body.ResolutionId);
            if (resolution.ResolutionId == null) return NotFound();
            if (body.Text == null) return BadRequest();
            if (body.Text == resolution.Header.SubmitterName) return Ok();

            resolution.Header.SubmitterName = body.Text;
            _ = _resolutionService.SaveResolution(resolution).ConfigureAwait(false);
            _ = this._hubContext.Clients.Group(body.ResolutionId).HeaderSubmitterNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> UpdateHeaderCommitteeName([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return Forbid();

            var resolution = await this._resolutionService.GetResolution(body.ResolutionId);
            if (resolution.ResolutionId == null) return NotFound();
            if (body.Text == null) return BadRequest();
            if (body.Text == resolution.Header.CommitteeName) return Ok();

            resolution.Header.CommitteeName = body.Text;
            _ = _resolutionService.SaveResolution(resolution).ConfigureAwait(false);
            _ = this._hubContext.Clients.Group(body.ResolutionId).HeaderCommitteeNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        private async Task<bool> CanUserEditResolution(string id, string password = null)
        {
            return true;    // TODO Remove this when done with testing
            var resolutionAuth = await this._resolutionService.GetResolutionAuth(id);
            if (resolutionAuth == null) return false;
            if (resolutionAuth.AllowPublicEdit) return true;
            // Check if the resolution has an edit password
            // do not set it to return password == resolutionUath.EditPassword
            // because the password can be wrong or not given but the user is
            // in the whitelist of the document and can authenticate in the next step!
            if (!string.IsNullOrEmpty(resolutionAuth.EditPassword))
                if (password == resolutionAuth.EditPassword) return true;
            // The resolution is not public and no User was found. So the user is not allowed to edit this document!
            if (User == null) return false;
            var user = this._authService.GetUserOfClaimPrincipal(User);
            if (user == null) return false;
            return resolutionAuth.Users.Any(n => n.User.MunityUserId == user.MunityUserId && n.CanWrite);
        }
    }
}
