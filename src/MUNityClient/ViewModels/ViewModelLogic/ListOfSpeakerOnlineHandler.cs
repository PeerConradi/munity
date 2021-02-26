using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.ListOfSpeakers;
using MUNityClient.Models.ListOfSpeaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using MUNity.Extensions.LoSExtensions;
using MUNity.Hubs;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public class ListOfSpeakerOnlineHandler : IListOfSpeakerHandler
    {
        public HubConnection HubConnection { get; set; }

        private ListOfSpeakerViewModel _viewModel;

        public event EventHandler<DateTime> SpeakerTimerStarted;
        public event EventHandler<DateTime> QuestionTimerStarted;
        public event EventHandler TimerStopped;
        public event EventHandler<Speaker> SpeakerAdded;
        public event EventHandler<string> SpeakerRemoved;
        public event EventHandler<Speaker> QuestionAdded;
        public event EventHandler NextSpeakerPushed;
        public event EventHandler NextQuestionPushed;
        public event EventHandler<DateTime> AnswerTimerStarted;
        public event EventHandler<MUNity.Schema.ListOfSpeakers.IListTimeSettings> SettingsChanged;
        public event EventHandler<int> QuestionSecondsAdded;
        public event EventHandler<int> SpeakerSecondsAdded;
        public event EventHandler ClearSpeaker;
        public event EventHandler ClearQuestion;
        public event EventHandler Paused;
        public event EventHandler<bool> SpeakerStateChanged;
        public event EventHandler<bool> QuestionsStateChanged;

        private HttpClient httpClient;

        public async Task Init(ListOfSpeakerViewModel viewModel)
        {
            this._viewModel = viewModel;
            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/slsocket").Build();
            await HubConnection.StartAsync();
            this.SpeakerAdded += ListOfSpeakerOnlineHandler_SpeakerAdded;
            this.SpeakerRemoved += ListOfSpeakerOnlineHandler_SpeakerRemoved;
            this.NextSpeakerPushed += ListOfSpeakerOnlineHandler_NextSpeaker;
            this.NextQuestionPushed += ListOfSpeakerOnlineHandler_NextQuestionPushed;
            this.SpeakerTimerStarted += ListOfSpeakerOnlineHandler_SpeakerTimerStarted;
            this.QuestionTimerStarted += ListOfSpeakerOnlineHandler_QuestionTimerStarted;
            this.AnswerTimerStarted += ListOfSpeakerOnlineHandler_AnswerTimerStarted;
            this.ClearSpeaker += ListOfSpeakerOnlineHandler_ClearSpeaker;
            this.ClearQuestion += ListOfSpeakerOnlineHandler_ClearQuestion;
            this.Paused += ListOfSpeakerOnlineHandler_Paused;
            this.SpeakerStateChanged += ListOfSpeakerOnlineHandler_SpeakerStateChanged;
            this.QuestionsStateChanged += ListOfSpeakerOnlineHandler_QuestionsStateChanged;

            HubConnection.On<Speaker>(nameof(ITypedListOfSpeakerHub.SpeakerAdded), (speaker) => this.SpeakerAdded?.Invoke(this, speaker));
            HubConnection.On<string>(nameof(ITypedListOfSpeakerHub.SpeakerRemoved), (id) => this.SpeakerRemoved?.Invoke(this, id));
            HubConnection.On(nameof(ITypedListOfSpeakerHub.NextSpeaker), () => this.NextSpeakerPushed?.Invoke(this, new EventArgs()));
            HubConnection.On(nameof(ITypedListOfSpeakerHub.NextQuestion), () => this.NextQuestionPushed?.Invoke(this, new EventArgs()));
            HubConnection.On<DateTime>(nameof(ITypedListOfSpeakerHub.SpeakerTimerStarted), (args) => this.SpeakerTimerStarted?.Invoke(this, args));
            HubConnection.On<DateTime>(nameof(ITypedListOfSpeakerHub.QuestionTimerStarted), (a) => QuestionTimerStarted?.Invoke(this, a));
            HubConnection.On<DateTime>(nameof(ITypedListOfSpeakerHub.AnswerTimerStarted), (a) => AnswerTimerStarted?.Invoke(this, a));
            HubConnection.On(nameof(ITypedListOfSpeakerHub.ClearSpeaker), () => ClearSpeaker?.Invoke(this, new EventArgs()));
            HubConnection.On(nameof(ITypedListOfSpeakerHub.ClearQuestion), () => ClearQuestion?.Invoke(this, new EventArgs()));
            HubConnection.On(nameof(ITypedListOfSpeakerHub.Pause), () => Paused?.Invoke(this, new EventArgs()));
            HubConnection.On<bool>(nameof(ITypedListOfSpeakerHub.SpeakersStateChanged), (args) => this.SpeakerStateChanged?.Invoke(this, args));
            HubConnection.On<bool>(nameof(ITypedListOfSpeakerHub.QuestionsStateChanged), (args) => this.QuestionsStateChanged?.Invoke(this, args));

            httpClient = new HttpClient();
        }

        private void ListOfSpeakerOnlineHandler_QuestionsStateChanged(object sender, bool e)
        {
            this._viewModel.SourceList.QuestionsClosed = e;
        }

        private void ListOfSpeakerOnlineHandler_SpeakerStateChanged(object sender, bool e)
        {
            this._viewModel.SourceList.ListClosed = e;
        }

        private void ListOfSpeakerOnlineHandler_Paused(object sender, EventArgs e)
        {
            this._viewModel.SourceList.Pause();
        }

        private void ListOfSpeakerOnlineHandler_ClearQuestion(object sender, EventArgs e)
        {
            this._viewModel.SourceList.ClearCurrentQuestion();
        }

        private void ListOfSpeakerOnlineHandler_ClearSpeaker(object sender, EventArgs e)
        {
            this._viewModel.SourceList.ClearCurrentSpeaker();
        }

        private void ListOfSpeakerOnlineHandler_AnswerTimerStarted(object sender, DateTime e)
        {
            this._viewModel.SourceList.StartAnswer();
        }

        private void ListOfSpeakerOnlineHandler_QuestionTimerStarted(object sender, DateTime e)
        {
            this._viewModel.SourceList.ResumeQuestion();
        }

        private void ListOfSpeakerOnlineHandler_NextQuestionPushed(object sender, EventArgs e)
        {
            this._viewModel.SourceList.NextQuestion();
        }

        private void ListOfSpeakerOnlineHandler_SpeakerTimerStarted(object sender, DateTime e)
        {
            this._viewModel.SourceList.ResumeSpeaker();
            //this._viewModel.SourceList.StartSpeakerTime = e.Value;
        }

        private void ListOfSpeakerOnlineHandler_NextSpeaker(object sender, EventArgs e)
        {
            this._viewModel.SourceList.NextSpeaker();
        }

        private void ListOfSpeakerOnlineHandler_SpeakerRemoved(object sender, string e)
        {
            var speakerToRemove = this._viewModel.SourceList.AllSpeakers.FirstOrDefault(n => n.Id == e);
            if (speakerToRemove != null)
                this._viewModel.SourceList.AllSpeakers.Remove(speakerToRemove);
        }

        private void ListOfSpeakerOnlineHandler_SpeakerAdded(object sender, Speaker e)
        {
            this._viewModel.SourceList.AllSpeakers.Add(e);
        }

        public async Task SetTimes(string speakerTime, string questionTime)
        {
            var body = new MUNity.Schema.ListOfSpeakers.SetListSettingsBody()
            {
                ListOfSpeakersId = this._viewModel.SourceList.ListOfSpeakersId,
                QuestionTime = questionTime,
                SpeakerTime = speakerTime,
                Token = "test"
            };
            string url = Program.API_URL + "/api/Speakerlist/SetSettings";
            var result = await httpClient.PutAsJsonAsync(url, body);
        }

        public async Task ClearCurrentSpeaker()
        {
            string url = Program.API_URL + "/api/Speakerlist/ClearSpeaker";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task AddSpeakerSeconds(int seconds)
        {
            string url = Program.API_URL + "/api/Speakerlist/AddSpeakerSeconds";
            var dto = new MUNity.Schema.ListOfSpeakers.AddSpeakerSeconds()
            {
                ListOfSpeakersId = this._viewModel.SourceList.ListOfSpeakersId,
                Token = "test",
                Seconds = seconds
            };
            var result = await httpClient.PutAsJsonAsync(url, dto);
        }

        public async Task Pause()
        {
            string url = Program.API_URL + "/api/Speakerlist/Pause";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task ResumeSpeaker()
        {
            string url = Program.API_URL + "/api/Speakerlist/StartSpeaker";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task NextSpeaker()
        {
            string url = Program.API_URL + "/api/Speakerlist/NextSpeaker";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public Task ResetSpeakerTime()
        {
            throw new NotImplementedException();
        }

        public async Task StartAnswer()
        {
            string url = Program.API_URL + "/api/Speakerlist/StartAnswer";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task CloseSpeakers()
        {
            string url = Program.API_URL + "/api/Speakerlist/CloseList";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task OpenSpeakers()
        {
            string url = Program.API_URL + "/api/Speakerlist/OpenList";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task ClearCurrentQuestion()
        {
            string url = Program.API_URL + "/api/Speakerlist/ClearQuestion";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task AddQuestionSeconds(int seconds)
        {
            string url = Program.API_URL + "/api/Speakerlist/AddQuestionSeconds";
            var dto = new MUNity.Schema.ListOfSpeakers.AddSpeakerSeconds()
            {
                ListOfSpeakersId = this._viewModel.SourceList.ListOfSpeakersId,
                Token = "test",
                Seconds = seconds
            };
            var result = await httpClient.PutAsJsonAsync(url, dto);
        }

        public async Task ResumeQuestion()
        {
            string url = Program.API_URL + "/api/Speakerlist/StartQuestion";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task NextQuestion()
        {
            string url = Program.API_URL + "/api/Speakerlist/NextQuestion";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task OpenQuestions()
        {
            string url = Program.API_URL + "/api/Speakerlist/OpenQuestions";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task CloseQuestions()
        {
            string url = Program.API_URL + "/api/Speakerlist/CloseQuestions";
            var result = await httpClient.PutAsJsonAsync(url, DefaultRequestBody);
        }

        public async Task AddSpeaker(SpeakerToAdd speaker)
        {
            string url = Program.API_URL + "/api/Speakerlist/AddSpeaker";
            var result = await httpClient.PostAsJsonAsync(url, AddSpeakerBody(speaker));
        }

        public async Task AddQuestion(SpeakerToAdd question)
        {
            string url = Program.API_URL + "/api/Speakerlist/AddQuestion";
            var result = await httpClient.PostAsJsonAsync(url, AddSpeakerBody(question));
        }

        public async Task Remove(Speaker speaker)
        {
            string url = Program.API_URL + "/api/Speakerlist/RemoveSpeakerFromList";
            var dto = new MUNity.Schema.ListOfSpeakers.RemoveSpeakerBody()
            {
                ListOfSpeakersId = _viewModel.SourceList.ListOfSpeakersId,
                SpeakerId = speaker.Id,
                Token = "test"
            };
            var result = await httpClient.PutAsJsonAsync(url, dto);
        }

        private MUNity.Schema.ListOfSpeakers.ListOfSpeakersRequest DefaultRequestBody
        {
            get
            {
                var dto = new MUNity.Schema.ListOfSpeakers.ListOfSpeakersRequest()
                {
                    ListOfSpeakersId = this._viewModel.SourceList.ListOfSpeakersId,
                    Token = "test"
                };
                return dto;
            }
        }

        private MUNity.Schema.ListOfSpeakers.AddSpeakerBody AddSpeakerBody(SpeakerToAdd speaker)
        {
            var body = new MUNity.Schema.ListOfSpeakers.AddSpeakerBody()
            {
                Iso = speaker.Iso,
                ListOfSpeakersId = _viewModel.SourceList.ListOfSpeakersId,
                Name = speaker.Name,
                Token = "test"
            };
            return body;
        }
    }
}
