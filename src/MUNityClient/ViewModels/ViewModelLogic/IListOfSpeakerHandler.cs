using MUNity.Models.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public interface IListOfSpeakerHandler
    {
        event EventHandler<int> QuestionTimerStarted;

        event EventHandler<ListOfSpeakers> SpeakerListChanged;

        event EventHandler<int> SpeakerTimerStarted;

        event EventHandler TimerStopped;

        Task Init(ListOfSpeakerViewModel viewModel);

        Task ClearCurrentSpeaker();

        Task AddSpeakerSeconds(int seconds);

        Task Pause();

        Task ResumeSpeaker();

        Task NextSpeaker();

        Task ResetSpeakerTime();

        Task StartAnswer();

        Task CloseSpeakers();

        Task OpenSpeakers();

        Task ClearCurrentQuestion();

        Task AddQuestionSeconds(int seconds);

        Task ResumeQuestion();

        Task NextQuestion();

        Task OpenQuestions();

        Task CloseQuestions();
    }
}
