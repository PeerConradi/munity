using MUNity.Models.ListOfSpeakers;
using MUNity.Schema.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public interface IListOfSpeakerHandler
    {
        event EventHandler<DateTime> QuestionTimerStarted;

        event EventHandler<DateTime> SpeakerTimerStarted;

        event EventHandler<DateTime> AnswerTimerStarted;

        event EventHandler TimerStopped;

        event EventHandler<Speaker> SpeakerAdded;

        event EventHandler<string> SpeakerRemoved;

        event EventHandler NextSpeakerPushed;

        event EventHandler NextQuestionPushed;

        event EventHandler<IListTimeSettings> SettingsChanged;

        event EventHandler<int> QuestionSecondsAdded;

        event EventHandler<int> SpeakerSecondsAdded;

        event EventHandler ClearSpeaker;

        event EventHandler ClearQuestion;

        event EventHandler Paused;

        event EventHandler<bool> SpeakerStateChanged;
        event EventHandler<bool> QuestionsStateChanged;

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

        Task AddSpeaker(Models.ListOfSpeaker.SpeakerToAdd speaker);

        Task AddQuestion(Models.ListOfSpeaker.SpeakerToAdd question);

        Task Remove(MUNity.Models.ListOfSpeakers.Speaker speaker);

        Task SetTimes(string speakerTime, string questionTime);
    }
}
