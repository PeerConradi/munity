using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Hubs
{
    public class SpeakerListHub : Hub<MUNity.Hubs.ITypedListOfSpeakerHub>
    {

        private readonly Services.SpeakerlistService _speakerlistService;

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Speakerlist-Client disconnected: {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Speakerlist-Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public SpeakerListHub(Services.SpeakerlistService speakerlistService)
        {
            this._speakerlistService = speakerlistService;
        }

        public async Task Subscribe(string listId)
        {
            await this.Groups.AddToGroupAsync(Context.ConnectionId, "los_" + listId);
        }

        public async Task AddSpeaker(string listId, string iso, string name)
        {
            if (this._speakerlistService.IsListClosed(listId))
                return;

            var newSpeaker = await this._speakerlistService.AddSpeaker(listId, iso, name);
            await Clients.Group("los_" + listId).SpeakerAdded(newSpeaker);
        }

        public async Task AddQuestion(string listId, string iso, string name)
        {
            if (this._speakerlistService.IsQuestionsClosed(listId))
                return;

            var result = await this._speakerlistService.AddQuestion(listId, iso, name);
            if (result != null)
                await Clients.Group("los_" + listId).SpeakerAdded(result);
        }

        public async Task RemoveSpeaker(string listId, string speakerId)
        {
            var result = this._speakerlistService.RemoveSpeaker(listId, speakerId);
            if (result)
                await Clients.Group("los_" + listId).SpeakerRemoved(speakerId);
        }

        public async Task NextSpeaker(string listId)
        {
            var result = this._speakerlistService.NextSpeaker(listId);
            if (result)
                await Clients.Group("los_" + listId).NextSpeaker();
        }

        public async Task NextQuestion(string listId)
        {
            var result = await this._speakerlistService.NextQuestion(listId);
            if (result)
                await Clients.Group("los_" + listId).NextQuestion();
        }

        public async Task AddSpeakerSeconds(string listId, int seconds)
        {
            var result = _speakerlistService.AddSpeakerSeconds(listId, seconds);
            if (result)
                await Clients.Group("los_" + listId).SpeakerSecondsAdded(seconds);
        }

        public async Task AddQuestionSeconds(string listId, int seconds)
        {
            var result = _speakerlistService.AddQuestionSeconds(listId, seconds);
            if (result)
                await Clients.Group("los_" + listId).QuestionSecondsAdded(seconds);
        }

        public async Task ResumeSpeaker(string listId)
        {
            var startTime = this._speakerlistService.ResumeSpeaker(listId);
            if (startTime.HasValue)
                await Clients.Group("los_" + listId).SpeakerTimerStarted(startTime.Value);
        }

        public async Task ResumeQuestion(string listId)
        {
            var startTime = this._speakerlistService.ResumeQuestion(listId);
            if (startTime.HasValue)
                await Clients.Group("los_" + listId).QuestionTimerStarted(startTime.Value);
        }

        public async Task StartAnswer(string listId)
        {
            var startTime = this._speakerlistService.ResumeAnswer(listId);
            if (startTime.HasValue)
                await Clients.Group("los_" + listId).AnswerTimerStarted(startTime.Value);
        }

        public async Task ClearSpeaker(string listId)
        {
            var result = this._speakerlistService.ClearSpeaker(listId);
            if (result)
                await Clients.Group("los_" + listId).ClearSpeaker();
        }

        public async Task ClearQuestion(string listId)
        {
            var result = this._speakerlistService.ClearQuestion(listId);
            if (result)
                await Clients.Group("los_" + listId).ClearQuestion();
        }

        public async Task Pause(string listId)
        {
            var result = this._speakerlistService.Pause(listId);
            if (result)
                await Clients.Group("los_" + listId).Pause();
        }
    }
}
