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
using MUNity.Models.ListOfSpeakers;
using MUNityCore.Services;
using System.Collections.ObjectModel;
using MUNity.Extensions.LoSExtensions;
using MUNity.Schema.ListOfSpeakers;

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
        private readonly IHubContext<Hubs.SpeakerListHub, MUNity.Hubs.ITypedListOfSpeakerHub> _hubContext;

        private readonly SpeakerlistService _speakerlistService;

        public SpeakerlistController(IHubContext<Hubs.SpeakerListHub, MUNity.Hubs.ITypedListOfSpeakerHub> hubContext,
            SpeakerlistService speakerlistService)
        {
            this._hubContext = hubContext;
            this._speakerlistService = speakerlistService;
        }

        /// <summary>
        /// Checks if a list of speaker is registered inside on the server and is available.
        /// Will return true if it extists and false if not. Both with an Ok Status Code!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<bool>> IsSpeakerlistOnline(string id)
        {
            var result = await _speakerlistService.IsOnline(id);
            return Ok(result);
        }

        /// <summary>
        /// Gets a List of speakers model by the given id.
        /// <see cref="ListOfSpeakers"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public ActionResult<ListOfSpeakers> GetSpeakerlist(string id)
        {
            //var authstate = authService.ValidateAuthKey(auth);

            //Is a speakerlist public or not needs to be checked
            var speakerlist = _speakerlistService.GetSpeakerlist(id);
            if (speakerlist == null)
                return NotFound("Speakerlist cannot be found!");

            return Ok(speakerlist);
        }

        /// <summary>
        /// Request to sync a list of speakers with the given model.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public ActionResult SyncSpeakerlist([FromBody]ListOfSpeakers list)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(list.ListOfSpeakersId);
            if (speakerlist == null) return NotFound();
            this._speakerlistService.OverwriteList(speakerlist, list);
            this._hubContext.Clients.Group($"los_{list.ListOfSpeakersId}").SpeakerListChanged(list);
            return Ok();
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
        /// <param name="listId"></param>
        /// <param name="connectionid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public IActionResult SubscribeToList(string listId, string connectionid)
        {
            _hubContext.Groups.AddToGroupAsync(connectionid, "los_" + listId);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Will add the given speaker to the list of speakers and return the
        /// new updated list. If the speaker was already in the list of speakers
        /// it will not add the speaker a second time and return the list as it is.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<ListOfSpeakers>> AddSpeakerModelToListAsync([FromBody]AddSpeakerBody body)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(body.ListOfSpeakersId);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            if (speakerlist.ListClosed)
                return Forbid("The list of speakers is currently closed!");

            if (speakerlist.Speakers.Any(n => n.Name == body.Speaker.Name && n.Iso == body.Speaker.Iso))
                return Ok(speakerlist);

            var result = await _speakerlistService.AddSpeaker(speakerlist, body.Speaker);
            _ = this._hubContext.Clients.Group("los_" + speakerlist.ListOfSpeakersId).SpeakerAdded(result);
            return StatusCode(StatusCodes.Status200OK, speakerlist);
        }


        /// <summary>
        /// Will add the given Question to the list of speakers but only if it isnt already inside the list.
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> AddQuestionModelToList([FromBody] AddSpeakerBody body)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(body.ListOfSpeakersId);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            if (speakerlist.QuestionsClosed)
                return Forbid("The List of questions is currently closed!");

            if (speakerlist.Questions.Any(n => n.Name == body.Speaker.Name && n.Iso == body.Speaker.Iso))
                return Ok(speakerlist);

            var result = await _speakerlistService.AddQuestion(speakerlist, body.Speaker);
            _ = this._hubContext.Clients.Group("los_" + speakerlist.ListOfSpeakersId).QuestionAdded(result);
            return Ok();
        }


        /// <summary>
        /// Removes a speaker from the speakerlist.
        /// This function will remove the first entry of the delegationid it will
        /// find.
        /// </summary>
        /// <param name="listid"></param>
        /// <param name="speakerId"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpDelete]
        public IActionResult RemoveSpeakerFromList(string listid, string speakerId)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            var item = speakerlist.Speakers.FirstOrDefault(n => n.Id == speakerId);
            speakerlist.AllSpeakers.Remove(item);
            this._hubContext.Clients.Group("los_" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            _speakerlistService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Will set the next speaker to the list.
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult NextSpeaker(string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.NextSpeaker();
            this._hubContext.Clients.Group("los_" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            _speakerlistService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Starts the Speaking Timer
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult StartSpeaker(string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.ResumeSpeaker();
            this._hubContext.Clients.Groups("los_" + speakerlist.PublicId).SpeakerTimerStarted((int)speakerlist.RemainingSpeakerTime.TotalSeconds);
            _speakerlistService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Sets the next question on the list
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult NextQuestion(string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.NextQuestion();
            this._hubContext.Clients.Group("los_" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            _speakerlistService.SaveChanges();
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

            speakerlist.ResumeQuestion();
            this._hubContext.Clients.Groups("los_" + speakerlist.PublicId).QuestionTimerStarted((int)speakerlist.RemainingQuestionTime.TotalSeconds);
            _speakerlistService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Pauses the speaker timer.
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public IActionResult PauseTimer(string listid)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            speakerlist.Pause();
            this._hubContext.Clients.Groups("los_" + speakerlist.PublicId).TimerStopped();
            _speakerlistService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Changes the maximum timefor each speaker. The time format is HH:MM:SS
        /// </summary>
        /// <param name="listid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        public IActionResult SetSpeakertime(string listid, string time)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            var newTime = time.ToTimeSpan();
            if (newTime.HasValue == false)
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Time Format it should be hh:mm:ss");

            speakerlist.SpeakerTime = newTime.Value;

            this._hubContext.Clients.Groups("los_" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            _speakerlistService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// changes the maximum time for each question. The time Format is HH:MM:SS
        /// </summary>
        /// <param name="listid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        public IActionResult SetQuestiontime(string listid, string time)
        {
            var speakerlist = _speakerlistService.GetSpeakerlist(listid);
            if (speakerlist == null)
                return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

            var newTime = time.ToTimeSpan();
            if (newTime.HasValue == false)
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Time Format it should be hh:mm:ss");

            speakerlist.QuestionTime = newTime.Value;

            this._hubContext.Clients.Groups("los_" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            _speakerlistService.SaveChanges();
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

            speakerlist.ClearCurrentSpeaker();

            this._hubContext.Clients.Groups("los_" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            _speakerlistService.SaveChanges();
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


            speakerlist.ClearCurrentQuestion();

            this._hubContext.Clients.Groups("los_" + speakerlist.PublicId).SpeakerListChanged(speakerlist);
            _speakerlistService.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}