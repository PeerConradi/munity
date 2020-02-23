using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityAngular.Models;
using MUNityAngular.Models.Conference;
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

        /// <summary>
        /// Creates a new Speakerlist and returns it.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="conferenceid"></param>
        /// <param name="committeeid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <param name="conferenceSerivce"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<SpeakerlistModel> CreateSpeakerlist([FromHeader]string auth,
            [FromHeader]string conferenceid,
            [FromHeader]string committeeid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService,
            [FromServices]ConferenceService conferenceSerivce)
        {
            var speakerlist = speakerlistService.CreateSpeakerlist();
            speakerlist.ConferenceId = conferenceid;
            speakerlist.CommitteeId = committeeid;

            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        /// <summary>
        /// Gets a speakerlist
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="id"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<SpeakerlistModel> GetSpeakerlist([FromHeader]string auth, [FromHeader]string id,
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

        /// <summary>
        /// Gets a speakerlist with the public ID
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="publicid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<SpeakerlistModel> ReadSpeakerlist([FromHeader]string auth, [FromHeader]string publicid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            if (int.TryParse(publicid, out int id))
            {
                var speakerlist = speakerlistService.GetSpeakerlistByPublicId(id);
                speakerlist.ID = "";
                return StatusCode(StatusCodes.Status200OK, speakerlist);
            }

            return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

        }

        /// <summary>
        /// Subscribes to a speakerlist
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="publicid"></param>
        /// <param name="connectionid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult SubscribeToList([FromHeader]string auth, [FromHeader]string publicid, [FromHeader]string connectionid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            _hubContext.Groups.AddToGroupAsync(connectionid, "s-list-" + publicid);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Adds a speaker to the speakerlist
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="delegationid"></param>
        /// <param name="speakerlistService"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public ActionResult<SpeakerlistModel> AddSpeakerToList([FromHeader]string auth,
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
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult<SpeakerlistModel> AddSpeakerModelToList([FromHeader]string auth,
            [FromHeader]string listid, [FromBody]DelegationModel model, 
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");
            model.ID = Guid.NewGuid().ToString();
            speakerlist.AddSpeaker(model);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        [Route("[action]")]
        [HttpPatch]
        public ActionResult<SpeakerlistModel> SpeakersOrderChanged([FromHeader]string auth,
            [FromHeader]string listid, [FromBody]List<DelegationModel> model,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            model.ForEach(n =>
            {
                if (string.IsNullOrWhiteSpace(n.ID))
                {
                    n.ID = Guid.NewGuid().ToString();
                }
            });
            speakerlist.Speakers = model;
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return speakerlist;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult<SpeakerlistModel> AddQuestionModelToList([FromHeader]string auth,
            [FromHeader]string listid, [FromBody]DelegationModel model,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");
            model.ID = Guid.NewGuid().ToString();
            speakerlist.AddQuestion(model);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        /// <summary>
        /// Adds a Question to the Speakerlist (Question Area)
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="delegationid"></param>
        /// <param name="speakerlistService"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public ActionResult<SpeakerlistModel> AddQuestionToList([FromHeader]string auth,
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
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        /// <summary>
        /// Removes a speaker from the speakerlist.
        /// This function will remove the first entry of the delegationid it will
        /// find.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="delegationid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpDelete]
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

        /// <summary>
        /// Will set the next speaker to the list.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
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

        /// <summary>
        /// Starts the Speaking Timer
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult StartSpeaker([FromHeader]string auth, [FromHeader]string listid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.StartSpeaker();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerTimerStarted((int)speakerlist.RemainingSpeakerTime.TotalSeconds);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Sets the next question on the list
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult NextQuestion([FromHeader]string auth, [FromHeader]string listid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.NextQuestion();
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Starts the Question Timer
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult StartQuestion([FromHeader]string auth, [FromHeader]string listid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.StartQuestion();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).QuestionTimerStarted((int)speakerlist.RemainingQuestionTime.TotalSeconds);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Starts the Answer Timer for the speaker.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult StartAnswer([FromHeader]string auth, [FromHeader]string listid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.StartAnswer();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerTimerStarted((int)speakerlist.RemainingSpeakerTime.TotalSeconds);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Pauses the speaker timer.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult PauseTimer([FromHeader]string auth, [FromHeader]string listid,
           [FromServices]AuthService authService,
           [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.PauseSpeaker();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).TimerStopped();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Changes the maximum timefor each speaker.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="time"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        public IActionResult SetSpeakertime([FromHeader]string auth, [FromHeader]string listid, [FromHeader]string time,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            var newTime = time.ToTimeSpan();
            if (newTime.HasValue == false)
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Time Format it should be hh:mm:ss");

            speakerlist.Speakertime = newTime.Value;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// changes the maximum time for each question.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="time"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        public IActionResult SetQuestiontime([FromHeader]string auth, [FromHeader]string listid, [FromHeader]string time,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            var newTime = time.ToTimeSpan();
            if (newTime.HasValue == false)
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Time Format it should be hh:mm:ss");

            speakerlist.Questiontime = newTime.Value;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Clears the current Speaker from the list.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult ClearSpeaker([FromHeader]string auth, [FromHeader]string listid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");


            if (speakerlist.Status == Models.SpeakerlistModel.EStatus.SPEAKING)
                speakerlist.Status = Models.SpeakerlistModel.EStatus.STOPPED;

            speakerlist.CurrentSpeaker = null;
            speakerlist.RemainingSpeakerTime = speakerlist.Speakertime;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Clears the current Question from the list
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="listid"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult ClearQuestion([FromHeader]string auth, [FromHeader]string listid,
            [FromServices]AuthService authService,
            [FromServices]SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            if (speakerlist.Status == Models.SpeakerlistModel.EStatus.QUESTION)
                speakerlist.Status = Models.SpeakerlistModel.EStatus.STOPPED;

            speakerlist.CurrentQuestion = null;
            speakerlist.RemainingQuestionTime = speakerlist.Speakertime;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Returns a list of all delegations that are inside the conference
        /// that this speakerlist is linked to. This will serve as a pre filter
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="id"></param>
        /// <param name="authService"></param>
        /// <param name="speakerlistService"></param>
        /// <param name="conferenceService"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<DelegationModel>> GetPossibleDelegations([FromHeader]string auth,
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