using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MUNityCore.DataHandlers.Database;
using MUNityCore.Models;
using Newtonsoft.Json;
using MUNityCore.Util.Extenstions;
using MUNityCore.DataHandlers.EntityFramework.Models;
using MUNityCore.Models.Core;
using MUNityCore.Models.Conference;
using MUNityCore.Schema.Request;
using MUNityCore.Schema.Request.Conference;
using MUNityCore.Services;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The Conference Controller needs a complete rework!
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {

        private readonly Services.IConferenceService _conferenceService;

        private readonly IOrganisationService _organisationService;

        private readonly IAuthService _authService;

        /// <summary>
        /// Returns all the root information of a conference with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<Conference>> GetConference(string id)
        {
            var conference = await this._conferenceService.GetConference(id);
            if (conference == null)
                return NotFound("Conference not found");

            // Check if the conference is visible, then give out the data
            if (conference.Visibility == Conference.EConferenceVisibilityMode.Public)
                return Ok(conference);
            else if (conference.Visibility == Conference.EConferenceVisibilityMode.Users)
            {
                if (User != null)
                    return Ok(conference);
            }

            return BadRequest("Unable to get the conference, maybe its not opened to the public.");
        }

        /// <summary>
        /// Selects all conferences that are visible to the public where the name, fullname or abbreviation contain
        /// the name. The search is case insensitive.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Conference>> FindConferencesByName(string name)
        {
            return Ok(this._conferenceService.FindPublicConferencesByName(name));
        }

        /// <summary>
        /// Creates a new Project. Every Project needs an organization, organisations
        /// can host different projects and projects can have different conferences.
        /// For example the Organization dmun e.V. has three different projects:
        /// MUN Schleswig-Holstein, MUN Baden-Würrtemberg, MUN Brandenburg
        /// every of this project has a list of conferences for example the project
        /// MUN Schleswig-Holstein has the conferences MUN Schleswig-Holstein 2008 and the
        /// conference MUN Schleswig-Holstein 2009. The Project does not have to be fixed
        /// to a location. You could also call projects: UN in the classroom.
        /// Or call your projects One-Day-Mun and Three-Day-Muns. You can use the project
        /// to create a category of Conferences your Organization hosts. But node that every conference
        /// has to be part of a project.
        /// <seealso cref="Conference"/>
        /// <seealso cref="CreateConference"/>
        /// <seealso cref="OrganisationController"/>
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectRequest body)
        {
            var organisation = await _organisationService.GetOrganisationWithMembers(body.OrganisationId);
            if (organisation == null)
                return NotFound("Not organization with the given Id was found.");

            // Check if the user is allowed to create projects inside
            var username = User.UsernameClaim();

            if (!this._organisationService.CanUserCreateProject(username, organisation.OrganizationId))
                return Forbid();

            var project = _conferenceService.CreateProject(body.Name, body.Abbreviation, organisation);
            if (project != null)
            {
                return Ok(project);
            }
            else
            {
                return BadRequest("Something went wrong creating the project!");
            }
        }

        /// <summary>
        /// Returns a specific project. Note that this will only give a limit amount of information
        /// about the project. You can also call GetProjectWithConferences, if you want the list of
        /// conferences that this project holds withing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<Project>> GetProject(string id)
        {
            var result = await this._conferenceService.GetProject(id);
            return Ok(result);
        }

        public ActionResult<IEnumerable<Project>> FindProjectsByName(string name)
        {
            return Ok(this._conferenceService.FindProjectsByName(name));
        }

        /// <summary>
        /// This will return a project with all the conferences inside this project. The Conferences will contain
        /// all information about the conference (except user information and committees). To get this information
        /// use different api calls.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<Project>> GetProjectWithConferences(string id)
        {
            var result = await this._conferenceService.GetProjectWithConferences(id);
            return Ok(result);
        }

        /// <summary>
        /// Returns a list of all Projects that an organization is hosting. This projects do not contain
        /// the conferences.
        /// </summary>
        /// <param name="organisationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Project>> GetOrganisationProjects(string organisationId)
        {
            var result = this._organisationService.GetOrganisationProjects(organisationId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new empty conference. The Conference has to be part of a project.
        /// For Example: Model United Nations London 2022 is part of the project
        /// Model United Nations London. When the Conference is Created the Role
        /// Leader will be created automatically and be assigned to the user that created
        /// this conference. You can change this later. Note that for this call you need to
        /// assign a Name, FullName, CommitteeShort and the ProjectId. The Start and EndDate are
        /// Optional.
        /// <seealso cref="CreateProject"/>
        /// <seealso cref="Conference"/>
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Conference>> CreateConference(CreateConferenceRequest body)
        {
            // Find the parent Project
            var project = await _organisationService.GetProjectWithOrganisation(body.ProjectId);

            // The parent project does not exist so return 404
            if (project == null)
                return NotFound("project not found");

            // Find the organization to check if the user is allowed to create conferences
            var organisation = project.ProjectOrganization;
            var user = _authService.GetUserOfClaimPrincipal(User);

            if (user == null)
                return Forbid();

            if (!this._organisationService.CanUserCreateProject(user.Username, organisation.OrganizationId))
                return Forbid();

            var conference = _conferenceService.CreateConference(body.Name, body.FullName, body.Abbreviation, project);
            if (conference != null)
            {
                
                // TODO: Set the Start and End Date
                

                // Create the Leader Role
                var role = _conferenceService.CreateLeaderRole(conference);
                if (role == null)
                    return StatusCode(500);

                _conferenceService.Participate(user, role);

                return Ok(conference);
            }
                

            // When getting to this point something broke
            return StatusCode(500);
        }

        /// <summary>
        /// Changes the name of a conference.
        /// Max-Length 150 characters
        /// The name has to be unique for each conference that exists on this page.
        /// An Example name could be: MUN Berlin 2021
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> SetConferenceName([FromBody] ConferenceRequests.ChangeConferenceName body)
        {
            if (!CanUserEditConference(body.ConferenceId))
                return Forbid("You are not allowed to change the conference settings");

            var taken = await this._conferenceService.IsConferenceNameTaken(body.NewName);
            if (taken) return Conflict("The new conference name is already taken by another conference.");

            var res =await this._conferenceService.SetConferenceName(body.ConferenceId, body.NewName);
            return Ok(res);
        }

        /// <summary>
        /// Changes the full name (long name) of a conference.
        /// Max: 250 characters
        /// Conference Full Names have to be unique. For Example: Model United Nations Berlin 2021
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> SetConferenceFullName([FromBody] ConferenceRequests.ChangeConferenceFullName body)
        {
            if (!CanUserEditConference(body.ConferenceId))
                return Forbid("You are not allowed to change the conference settings");

            var taken = await this._conferenceService.IsConferenceFullNameTaken(body.NewFullName);
            if (taken) return Conflict("The new conference full name is already taken by another conference.");

            var res = await this._conferenceService.SetConferenceFullName(body.ConferenceId, body.NewFullName);
            return Ok(res);
        }

        /// <summary>
        /// Change the short name of the conference. You need to be allowed to Edit the Conference Settings to do this.
        /// The maxLenght for the conferenceid is 80 characters  and the max-Length of the abbreviation is 18 chars.
        /// An Example could be MUN-BER 2021
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> SetConferenceAbbreviation([FromBody] ConferenceRequests.ChangeConferenceAbbreviation body)
        {
            if (!CanUserEditConference(body.ConferenceId))
                return Forbid("You are not allowed to change the conference settings");

            var res = await this._conferenceService.SetConferenceAbbreviation(body.ConferenceId, body.NewAbbreviation);
            return Ok(res);
        }

        /// <summary>
        /// Changes the Dates when the conference will take place.
        /// This is a DateTime so make sure you give it a Date in the needed format
        /// and the start Date is before the end Date. You are only allowed to use this route when you
        /// are logged in and are allowed to Edit Conference Settings!
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> SetConferenceDate([FromBody] ConferenceRequests.ChangeConferenceDate body)
        {
            if (body.StartDate > body.EndDate)
                return BadRequest("The Start Date has to be before the End Date!");
            if (!CanUserEditConference(body.ConferenceId))
                return Forbid("You are not allowed to change the conference settings");

            var res = await this._conferenceService.SetConferenceDate(body.ConferenceId, body.StartDate, body.EndDate);
            return Ok(res);
        }

        /// <summary>
        /// Creates a new Committee inside a Conference.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Committee>> CreateCommittee([FromBody] ConferenceRequests.CreateCommittee body)
        {
            if (!CanUserEditConference(body.ConferenceId))
                return Forbid("You are not allowed to change the conference settings");

            var conference = await this._conferenceService.GetConference(body.ConferenceId);
            if (conference == null) return NotFound("Conference with the given id not found!");

            if (conference.Committees.Any(n => n.Name.ToLower() == body.Name.ToLower()
                                               || n.FullName.ToLower() == body.FullName.ToLower()))
                return Conflict("There is already a committee with the given name in this conference.");

            var newCommittee = new Committee(body);
            if (!string.IsNullOrEmpty(body.ResolutlyCommitteeId))
            {
                var parentCommittee = await this._conferenceService.GetCommittee(body.ResolutlyCommitteeId);
                if (parentCommittee == null) return NotFound("The parent committee was not found!");

                newCommittee.ResolutlyCommittee = parentCommittee;
            }

            conference.Committees.Add(newCommittee);
            await this._conferenceService.SaveDatabaseChanges();
            return Ok(newCommittee);
        }




        /// <summary>
        /// No API Call. A function to get if the current UserPrincipal is allowed to edit the Conference.
        /// </summary>
        /// <param name="conferenceid"></param>
        /// <returns></returns>
        [NonAction]
        private bool CanUserEditConference(string conferenceid)
        {
            var user = this._authService.GetUserOfClaimPrincipal(User);
            var roles = this._conferenceService.GetUserRolesOnConference(user.Username, conferenceid)
                .Include(n => n.RoleAuth);
            if (!roles.Any(n => n.RoleAuth.CanEditConferenceSettings)) return true;

            return false;
        }

        public ConferenceController(IConferenceService conferenceService, IAuthService authService, IOrganisationService organisationService)
        {
            this._conferenceService = conferenceService;
            this._authService = authService;
            this._organisationService = organisationService;
        }

    }
}