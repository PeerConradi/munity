using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Models;
using MUNityAngular.Services;
using Newtonsoft.Json;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            string guide = "";
            guide += "To Create a new conference call: Create?auth=[AUTH_CODE]";
            return guide;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetNameOfAllConferences(string auth, 
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, service.GetNameOfAllConferences());
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetConferences([FromServices]ConferenceService service)
        {
            return StatusCode(StatusCodes.Status200OK, service.GetAllConferences());
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetConferencesFormatted(string auth,
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            var text = JsonConvert.SerializeObject(service.GetAllConferences(), Newtonsoft.Json.Formatting.Indented);
            return StatusCode(StatusCodes.Status200OK, text);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ChangeConferenceName([FromHeader]string auth, [FromHeader]string conferenceid, [FromHeader]string password, [FromHeader]string newname, 
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            //Auth not requered now but to be implemented later

            //Check the Conference password with the auth by now!
            if (!service.CheckConferencePassword(conferenceid, password))
                return StatusCode(StatusCodes.Status401Unauthorized, "You are not allowed to do that. The password may be invalid!");

            var conference = service.GetConference(conferenceid);

            if (conference != null)
            {
                var success = service.ChangeConferenceName(conference, newname);
                if (success)
                {
                    return StatusCode(StatusCodes.Status200OK, conference);
                }
                else
                {
                   return  StatusCode(StatusCodes.Status500InternalServerError, "Unable to change Conference Name");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");
            }
            
        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult Create(string auth, [FromHeader]string Name, [FromHeader]string FullName, 
            [FromHeader]string Abbreviation, [FromHeader]string password, [FromHeader]DateTime StartDate, 
            [FromHeader]DateTime EndDate, 
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            var model = new ConferenceModel();
            model.CreationDate = DateTime.Now;
            model.Name = Name;
            model.FullName = FullName;
            model.Abbreviation = Abbreviation;
            model.StartDate = StartDate;
            model.EndDate = EndDate;
            service.CreateConference(model, password);
            return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(model));
        }
    }
}