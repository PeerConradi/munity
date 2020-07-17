using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Models;
using MUNityAngular.Services;
using Newtonsoft.Json;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.Schema.Request;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Core;
using MUNityAngular.Schema.Request.Conference;

namespace MUNityAngular.Controllers
{

    /// <summary>
    /// The Conference Controller needs a complete rework!
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {

        /// <summary>
        /// Returns all the root information of a conference with the given id.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public Task<Conference> GetConference([FromServices]IConferenceService service, string id)
        {
            return service.GetConference(id);
        }

        /// <summary>
        /// Creates a new Project. Every Project needs an organisation, organisations
        /// can host different projects and projects can have different conferences.
        /// For example the Organisation dmun e.V. has three different projects:
        /// MUN Schleswig-Holstein, MUN Baden-Würrtemberg, MUN Brandenburg
        /// every of this project has a list of conferences for example the project
        /// MUN Schleswig-Holstein has the conferences MUN Schleswig-Holstein 2008 and the
        /// conference MUN Schleswig-Holstein 2009. The Project does not have to be fixed
        /// to a location. You could also call projects: UN in the classroom.
        /// Or call your projects One-Day-Mun and Three-Day-Muns. You can use the project
        /// to create a category of Conferences your Organisation hosts. But node that every conference
        /// has to be part of a project.
        /// <seealso cref="Conference"/>
        /// <seealso cref="CreateConference"/>
        /// <seealso cref="OrganisationController"/>
        /// </summary>
        /// <param name="organisationService"></param>
        /// <param name="conferenceService"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<Project>> CreateProject([FromServices] IOrganisationService organisationService,
            [FromServices] IConferenceService conferenceService, [FromBody] CreateProjectRequest body)
        {
            var organisation = await organisationService.GetOrganisation(body.OrganisationId);
            if (organisation == null)
                return NotFound("Not organisation with the given Id was found.");

            // Check if the user is allowed to create projects inside
            var username = User.UsernameClaim();

            var orgaUser = organisation.Member.FirstOrDefault(n => n.User.Username == username);
            if (orgaUser == null)
                return Forbid();

            if (!orgaUser.Role.CanCreateProject)
                return Forbid();


            var project = conferenceService.CreateProject(body.Name, body.Abbreviation, organisation);
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
        /// Creates a new empty conference. The Conference has to be part of a project.
        /// For Example: Model United Nations London 2022 is part of the project
        /// Model United Nations London. When the Conference is Created the Role
        /// Leader will be created automatically and be assigned to the user that created
        /// this conference. You can change this later. Note that for this call you need to
        /// assign a Name, FullName, Abbreviation and the ProjectId. The Start and EndDate are
        /// Optional.
        /// <seealso cref="CreateProject"/>
        /// <seealso cref="Conference"/>
        /// </summary>
        /// <param name="conferenceService"></param>
        /// <param name="authService"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<Conference>> CreateConference(
            [FromServices] IConferenceService conferenceService,
            [FromServices] IAuthService authService,
            CreateConferenceRequest body)
        {
            // Find the parent Project
            var project = await conferenceService.GetProject(body.ProjectId);

            // The parent project does not exist so return 404
            if (project == null)
                return NotFound("project not found");

            // Find the organisation to check if the user is allowed to create conferences
            var organisation = project.ProjectOrganisation;
            var username = User.UsernameClaim();
            var orgaUser = organisation.Member.FirstOrDefault(n => n.User.Username == username);
            if (orgaUser == null)
                return Forbid();
            if (!orgaUser.Role.CanCreateProject)
                return Forbid();

            var user = authService.GetUserOfClaimPrincipal(User);

            var conference = conferenceService.CreateConference(body.Name, body.FullName, body.Abbreviation, project);
            if (conference != null)
            {
                
                // Create the Leader Role
                var role = conferenceService.CreateLeaderRole(conference);
                if (role == null)
                    return StatusCode(500);

                conferenceService.Participate(user, role);

                return Ok(conference);
            }
                

            // When getting to this point something broke
            return StatusCode(500);
            
        }

    }
}