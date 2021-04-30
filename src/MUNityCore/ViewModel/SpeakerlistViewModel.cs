using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Extensions.LoSExtensions;
using MUNity.Hubs;
using MUNity.Models.ListOfSpeakers;
using MUNity.Schema.ListOfSpeakers;

namespace MUNityCore.ViewModel
{
    public class SpeakerlistViewModel : IDisposable
    {
        private HubConnection _hubConnection;

        private MUNity.Models.ListOfSpeakers.ListOfSpeakers _listOfSpeakers;

        public MUNity.Models.ListOfSpeakers.ListOfSpeakers Speakerlist => _listOfSpeakers;

        public event EventHandler SpeakerlistChanged;

        private string _socketUrl;

        public string SpeakerlistId => _listOfSpeakers?.ListOfSpeakersId;

        public SpeakerlistViewModel(MUNity.Models.ListOfSpeakers.ListOfSpeakers listOfSpeakers, string socketUrl)
        {
            this._listOfSpeakers = listOfSpeakers;
            this._socketUrl = socketUrl;
        }

        public async Task Start()
        {
            this._hubConnection = new HubConnectionBuilder().WithUrl(_socketUrl).Build();

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
            await Subscribe();
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
            Console.WriteLine("NextSpeaker recieved from HUB!");
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
            if (this._listOfSpeakers.AllSpeakers.All(n => n.Id != speaker.Id))
            {
                this._listOfSpeakers.AllSpeakers.Add(speaker);
                this.SpeakerlistChanged?.Invoke(this, new EventArgs());
            }
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

        private Task Subscribe()
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.Subscribe), Speakerlist.ListOfSpeakersId);
        }

        public Task AddSpeaker(string iso, string name)
        {
            
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.AddSpeaker), _listOfSpeakers.ListOfSpeakersId, iso, name);
        }

        public Task AddQuestion(string iso, string name)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.AddQuestion), _listOfSpeakers.ListOfSpeakersId, iso, name);
        }

        public Task ResumeSpeaker()
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.ResumeSpeaker), _listOfSpeakers.ListOfSpeakersId);
        }

        public Task ResumeQuestion()
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.ResumeQuestion), _listOfSpeakers.ListOfSpeakersId);
        }

        public Task Pause()
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.Pause), _listOfSpeakers.ListOfSpeakersId);
        }

        public Task NextSpeaker()
        {
            Console.WriteLine("NextSpeaker Client called!");
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.NextSpeaker), _listOfSpeakers.ListOfSpeakersId);
        }

        public Task NextQuestion()
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.NextQuestion), _listOfSpeakers.ListOfSpeakersId);
        }

        public Task ClearSpeaker()
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.ClearSpeaker), _listOfSpeakers.ListOfSpeakersId);
        }

        public Task ClearQuestion()
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.ClearQuestion), _listOfSpeakers.ListOfSpeakersId);
        }

        public Task RemoveSpeaker(string speakerId)
        {
            return this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SpeakerListHub.RemoveSpeaker), _listOfSpeakers.ListOfSpeakersId, speakerId);
        }

        public void Dispose()
        {
            if (_hubConnection != null)
                _hubConnection.DisposeAsync();
        }
    }
}
