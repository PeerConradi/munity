using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.ListOfSpeakers;
using MUNityClient.Models.ListOfSpeaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public class ListOfSpeakerOnlineHandler : IListOfSpeakerHandler
    {
        public HubConnection HubConnection { get; set; }

        private ListOfSpeakerViewModel _viewModel;

        public event EventHandler<int> QuestionTimerStarted;

        public event EventHandler<ListOfSpeakers> SpeakerListChanged;

        public event EventHandler<int> SpeakerTimerStarted;

        public event EventHandler TimerStopped;

        public event EventHandler<Speaker> SpeakerAdded;

        private HttpClient httpClient;

        public async Task Init(ListOfSpeakerViewModel viewModel)
        {
            this._viewModel = viewModel;
            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/slsocket").Build();
            await HubConnection.StartAsync();
            this.SpeakerAdded += ListOfSpeakerOnlineHandler_SpeakerAdded;

            HubConnection.On<Speaker>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.SpeakerAdded), (speaker) => this.SpeakerAdded?.Invoke(this, speaker));
            
            httpClient = new HttpClient();
        }

        private void ListOfSpeakerOnlineHandler_SpeakerAdded(object sender, Speaker e)
        {
            this._viewModel.SourceList.AllSpeakers.Add(e);
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

        public Task CloseSpeakers()
        {
            throw new NotImplementedException();
        }

        public Task OpenSpeakers()
        {
            throw new NotImplementedException();
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

        public Task OpenQuestions()
        {
            throw new NotImplementedException();
        }

        public Task CloseQuestions()
        {
            throw new NotImplementedException();
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
