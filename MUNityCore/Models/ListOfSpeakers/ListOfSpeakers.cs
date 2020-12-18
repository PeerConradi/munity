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
            Stopped,
            Speaking,
            Question,
            Answer,
            SpeakerPaused,
            QuestionPaused,
            AnswerPaused
        }

        public string ListOfSpeakersId { get; set; }

        public string PublicId { get; set; }

        public string Name { get; set; }

        public EStatus Status { get; set; }

        public TimeSpan SpeakerTime { get; set; }

        public TimeSpan QuestionTime { get; set; }

        public TimeSpan PausedSpeakerTime { get; set; }

        public TimeSpan PausedQuestionTime { get; set; }

        public TimeSpan RemainingSpeakerTime
        {
            get
            {
                if (Status == EStatus.Stopped || Status == EStatus.Question || Status == EStatus.SpeakerPaused || Status == EStatus.QuestionPaused)
                {
                    return PausedSpeakerTime;
                }
                else if (Status == EStatus.Speaking)
                {
                    var finishTime = StartSpeakerTime.AddSeconds(SpeakerTime.TotalSeconds);
                    return finishTime - DateTime.Now;
                }
                var finishTimeQuestion = StartSpeakerTime.AddSeconds(QuestionTime.TotalSeconds);
                return finishTimeQuestion - DateTime.Now;
            }
        }

        public TimeSpan RemainingQuestionTime
        {
            get
            {
                if (Status == EStatus.Stopped || Status == EStatus.Speaking || Status == EStatus.Answer) return PausedQuestionTime;

                var finishTime = StartQuestionTime.AddSeconds(QuestionTime.TotalSeconds);
                return finishTime - DateTime.Now;
            }
        }

        // Es wäre theoretisch möglich eine Liste zu erstellen bei welcher
        // im Speaker selbst die Art: Speaker/Question eingetragen wird als Enum
        // Diese Felder filtern den unterschied dann dadurch raus, dass sie im Where auf
        // den entsprechende Parameter prüfen. Das würde theoretisch eine weitere Liste/Listenart ermöglichen.

        public List<Speaker> Speakers { get; set; }

        public List<Speaker> Questions { get; set; }

        public Speaker CurrentSpeaker { get; set; }

        public Speaker CurrentQuestion { get; set; }


        public bool ListClosed { get; set; } = false;

        public bool QuestionsClosed { get; set; } = false;

        public DateTime StartSpeakerTime { get; set; }

        public DateTime StartQuestionTime { get; set; }

        public void NextSpeaker()
        {
            if (Speakers.Any())
            {
                CurrentSpeaker = Speakers.First();
                Speakers.Remove(Speakers.First());
                CurrentQuestion = null;
            }
            this.Status = EStatus.Stopped;
        }

        public void NextQuestion()
        {
            if (Questions.Any())
            {
                CurrentQuestion = Questions.First();
                Questions.Remove(Questions.First());
            }
            this.Status = EStatus.Stopped;
        }

        public void StartSpeaker()
        {
            if (CurrentSpeaker != null)
            {
                this.PausedQuestionTime = QuestionTime;
                this.StartSpeakerTime = DateTime.Now;
                this.Status = EStatus.Speaking;
            }
            else
            {
                this.Status = EStatus.Stopped;
            }
        }

        public void StartQuestion()
        {
            if (this.CurrentQuestion != null)
            {
                this.PausedSpeakerTime = SpeakerTime;
                this.StartQuestionTime = DateTime.Now;
                this.Status = EStatus.Question;
            }
            else
            {
                this.Status = EStatus.Stopped;
            }
        }

        public void StartAnswer()
        {
            if (CurrentSpeaker != null)
            {
                this.PausedQuestionTime = QuestionTime;
                this.StartSpeakerTime = DateTime.Now;
                this.Status = EStatus.Answer;
            }
        }

        public void PauseSpeaker()
        {
            this.PausedSpeakerTime = RemainingSpeakerTime;
            if (Status == EStatus.Speaking)
                this.Status = EStatus.SpeakerPaused;
            else if (Status == EStatus.Answer)
                this.Status = EStatus.AnswerPaused;
            else
                this.Status = EStatus.Stopped;
        }

        public void PauseQuestion()
        {
            this.PausedQuestionTime = RemainingQuestionTime;
            this.Status = EStatus.QuestionPaused;
        }

        public void ResumeSpeaker()
        {
            if (CurrentSpeaker != null)
            {
                if (Status == EStatus.SpeakerPaused)
                    this.StartSpeakerTime = DateTime.Now.AddSeconds(RemainingSpeakerTime.TotalSeconds - SpeakerTime.TotalSeconds);
                else if (Status == EStatus.AnswerPaused)
                    this.StartSpeakerTime = DateTime.Now.AddSeconds(RemainingSpeakerTime.TotalSeconds - QuestionTime.TotalSeconds);
                else
                    this.StartQuestionTime = DateTime.Now;

                this.Status = EStatus.Speaking;
            }
            else
            {
                this.Status = EStatus.Stopped;
            }
        }

        public void ResumeQuestion()
        {
            if (CurrentQuestion != null)
            {
                this.StartQuestionTime = DateTime.Now.AddSeconds(RemainingQuestionTime.TotalSeconds - QuestionTime.TotalSeconds);
                this.Status = EStatus.Question;
            }
            else
            {
                this.Status = EStatus.Stopped;
            }
        }

        public Speaker AddSpeaker(string name, string iso = "")
        {
            var newSpeaker = new Speaker()
            {
                Id = Guid.NewGuid().ToString(),
                Iso = iso,
                Name = name
            };
            Speakers.Add(newSpeaker);
            Questions.Clear();
            return newSpeaker;
        }

        public void AddSpeaker(Speaker speaker)
        {
            if (string.IsNullOrEmpty(speaker.Id) || Speakers.Any(n => n.Id == speaker.Id))
                speaker.Id = Guid.NewGuid().ToString();
            Speakers.Add(speaker);
            Questions.Clear();
        }

        public Speaker AddQuestion(string name, string iso = "")
        {
            var newSpeaker = new Speaker()
            {
                Id = Guid.NewGuid().ToString(),
                Iso = iso,
                Name = name
            };
            Questions.Add(newSpeaker);
            return newSpeaker;
        }

        public void AddQuestion(Speaker question)
        {
            if (string.IsNullOrEmpty(question.Id) || Speakers.Any(n => n.Id == question.Id))
                question.Id = Guid.NewGuid().ToString();
            Questions.Add(question);
        }

        public void RemoveSpeaker(string id)
        {
            Speakers.RemoveAll(n => n.Id == id);
        }

        public void RemoveQuestion(string id)
        {
            Speakers.RemoveAll(n => n.Id == id);
        }

        public void ClearCurrentSpeaker()
        {
            if (this.Status == EStatus.Speaking || this.Status == EStatus.SpeakerPaused || this.Status == EStatus.Answer || this.Status == EStatus.AnswerPaused)
                this.Status = EStatus.Stopped;
            this.CurrentQuestion = null;
        }

        public void ClearCurrentQuestion()
        {
            if (this.Status == EStatus.Question || this.Status == EStatus.QuestionPaused)
                this.Status = EStatus.Stopped;
            this.CurrentQuestion = null;
        }

        public void AddSpeakerSeconds(int seconds)
        {
            this.StartSpeakerTime = this.StartSpeakerTime.AddSeconds(seconds);
        }

        public void AddQuestionSeconds(int seconds)
        {
            this.StartSpeakerTime = this.StartQuestionTime.AddSeconds(seconds);
        }

        public ListOfSpeakers()
        {
            this.Speakers = new List<Speaker>();
            this.Questions = new List<Speaker>();
            this.ListOfSpeakersId = Guid.NewGuid().ToString();
            this.SpeakerTime = new TimeSpan(0, 3, 0);
            this.QuestionTime = new TimeSpan(0, 0, 30);
            this.PausedSpeakerTime = this.SpeakerTime;
            this.PausedQuestionTime = this.QuestionTime;
        }

    }
}
