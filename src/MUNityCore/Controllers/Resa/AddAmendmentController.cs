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
    /// <summary>
    /// Controller to handle amendments to add a new operative paragraph.
    /// </summary>
    [Route("api/Resa/Amendment/Add")]
    [ApiController]
    public class AddAmendmentController : ControllerBase
    {
        readonly IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> _hubContext;

        private readonly SqlResolutionService _resolutionService;

        public AddAmendmentController(IHubContext<Hubs.ResolutionHub, MUNity.Hubs.ITypedResolutionHub> hubContext,
            SqlResolutionService resolutionService)
        {
            _hubContext = hubContext;
            _resolutionService = resolutionService;
        }

        /// <summary>
        /// Creates a new AddAmendment. This type of amendment is to add a new operative paragraph to a given position.
        /// It will return an object that is containing the AddAmendment object that has been created and the virtual paragraph
        /// that has to be added into the operative section. To know where to place the Paragraph use the VirtualParagraphIndex.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public ActionResult<AddAmendmentCreatedEventArgs> Create([FromBody]CreateAddAmendmentRequest body)
        {
            var mdl = this._resolutionService.CreateAddAmendment(body.ResolutionId, body.Index, body.SubmitterName, body.Text);
            if (mdl == null)
                return NotFound();

            var args = new AddAmendmentCreatedEventArgs()
            {
                ResolutionId = body.ResolutionId,
                Tan = "12345",
                Amendment = new AddAmendment()
                {
                    Activated = mdl.Activated,
                    Id = mdl.ResaAmendmentId,
                    Name = "AddAmendment",
                    SubmitterName = mdl.SubmitterName,
                    SubmitTime = mdl.SubmitTime,
                    TargetSectionId = mdl.VirtualParagraph.ResaOperativeParagraphId,
                    Type = mdl.ResaAmendmentType
                },
                VirtualParagraph = new OperativeParagraph()
                {
                    Children = new List<OperativeParagraph>(),
                    Comment = "",
                    Corrected = false,
                    IsLocked = mdl.VirtualParagraph.IsLocked,
                    IsVirtual = mdl.VirtualParagraph.IsVirtual,
                    Name = "Virtual Paragraph",
                    OperativeParagraphId = mdl.VirtualParagraph.ResaOperativeParagraphId,
                    Text = mdl.VirtualParagraph.Text,
                    Visible = mdl.VirtualParagraph.Visible
                },
                VirtualParagraphIndex = mdl.VirtualParagraph.OrderIndex
            };

            GetHub(body)?.AddAmendmentCreated(args);

            return Ok(args);
        }

        /// <summary>
        /// Activates the amendment with the given id to display it differently in the given editors.
        /// This will also turn any virtual paragraph that is linked to this amendment to visible.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult ActivateAmendment([FromBody] ActivateAmendmentRequest body)
        {
            this._resolutionService.ActivateAmendment(body.AmendmentId);
            var args = new AmendmentActivatedChangedEventArgs()
            {
                Activated = true,
                AmendmentId = body.AmendmentId,
                ResolutionId = body.ResolutionId,
                Tan = "12345"
            };
            GetHub(body)?.AmendmentActivatedChanged(args);
            return Ok();
        }

        /// <summary>
        /// Deactivates the given amendment inside any view. This means it should not be highlighted inside any views
        /// and also the operative paragraphs that are linked to this amendment should not be visible.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult DeactivateAmendment([FromBody] ActivateAmendmentRequest body)
        {
            this._resolutionService.DeactivateAmendment(body.AmendmentId);
            var args = new AmendmentActivatedChangedEventArgs()
            {
                Activated = false,
                AmendmentId = body.AmendmentId,
                ResolutionId = body.ResolutionId,
                Tan = "12345"
            };
            GetHub(body)?.AmendmentActivatedChanged(args);
            return Ok();
        }

        /// <summary>
        /// Removes the amendment with the given id. This will also remove the virtual paragraph.
        /// This function will only return an empty ok Result. 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult Remove([FromBody]AmendmentRequest body)
        {
            bool success = this._resolutionService.RemoveAddAmendment(body.AmendmentId);
            if (!success)
                return NotFound();

            GetHub(body)?.AmendmentRemoved(body.AmendmentId);

            return Ok();
        }

        /// <summary>
        /// Applies/Submits the given AddAMendment. It will set the VirtualParagraph that has been
        /// created for this amendment to a normal paragraph and remove the amendment.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult Submit([FromBody]AmendmentRequest body)
        {
            bool success = this._resolutionService.SubmitAddAmendment(body.AmendmentId);
            if (!success)
                return NotFound();

            GetHub(body).AmendmentSubmitted(body.AmendmentId);

            return Ok();
        }

        private MUNity.Hubs.ITypedResolutionHub GetHub(ResolutionRequest args)
        {
            return this._hubContext?.Clients?.Group(args.ResolutionId);
        }
    }
}
