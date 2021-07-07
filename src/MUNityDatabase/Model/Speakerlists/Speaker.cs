using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace MUNity.Database.Models.ListOfSpeakers
{

    /// <summary>
    /// A Speaker is someone who can be added to the Speakers or Questions inside a List of Speakers.
    /// You can give any time of name, so you could set it to a person a Country or Delegation.
    /// </summary>
    public class Speaker
    {
        /// <summary>
        /// The Mode with what the Speaker is handled
        /// </summary>
        public enum SpeakerModes
        {
            /// <summary>
            /// Is the Speaker on the List of Speakers
            /// </summary>
            WaitToSpeak,
            /// <summary>
            /// Is the Speaker on the List of Questions
            /// </summary>
            WaitForQuesiton,
            /// <summary>
            /// Is The speaker Currently speaking
            /// </summary>
            CurrentlySpeaking,
            /// <summary>
            /// Is the speaker currently asking a question
            /// </summary>
            CurrentQuestion
        }

        /// <summary>
        /// The Id of the Speaker. This can and should change every time
        /// even if the same person is in one of the lists twice to be able to identify it exact.
        /// The Id has nothing to do with the Paricipant, Delegation, Country etc.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Name that will be displayed.
        /// </summary>
        public string Name { get; set; }

        private string _iso;
        /// <summary>
        /// An Iso code because mostly Counties will be used in this list. You could use the
        /// Iso to identify an icon.
        /// </summary>
        public string Iso { get; set; }

        /// <summary>
        /// The Mode if the Speaker is on the List of Speakers or asking a question
        /// </summary>
        public SpeakerModes Mode { get; set; }

        /// <summary>
        /// The Index of the Speaker.
        /// </summary>
        public int OrdnerIndex { get; set; }

        /// <summary>
        /// The Parent SpeakerlistId
        /// </summary>
        public ListOfSpeakers ListOfSpeakers { get; set; }

    }
}
