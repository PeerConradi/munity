using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResolutionController : ControllerBase
    {
        IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> _hubContext;

        public ResolutionController(IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> hubContext, [FromServices]ResolutionService service)
        {
            _hubContext = hubContext;
            if (service.HubContext == null)
                service.HubContext = hubContext;
        }

        /// <summary>
        /// Gives an overview of thats possible inside the resolution Controller
        /// </summary>
        /// <returns>a Text that gives a small overview of all posibilities with the Resolution Controller</returns>
        [HttpGet]
        public string Get()
        {
            string description = "Read the Resolution Documentation!";
            return description;
        }

        /// <summary>
        /// Creates a new Resolution
        /// </summary>
        /// <param name="auth">The authenticator-code </param>
        /// <returns>The new created Resolution in a json format.</returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create([FromHeader]string auth, [FromServices]Services.ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            if (!authService.CanCreateResolution(auth))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");


            string json;
            if (authService.isDefaultAuth(auth))
                json = resolutionService.CreateResolution(true, true).ToJson();
            else
                json = resolutionService.CreateResolution(userid: authService.ValidateAuthKey(auth).userid).ToJson();
            return StatusCode(StatusCodes.Status200OK, json);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddPreambleParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromServices]Services.ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");


            if (resolution != null)
            {
                var newPP = resolution.Preamble.AddParagraph();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                resolutionService.RequestSave(resolution);
                _hubContext.Clients.Group(resolutionid).PreambleParagraphAdded(resolution.Preamble.Paragraphs.IndexOf(newPP), newPP.ID, newPP.Text);
                return StatusCode(StatusCodes.Status200OK, json);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong, this should not have appened");
        }

        /// <summary>
        /// Adds an Premable Paragraph to the Resolution.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="resolutionid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult AddOperativeParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            if (resolution != null)
            {
                var newPP = resolution.AddOperativeParagraph();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                resolutionService.RequestSave(resolution);
                _hubContext.Clients.Group(resolutionid).OperativeParagraphAdded(resolution.OperativeSections.IndexOf(newPP), new Hubs.HubObjects.HUBOperativeParagraph(newPP));
                return StatusCode(StatusCodes.Status200OK, json);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong, this should not have appened");

        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ChangePreambleParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid, [FromHeader]string newtext,
            [FromServices]Services.ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var realText = newtext.DecodeUrl();

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            if (resolution != null)
            {
                var newPP = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
                if (newPP != null)
                {
                    newPP.Text = realText;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                    resolutionService.RequestSave(resolution);
                    _hubContext.Clients.Group(resolutionid).PreambleParagraphChanged(newPP.ID, newPP.Text);
                    return StatusCode(StatusCodes.Status200OK, json);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Preamble Paragraph not found");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Resolution not found");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ChangeOperativeParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid, [FromHeader]string newtext,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var realText = newtext.DecodeUrl();

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            if (resolution != null)
            {
                var newPP = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
                if (newPP != null)
                {
                    newPP.Text = realText;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                    resolutionService.RequestSave(resolution);
                    _hubContext.Clients.Group(resolutionid).OperativeParagraphChanged(paragraphid, realText);
                    return StatusCode(StatusCodes.Status200OK, json);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Operative Paragraph not found");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Resolution not found");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult RemovePreambleParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            resolution.Preamble.Paragraphs.Remove(paragraph);
            resolutionService.RequestSave(resolution);
            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult MovePreambleParagraphUp([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            paragraph.MoveUp();
            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Groups(resolutionid).PreambleSectionOrderChanged(resolution.Preamble.Paragraphs);

            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult MovePreambleParahraphDown([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices] ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            paragraph.MoveDown();
            resolutionService.RequestSave(resolution);
            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult MoveOperativeParagraphUp([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices] ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            paragraph.MoveUp();
            resolutionService.RequestSave(resolution);
            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult MoveOperativeParagraphDown([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices] ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            paragraph.MoveDown();
            resolutionService.RequestSave(resolution);
            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult MoveOperativeParagraphLeft([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices] ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            paragraph.MoveLeft();
            resolutionService.RequestSave(resolution);
            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult MoveOperativeParagraphRight([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices] ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            paragraph.MoveRight();
            resolutionService.RequestSave(resolution);
            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult RemoveOperativeParagraph([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string paragraphid,
            [FromServices] ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var paragraph = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
            if (paragraph == null)
                return StatusCode(StatusCodes.Status404NotFound, "The Paragraph has not been found!");

            paragraph.Remove();
            resolutionService.RequestSave(resolution);
            return StatusCode(StatusCodes.Status200OK, resolution);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ChangeTitle([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string newtitle,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var realtext = newtitle.DecodeUrl();

            if (string.IsNullOrEmpty(realtext))
                return StatusCode(StatusCodes.Status400BadRequest, "You are not allowed to set an empty title.");

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            resolution.Topic = realtext ?? string.Empty;
            resolutionService.UpdateResolutionName(resolutionid, realtext);
            resolutionService.RequestSave(resolution);
            _hubContext?.Clients.Group(resolution.ID).TitleChanged(realtext);
            return StatusCode(StatusCodes.Status200OK, resolution.ToJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ChangeCommittee([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string newcommitteename,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var realtext = newcommitteename.DecodeUrl();

            if (string.IsNullOrEmpty(realtext))
                return StatusCode(StatusCodes.Status400BadRequest, "You are not allowed to set an empty title.");

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            resolution.CommitteeName = realtext;
            resolutionService.RequestSave(resolution);
            _hubContext?.Clients.Group(resolution.ID).CommitteeChanged(realtext);
            return StatusCode(StatusCodes.Status200OK, resolution.ToJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddDeleteAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string sectionid, [FromHeader]string submittername,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == sectionid);
            if (section == null)
                return StatusCode(StatusCodes.Status404NotFound, "Operative Paragraph Not found!");

            var amendment = new Models.DeleteAmendmentModel();
            amendment.SubmitterName = submittername;
            amendment.TargetSection = section;

            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Group(resolutionid).DeleteAmendmentAdded(new Hubs.HubObjects.HUBDeleteAmendment(amendment));
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddChangeAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string sectionid, [FromHeader]string submittername, [FromHeader]string newtext,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var realText = newtext.DecodeUrl();

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == sectionid);
            if (section == null)
                return StatusCode(StatusCodes.Status404NotFound, "Operative Paragraph Not found!");

            var amendment = new Models.ChangeAmendmentModel();
            amendment.SubmitterName = submittername;
            amendment.NewText = realText;
            amendment.TargetSection = section;

            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Group(resolutionid).ChangeAmendmentAdded(new Hubs.HubObjects.HUBChangeAmendment(amendment));
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddMoveAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string sectionid, [FromHeader]string submittername, [FromHeader]string newposition,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var section = resolution.OperativeSections.FirstOrDefault(n => n.ID == sectionid);
            if (section == null)
                return StatusCode(StatusCodes.Status404NotFound, "Operative Paragraph Not found!");

            int np;
            if (int.TryParse(newposition, out np))
            {
                var amendment = new Models.MoveAmendment();
                amendment.SubmitterName = submittername;
                amendment.NewPosition = np;
                amendment.TargetSection = section;

                resolutionService.RequestSave(resolution);

                _hubContext.Clients.Group(resolutionid).MoveAmendmentAdded(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBMoveAmendment(amendment));
                return StatusCode(StatusCodes.Status200OK);
            }
            return StatusCode(StatusCodes.Status406NotAcceptable, "The new Position has to be a number!");
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddAddAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string submittername, [FromHeader]string newposition,
            [FromHeader]string newtext,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var realtext = newtext.DecodeUrl();

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            int np;
            if (int.TryParse(newposition, out np))
            {
                var amendment = new Models.AddAmendmentModel();
                amendment.SubmitterName = submittername;
                amendment.TargetPosition = np;
                amendment.NewText = realtext;
                amendment.TargetResolution = resolution;

                resolutionService.RequestSave(resolution);

                _hubContext.Clients.Group(resolutionid).AddAmendmentAdded(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBAddAmendment(amendment));
                return StatusCode(StatusCodes.Status200OK);
            }
            return StatusCode(StatusCodes.Status406NotAcceptable, "The new Position has to be a number!");
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult RemoveAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string amendmentid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
            if (amendment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
            }

            amendment.Remove();
            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Group(resolutionid).AmendmentRemoved(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBAbstractAmendment(amendment));
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ActivateAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string amendmentid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
            if (amendment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
            }

            amendment.Activate();
            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Group(resolutionid).AmendmentActivated(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBAbstractAmendment(amendment));
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult DeactivateAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string amendmentid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
            if (amendment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
            }

            amendment.Deactivate();
            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Group(resolutionid).AmendmentDeactivated(new Hubs.HubObjects.HUBResolution(resolution),
                new Hubs.HubObjects.HUBAbstractAmendment(amendment));
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult SubmitAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string amendmentid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
            if (amendment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
            }

            amendment.Submit();
            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Group(resolutionid).AmendmentSubmitted(new Hubs.HubObjects.HUBResolution(resolution));
            return StatusCode(StatusCodes.Status200OK);
        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult DenyAmendment([FromHeader]string auth, [FromHeader]string resolutionid,
            [FromHeader]string amendmentid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var amendment = resolution.Amendments.FirstOrDefault(n => n.ID == amendmentid);
            if (amendment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Amendment not found");
            }

            amendment.Deny();
            resolutionService.RequestSave(resolution);

            _hubContext.Clients.Group(resolutionid).AmendmentDenied(new Hubs.HubObjects.HUBResolution(resolution), new Hubs.HubObjects.HUBAbstractAmendment(amendment));
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult MyResolutions([FromHeader]string auth,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var authresult = authService.ValidateAuthKey(auth);
            if (authresult.valid == false)
                return StatusCode(StatusCodes.Status403Forbidden, "The auth is not valid!");

            var resolutions = resolutionService.GetResolutionsOfUser(authresult.userid);
            return StatusCode(StatusCodes.Status200OK, resolutions);
        }
        
        [Route("[action]")]
        [HttpGet]
        public IActionResult Get([FromHeader]string auth, [FromHeader]string id,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(id);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            return StatusCode(StatusCodes.Status200OK, resolution.ToJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult SubscribeToResolution([FromHeader]string auth, [FromHeader]string id,
            [FromHeader]string connectionid,
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var resolution = resolutionService.GetResolution(id);
            if (resolution == null)
                return StatusCode(StatusCodes.Status404NotFound, "Document not found or you have no right to do that.");

            if (!authService.CanEditResolution(authService.ValidateAuthKey(auth).userid, resolution))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            _hubContext.Groups.AddToGroupAsync(connectionid, id);

            return StatusCode(StatusCodes.Status200OK);
        }

        // PUT: api/Resolution/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
