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
using MUNitySchema.Schema.ListOfSpeakers;

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

        [HttpGet]
        [Route("[action]")]
        public ActionResult<CreatedResponse> CreateListOfSpeaker()
        {
            var list = this._speakerlistService.CreateSpeakerlist();
            var response = new CreatedResponse()
            {
                ListOfSpeakersId = list.ListOfSpeakersId,
                AccessToken = Util.Tools.IdGenerator.RandomString(32)
            };
            return Ok(response);
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
            //Is a speakerlist public or not needs to be checked
            var speakerlist = _speakerlistService.GetSpeakerlist(id);
            if (speakerlist == null)
                return NotFound("Speakerlist cannot be found!");

            return Ok(speakerlist);
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
        public async Task<IActionResult> AddSpeaker([FromBody]AddSpeakerBody body)
        {
            if (this._speakerlistService.IsListClosed(body))
                return Forbid("The list of speakers is currently closed!");

            var result = await _speakerlistService.AddSpeaker(body);
            if (result == null)
                return NotFound();

            _ = GetHubGroup(body)?.SpeakerAdded(result);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody]AddSpeakerBody body)
        {
            if (this._speakerlistService.IsQuestionsClosed(body))
                return Forbid("The list of speakers is currently closed!");

            var result = await _speakerlistService.AddQuestion(body);
            if (result == null)
                return NotFound();

            _ = GetHubGroup(body)?.QuestionAdded(result);
            return Ok();
        }


        /// <summary>
        /// Will add the given Question to the list of speakers but only if it isnt already inside the list.
        /// </summary>
        /// <returns></returns>
        //[Route("[action]")]
        //[HttpPost]
        //public async Task<ActionResult> AddQuestionModelToList([FromBody] AddSpeakerBody body)
        //{
        //    var speakerlist = _speakerlistService.GetSpeakerlist(body.ListOfSpeakersId);
        //    if (speakerlist == null)
        //        return StatusCode(StatusCodes.Status404NotFound, "Speakerlist not found!");

        //    if (speakerlist.QuestionsClosed)
        //        return Forbid("The List of questions is currently closed!");

        //    if (speakerlist.Questions.Any(n => n.Name == body.Speaker.Name && n.Iso == body.Speaker.Iso))
        //        return Ok(speakerlist);

        //    var result = await _speakerlistService.AddQuestion(speakerlist, body.Speaker);
        //    _ = this._hubContext.Clients.Group("los_" + speakerlist.ListOfSpeakersId).QuestionAdded(result);
        //    return Ok();
        //}


        /// <summary>
        /// Removes a speaker from the speakerlist. It will remove the first entry with the given
        /// SpeakerId from the list from both of the possible lists (Speakers or questions).
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> RemoveSpeakerFromList([FromBody]RemoveSpeakerBody body)
        {

            var result = await _speakerlistService.RemoveSpeaker(body);
            if (result)
            {
                _ = GetHubGroup(body)?.SpeakerRemoved(body.SpeakerId);
                return Ok();
            }
            return NotFound("Speaker or question not found");
        }

        private MUNity.Hubs.ITypedListOfSpeakerHub GetHubGroup(ListOfSpeakersRequest request)
        {
            return this._hubContext?.Clients?.Group("los_" + request.ListOfSpeakersId);
        }

        /// <summary>
        /// Will set the next speaker to the list.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> NextSpeaker([FromBody]ListOfSpeakersRequest body)
        {
            bool result = await _speakerlistService.NextSpeaker(body.ListOfSpeakersId);
            if (result)
            {
                _ = GetHubGroup(body)?.NextSpeaker();
                return Ok();
            }
            return NotFound("List of speakers or speaker not found!");
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> NextQuestion([FromBody]ListOfSpeakersRequest body)
        {
            bool result = await _speakerlistService.NextQuestion(body.ListOfSpeakersId);
            if (result)
            {
                _ = GetHubGroup(body)?.NextQuestion();
                return Ok();
            }
            return NotFound("List of speakers or speaker not found!");
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult AddQuestionSeconds([FromBody]AddSpeakerSeconds body)
        {
            var result = _speakerlistService.AddQuestionSeconds(body);
            if (result)
            {
                _ = GetHubGroup(body)?.QuestionSecondsAdded(body.Seconds);
                return Ok();
            }
            return NotFound("List of speaker not found!");
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult AddSpeakerSeconds([FromBody] AddSpeakerSeconds body)
        {
            var result = _speakerlistService.AddSpeakerSeconds(body);
            if (result)
            {
                _ = GetHubGroup(body)?.SpeakerSecondsAdded(body.Seconds);
                return Ok();
            }
            return NotFound("List of speaker not found!");
        }

        /// <summary>
        /// Starts the Speaking Timer
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult StartSpeaker([FromBody]ListOfSpeakersRequest body)
        {
            var startTime = _speakerlistService.ResumeSpeaker(body);
            if (startTime == null) return NotFound("List of speakers not found!");

            GetHubGroup(body)?.SpeakerTimerStarted(startTime.Value);    
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult StartQuestion([FromBody]ListOfSpeakersRequest body)
        {
            var startTime = _speakerlistService.ResumeQuestion(body);
            if (startTime == null) return NotFound("List of speakers not found!");

            GetHubGroup(body)?.QuestionTimerStarted(startTime.Value);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult StartAnswer([FromBody]ListOfSpeakersRequest body)
        {
            var startTime = _speakerlistService.ResumeAnswer(body);
            if (startTime == null) return NotFound("List of speakers not found!");
            GetHubGroup(body)?.AnswerTimerStarted(startTime.Value);
            return Ok();
        }

        [Route("[action]")]
        [HttpPut]
        public IActionResult SetSettings([FromBody]SetListSettingsBody body)
        {
            // TODO: Auth

            var success = this._speakerlistService.SetSettings(body);
            if (!success) return NotFound();

            GetHubGroup(body)?.SettingsChanged(body);
            return Ok();
        }

        /// <summary>
        /// Clears the current Speaker from the list.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult ClearSpeaker([FromBody]ListOfSpeakersRequest body)
        {
            bool result = _speakerlistService.ClearSpeaker(body);

            if (!result) return NotFound();

            GetHubGroup(body)?.ClearSpeaker();
            return Ok();
        }

        /// <summary>
        /// Clears the current Question from the list
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPut]
        public IActionResult ClearQuestion([FromBody]ListOfSpeakersRequest body)
        {
            bool result = _speakerlistService.ClearQuestion(body);
            if (!result) return NotFound();
            GetHubGroup(body)?.ClearQuestion();
            return Ok();
        }

    }
}