using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Services;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerlistController : ControllerBase
    {
        [Route("[action]")]
        [HttpGet]
        public IActionResult GetSpeakerlist([FromHeader]string auth, [FromHeader]string id,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            //var authstate = authService.ValidateAuthKey(auth);

            //Is a speakerlist public or not needs to be checked
            var speakerlist = speakerlistService.GetSpeakerlist(id);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist cannot be found!");

            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult CreateSpeakerlist([FromHeader]string auth,
            [FromHeader]string conferenceid,
            [FromHeader]string committeeid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService,
            [FromServices]ConferenceService conferenceSerivce)
        {
            var speakerlist = new Models.SpeakerlistModel();
            speakerlistService.AddSpeakerlist(speakerlist);
            speakerlist.ConferenceId = conferenceid;
            speakerlist.CommitteeId = committeeid;

            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetPossibleDelegations([FromHeader]string auth,
            [FromHeader]string id,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService,
            [FromServices]ConferenceService conferenceService)
        {
            //TODO: Auth stuff

            var speakerlist = speakerlistService.GetSpeakerlist(id);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            if (speakerlist.ConferenceId == null || speakerlist.ConferenceId == string.Empty)
                return StatusCode(StatusCodes.Status428PreconditionRequired, "No Delegations found, because the Speaerklsit is not bound to a conference");

            var delegations = conferenceService.GetDelegationsOfConference(speakerlist.ConferenceId);
            return StatusCode(StatusCodes.Status200OK, delegations);
        }
    }
}