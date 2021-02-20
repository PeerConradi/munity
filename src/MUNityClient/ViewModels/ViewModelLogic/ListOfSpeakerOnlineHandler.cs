using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.ListOfSpeakers;
using MUNityClient.Models.ListOfSpeaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task Init(ListOfSpeakerViewModel viewModel)
        {
            this._viewModel = viewModel;
            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/slsocket").Build();

            //SpeakerListChanged += ListOfSpeakerSocketHandler_SpeakerListChanged;

            HubConnection.On<int>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.QuestionTimerStarted), (seconds) => QuestionTimerStarted?.Invoke(this, seconds));
            HubConnection.On<ListOfSpeakers>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.SpeakerListChanged), (list) => SpeakerListChanged?.Invoke(this, list));
            HubConnection.On<int>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.SpeakerTimerStarted), (seconds) => SpeakerTimerStarted.Invoke(this, seconds));
            HubConnection.On<string>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.TimerStopped), (s) => TimerStopped?.Invoke(this, new EventArgs()));
        }

        public Task ClearCurrentSpeaker()
        {
            throw new NotImplementedException();
        }

        public Task AddSpeakerSeconds(int seconds)
        {
            throw new NotImplementedException();
        }

        public Task Pause()
        {
            throw new NotImplementedException();
        }

        public Task ResumeSpeaker()
        {
            throw new NotImplementedException();
        }

        public Task NextSpeaker()
        {
            throw new NotImplementedException();
        }

        public Task ResetSpeakerTime()
        {
            throw new NotImplementedException();
        }

        public Task StartAnswer()
        {
            throw new NotImplementedException();
        }

        public Task CloseSpeakers()
        {
            throw new NotImplementedException();
        }

        public Task OpenSpeakers()
        {
            throw new NotImplementedException();
        }

        public Task ClearCurrentQuestion()
        {
            throw new NotImplementedException();
        }

        public Task AddQuestionSeconds(int seconds)
        {
            throw new NotImplementedException();
        }

        public Task ResumeQuestion()
        {
            throw new NotImplementedException();
        }

        public Task NextQuestion()
        {
            throw new NotImplementedException();
        }

        public Task OpenQuestions()
        {
            throw new NotImplementedException();
        }

        public Task CloseQuestions()
        {
            throw new NotImplementedException();
        }

        public Task AddSpeaker(SpeakerToAdd speaker)
        {
            throw new NotImplementedException();
        }

        public Task AddQuestion(SpeakerToAdd question)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Speaker speaker)
        {
            throw new NotImplementedException();
        }
    }
}
