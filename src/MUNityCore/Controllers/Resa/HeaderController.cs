using Microsoft.AspNetCore.Cors;
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
    /// <summary>
    /// Controller to edit the header of a resolution.
    /// </summary>
    [Route("api/Resa/Header")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly SqlResolutionService _resolutionService;

        public HeaderController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            SqlResolutionService resolutionService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
        }

        /// <summary>
        /// Changes the name of the resolution and will notify everyone listening on the ResaSocket to the group
        /// of the resolution with "HeaderNameChanged" and a HeaderStringPropChangedEventArgs body.
        /// So the data-transfer-object inside the body of this call will be sent to all users of the group
        /// via the resasocket.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> Name([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var succes = await _resolutionService.SetNameAsync(body.ResolutionId, body.Text);
            if (!succes)
                return NotFound();

            _ = GetHub(body)?.HeaderNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Changes the FullName of a resolution and notify all clients listening to the WebSocket that are
        /// attached to the group of this resolution on "HeaderFullNameChanged" with the content given within
        /// the request body.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> FullName([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var success = await _resolutionService.SetFullNameAsync(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();

            _ = GetHub(body)?.HeaderFullNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Changes the topic and sents the content of the body via HeaderTopicChanged to anyone listening 
        /// to the resasocket.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> Topic([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var success = await this._resolutionService.SetTopicAsync(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();

            _ = GetHub(body)?.HeaderTopicChanged(body).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Changes the Text of the AgendaItem and notifies everyone listening via HeaderAgendaItemChanged
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> AgendaItem([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var success = await this._resolutionService.SetAgendaItem(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();
            
            _ = GetHub(body)?.HeaderAgendaItemChanged(body).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Changes the text of the Session and notifies everyone listening via HeaderSessionChanged
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> Session([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var success = await this._resolutionService.SetSessionAsync(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();

            _ = GetHub(body)?.HeaderSessionChanged(body).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Changes the SubmitterName and notifies everyone on the resasocket via "HeaderSubmitterChanged"
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> SubmitterName([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var success = await this._resolutionService.SetSubmitterNameAsync(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();

            _ = GetHub(body)?.HeaderSubmitterNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Changes the committeeName and notifies anyone listening on the resasocket and inside the given group
        /// via HeaderCommitteeNameChanged.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> CommitteeName([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var success = await this._resolutionService.SetCommitteeNameAsync(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();

            _ = GetHub(body)?.HeaderCommitteeNameChanged(body).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Changes the supporters text and notifies everyone with HeaderSupportersChanged.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> Supporters([FromBody]HeaderStringPropChangedEventArgs body)
        {
            var success = await this._resolutionService.SetSupportersAsync(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();

            var args = new HeaderStringPropChangedEventArgs(body.ResolutionId, body.Text);

            GetHub(body)?.HeaderSupportersChanged(args);

            return Ok();
        }

        private async Task<bool> CanUserEditResolution(string id, string password = null)
        {
            return await Task.FromResult(true);    // TODO Remove this when done with testing
        }

        private MUNity.Hubs.ITypedResolutionHub GetHub(HeaderStringPropChangedEventArgs args)
        {
            return this._hubContext?.Clients?.Group(args.ResolutionId);
        }
    }
}
