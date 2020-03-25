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
using MUNityAngular.Schema.Request;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        /// <summary>
        /// Returns a list of string that contains the Name of all Conferences that are registered on 
        /// the plattform.
        /// </summary>
        /// <param name="auth">Not needed at the moment, could stay empty</param>
        /// <param name="service"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<string>> GetNameOfAllConferences([FromHeader]string auth,
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, service.GetNameOfAllConferences());
        }

        /// <summary>
        /// Returns the information of a conference with the given id, will return Status 403 if the auth
        /// is not valid. Use the default auth if no other auth exists.
        /// Will return Status 404 if the conference was not found.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="id"></param>
        /// <param name="authService"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<Conference> GetConference([FromHeader]string auth, [FromHeader]string id,
            [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var validation = authService.ValidateAuthKey(auth);
            if (!validation.valid && !authService.IsDefaultAuth(auth))
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to perform this action");

            var conference = conferenceService.GetConference(id);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "I tried so hard and got so far, but in the end, I couldnt find this conference");

            return StatusCode(StatusCodes.Status200OK, conference);

        }

        /// <summary>
        /// Returns a list of all conferences with details were the given Auth as access.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="conferenceService"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<Conference>> GetConferences([FromHeader]string auth,
            [FromServices]ConferenceService conferenceService,
            [FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status200OK, conferenceService.GetAllConferencesOfAuth(auth));
        }

        /// <summary>
        /// Gets the same list as GetConferences but formatted.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="service"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ActionResult<List<Conference>> GetConferencesFormatted([FromHeader]string auth,
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            return StatusCode(StatusCodes.Status203NonAuthoritative, service.GetAllConferencesOfAuth(auth));
        }

        /// <summary>
        /// Change the name of the conference with the given id.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="request"></param>
        /// <param name="service"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("[action]")]
        public ActionResult<Conference> ChangeConferenceName([FromHeader]string auth, [FromBody]ChangeConferenceNameRequest request,
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {
            var authValidation = authService.ValidateAuthKey(auth);
            if (authValidation.valid == false)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");


            

            var conference = service.GetConference(request.ConferenceID);
            if (conference != null)
            {
                var user = authService.GetUserByAuth(auth);

                if (service.GetConferenceUserAuth(conference, user).CanEditSettings == false)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");
                }

                conference.Name = request.NewName;
                service.UpdateConference(conference);
                return StatusCode(StatusCodes.Status200OK, conference);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");
            }

        }

        /// <summary>
        /// Creates a new Conference
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="request"></param>
        /// <param name="service"></param>
        /// <param name="authService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public ActionResult<Conference> Create([FromHeader]string auth, [FromBody]CreateConferenceRequest request,
            [FromServices]ConferenceService service,
            [FromServices]AuthService authService)
        {

            var authstate = authService.GetAuthsByAuthkey(auth);

            if (!authstate.CanCreateConference)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to create a conference!");

            var model = new Conference();
            model.CreationDate = DateTime.Now;
            model.Name = request.Name;
            model.FullName = request.FullName;
            model.Abbreviation = request.Abbreviation;
            model.StartDate = request.StartDate;
            model.EndDate = request.EndDate;
            service.CreateConference(model, authService.GetUserByAuth(auth).UserId);
            return StatusCode(StatusCodes.Status200OK, model);
        }

        /// <summary>
        /// Creates a new Committe inside a conference and returns the new created committee with all
        /// informations and the generated ID for this committee.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="request"></param>
        /// <param name="authService"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public ActionResult<Committee> AddCommittee([FromHeader]string auth, [FromBody]AddCommitteeRequest request,
            [FromServices]AuthService authService,
            [FromServices]ConferenceService conferenceService)
        {
            var canEdit = conferenceService.CanAuthEditConference(auth, request.ConferenceId);
            if (!canEdit)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var conference = conferenceService.GetConference(request.ConferenceId);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found!");


            var committee = new Committee();
            committee.Name = request.Name;
            committee.FullName = request.FullName;
            committee.Abbreviation = request.Abbreviation;
            committee.Article = request.Article;
            committee.ResolutlyCommittee = (string.IsNullOrEmpty(request.ResolutlyCommittee)) ? null : conferenceService.GetCommittee(request.ResolutlyCommittee);
            conferenceService.AddCommittee(conference, committee);
            return StatusCode(StatusCodes.Status200OK, committee);
        }


        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Committee>> GetCommitteesOfConference([FromHeader]string auth, [FromHeader]string conferenceid,
            [FromServices]ConferenceService conferenceService)
        {
            var conference = conferenceService.GetConference(conferenceid);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");

            return conference.Committees;
        }

        /// <summary>
        /// Creates a new Delegation if the old one is not in the list.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="request"></param>
        /// <param name="authService"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public ActionResult<Delegation> CreateDelegation([FromHeader]string auth,
            [FromBody]CreateDelegationRequest request,
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
            
            var delegation = conferenceService.CreateDelegation(request.Name, request.FullName, request.Abbreviation, request.Type.ToString(), "");
            //conferenceService.AddDelegationToConference(conferenceid, delegation.ID);
            return StatusCode(StatusCodes.Status200OK, delegation);
        }

        /// <summary>
        /// Binds a Delegation to a Committee
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="committeeid"></param>
        /// <param name="delegationid"></param>
        /// <param name="mincount"></param>
        /// <param name="maxcount"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public ActionResult<Committee> AddDelegationToCommittee([FromHeader]string auth, [FromHeader]string committeeid, [FromHeader]string delegationid,
            [FromHeader]string mincount, [FromHeader]string maxcount,
            [FromServices]ConferenceService conferenceService)
        {
            

            var committee = conferenceService.GetCommittee(committeeid);
            if (committee == null)
                return StatusCode(StatusCodes.Status404NotFound, "Committee not found!");

            var conference = conferenceService.GetConferenceOfCommittee(committee);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");

            var canEdit = conferenceService.CanAuthEditConference(auth, conference.ConferenceId);
            if (!canEdit)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var delegation = conferenceService.GetDelegation(delegationid);
            if (delegation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Delegation not found with the given id");

            var result = conferenceService.AddDelegationToCommittee(committee, delegation, mincount.ToIntOrDefault(1), maxcount.ToIntOrDefault(1));

            if (result == true)
            {
                return StatusCode(StatusCodes.Status200OK, committee);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to add Delegation to Committee");
            }
        }

        /// <summary>
        /// Returns all delegations that are inside a given committee
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="committeeid"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<Delegation> GetDelegationsOfCommittee([FromHeader]string auth, [FromHeader]string committeeid,
            [FromServices]ConferenceService conferenceService)
        {
            //Authentication egal
            var delegations = conferenceService.GetDelegationsOfCommittee(committeeid);
            return StatusCode(StatusCodes.Status200OK, delegations);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<TeamUser>> GetTeam([FromHeader]string auth, [FromHeader]string conferenceid,
            [FromServices]ConferenceService conferenceService)
        {
            //Das Team darf ersteinmal jeder sehen
            var conference = conferenceService.GetConference(conferenceid);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");
            return StatusCode(StatusCodes.Status200OK, conferenceService.GetConferenceTeam(conference));
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<TeamRole>> GetTeamRoles([FromHeader]string auth,
            [FromHeader]string conferenceid, [FromServices]ConferenceService conferenceService)
        {
            //Die Rollen dürfen auch zunächst öffentlich sein
            var roles = conferenceService.GetRolesOfConference(conferenceid);
            return StatusCode(StatusCodes.Status200OK, roles);
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult AddTeamRole([FromHeader]string auth, [FromBody]TeamRoleRequest role,
            [FromServices]ConferenceService conferenceService)
        {
            var canEdit = conferenceService.CanAuthEditConference(auth, role.ConferenceId);
            if (!canEdit)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var conference = conferenceService.GetConference(role.ConferenceId);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found");

            var model = new TeamRole();
            model.Name = role.Name;
            model.Description = role.Description;
            model.MinCount = role.MinCount.ToIntOrDefault(1);
            model.MaxCount = role.MaxCount.ToIntOrDefault(1);
            model.Conference = conference;
            if (!string.IsNullOrEmpty(role.ParentRoleId))
            {
                model.ParentRole = conferenceService.GetTeamRole(role.ParentRoleId.ToIntOrDefault(-1));
            }
            conferenceService.AddTeamRole(model);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult AddUserToTeam([FromHeader]string auth, [FromBody]AddTeamMemberRequestBody value,
            [FromServices]ConferenceService conferenceService, [FromServices]AuthService authService)
        {
            var role = conferenceService.GetTeamRole(value.RoleId.ToIntOrDefault(-1));
            if (role == null)
                return StatusCode(StatusCodes.Status404NotFound, "Role not found!");

            var canEdit = conferenceService.CanAuthEditConference(auth, role.Conference.ConferenceId);
            if (!canEdit)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var user = authService.GetUserByUsername(value.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, "User not found");

            conferenceService.AddUserToConferenceTeam(user, role);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<Committee> GetCommittee([FromHeader]string auth, [FromHeader]string committeeid,
            [FromServices]ConferenceService conferenceService)
        {
            return conferenceService.GetCommittee(committeeid);
        }

        [Route("[action]")]
        [HttpPut]
        public ActionResult SetCommitteeStatus([FromHeader]string auth, [FromBody]CommitteeStatusRequestBody status,
            [FromServices]ConferenceService conferenceService, [FromServices]AuthService authService)
        {
            var user = authService.GetUserByAuth(auth);
            if (user == null)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that");

            var committee = conferenceService.GetCommittee(status.CommitteeId);
            if (committee == null)
                return StatusCode(StatusCodes.Status404NotFound, "Committee not found");

            var conference = conferenceService.GetConferenceOfCommittee(committee);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference for the committee nout found");

            var team = conferenceService.GetTeamUsers(conference);
            if (team.Find(n => n == user) == null)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var model = new CommitteeStatus();
            model.AgendaItem = status.AgendaItem;
            model.Committee = committee;
            model.Status = status.Status;
            model.Timestamp = DateTime.Now;
            conferenceService.SetCommitteeStatus(model);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<CommitteeStatus> GetCommitteeStatus([FromHeader]string committeeid,
            [FromServices]ConferenceService conferenceService)
        {
            var committee = conferenceService.GetCommittee(committeeid);
            if (committee == null)
                return StatusCode(StatusCodes.Status404NotFound, "Committee not found");
            return conferenceService.GetCommitteeStatus(committee);
        }

        //Returns a list of all public visible Delegations.
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<Delegation>> AllDelegations([FromServices]ConferenceService conferenceService)
        {
            return StatusCode(StatusCodes.Status200OK, conferenceService.GetAllDelegations());
        }
    }
}