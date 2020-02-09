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
    public class SpeakerlistController : ControllerBase
    {

        IHubContext<Hubs.SpeakerListHub, Hubs.ITypedSpeakerlistHub> _hubContext;

        public SpeakerlistController(IHubContext<Hubs.SpeakerListHub, Hubs.ITypedSpeakerlistHub> hubContext)
        {
            this._hubContext = hubContext;
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
            var speakerlist = speakerlistService.CreateSpeakerlist();
            speakerlist.ConferenceId = conferenceid;
            speakerlist.CommitteeId = committeeid;

            return StatusCode(StatusCodes.Status200OK, speakerlist.AsNewtonsoftJson());
        }

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

            return StatusCode(StatusCodes.Status200OK, speakerlist.AsNewtonsoftJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult ReadSpeakerlist([FromHeader]string auth, [FromHeader]string publicid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            if (int.TryParse(publicid, out int id))
            {
                var speakerlist = speakerlistService.GetSpeakerlistByPublicId(id);
                speakerlist.ID = "";
                return StatusCode(StatusCodes.Status200OK, speakerlist.AsNewtonsoftJson());
            }

            return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");
            
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult SubscribeToList([FromHeader]string auth, [FromHeader]string publicid, [FromHeader]string connectionid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            _hubContext.Groups.AddToGroupAsync(connectionid,"s-list-" + publicid);
            return StatusCode(StatusCodes.Status200OK);    
        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult AddSpeakerToList([FromHeader]string auth,
            [FromHeader]string listid,
            [FromHeader]string delegationid,
            [FromServices]SpeakerlistService speakerlistService,
            [FromServices]ConferenceService conferenceService)
        {
            var delegation = conferenceService.GetDelegation(delegationid);
            if (delegation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Delegation not found!");

            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.AddSpeaker(delegation);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            //this._hubContext.Clients.All.SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK, speakerlist.AsNewtonsoftJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult AddQuestionToList([FromHeader]string auth,
            [FromHeader]string listid, [FromHeader]string delegationid,
            [FromServices]SpeakerlistService speakerlistService,
            [FromServices]ConferenceService conferenceService)
        {
            var delegation = conferenceService.GetDelegation(delegationid);
            if (delegation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Delegation not found!");

            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.AddQuestion(delegation);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            //this._hubContext.Clients.All.SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK, speakerlist.AsNewtonsoftJson());
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult RemoveSpeakerFromList([FromHeader]string auth,
            [FromHeader]string listid, [FromHeader]string delegationid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.RemoveSpeaker(delegationid);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [Route("[action]")]
        [HttpGet]
        public IActionResult NextSpeaker([FromHeader]string auth, [FromHeader]string listid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.NextSpeaker();
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult StartSpeaker([FromHeader]string auth, [FromHeader]string listid, [FromHeader]string remainingTime,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            int time = (int)speakerlist.RemainingSpeakerTime.TotalSeconds;
            if (int.TryParse(remainingTime, out int secs))
            {
                time = secs;
            }

            speakerlist.StartSpeaker();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerTimerStarted(time);
            return StatusCode(StatusCodes.Status200OK);
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