using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Models.ListOfSpeakers;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using MUNity.Hubs;
using MUNity.Extensions.LoSExtensions;
using MUNity.Schema.ListOfSpeakers;

namespace MUNityCore.Services
{
    public class SpeakerlistHubService
    {
        public HubConnection _hubConnection;

        private readonly IHubContext<Hubs.SpeakerListHub, MUNity.Hubs.ITypedListOfSpeakerHub> _hubContext;

        private readonly SpeakerlistService _speakerListService;

        private readonly NavigationManager _navigationManager;

        private MUNity.Models.ListOfSpeakers.ListOfSpeakers _listOfSpeakers;

        public event EventHandler SpeakerlistChanged;

        public SpeakerlistHubService(IHubContext<Hubs.SpeakerListHub, MUNity.Hubs.ITypedListOfSpeakerHub> hubContext, SpeakerlistService speakerListService, NavigationManager navManager)
        {
            this._hubContext = hubContext;
            this._speakerListService = speakerListService;
            this._navigationManager = navManager;
        }

        public async Task InitHub(MUNity.Models.ListOfSpeakers.ListOfSpeakers listOfSpeakers)
        {
            this._listOfSpeakers = listOfSpeakers;
            this._hubConnection = new HubConnectionBuilder().WithUrl($"{_navigationManager.BaseUri}slsocket").Build();

            this._hubConnection.On<DateTime>(nameof(ITypedListOfSpeakerHub.SpeakerTimerStarted), HandleSpeakerTimerStarted);
            this._hubConnection.On<DateTime>(nameof(ITypedListOfSpeakerHub.QuestionTimerStarted), HandleQuestionTimerStarted);
            this._hubConnection.On<Speaker>(nameof(ITypedListOfSpeakerHub.SpeakerAdded), HandleSpeakerAdded);
            this._hubConnection.On<string>(nameof(ITypedListOfSpeakerHub.SpeakerRemoved), HandleSpeakerRemoved);
            this._hubConnection.On(nameof(ITypedListOfSpeakerHub.NextSpeaker), HandleNextSpeaker);
            this._hubConnection.On(nameof(ITypedListOfSpeakerHub.NextQuestion), HandleNextQuestion);
            this._hubConnection.On<IListTimeSettings>(nameof(ITypedListOfSpeakerHub.SettingsChanged), HandleSettingsChanged);
            this._hubConnection.On<int>(nameof(ITypedListOfSpeakerHub.QuestionSecondsAdded), HandleQuestionSecondsAdded);
            this._hubConnection.On<int>(nameof(ITypedListOfSpeakerHub.SpeakerSecondsAdded), HandleSpeakerSecondsAdded);
            this._hubConnection.On<DateTime>(nameof(ITypedListOfSpeakerHub.AnswerTimerStarted), HandleAnswerTimerStarted);
            this._hubConnection.On(nameof(ITypedListOfSpeakerHub.ClearSpeaker), HandleClearSpeaker);
            this._hubConnection.On(nameof(ITypedListOfSpeakerHub.ClearQuestion), HandleClearQuestion);
            this._hubConnection.On(nameof(ITypedListOfSpeakerHub.Pause), HandlePauseBy);
            this._hubConnection.On<bool>(nameof(ITypedListOfSpeakerHub.SpeakersStateChanged), HandleSpeakerStateChanged);
            this._hubConnection.On<bool>(nameof(ITypedListOfSpeakerHub.QuestionsStateChanged), HandleQuestionStateChanged);

            await _hubConnection.StartAsync();
        }

        private void HandleQuestionStateChanged(bool state)
        {
            this._listOfSpeakers.QuestionsClosed = state;
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleSpeakerStateChanged(bool state)
        {
            this._listOfSpeakers.ListClosed = state;
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandlePauseBy()
        {
            this._listOfSpeakers.Pause();
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleClearQuestion()
        {
            this._listOfSpeakers.ClearCurrentQuestion();
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleClearSpeaker()
        {
            this._listOfSpeakers.ClearCurrentSpeaker();
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleSpeakerSecondsAdded(int seconds)
        {
            this._listOfSpeakers.AddSpeakerSeconds(seconds);
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleQuestionSecondsAdded(int seconds)
        {
            this._listOfSpeakers.AddQuestionSeconds(seconds);
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleSettingsChanged(IListTimeSettings settings)
        {
            this._listOfSpeakers.SpeakerTime = TimeSpan.ParseExact(settings.SpeakerTime, @"mm\:ss", null);
            this._listOfSpeakers.QuestionTime = TimeSpan.ParseExact(settings.QuestionTime, @"mm\:ss", null);
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleNextQuestion()
        {
            this._listOfSpeakers.NextQuestion();
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleNextSpeaker()
        {
            this._listOfSpeakers.NextSpeaker();
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleSpeakerRemoved(string speakerId)
        {
            var target = this._listOfSpeakers.AllSpeakers.FirstOrDefault(n => n.Id == speakerId);
            if (target != null)
            {
                this._listOfSpeakers.AllSpeakers.Remove(target);
                this.SpeakerlistChanged?.Invoke(this, new EventArgs());
            }
                
        }

        private void HandleSpeakerAdded(Speaker speaker)
        {
            this._listOfSpeakers.AllSpeakers.Add(speaker);
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleQuestionTimerStarted(DateTime dateTime)
        {
            this._listOfSpeakers.ResumeQuestion(dateTime);
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleSpeakerTimerStarted(DateTime dateTime)
        {
            this._listOfSpeakers.ResumeSpeaker(dateTime);
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        private void HandleAnswerTimerStarted(DateTime dateTime)
        {
            this._listOfSpeakers.StartAnswer(dateTime);
            this.SpeakerlistChanged?.Invoke(this, new EventArgs());
        }

        public Task Subscribe(string listId)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.Subscribe), listId);
        }

        public Task AddSpeaker(ListOfSpeakers list, string iso, string name)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.AddSpeaker), list.ListOfSpeakersId, iso, name);
        }

        public Task AddQuestion(ListOfSpeakers list, string iso, string name)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.AddQuestion), list.ListOfSpeakersId, iso, name);
        }

        public Task ResumeSpeaker(ListOfSpeakers list)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.ResumeSpeaker), list.ListOfSpeakersId);
        }

        public Task Pause(ListOfSpeakers list)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.Pause), list.ListOfSpeakersId);
        }

        public Task NextSpeaker(ListOfSpeakers list)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.NextSpeaker), list.ListOfSpeakersId);
        }

        public Task ClearSpeaker(ListOfSpeakers list)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.ClearSpeaker), list.ListOfSpeakersId);
        }

        public Task RemoveSpeaker(ListOfSpeakers list, string speakerId)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.RemoveSpeaker), list.ListOfSpeakersId, speakerId);
        }

    }
}
