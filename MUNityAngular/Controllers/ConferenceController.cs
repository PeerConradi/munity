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
using MUNityAngular.Util.Extenstions;
using MUNityAngular.Models.Conference;

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
        public IActionResult GetNameOfAllConferences([FromHeader]string auth, 
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, service.GetNameOfAllConferences().ToNewtonsoftJson());
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetConference([FromHeader]string auth, [FromHeader]string id,
            [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (!validation.valid && !authService.IsDefaultAuth(auth))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to perform this action");

            var conference = conferenceService.GetConference(id);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "I tried so hard and got so far, but in the end, I couldnt find this conference");

            return StatusCode(StatusCodes.Status200OK, conference.AsNewtonsoftJson());

        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetConferences([FromHeader]string auth, 
            [FromServices]ConferenceService conferenceService,
            [FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, conferenceService.GetAllConferencesOfAuth(auth).ToNewtonsoftJson());
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetConferencesFormatted([FromHeader]string auth,
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            var text = JsonConvert.SerializeObject(service.GetAllConferencesOfAuth(auth), Newtonsoft.Json.Formatting.Indented);
            return StatusCode(StatusCodes.Status200OK, text);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult ChangeConferenceName([FromHeader]string auth, [FromHeader]string conferenceid, [FromHeader]string newname, 
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            bool hasChangeAccess = false;

            //Auth not requered now but to be implemented later
            if (service.CanAuthEditConference(auth, conferenceid))
                hasChangeAccess = true;

            if (!hasChangeAccess)
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
        public IActionResult Create([FromHeader]string auth, [FromHeader]string Name, [FromHeader]string FullName, 
            [FromHeader]string Abbreviation, [FromHeader]DateTime StartDate, 
            [FromHeader]DateTime EndDate, 
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            var authstate = authService.GetAuthsByAuthkey(auth);

            if (!authstate.CreateConference)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to create a conference!");

            var model = new ConferenceModel();
            model.CreationDate = DateTime.Now;
            model.Name = Name;
            model.FullName = FullName;
            model.Abbreviation = Abbreviation;
            model.StartDate = StartDate;
            model.EndDate = EndDate;
            service.CreateConference(model, authstate.UserId);
            return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(model));
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddCommittee([FromHeader]string auth, [FromHeader]string conferenceid,
            [FromHeader]string name, [FromHeader]string fullname, [FromHeader]string abbreviation,
            [FromHeader]string article, [FromHeader]string resolutlycommittee,
            [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var canEdit = conferenceService.CanAuthEditConference(auth, conferenceid);
            if (!canEdit)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var conference = conferenceService.GetConference(conferenceid);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found!");


            var committee = new CommitteeModel();
            committee.Name = name;
            committee.FullName = fullname;
            committee.Abbreviation = abbreviation;
            committee.Article = article;
            committee.ResolutlyCommitteeID = (string.IsNullOrEmpty(resolutlycommittee)) ? resolutlycommittee : null;
            conferenceService.AddCommittee(conference, committee);
            return StatusCode(StatusCodes.Status200OK, committee.AsNewtonsoftJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult CreateDelegation([FromHeader]string auth,
            [FromHeader]string name, [FromHeader]string fullname, [FromHeader]string abbreviation,
            [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            //Eine Delegation existiert global über mehrere Konferenzen hinweg, so kann eine default Palette an Delegationen gestellt werden,
            //welche erst einmal nichts miteinander zutun haben. 
            //Später muss es möglich sein, diese als "NOT LISTED" zu markieren, damit nicht so viel Noise entsteht falls jemand 20x Vereinigte Staaten anlegt...
            //Dazu könnte es ein "See also" geben...
            var authCheck = authService.ValidateAuthKey(auth);

            if (!authCheck.valid)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var delegation = conferenceService.CreateDelegation(name.DecodeUrl(), fullname.DecodeUrl(), abbreviation.DecodeUrl(), "COUNTRY");
            //conferenceService.AddDelegationToConference(conferenceid, delegation.ID);
            return StatusCode(StatusCodes.Status200OK, delegation.AsNewtonsoftJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddDelegationToConference([FromHeader]string auth, [FromHeader]string conferenceid, [FromHeader]string delegationid,
            [FromHeader]string mincount, [FromHeader]string maxcount,
            [FromServices]ConferenceService conferenceService)
        {
            var canEdit = conferenceService.CanAuthEditConference(auth, conferenceid);
            if (!canEdit)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var delegation = conferenceService.GetDelegation(delegationid);
            if (delegation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Delegation not found with the given id");

            var conference = conferenceService.GetConference(conferenceid);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");



            conferenceService.AddDelegationToConference(conferenceid, delegationid, mincount.ToIntOrDefault(1), maxcount.ToIntOrDefault(1));
            conference.Delegations.Add(delegation);
            return StatusCode(StatusCodes.Status200OK, delegation.AsNewtonsoftJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddDelegationToCommittee([FromHeader]string auth, [FromHeader]string committeeid, [FromHeader]string delegationid,
            [FromHeader]string mincount, [FromHeader]string maxcount,
            [FromServices]ConferenceService conferenceService)
        {
            var committee = conferenceService.GetCommittee(committeeid);
            if (committee == null)
                return StatusCode(StatusCodes.Status404NotFound, "Committee not found!");

            

            var canEdit = conferenceService.CanAuthEditConference(auth, committee.ConferenceID);
            if (!canEdit)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var delegation = conferenceService.GetDelegation(delegationid);
            if (delegation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Delegation not found with the given id");

            var conference = conferenceService.GetConference(committee.ConferenceID);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");


            var result = conferenceService.AddDelegationToCommittee(committee, delegation, mincount.ToIntOrDefault(1), maxcount.ToIntOrDefault(1));
            var committeeInConferece = conferenceService.GetConference(committee.ConferenceID).Committees.FirstOrDefault(n => n.ID == committeeid);

            if (result == true)
            {
                return StatusCode(StatusCodes.Status200OK, committeeInConferece.AsNewtonsoftJson());
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to add Delegation to Committee");
            }
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetDelegationsOfCommittee([FromHeader]string auth, [FromHeader]string committeeid,
            [FromServices]ConferenceService conferenceService)
        {
            //Authentication egal
            var delegations = conferenceService.GetDelegationsOfCommittee(committeeid);
            return StatusCode(StatusCodes.Status200OK, delegations.AsNewtonsoftJson());

        }
        [Route("[action]")]
        [HttpGet]
        public IActionResult AllDelegations([FromServices]ConferenceService conferenceService)
        {
            return StatusCode(StatusCodes.Status200OK, conferenceService.GetAllDelegations().ToNewtonsoftJson());
        }
    }
}