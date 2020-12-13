using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MUNityCore.Util.Extensions;
using MUNityCore.Models.Conference;
using MUNityCore.Models;
using MUNityCore.Models.ListOfSpeakers;
using MUNityCore.Services;

namespace MUNityCore.Controllers
{

    /// <summary>
    /// The controller to be able to edit and Change a List of Speakers.
    /// This Route will may be changed to "api/los" in the future.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerlistController : ControllerBase
    {
        private readonly IHubContext<Hubs.SpeakerListHub, Hubs.ITypedListOfSpeakerHub> _hubContext;

        private readonly SpeakerlistService _speakerlistService;

        public SpeakerlistController(IHubContext<Hubs.SpeakerListHub, Hubs.ITypedListOfSpeakerHub> hubContext,
            SpeakerlistService speakerlistService)
        {
            this._hubContext = hubContext;
            this._speakerlistService = speakerlistService;
        }

        /// <summary>
        /// Creates a new Speakerlist and returns it.
        /// </summary>
        /// <param name="conferenceid"></param>
        /// <param name="committeeid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<ListOfSpeakers> CreateSpeakerlist(
            [FromHeader]string conferenceid,
            [FromHeader]string committeeid)
        {
            var speakerlist = _speakerlistService.CreateSpeakerlist();
            speakerlist.ConferenceId = conferenceid;
            speakerlist.CommitteeId = committeeid;

            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        /// <summary>
        /// Gets a speakerlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<ListOfSpeakers> GetSpeakerlist([FromHeader]string id)
        {
            //var authstate = authService.ValidateAuthKey(auth);

            //Is a speakerlist public or not needs to be checked
            var speakerlist = _speakerlistService.GetSpeakerlist(id);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist cannot be found!");

            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        /// <summary>
        /// Gets a speakerlist with the public ResolutionId
        /// </summary>
        /// <param name="publicid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<ListOfSpeakers> ReadSpeakerlist([FromHeader]string publicid)
        {

            var speakerlist = _speakerlistService.GetSpeakerlistByPublicId(publicid);
            if (speakerlist != null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");
            speakerlist.ListOfSpeakersId = "";
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        /// <summary>
        /// Subscribes to a List of speakers
        /// </summary>
        /// <param name="publicid"></param>
        /// <param name="connectionid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult SubscribeToList([FromHeader]string publicid, [FromHeader]string connectionid)
        {
            _hubContext.Groups.AddToGroupAsync(connectionid, "s-list-" + publicid);
            return StatusCode(StatusCodes.Status200OK);
        }


        [Route("[action]")]
        [HttpPost]
        public ActionResult<ListOfSpeakers> AddSpeakerModelToList(
            [FromHeader] string listid, [FromBody] Speaker model,
            [FromServices] SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");
            model.Id = Guid.NewGuid().ToString();
            speakerlist.AddSpeaker(model);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }

        [Route("[action]")]
        [HttpPatch]
        public ActionResult<ListOfSpeakers> SpeakersOrderChanged(
            [FromHeader] string listid, [FromBody] List<Speaker> model,
            [FromServices] SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            model.ForEach(n =>
            {
                if (string.IsNullOrWhiteSpace(n.Id))
                {
                    n.Id = Guid.NewGuid().ToString();
                }
            });
            speakerlist.Speakers = model;
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return speakerlist;
        }

        [Route("[action]")]
        [HttpPatch]
        public ActionResult<ListOfSpeakers> QuestionsOrderChanged(
            [FromHeader] string listid, [FromBody] List<Speaker> model,
            [FromServices] SpeakerlistService speakerlistService)
        {
            var speakerlist = speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            model.ForEach(n =>
            {
                if (string.IsNullOrWhiteSpace(n.Id))
                {
                    n.Id = Guid.NewGuid().ToString();
                }
            });
            speakerlist.Questions = model;
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return speakerlist;
        }



        [Route("[action]")]
        [HttpPost]
        public ActionResult<ListOfSpeakers> AddQuestionModelToList(
            [FromHeader] string listid, [FromBody] Speaker model)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");
            model.Id = Guid.NewGuid().ToString();
            speakerlist.AddQuestion(model);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }


        /// <summary>
        /// Removes a speaker from the speakerlist.
        /// This function will remove the first entry of the delegationid it will
        /// find.
        /// </summary>
        /// <param name="listid"></param>
        /// <param name="delegationid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpDelete]
        public IActionResult RemoveSpeakerFromList([FromHeader]string listid, [FromHeader]string delegationid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.RemoveSpeaker(delegationid);
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Will set the next speaker to the list.
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult NextSpeaker([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.NextSpeaker();
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Starts the Speaking Timer
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult StartSpeaker([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.StartSpeaker();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerTimerStarted((int)speakerlist.RemainingSpeakerTime.TotalSeconds);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Sets the next question on the list
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult NextQuestion([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.NextQuestion();
            this._hubContext.Clients.Group("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Starts the Question Timer
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult StartQuestion([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.StartQuestion();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).QuestionTimerStarted((int)speakerlist.RemainingQuestionTime.TotalSeconds);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Starts the Answer Timer for the speaker.
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult StartAnswer([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.StartAnswer();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerTimerStarted((int)speakerlist.RemainingSpeakerTime.TotalSeconds);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Pauses the speaker timer.
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult PauseTimer([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.PauseSpeaker();
            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).TimerStopped();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Changes the maximum timefor each speaker.
        /// </summary>
        /// <param name="listid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        public IActionResult SetSpeakertime([FromHeader]string listid, [FromHeader]string time)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            var newTime = time.ToTimeSpan();
            if (newTime.HasValue == false)
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Time Format it should be hh:mm:ss");

            speakerlist.SpeakerTime = newTime.Value;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// changes the maximum time for each question.
        /// </summary>
        /// <param name="listid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        public IActionResult SetQuestiontime([FromHeader]string listid, [FromHeader]string time)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            var newTime = time.ToTimeSpan();
            if (newTime.HasValue == false)
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Time Format it should be hh:mm:ss");

            speakerlist.QuestionTime = newTime.Value;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Clears the current Speaker from the list.
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult ClearSpeaker([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.CurrentSpeaker = null;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Clears the current Question from the list
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult ClearQuestion([FromHeader]string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");


            speakerlist.CurrentQuestion = null;

            this._hubContext.Clients.Groups("s-list-" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}