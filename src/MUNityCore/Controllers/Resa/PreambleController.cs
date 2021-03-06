using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using MUNity.Schema.Resolution;
using MUNityCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Controllers.Resa
{
    [Route("api/Resa/Preamble")]
    [ApiController]
    [EnableCors("munity")]
    public class PreambleController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly SqlResolutionService _resolutionService;

        public PreambleController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            SqlResolutionService resolutionService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<PreambleParagraph> AddParagraph([FromBody]AddPreambleParagraphRequest body)
        {
            var paragraph = this._resolutionService.CreatePreambleParagraph(body.ResolutionId);
            if (paragraph == null) return NotFound();

            var returnElement = new PreambleParagraph()
            {
                Comment = paragraph.Comment,
                Corrected = paragraph.IsCorrected,
                IsLocked = paragraph.IsLocked,
                PreambleParagraphId = paragraph.ResaPreambleParagraphId,
                Text = paragraph.Text
            };

            _ = GetHub(body)?.PreambleParagraphAdded(new PreambleParagraphAddedEventArgs(body.ResolutionId, returnElement));

            return (Ok(returnElement));
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Text([FromBody]ChangePreambleParagraphTextRequest body)
        {
            var success = this._resolutionService.SetPreambleParagraphText(body.PreambleParagraphId, body.NewText);
            if (!success) return NotFound();

            var paragraphChanged = new PreambleParagraphTextChangedEventArgs()
            {
                ParagraphId = body.PreambleParagraphId,
                ResolutionId = body.ResolutionId,
                Tan = "12345",
                Text = body.NewText
            };
            _ = GetHub(body)?.PreambleParagraphTextChanged(paragraphChanged).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Comment([FromBody] ChangePreambleParagraphTextRequest body)
        {
            var success = this._resolutionService.SetPreambleParagraphComment(body.PreambleParagraphId, body.NewText);
            if (!success) return NotFound();

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Remove([FromBody]RemovePreambleParagraphRequest body)
        {
            var success = this._resolutionService.RemovePreambleParagraph(body.PreambleParagraphId);
            if (!success) return NotFound();

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Reorder([FromBody]ReorderPreambleRequest body)
        {
            var success = this._resolutionService.ReorderPreamble(body.ResolutionId, body.NewOrder);
            if (!success) return NotFound();
            return Ok();
        }

        private MUNity.Hubs.ITypedResolutionHub GetHub(ResolutionRequest args)
        {
            return this._hubContext?.Clients?.Group(args.ResolutionId);
        }

        
    }
}
