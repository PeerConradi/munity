using MUNity.Models.ListOfSpeakers;
using MUNity.Schema.ListOfSpeakers;
using MUNityClient.Models.ListOfSpeaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public class ListOfSpeakerOfflineHandler : IListOfSpeakerHandler
    {
        public event EventHandler<DateTime> QuestionTimerStarted;
        public event EventHandler<DateTime> SpeakerTimerStarted;
        public event EventHandler TimerStopped;
        public event EventHandler<Speaker> SpeakerAdded;
        public event EventHandler<string> SpeakerRemoved;
        public event EventHandler NextSpeakerPushed;
        public event EventHandler<DateTime> AnswerTimerStarted;
        public event EventHandler NextQuestionPushed;
        public event EventHandler<IListTimeSettings> SettingsChanged;
        public event EventHandler<int> QuestionSecondsAdded;
        public event EventHandler<int> SpeakerSecondsAdded;
        public event EventHandler ClearSpeaker;
        public event EventHandler ClearQuestion;
        public event EventHandler Paused;

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
