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
    [Route("api/Resa/Amendment/Delete")]
    [ApiController]
    public class DeleteAmendmentController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly SqlResolutionService _resolutionService;

        public DeleteAmendmentController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            SqlResolutionService resolutionService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<DeleteAmendment> Create([FromBody]CreateDeleteAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateDeleteAmendment(body.ParagraphId, body.SubmitterName);
            if (mdl == null)
                return NotFound();

            var dto = new DeleteAmendment()
            {
                Activated = mdl.Activated,
                Id = mdl.ResaAmendmentId,
                Name = "delete",
                SubmitterName = mdl.SubmitterName,
                SubmitTime = mdl.SubmitTime,
                TargetSectionId = mdl.TargetParagraph.ResaOperativeParagraphId,
                Type = mdl.ResaAmendmentType
            };
            return Ok(dto);
        }
    }
}
