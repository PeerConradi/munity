using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;

namespace MUNityCore.Models.ListOfSpeakers
{
    public class ListOfSpeakers
    {

        public const string newListName = "Neue Redeliste";

        public enum EStatus
        {
            STOPPED,
            SPEAKING,
            QUESTION,
            ANSWER
        }

        public string ListOfSpeakersId { get; set; }

        public string PublicId { get; set; }

        public string Name { get; set; }

        public TimeSpan SpeakerTime { get; set; }

        public TimeSpan QuestionTime { get; set; }


        private TimeSpan _remainingSpeakerTime;
        public TimeSpan RemainingSpeakerTime
        {
            get => _remainingSpeakerTime - new TimeSpan(0,0, (int)(DateTime.Now - StartSpeakerTime).TotalSeconds);
            set  => _remainingSpeakerTime = value;
        }

        private TimeSpan _remainingQuestionTime;
        public TimeSpan RemainingQuestionTime { 
            get => _remainingQuestionTime - new TimeSpan(0,0,(int)(DateTime.Now - StartQuestionTime).TotalSeconds);
            set => _remainingQuestionTime = value;
        }

        public List<Speaker> Speakers { get; set; }

        public List<Speaker> Questions { get; set; }


        private Speaker _currentSpeaker;
        public Speaker CurrentSpeaker { get => _currentSpeaker; 
            set
            {
                _currentSpeaker = value;
                if (value == null)
                {
                    if (Status != EStatus.QUESTION)
                    {
                        StartQuestionTime = DateTime.Now;
                    }

                    if (Status == EStatus.SPEAKING || Status == EStatus.QUESTION)
                    {
                        Status = EStatus.STOPPED;
                    }
                    StartSpeakerTime = DateTime.Now;
                    RemainingSpeakerTime = SpeakerTime;
                }
            }
        }

        private Speaker _currentQuestion;
        public Speaker CurrentQuestion { get => _currentQuestion; 
            set
            {
                _currentQuestion = value;
                if (value == null)
                {
                    if (Status != EStatus.SPEAKING && Status != EStatus.QUESTION)
                    {
                        StartSpeakerTime = DateTime.Now;
                    }

                    if (Status == EStatus.QUESTION)
                    {
                        Status = EStatus.STOPPED;
                    }
                    StartQuestionTime = DateTime.Now;
                    RemainingQuestionTime = QuestionTime;
                }
            }
        }

        public EStatus Status { get; set; }

        public bool IsSpeaking => Status == EStatus.SPEAKING;

        public bool IsQuestioning  => Status == EStatus.QUESTION;

        public bool ListClosed { get; set; }

        public bool QuestionsClosed { get; set; }

        public TimeSpan LowTimeMark { get; set; }

        public bool SpeakerLowTime { get; set; }

        public bool QuestionLowTime { get; set; }

        public bool SpeakerTimeout { get; set; }

        public bool QuestionTimeout { get; set; }

        public string ConferenceId { get; set; }

        public string CommitteeId { get; set; }

        public DateTime StartSpeakerTime { get; set; }

        public DateTime StartQuestionTime { get; set; }


        public void AddSpeaker(Speaker speaker)
        {
            Speakers.Add(speaker);
            if (Status != EStatus.SPEAKING && Status != EStatus.ANSWER)
            {
                this.StartSpeakerTime = DateTime.Now;
            }
            if (Status != EStatus.QUESTION)
            {
                this.StartQuestionTime = DateTime.Now;
            }
        }

        public void AddQuestion(Speaker question)
        {
            Questions.Add(question);
            if (Status != EStatus.SPEAKING && Status != EStatus.ANSWER)
            {
                this.StartSpeakerTime = DateTime.Now;
            }
            if (Status != EStatus.QUESTION)
            {
                this.StartQuestionTime = DateTime.Now;
            }
        }

        public ListOfSpeakers()
        {
            ListOfSpeakersId = Guid.NewGuid().ToString();
            Name = "";
            Speakers = new List<Speaker>();
            Questions = new List<Speaker>();
            SpeakerTime = new TimeSpan(0, 3, 0);
            QuestionTime = new TimeSpan(0, 0, 30);
            LowTimeMark = new TimeSpan(0, 0, 10);
            _remainingSpeakerTime = new TimeSpan(SpeakerTime.Ticks);
            _remainingQuestionTime = new TimeSpan(QuestionTime.Ticks);
            StartSpeakerTime = DateTime.Now;
            StartQuestionTime = DateTime.Now;
        }

        public ListOfSpeakers(TimeSpan nSpeakerTime, TimeSpan nQuestionTime)
        {
            Speakers = new List<Speaker>();
            Questions = new List<Speaker>();
            SpeakerTime = nSpeakerTime;
            QuestionTime = nQuestionTime;
        }

        public void StartSpeaker()
        {
            //TODO: Calculate finished time
            this.StartSpeakerTime = DateTime.Now;
            Status = EStatus.SPEAKING;
        }

        public void PauseSpeaker()
        {
            Status = EStatus.STOPPED;
            _remainingSpeakerTime = RemainingSpeakerTime;
            _remainingQuestionTime = RemainingQuestionTime;
        }

        public void StartQuestion()
        {
            Status = EStatus.QUESTION;
            this.StartQuestionTime = DateTime.Now;
        }


        public void StartAnswer()
        {
            RemainingSpeakerTime = QuestionTime;
            StartSpeakerTime = DateTime.Now;
            Status = EStatus.ANSWER;
        }

        /// <summary>
        /// Stopps the Speakerlist and sets the Next Speaker.
        /// </summary>
        public void NextSpeaker()
        {
            if (Speakers.Count > 0)
            {
                CurrentSpeaker = Speakers[0];
                Speakers.Remove(Speakers[0]);
            }
            else
            {
                CurrentSpeaker = null;
                
            }
            Status = EStatus.STOPPED;
            RemainingSpeakerTime = SpeakerTime;
            StartSpeakerTime = DateTime.Now;
            // Also reset the question timers!
            RemainingQuestionTime = QuestionTime;
            StartQuestionTime = DateTime.Now;
            CurrentQuestion = null;
            Questions.Clear();
        }

        public void NextQuestion()
        {
            if (Questions.Count > 0)
            {
                CurrentQuestion = Questions[0];
                Questions.Remove(Questions[0]);
            }
            else
            {
                CurrentQuestion = null;
            }

            Status = EStatus.STOPPED;
            RemainingQuestionTime = QuestionTime;
            StartQuestionTime = DateTime.Now;

            RemainingSpeakerTime = SpeakerTime;
            StartSpeakerTime = DateTime.Now;
        }

        public void RemoveSpeaker(Speaker delegation)
        {
            Speakers.Remove(delegation);
        }

        public bool RemoveSpeaker(string id)
        {
            var speaker = Speakers.FirstOrDefault(n => n.Id == id);
            if (speaker != null)
            {
                Speakers.Remove(speaker);
                return true;
            }
            return false;
        }

    }
}
