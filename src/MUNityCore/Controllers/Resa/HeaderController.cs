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
    [Route("api/Resa/Header")]
    [ApiController]
    [EnableCors("munity")]
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

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult> Session([FromBody] HeaderStringPropChangedEventArgs body)
        {
            if (!await CanUserEditResolution(body.ResolutionId))
                return BadRequest();

            var success = await this._resolutionService.SetSession(body.ResolutionId, body.Text);
            if (!success)
                return NotFound();

            _ = GetHub(body)?.HeaderSessionChanged(body).ConfigureAwait(false);
            return Ok();
        }

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
            return true;    // TODO Remove this when done with testing
        }

        private MUNity.Hubs.ITypedResolutionHub GetHub(HeaderStringPropChangedEventArgs args)
        {
            return this._hubContext?.Clients?.Group(args.ResolutionId);
        }
    }
}
