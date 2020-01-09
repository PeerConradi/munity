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

        public ResolutionController(IHubContext<Hubs.ResolutionHub, Hubs.ITypedResolutionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Gives an overview of thats possible inside the resolution Controller
        /// </summary>
        /// <returns>a Text that gives a small overview of all posibilities with the Resolution Controller</returns>
        [HttpGet]
        public string Get()
        {
            string description = "";
            description += "To create a new Resolution Call: /Create?auth=[AUTH_CODE]\n";
            description += "To get the Current Version of a Resolution call Get?auth=[AUTH_CODE]&id=[RESOLUTION_ID]\n";
            description += "The default AUTH_CODE is 'default'\n";
            description += "If you want an example call /Example\n";
            description += "Want to add a preamble paragraph: AddPreambleParagraph?auth=[AUTH_CODE]&resolutionid=[RESOLUTION_ID]\n";
            description += "If you want to add an Operative Paragraph use: AddOperativeParagraph?auth=[AUTH_CODE]&resolutionid=[RESOLUTION_ID]";
            description += "Change Operative Paragraph";
            description += "ChangeOperativeParagraph?auth=[AUTH_ID]&resolutionid=[]&paragraphid=[]&newtext=[]\n";
            description += "\n";
            return description;
        }

        /// <summary>
        /// Creates a new Resolution
        /// </summary>
        /// <param name="auth">The authenticator-code </param>
        /// <returns>The new created Resolution in a json format.</returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create(string auth, [FromServices]Services.ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            if (!authService.ValidateAuthKey(auth).valid)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            return StatusCode(StatusCodes.Status200OK, resolutionService.CreateResolution().ToJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddPreambleParagraph(string auth, string resolutionid, 
            [FromServices]Services.ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {

            if (!authService.ValidateAuthKey(auth).valid)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that.");

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution != null)
            {
                var newPP = resolution.Preamble.AddParagraph();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                resolutionService.Save(resolution);
                _hubContext.Clients.Group(resolutionid).PreambleParagraphAdded(resolution.Preamble.Paragraphs.IndexOf(newPP), newPP.ID, newPP.Text);
                return StatusCode(StatusCodes.Status200OK, json);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Resolution not found");
            }
        }

        /// <summary>
        /// Adds an Premable Paragraph to the Resolution.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="resolutionid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult AddOperativeParagraph(string auth, string resolutionid, 
            [FromServices]ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            if (!authService.ValidateAuthKey(auth).valid)
                return StatusCode(StatusCodes.Status403Forbidden, "You have no right to do that.");

            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution != null)
            {
                var newPP = resolution.AddOperativeParagraph();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                return StatusCode(StatusCodes.Status200OK, json);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Resolution not found");
                //return "error: Resolution Not Found!";
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ChangePreambleParagraph(string auth, string resolutionid, string paragraphid, [FromHeader]string newtext, 
            [FromServices]Services.ResolutionService resolutionService,
            [FromServices]AuthService authService)
        {
            var re = Request;
            var headers = re.Headers;

            if (!authService.ValidateAuthKey(auth).valid)
                return StatusCode(StatusCodes.Status403Forbidden, "You have no right to do that.");


            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution != null)
            {
                var newPP = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.ID == paragraphid);
                if (newPP != null)
                {
                    newPP.Text = newtext;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                    resolutionService.Save(resolution);
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
        public IActionResult ChangeOperativeParagraph(string auth, string resolutionid, string paragraphid, [FromHeader]string newtext, [FromServices]ResolutionService resolutionService)
        {
            var resolution = resolutionService.GetResolution(resolutionid);
            if (resolution != null)
            {
                var newPP = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
                if (newPP != null)
                {
                    newPP.Text = newtext;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
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
        public IActionResult Get(string auth, string id, [FromServices]Services.ResolutionService resolutionService)
        {
            if (auth.ToLower() == "default")
                return StatusCode(StatusCodes.Status200OK, resolutionService.GetResolution(id).ToJson());
            else
                return StatusCode(StatusCodes.Status403Forbidden, "You have no access to this document");
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
