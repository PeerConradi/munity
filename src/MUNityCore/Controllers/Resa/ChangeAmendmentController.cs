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
    [Route("api/Resa/Amendment/Change")]
    [ApiController]
    public class ChangeAmendmentController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly SqlResolutionService _resolutionService;

        public ChangeAmendmentController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            SqlResolutionService resolutionService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<ChangeAmendment> Create([FromBody]CreateChangeAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateChangeAmendment(body.ParagraphId, body.SubmitterName, body.NewText);
            if (mdl == null)
                return NotFound();

            var dto = new ChangeAmendment()
            {
                Activated = mdl.Activated,
                Id = mdl.ResaAmendmentId,
                Name = "change",
                NewText = mdl.NewText,
                SubmitterName = mdl.SubmitterName,
                SubmitTime = mdl.SubmitTime,
                TargetSectionId = mdl.TargetParagraph.ResaOperativeParagraphId,
                Type = mdl.ResaAmendmentType
            };

            return Ok(dto);
        }
    }
}
