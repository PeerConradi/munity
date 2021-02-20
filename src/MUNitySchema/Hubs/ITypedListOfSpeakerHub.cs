using MUNity.Models.ListOfSpeakers;
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
        /// Somehting in this listOfSpeaker has changed. The Clients should reload the whole list or figure out what
        /// has changed on their own.
        /// </summary>
        /// <param name="listOfSpeaker"></param>
        /// <returns></returns>
        Task SpeakerListChanged(ListOfSpeakers listOfSpeaker);

        /// <summary>
        /// The SpeakerTimer has started with the given seconds as remaining time. The clients should Sync their 
        /// SpeakerStartTime according.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        Task SpeakerTimerStarted(int seconds);

        /// <summary>
        /// The QuestionTimer was started the clients should set the QuestionStartTime accordingly.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        Task QuestionTimerStarted(int seconds);

        /// <summary>
        /// The Status of the List Of Speaker has been set to STOPPED.
        /// </summary>
        /// <returns></returns>
        Task TimerStopped();

        Task SpeakerAdded(Speaker speaker);

        Task QuestionAdded(Speaker question);
    }
}
