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

namespace MUNityCore.Controllers.Resa
{
    [Route("api/Resa/Operative[controller]")]
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
            return Ok(mdl);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Text(ChangeOperativeParagraphTestRequest body)
        {
            var success = this._resolutionService.SetOperativeParagraphText(body.OperativeParagraphId, body.NewText);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
