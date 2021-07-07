using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace MUNity.Database.Models.ListOfSpeakers
{

    public class ListOfSpeakers
    {
        /// <summary>
        /// Possible states that can be used within a list of Speakers.
        /// </summary>
        public enum EStatus
        {
            /// <summary>
            /// Noone is Speaking and the last status is unknown. Resume a speaker or Questions will 
            /// give them the full SpeakerTime/QuestionTime
            /// </summary>
            Stopped,
            /// <summary>
            /// The CurrentSpeaker is talking.
            /// </summary>
            Speaking,
            /// <summary>
            /// The Current Question is talking.
            /// </summary>
            Question,
            /// <summary>
            /// The CurrentSpeaker is Answering. Meaning he/she has the same time talking as the one asking the Question.
            /// </summary>
            Answer,
            /// <summary>
            /// The Speaker is paused and will continue with the time that he/she was paused at.
            /// </summary>
            SpeakerPaused,
            /// <summary>
            /// The Question is paused an will continue with the time that he/she was paused at.
            /// </summary>
            QuestionPaused,
            /// <summary>
            /// The answer has been paused and will continue with the time that he/she was paused at.
            /// </summary>
            AnswerPaused
        }

        /// <summary>
        /// The Id of the List of Speakers will be given a new GUID when the constructor is called.
        /// </summary>
        public string ListOfSpeakersId { get; set; }

        /// <summary>
        /// A public Id for example a code that you can give out to others to be able to read the List of Speakers
        /// to be able to read but not interact with it. Note that the MUNityBase does not have a logic for this
        /// and it will be implemented in the API.
        /// </summary>
        public string PublicId { get; set; }

        private string _name;
        /// <summary>
        /// A Name of a list of Speakers. That can be displayed. The Name is not used to identify the list, to identitfy the list use
        /// the ListOfSpeakersId.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// The Current Status of the list, is someone talking, paused or is the List reset to default.
        /// </summary>
        public EStatus Status { get; set; }

        /// <summary>
        /// The time that the Speakers are allowed to talk.
        /// </summary>
        public TimeSpan SpeakerTime { get; set; }

        /// <summary>
        /// The time that someone asking a question is allowed to talk and also how long the Speaker is allowed to answer a question.
        /// </summary>
        public TimeSpan QuestionTime { get; set; }

        /// <summary>
        /// The Remaining Time that a Speaker had when he/she had been paused.
        /// </summary>
        public TimeSpan PausedSpeakerTime { get; set; }

        private TimeSpan _pausedQuestionTime;
        /// <summary>
        /// The Remaining Time that a speaker had when he/she had been paused.
        /// </summary>
        public TimeSpan PausedQuestionTime { get; set; }

        /// <summary>
        /// List that holds all Speakers that are inside the Speakers or Questions List and also the Current Speaker/Question.
        /// </summary>
        public List<Speaker> AllSpeakers { get; set; }




        /// <summary>
        /// Is the List of Speakers closed. If this is true you should not add people to the Speakers.
        /// This will not be catched when calling Speakers.Add()/AddSpeaker("",""). This is more for visual
        /// feedback of a closed List.
        /// </summary>
        public bool ListClosed { get; set; }

        /// <summary>
        /// Are people allowed to get on the List of questions. This is only for visual feedback, you can
        /// technacally still add people to the list.
        /// </summary>
        public bool QuestionsClosed { get; set; }

        /// <summary>
        /// The time when to speaker started talking. With the diff between the StartTime and the SpeakerTime the 
        /// RemainingSpeakerTime will be calculated.
        /// </summary>
        public DateTime StartSpeakerTime { get; set; }

        /// <summary>
        /// The time when the question started beeing asked. WIth the diff between this and the QuestionTime the
        /// RaminingQuestionTime will be calculated.
        /// </summary>
        public DateTime StartQuestionTime { get; set; }

        /// <summary>
        /// Will create a new ListOfSpeakers and generate a new GUID for it, will also init the Speakers and Questions
        /// as an empty collection and set the default SpeakerTime to 3 minutes and the QuestionTime to 30 seconds.
        /// </summary>
        public ListOfSpeakers()
        {
            this.ListOfSpeakersId = Guid.NewGuid().ToString();
            this.SpeakerTime = new TimeSpan(0, 3, 0);
            this.QuestionTime = new TimeSpan(0, 0, 30);
            this.PausedSpeakerTime = this.SpeakerTime;
            this.PausedQuestionTime = this.QuestionTime;
            AllSpeakers = new List<Speaker>();
        }

    }
}
