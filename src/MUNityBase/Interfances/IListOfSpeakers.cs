using System;
using System.Collections.Generic;
using System.Text;

namespace MUNityBase.Interfances
{
    /// <summary>
    /// This interface describes all basic methods that should be used in any form of List of speaker implementaiton.
    /// With ViewModels or when working with a model for the Database.
    /// </summary>
    public interface IListOfSpeakers : IListOfSpeakersModel
    {
        /// <summary>
        /// Adds a speaker to the list of speakers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iso"></param>
        /// <returns></returns>
        ISpeaker AddSpeaker(string name, string iso = "");

        /// <summary>
        /// Removes a speaker with the given Id from the list of speakers. Note that this should
        /// only effect people on the List that have the type "Speaker"/"WaitToSpeak" and not question
        /// </summary>
        /// <param name="id"></param>
        void RemoveSpeaker(string id);

        /// <summary>
        /// Should add someone to the list of questions/comments.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iso"></param>
        /// <returns></returns>
        ISpeaker AddQuestion(string name, string iso = "");

        /// <summary>
        /// Should remove someone from the list of questions/comments
        /// </summary>
        /// <param name="id"></param>
        void RemoveQuestion(string id);

        /// <summary>
        /// Should give the current speaker some extra seconds. Use a negativ value to remove some seconds from the speaker.
        /// </summary>
        /// <param name="seconds"></param>
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
