using MUNity.Models.ListOfSpeakers;
using MUNity.Schema.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Hubs
{

    /// <summary>
    /// The ITypedListOfSpeakerHub is used for the SignalR communication between the Server and the Clients.
    /// The Functions of this hub will be used by the Server and sent to the clients.
    /// </summary>
    public interface ITypedListOfSpeakerHub
    {

        /// <summary>
        /// The SpeakerTimer has started with the given seconds as remaining time. The clients should Sync their 
        /// SpeakerStartTime according.
        /// </summary>
        /// <param name="speakerStartTime">The timestamp when the speaker started talking in universal-time.</param>
        /// <returns></returns>
        Task SpeakerTimerStarted(DateTime speakerStartTime);

        /// <summary>
        /// The QuestionTimer was started the clients should set the QuestionStartTime accordingly.
        /// </summary>
        /// <param name="questionStartTime">The timestamp when the questionair started talking in universal-time</param>
        /// <returns></returns>
        Task QuestionTimerStarted(DateTime questionStartTime);

        Task SpeakerAdded(Speaker speaker);

        Task QuestionAdded(Speaker question);

        /// <summary>
        /// Gets called when a speaker or question gets removed
        /// </summary>
        /// <param name="speakerId"></param>
        /// <returns></returns>
        Task SpeakerRemoved(string speakerId);

        Task NextSpeaker();

        Task NextQuestion();

        Task SettingsChanged(IListTimeSettings settings);
        Task QuestionSecondsAdded(int seconds);

        Task SpeakerSecondsAdded(int seconds);
        Task AnswerTimerStarted(DateTime value);
        Task ClearSpeaker();
        Task ClearQuestion();

        Task Pause();
    }
}
