using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models
{
    public class SpeakerlistModel
    {

        public const string newListName = "Neue Redeliste";


        public enum EStatus
        {
            STOPPED,
            SPEAKING,
            QUESTION,
            ANSWER
        }

        public string ID { get; set; }

        public int PublicId { get; set; }

        public string Name { get; set; }

        public TimeSpan Speakertime { get; set; }

        public TimeSpan Questiontime { get; set; }


        private TimeSpan _remainingSpeakerTime;
        public TimeSpan RemainingSpeakerTime
        {
            get
            {
                return _remainingSpeakerTime - new TimeSpan(0,0, (int)(DateTime.Now - StartSpeakerTime).TotalSeconds);
            }
            set
            {
                _remainingSpeakerTime = value;
            }
        }

        private TimeSpan _remainingQuestionTime;
        public TimeSpan RemainingQuestionTime { get
            {
                return _remainingQuestionTime - new TimeSpan(0,0,(int)(DateTime.Now - StartQuestionTime).TotalSeconds);
            }
            set
            {
                _remainingQuestionTime = value;
            }
        }

        public List<DelegationModel> Speakers { get; set; }

        public List<DelegationModel> Questions { get; set; }


        private DelegationModel _currentSpeaker;
        public DelegationModel CurrentSpeaker { get => _currentSpeaker; 
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
                    RemainingSpeakerTime = Speakertime;
                }
            }
        }

        private DelegationModel _currenQuestion;
        public DelegationModel CurrentQuestion { get => _currenQuestion; 
            set
            {
                _currenQuestion = value;
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
                    RemainingQuestionTime = Questiontime;
                }
            }
        }

        public EStatus Status { get; set; }

        public bool IsSpeaking { get => Status == EStatus.SPEAKING; }

        public bool IsQuestioning { get => Status == EStatus.QUESTION; }

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


        public void AddSpeaker(DelegationModel speaker)
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

        public void AddQuestion(DelegationModel question)
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

        public SpeakerlistModel(string id = null, string name = newListName)
        {

            ID = id ?? Guid.NewGuid().ToString();
            Name = name;
            Speakers = new List<DelegationModel>();
            Questions = new List<DelegationModel>();
            Speakertime = new TimeSpan(0, 3, 0);
            Questiontime = new TimeSpan(0, 0, 30);
            LowTimeMark = new TimeSpan(0, 0, 10);
            _remainingSpeakerTime = new TimeSpan(Speakertime.Ticks);
            _remainingQuestionTime = new TimeSpan(Questiontime.Ticks);
            StartSpeakerTime = DateTime.Now;
            StartQuestionTime = DateTime.Now;
            //RemainingSpeakerTime = new TimeSpan(0, 3, 0);
            //RemainingQuestionTime = new TimeSpan(0, 1, 0);
        }

        public SpeakerlistModel(TimeSpan n_speakertime, TimeSpan n_questiontime)
        {
            Speakers = new List<DelegationModel>();
            Questions = new List<DelegationModel>();
            Speakertime = n_speakertime;
            Questiontime = n_questiontime;
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



        public void ToggleSpeaker()
        {
            if (Status == EStatus.SPEAKING)
                PauseSpeaker();
            else
                StartSpeaker();
        }

        public void StartAnswer()
        {
            RemainingSpeakerTime = Questiontime;
            StartSpeakerTime = DateTime.Now;
            Status = EStatus.ANSWER;
        }

        public void ToggleQuestion()
        {
            if (Status == EStatus.QUESTION)
                PauseSpeaker();
            else
                StartQuestion();
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
            RemainingSpeakerTime = Speakertime;
            StartSpeakerTime = DateTime.Now;
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
            RemainingQuestionTime = Questiontime;
            StartQuestionTime = DateTime.Now;
        }

        public void RemoveSpeaker(DelegationModel delegation)
        {
            Speakers.Remove(delegation);
        }

        public bool RemoveSpeaker(string id)
        {
            var speaker = Speakers.FirstOrDefault(n => n.ID == id);
            if (speaker != null)
            {
                Speakers.Remove(speaker);
                return true;
            }
            return false;
        }

        public void RemoveQuestion(DelegationModel delegation)
        {
            Questions.Remove(delegation);
        }

        public void MoveSpeakerUp(DelegationModel delegation)
        {
            int index = Speakers.IndexOf(delegation);
            if (index == -1 || index == 0)
                return;

            Speakers.Remove(delegation);
            Speakers.Insert(index - 1, delegation);
        }

        public void MoveQuestionUp(DelegationModel delegation)
        {
            int index = Questions.IndexOf(delegation);
            if (index == -1 || index == 0)
                return;

            Questions.Remove(delegation);
            Questions.Insert(index - 1, delegation);
        }

        public void MoveSpeakerDown(DelegationModel delegation)
        {
            int index = Speakers.IndexOf(delegation);
            if (index == -1 || index + 1 == Speakers.Count)
                return;

            Speakers.Remove(delegation);
            Speakers.Insert(index + 1, delegation);
        }

        public void MoveQuestionDown(DelegationModel delegation)
        {
            int index = Questions.IndexOf(delegation);
            if (index == -1 || index + 1 == Speakers.Count)
                return;

            Questions.Remove(delegation);
            Questions.Insert(index + 1, delegation);
        }
    }
}
