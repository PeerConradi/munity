using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNity.Models.Resolution;
using MUNity.Schema.Resolution;
using MUNityCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Models.Resolution.EventArguments;
using Microsoft.AspNetCore.Cors;

namespace MUNityCore.Controllers.Resa
{
    [Route("api/Resa/Operative")]
    [ApiController]
    public class OperativeParagraphController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly SqlResolutionService _resolutionService;

        public OperativeParagraphController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            SqlResolutionService resolutionService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<OperativeParagraph> AddParagraph([FromBody]AddOperativeParagraphRequest body)
        {
            var newParagraph = _resolutionService.CreateOperativeParagraph(body.ResolutionId);
            if (newParagraph == null)
                return NotFound();
            var mdl = new OperativeParagraph()
            {
                Children = new List<OperativeParagraph>(),
                Comment = newParagraph.Comment,
                Corrected = newParagraph.Corrected,
                IsLocked = newParagraph.Corrected,
                IsVirtual = newParagraph.IsVirtual,
                Name = newParagraph.Name,
                OperativeParagraphId = newParagraph.ResaOperativeParagraphId,
                Text = newParagraph.Text,
                Visible = newParagraph.Visible
            };

            GetHub(body)?.OperativeParagraphAdded(new OperativeParagraphAddedEventArgs(body.ResolutionId, mdl)).ConfigureAwait(false);

            return Ok(mdl);
        }
        

        [HttpPut]
        [Route("[action]")]
        public IActionResult Text(ChangeOperativeParagraphTextRequest body)
        {
            var success = this._resolutionService.SetOperativeParagraphText(body.OperativeParagraphId, body.NewText);
            if (!success)
                return NotFound();

            var args = new OperativeParagraphTextChangedEventArgs()
            {
                ParagraphId = body.OperativeParagraphId,
                ResolutionId = body.ResolutionId,
                Tan = "1234",
                Text = body.NewText
            };

            GetHub(body)?.OperativeParagraphTextChanged(args).ConfigureAwait(false);

            return Ok();
        }

        private MUNity.Hubs.ITypedResolutionHub GetHub(ResolutionRequest args)
        {
            return this._hubContext?.Clients?.Group(args.ResolutionId);
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Comment([FromBody]ChangeOperativeParagraphCommentRequest body)
        {
            var success = this._resolutionService.SetOperativeParagraphComment(body.OperativeParagraphId, body.NewText);
            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Remove([FromBody]RemoveOperativeParagraphRequest body)
        {
            var success = this._resolutionService.RemoveOperativeParagraph(body.OperativeParagraphId);
            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Reorder([FromBody]ReorderOperativeParagraphsRequest body)
        {
            var success = this._resolutionService.ReorderOperative(body.ResolutionId, body.NewOrder);
            if (!success) return NotFound();

            return Ok();
        }
    }
}
