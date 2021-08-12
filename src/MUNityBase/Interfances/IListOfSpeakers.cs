using System;
using System.Collections.Generic;
using System.Text;

namespace MUNityBase.Interfances
{
    public interface IListOfSpeakers : IListOfSpeakersModel
    {
        
        ISpeaker AddSpeaker(string name, string iso = "");

        void RemoveSpeaker(string id);

        ISpeaker AddQuestion(string name, string iso = "");

        void RemoveQuestion(string id);

        void AddSpeakerSeconds(double seconds);

        void AddQuestionSeconds(double seconds);

        ISpeaker NextSpeaker();

        ISpeaker NextQuestion();

        void Pause();

        void ResumeQuestion();

        void StartAnswer();

        ISpeaker CurrentSpeaker { get; }

        ISpeaker CurrentQuestion { get; }

        TimeSpan RemainingSpeakerTime { get; }

        TimeSpan RemainingQuestionTime { get; }
    }
}
