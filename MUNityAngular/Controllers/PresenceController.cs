using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models;
using MUNityAngular.Services;

namespace MUNityAngular.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PresenceController : Controller
    {
        [Route("[action]")]
        [HttpPut]
        public ActionResult SavePresence([FromHeader]string auth, [FromBody]PresenceModel presence,
            [FromServices]PresenceService presenceService,
            [FromServices]ConferenceService conferenceService,
            [FromServices]AuthService authService)
        {
            var user = authService.GetUserByAuth(auth);
            if (user == null)
                return StatusCode(StatusCodes.Status403Forbidden, "The authkey is not linked to a user");
            var committee = conferenceService.GetCommittee(presence.CommitteeId);
            if (committee == null)
                return StatusCode(StatusCodes.Status404NotFound, "Committee not found!");
            var conference = conferenceService.GetConferenceOfCommittee(committee);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found!");
            var teamRole = conferenceService.GetUserTeamRolesAtConference(user, conference);
            if (teamRole == null)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            presence.CheckDate = DateTime.Now;
            presenceService.SavePresence(presence);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<PresenceModel> GetCommitteePresence([FromHeader]string auth, [FromHeader]string committeeid,
            [FromServices]PresenceService presenceService,
            [FromServices]ConferenceService conferenceService,
            [FromServices]AuthService authService)
        {
            var user = authService.GetUserByAuth(auth);
            if (user == null)
                return StatusCode(StatusCodes.Status403Forbidden, "The authkey is not linked to a user");
            var committee = conferenceService.GetCommittee(committeeid);
            if (committee == null)
                return StatusCode(StatusCodes.Status404NotFound, "Committee not found!");
            var conference = conferenceService.GetConferenceOfCommittee(committee);
            if (conference == null)
                return StatusCode(StatusCodes.Status404NotFound, "Conference not found!");
            var teamRole = conferenceService.GetUserTeamRolesAtConference(user, conference);
            if (teamRole == null)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            var presence = presenceService.GetLatestPresence(committeeid);
            return presence;
        }
    }
}