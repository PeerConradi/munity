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
    [Route("api/Resa/Amendment/Move")]
    [ApiController]
    public class MoveAmendmentController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly SqlResolutionService _resolutionService;

        public MoveAmendmentController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            SqlResolutionService resolutionService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
        }

        /// <summary>
        /// Creates a new amendment to move a given operative paragraph to a new position.
        /// This will generate a new virtual operative paragraph that represents the position where
        /// the operative paragraph will be moved to. It will inform anyone listening to the
        /// resasocket that is subscribed to the group (see Resa/SubscribeToResolution) <see cref="MainResaController.SubscribeToResolution(string, string)"/>
        /// about the changes.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public ActionResult<MoveAmendmentCreatedEventArgs> Create([FromBody]CreateMoveAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateMoveAmendment(body.ParagraphId,body.SubmitterName, body.NewIndex);
            if (mdl == null)
                return NotFound();

            var dto = new MoveAmendmentCreatedEventArgs()
            {
                ResolutionId = body.ResolutionId,
                Tan = "12345",
                Amendment = new MoveAmendment()
                {
                    Activated = mdl.Activated,
                    Id = mdl.ResaAmendmentId,
                    Name = "move",
                    NewTargetSectionId = mdl.VirtualParagraph.ResaOperativeParagraphId,
                    SubmitterName = mdl.SubmitterName,
                    SubmitTime = mdl.SubmitTime,
                    TargetSectionId = mdl.SourceParagraph.ResaOperativeParagraphId,
                    Type = mdl.ResaAmendmentType
                },
                VirtualParagraph = new OperativeParagraph()
                {
                    Children = new List<OperativeParagraph>(),
                    Comment = "",
                    Corrected = false,
                    IsLocked = mdl.VirtualParagraph.IsLocked,
                    IsVirtual = mdl.VirtualParagraph.IsVirtual,
                    Name = "virtual",
                    OperativeParagraphId = mdl.VirtualParagraph.ResaOperativeParagraphId,
                    Text = mdl.VirtualParagraph.Text,
                    Visible = mdl.VirtualParagraph.Visible
                },
                VirtualParagraphIndex = mdl.VirtualParagraph.OrderIndex
            };

            GetHub(body)?.MoveAmendmentCreated(dto);

            return Ok(dto);
        }

        /// <summary>
        /// Removes the amendment and the virtual paragraph that has been created for this amendment.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult Remove([FromBody]AmendmentRequest body)
        {
            bool success = _resolutionService.RemoveMoveAmendment(body.AmendmentId);
            if (!success)
                return NotFound();

            GetHub(body)?.AmendmentRemoved(body.AmendmentId);
            return Ok();
        }

        /// <summary>
        /// Submits the amendment. This will remove the original operative paragraph and turn the virtual paragraph
        /// into a normal paragraph.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public IActionResult Submit([FromBody]AmendmentRequest body)
        {
            bool success = _resolutionService.SubmitMoveAmendment(body.AmendmentId);
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
