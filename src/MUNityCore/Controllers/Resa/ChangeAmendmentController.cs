using Microsoft.AspNetCore.Cors;
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


        /// <summary>
        /// Creates a new amendment to change the text inside an operative paragraph.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
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

            GetHub(body)?.ChangeAmendmentCreated(dto);

            return Ok(dto);
        }

        /// <summary>
        /// Removes an amendment from the list of changeAmendments.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult Remove([FromBody]AmendmentRequest body)
        {
            bool success = this._resolutionService.RemoveChangeAmendment(body.AmendmentId);
            if (!success)
                return NotFound();

            GetHub(body)?.AmendmentRemoved(body.AmendmentId);
            return Ok();
        }

        /// <summary>
        /// Applies/Submits a change amendment. The Text of the Operative Paragraph will become
        /// the text that is inside the ChangeAmendment.Text-Field.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult Submit([FromBody]AmendmentRequest body)
        {
            bool success = this._resolutionService.SubmitChangeAmendment(body.AmendmentId);
            if (!success)
                return NotFound();

            GetHub(body)?.AmendmentSubmitted(body.AmendmentId);
            return Ok();
        }

        private MUNity.Hubs.ITypedResolutionHub GetHub(ResolutionRequest args)
        {
            return this._hubContext?.Clients?.Group(args.ResolutionId);
        }
    }
}
