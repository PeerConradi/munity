using MUNity.Models.ListOfSpeakers;
using MUNityClient.Models.ListOfSpeaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public class ListOfSpeakerOfflineHandler : IListOfSpeakerHandler
    {
        public event EventHandler<int> QuestionTimerStarted;
        public event EventHandler<ListOfSpeakers> SpeakerListChanged;
        public event EventHandler<int> SpeakerTimerStarted;
        public event EventHandler TimerStopped;

        public Task AddQuestion(SpeakerToAdd question)
        {
            throw new NotImplementedException();
        }

        public Task AddQuestionSeconds(int seconds)
        {
            throw new NotImplementedException();
        }

        public Task AddSpeaker(SpeakerToAdd speaker)
        {
            throw new NotImplementedException();
        }

        public Task AddSpeakerSeconds(int seconds)
        {
            throw new NotImplementedException();
        }

        public Task ClearCurrentQuestion()
        {
            throw new NotImplementedException();
        }

        public Task ClearCurrentSpeaker()
        {
            throw new NotImplementedException();
        }

        public Task CloseQuestions()
        {
            throw new NotImplementedException();
        }

        public Task CloseSpeakers()
        {
            throw new NotImplementedException();
        }

        public Task Init(ListOfSpeakerViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task NextQuestion()
        {
            throw new NotImplementedException();
        }

        public Task NextSpeaker()
        {
            throw new NotImplementedException();
        }

        public Task OpenQuestions()
        {
            throw new NotImplementedException();
        }

        public Task OpenSpeakers()
        {
            throw new NotImplementedException();
        }

        public Task Pause()
        {
            throw new NotImplementedException();
        }

        public Task Remove(Speaker speaker)
        {
            throw new NotImplementedException();
        }

        public Task ResetSpeakerTime()
        {
            throw new NotImplementedException();
        }

        public Task ResumeQuestion()
        {
            throw new NotImplementedException();
        }

        public Task ResumeSpeaker()
        {
            throw new NotImplementedException();
        }

        public Task StartAnswer()
        {
            throw new NotImplementedException();
        }
    }
}
