using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Text.Json.Serialization;
using MUNityBase;
using MUNityBase.Interfances;
using System.Runtime.CompilerServices;

namespace MUNity.ViewModels.ListOfSpeakers
{

    /// <summary>
    /// The Base Structure for the List Of Speakers. Note that the logic can be found when using
    /// MUNity.Extensions.LoSExtensions.
    /// The ListOfSpeakers dont work with timers but with timestamps that can be tracked.
    /// To create a new List simply call the constructor.
    /// 
    /// <code>
    /// var listOfSpeakers = new ListOfSpeakers();
    /// // Remember to import MUNity.Extension.LoSExtensions!
    /// // This will add a new Speaker to the list.
    /// listOfSpeakers.AddSpeaker("Germany", "de");
    /// 
    /// // This will set Germany as the CurrentSpeaker
    /// listOfSpeakers.NextSpeaker();
    /// 
    /// // This will activate the Speaking Mode.
    /// listOfSpeakers.StartSpeaker();
    /// // You could also use:
    /// // listofSpeakers.ResumeSpeaker();
    /// 
    /// // You can get the remaining time from:
    /// var remaingingTime = listOfSpeakers.RemainingSpeakerTime;
    /// // You could use a timer to refresh/reload the ReaminingSpeakerTime every second to get a countdown.
    /// </code>
    /// <seealso cref="MUNity.Extensions.LoSExtensions"/>
    /// </summary>
    public class ListOfSpeakersViewModel : INotifyPropertyChanged, IComparable<ListOfSpeakersViewModel>, IListOfSpeakers
    {
        

        /// <summary>
        /// The Id of the List of Speakers will be given a new GUID when the constructor is called.
        /// </summary>
        public string ListOfSpeakersId { get; set; }

        private string _publicId;
        /// <summary>
        /// A public Id for example a code that you can give out to others to be able to read the List of Speakers
        /// to be able to read but not interact with it. Note that the MUNityBase does not have a logic for this
        /// and it will be implemented in the API.
        /// </summary>
        public string PublicId 
        {
            get => _publicId; 
            set
            {
                if (value != this._publicId)
                {
                    this._publicId = value;
                    NotifyPropertyChanged(nameof(PublicId));
                }
            } 
        }

        private string _name;
        /// <summary>
        /// A Name of a list of Speakers. That can be displayed. The Name is not used to identify the list, to identitfy the list use
        /// the ListOfSpeakersId.
        /// </summary>
        public string Name 
        {
            get => _name; 
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        private ESpeakerListStatus _status;
        /// <summary>
        /// The Current Status of the list, is someone talking, paused or is the List reset to default.
        /// </summary>
        public ESpeakerListStatus Status 
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    NotifyPropertyChanged(nameof(Status));
                }
            }
        }

        private TimeSpan _speakerTime;
        /// <summary>
        /// The time that the Speakers are allowed to talk.
        /// </summary>
        public TimeSpan SpeakerTime 
        {
            get => _speakerTime;
            set
            {
                if (_speakerTime != value)
                {
                    _speakerTime = value;
                    NotifyPropertyChanged(nameof(SpeakerTime));
                }
            }
        }

        private TimeSpan _questionTime;
        /// <summary>
        /// The time that someone asking a question is allowed to talk and also how long the Speaker is allowed to answer a question.
        /// </summary>
        public TimeSpan QuestionTime 
        {
            get => _questionTime;
            set
            {
                if (_questionTime != value)
                {
                    _questionTime = value;
                    NotifyPropertyChanged(nameof(QuestionTime));
                }
            }
        }

        private TimeSpan _pausedSpeakerTime;
        /// <summary>
        /// The Remaining Time that a Speaker had when he/she had been paused.
        /// </summary>
        public TimeSpan PausedSpeakerTime 
        {
            get => _pausedSpeakerTime;
            set
            {
                if (_pausedSpeakerTime != value)
                {
                    _pausedSpeakerTime = value;
                    NotifyPropertyChanged(nameof(PausedSpeakerTime));
                }
            }
        }

        private TimeSpan _pausedQuestionTime;
        /// <summary>
        /// The Remaining Time that a speaker had when he/she had been paused.
        /// </summary>
        public TimeSpan PausedQuestionTime 
        {
            get => _pausedQuestionTime;
            set
            {
                if (_pausedQuestionTime != value)
                {
                    _pausedQuestionTime = value;
                    NotifyPropertyChanged(nameof(_pausedQuestionTime));
                }
            }
        }

        /// <summary>
        /// Gives you the Remaining time a speaker had at the moment you call this Getter.
        /// This will not fire any sort of PropertyChanged event. If you want to implement a countdown
        /// you will have to create a timer and recall this getter every Second.
        /// </summary>
        [JsonIgnore]
        public TimeSpan RemainingSpeakerTime
        {
            get
            {
                if (Status == ESpeakerListStatus.Stopped)
                    return SpeakerTime;
                if (Status == ESpeakerListStatus.Question || 
                    Status == ESpeakerListStatus.SpeakerPaused || 
                    Status == ESpeakerListStatus.QuestionPaused || 
                    Status == ESpeakerListStatus.AnswerPaused)
                {
                    return PausedSpeakerTime;
                }
                else if (Status == ESpeakerListStatus.Speaking)
                {
                    var finishTime = StartSpeakerTime.AddSeconds(SpeakerTime.TotalSeconds);
                    return finishTime - DateTime.Now.ToUniversalTime();
                    // Startzeitpunkt                 Startzeitpunkt + Speakertime
                    //       |---------------|<-------->|
                    //                          Verbleibende Zeit
                }
                else if (Status == ESpeakerListStatus.Answer)
                {
                    var finishTime = StartSpeakerTime.AddSeconds(QuestionTime.TotalSeconds);
                    return finishTime - DateTime.Now.ToUniversalTime();
                }

                // Default return
                var defaultReturn = StartSpeakerTime.AddSeconds(SpeakerTime.TotalSeconds);
                return defaultReturn - DateTime.Now.ToUniversalTime();
            }
        }

        /// <summary>
        /// Will return the Remaining QuestionTime at the moment this Getter is called.
        /// Note that this will not fire a PropertyChangedEvent. If you want to create a countdown
        /// you will have to call this getter with a timer every second or minute howevery you want the countdown to happen.
        /// </summary>
        [JsonIgnore]
        public TimeSpan RemainingQuestionTime
        {
            get
            {
                if (Status == ESpeakerListStatus.Stopped) return QuestionTime;
                if (Status != ESpeakerListStatus.Question) return PausedQuestionTime;

                var finishTime = StartQuestionTime.AddSeconds(QuestionTime.TotalSeconds);
                return finishTime - DateTime.Now.ToUniversalTime();
            }
        }

        /// <summary>
        /// List that holds all Speakers that are inside the Speakers or Questions List and also the Current Speaker/Question.
        /// </summary>
        public ObservableCollection<SpeakerViewModel> AllSpeakers { get; set; }

        /// <summary>
        /// A list of speakers that are waiting to speak next.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<SpeakerViewModel> Speakers
        {
            get
            {
                return AllSpeakers.Where(n => n.Mode == SpeakerModes.WaitToSpeak).OrderBy(n => n.OrdnerIndex);
            }
        }

        /// <summary>
        /// A list of people that want to ask a question.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<SpeakerViewModel> Questions
        {
            get
            {
                return AllSpeakers.Where(n => n.Mode == SpeakerModes.WaitForQuesiton).OrderBy(n => n.OrdnerIndex);
            }
        }

        /// <summary>
        /// The person currently speaking or waiting to answer a question.
        /// </summary>
        [JsonIgnore]
        public ISpeaker CurrentSpeaker => AllSpeakers.FirstOrDefault(n => n.Mode == SpeakerModes.CurrentlySpeaking);

        /// <summary>
        /// The person currently asking a question.
        /// </summary>
        [JsonIgnore]
        public ISpeaker CurrentQuestion => AllSpeakers.FirstOrDefault(n => n.Mode == SpeakerModes.CurrentQuestion);


        private bool _listClosed = false;
        /// <summary>
        /// Is the List of Speakers closed. If this is true you should not add people to the Speakers.
        /// This will not be catched when calling Speakers.Add()/AddSpeaker("",""). This is more for visual
        /// feedback of a closed List.
        /// </summary>
        public bool ListClosed 
        {
            get => _listClosed;
            set
            {
                if (_listClosed != value)
                {
                    _listClosed = value;
                    NotifyPropertyChanged(nameof(ListClosed));
                }
            }
        }

        private bool _questionsClosed = false;
        /// <summary>
        /// Are people allowed to get on the List of questions. This is only for visual feedback, you can
        /// technacally still add people to the list.
        /// </summary>
        public bool QuestionsClosed 
        {
            get => _questionsClosed;
            set
            {
                if (_questionsClosed != value)
                {
                    _questionsClosed = value;
                    NotifyPropertyChanged(nameof(QuestionsClosed));
                }
            }
        }

        private DateTime _startSpeakerTime;
        /// <summary>
        /// The time when to speaker started talking. With the diff between the StartTime and the SpeakerTime the 
        /// RemainingSpeakerTime will be calculated.
        /// </summary>
        public DateTime StartSpeakerTime 
        {
            get => _startSpeakerTime;
            set
            {
                if (_startSpeakerTime != value)
                {
                    _startSpeakerTime = value;
                    NotifyPropertyChanged(nameof(StartSpeakerTime));
                }
            }
        }

        private DateTime _startQuestionTime;
        /// <summary>
        /// The time when the question started beeing asked. WIth the diff between this and the QuestionTime the
        /// RaminingQuestionTime will be calculated.
        /// </summary>
        public DateTime StartQuestionTime 
        {
            get => _startQuestionTime;
            set
            {
                if (_startQuestionTime != value)
                {
                    _startQuestionTime = value;
                    NotifyPropertyChanged(nameof(StartQuestionTime));
                }
            }
        }

        /// <summary>
        /// Will create a new ListOfSpeakers and generate a new GUID for it, will also init the Speakers and Questions
        /// as an empty collection and set the default SpeakerTime to 3 minutes and the QuestionTime to 30 seconds.
        /// </summary>
        public ListOfSpeakersViewModel()
        {
            this.ListOfSpeakersId = Guid.NewGuid().ToString();
            this.SpeakerTime = new TimeSpan(0, 3, 0);
            this.QuestionTime = new TimeSpan(0, 0, 30);
            this.PausedSpeakerTime = this.SpeakerTime;
            this.PausedQuestionTime = this.QuestionTime;
            AllSpeakers = new ObservableCollection<SpeakerViewModel>();
            AllSpeakers.CollectionChanged += _allSpeakers_CollectionChanged;
        }

        private void _allSpeakers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems.OfType<SpeakerViewModel>().Any(n => n.Mode == SpeakerModes.WaitToSpeak))
                {
                    NotifyPropertyChanged(nameof(Speakers));
                }
                if (e.NewItems.OfType<SpeakerViewModel>().Any(n => n.Mode == SpeakerModes.WaitForQuesiton))
                {
                    NotifyPropertyChanged(nameof(Questions));
                }
            }
        }

        /// <summary>
        /// Gets called when a property inside the ListOfSpeakers has changed. This does not include the ListOfSpeakersId and the Speakers/Questions.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fire the PropertyChanged Event for a property with the given name.
        /// </summary>
        /// <param name="name"></param>
        public void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Creates a new instance of s speaker and adds it to the end of speakers.
        /// The speaker will get an Id from a new Guid.
        /// </summary>
        /// <param name="list">The list of speakers that this should be added to.</param>
        /// <param name="name">The display name of the speaker.</param>
        /// <param name="iso">The iso that could be used to get an icon.</param>
        /// <returns></returns>
        public ISpeaker AddSpeaker(string name, string iso = "")
        {
            var newSpeaker = new SpeakerViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                Iso = iso,
                Name = name,
                ListOfSpeakers = this,
                Mode = SpeakerModes.WaitToSpeak
            };
            if (this.Speakers.Any())
            {
                newSpeaker.OrdnerIndex = this.Speakers.Max(n => n.OrdnerIndex) + 1;
            }
            this.AllSpeakers.Add(newSpeaker);
            return newSpeaker;
        }

        //public ISpeaker AddSpeaker(SpeakerViewModel speaker)
        //{
        //    var exisiting = this.AllSpeakers.FirstOrDefault(n => n.Id == speaker.Id);
        //    if (exisiting != null) return exisiting;

        //    this.AllSpeakers.Add(speaker);
        //    return speaker;
        //}

        public void RemoveSpeaker(string id)
        {
            var speaker = this.AllSpeakers.FirstOrDefault(n => n.Id == id);
            if (speaker != null)
                this.AllSpeakers.Remove(speaker);
        }

        /// <summary>
        /// Adds someone to the list of questions.
        /// </summary>
        /// <param name="list">The list that this should be added to.</param>
        /// <param name="name">The display name that should be shown inside the list of questions and the current question.</param>
        /// <param name="iso">The iso that can be used to find an icon.</param>
        /// <returns></returns>
        public ISpeaker AddQuestion(string name, string iso = "")
        {
            var newSpeaker = new SpeakerViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                Iso = iso,
                Name = name,
                ListOfSpeakers = this,
                Mode = SpeakerModes.WaitForQuesiton
            };
            if (this.Questions.Any())
            {
                newSpeaker.OrdnerIndex = this.Questions.Max(n => n.OrdnerIndex) + 1;
            }
            this.AllSpeakers.Add(newSpeaker);
            return newSpeaker;
        }

        /// <summary>
        /// Compares this list of Speakers to another list of speakers by the given values.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ListOfSpeakersViewModel other)
        {
            if (this.ListOfSpeakersId != other.ListOfSpeakersId) 
                return 1;
            if (this.CurrentQuestion == null && other.CurrentQuestion != null) 
                return 1;
            if (this.CurrentQuestion != null && other.CurrentQuestion == null) 
                return 1;
            if (this.CurrentQuestion != null && other.CurrentQuestion != null)
                if (this.CurrentQuestion.CompareTo(other.CurrentQuestion) != 0) 
                    return 1;

            if (this.CurrentSpeaker == null && other.CurrentSpeaker != null) 
                return 1;
            if (this.CurrentSpeaker != null && other.CurrentSpeaker == null) 
                return 1;
            if (this.CurrentSpeaker != null && other.CurrentSpeaker != null)
                if (this.CurrentSpeaker.CompareTo(other.CurrentSpeaker) != 0) 
                    return 1;

            if (this.ListClosed != other.ListClosed) 
                return 1;
            if (this.Name != other.Name) 
                return 1;
            if (this.PausedQuestionTime != other.PausedQuestionTime) 
                return 1;
            if (this.PausedSpeakerTime != other.PausedSpeakerTime) 
                return 1;
            if (this.PublicId != other.PublicId) 
                return 1;
            if (this.QuestionsClosed != other.QuestionsClosed) 
                return 1;
            if (this.QuestionTime != other.QuestionTime) 
                return 1;
            if (this.SpeakerTime != other.SpeakerTime) 
                return 1;
            if (this.StartQuestionTime != other.StartQuestionTime) 
                return 1;
            if (this.StartSpeakerTime != other.StartSpeakerTime) 
                return 1;
            if (this.Status != other.Status) 
                return 1;
            if (this.AllSpeakers.Count != other.AllSpeakers.Count) 
                return 1;
            if (this.AllSpeakers.Any() && other.AllSpeakers.Any())
            {
                for (int i = 0; i < this.AllSpeakers.Count; i++)
                {
                    if (this.AllSpeakers[i].CompareTo(other.AllSpeakers[i]) != 0) 
                        return 1;
                }
            }

            return 0;
            
        }

        public void RemoveQuestion(string id)
        {
            var speakerToRemove = this.AllSpeakers.FirstOrDefault(n => n.Id == id);
            if (speakerToRemove != null)
                this.AllSpeakers.Remove(speakerToRemove);
        }

        public void AddSpeakerSeconds(double seconds)
        {
            this.StartSpeakerTime = this.StartSpeakerTime.AddSeconds(seconds);
        }

        public void AddQuestionSeconds(double seconds)
        {
            this.StartQuestionTime = this.StartQuestionTime.AddSeconds(seconds);
        }

        public ISpeaker NextSpeaker()
        {
            if (this.AllSpeakers.Any(n => n.Mode == SpeakerModes.WaitToSpeak))
            {

                // Remove all Questions, Current Speakers and the one currently asking a Question.
                var questions = this.AllSpeakers.Where(n => n.Mode != SpeakerModes.WaitToSpeak).ToList();
                questions.ForEach(n => AllSpeakers.Remove(n));

                // Remove the current Question
                ClearCurrentQuestion();

                // Pick the first speaker in line
                var nextSpeaker = AllSpeakers.OrderBy(n => n.OrdnerIndex).First();
                nextSpeaker.Mode = SpeakerModes.CurrentlySpeaking;
                NotifyPropertyChanged(nameof(CurrentSpeaker));
                NotifyPropertyChanged(nameof(CurrentQuestion));
                NotifyPropertyChanged(nameof(Questions));
                NotifyPropertyChanged(nameof(Speakers));
            }
            else
            {
                ClearCurrentSpeaker();
            }
            Status = ESpeakerListStatus.Stopped;
            return CurrentSpeaker;
        }

        public ISpeaker NextQuestion()
        {
            if (Questions.Any())
            {
                // Delete the current Questions (remove all of this type of there is a bug and for some reason two are current Speaker)
                var currentQuestion = AllSpeakers.Where(n => n.Mode == SpeakerModes.CurrentQuestion).ToList();
                currentQuestion.ForEach(n => 
                    AllSpeakers.Remove(n));

                var nextQuestion = AllSpeakers.Where(n => n.Mode == SpeakerModes.WaitForQuesiton).OrderBy(n => n.OrdnerIndex).FirstOrDefault();
                nextQuestion.Mode = SpeakerModes.CurrentQuestion;
                NotifyPropertyChanged(nameof(Questions));
                NotifyPropertyChanged(nameof(CurrentQuestion));
            }
            else
            {
                ClearCurrentQuestion();
            }

            if (Status == ESpeakerListStatus.Speaking)
            {
                PauseSpeaker();
            }
            else
            {
                Status = ESpeakerListStatus.Stopped;
            }
            return CurrentQuestion;
        }

        public void Pause()
        {
            if (Status == ESpeakerListStatus.Question)
                PauseQuestion();
            else if (Status == ESpeakerListStatus.Speaking || 
                Status == ESpeakerListStatus.Answer)
                PauseSpeaker();
        }

        public void ResumeQuestion()
        {
            if (CurrentQuestion != null)
            {
                if (Status == ESpeakerListStatus.QuestionPaused)
                {
                    StartQuestionTime = DateTime.Now.ToUniversalTime().AddSeconds(RemainingQuestionTime.TotalSeconds - QuestionTime.TotalSeconds);
                }
                else
                {
                    StartQuestion();
                }

                Status = ESpeakerListStatus.Question;
            }
            else
            {
                Status = ESpeakerListStatus.Stopped;
            }
        }

        public void StartAnswer()
        {
            if (CurrentSpeaker != null)
            {
                PausedQuestionTime = QuestionTime;
                StartSpeakerTime = DateTime.Now.ToUniversalTime();
                Status = ESpeakerListStatus.Answer;
            }
            else
            {
                Status = ESpeakerListStatus.Stopped;
            }
        }

        /// <summary>
        /// Resets the Current Speaker and sets that Status to Stopped if the current Status has something to do with the speaker (talking or paused).
        /// </summary>
        /// <param name="list"></param>
        public void ClearCurrentSpeaker()
        {
            if (Status == ESpeakerListStatus.Speaking || 
                Status == ESpeakerListStatus.SpeakerPaused || 
                Status == ESpeakerListStatus.Answer || 
                Status == ESpeakerListStatus.AnswerPaused)
                Status = ESpeakerListStatus.Stopped;
            // Delete the current Speaker (remove all of this type of there is a bug and for some reason two are current Speaker)
            var currentSpeakers = AllSpeakers.Where(n => n.Mode == SpeakerModes.CurrentlySpeaking).ToList();
            currentSpeakers.ForEach(n => AllSpeakers.Remove(n));
            NotifyPropertyChanged(nameof(CurrentSpeaker));
        }

        /// <summary>
        /// Removes the current Question and sets the status to stopped if the CurrentQuestion was talking of is paused.
        /// </summary>
        /// <param name="list"></param>
        public void ClearCurrentQuestion()
        {
            if (Status == ESpeakerListStatus.Question || 
                Status == ESpeakerListStatus.QuestionPaused)
                Status = ESpeakerListStatus.Stopped;
            // Delete the current Questions (remove all of this type of there is a bug and for some reason two are current Speaker)
            var currentQuestion = AllSpeakers.Where(n => n.Mode == SpeakerModes.CurrentQuestion).ToList();
            currentQuestion.ForEach(n => AllSpeakers.Remove(n));
            NotifyPropertyChanged(nameof(CurrentQuestion));
        }

        private void PauseSpeaker()
        {
            PausedSpeakerTime = RemainingSpeakerTime;
            if (Status == ESpeakerListStatus.Speaking)
                Status = ESpeakerListStatus.SpeakerPaused;
            else if (Status == ESpeakerListStatus.Answer)
                Status = ESpeakerListStatus.AnswerPaused;
        }

        private void PauseQuestion()
        {
            PausedQuestionTime = RemainingQuestionTime;
            if (Status == ESpeakerListStatus.Question)
                Status = ESpeakerListStatus.QuestionPaused;
        }

        private void StartQuestion()
        {
            if (CurrentQuestion != null)
            {
                // Reset the current Speaker time
                PausedSpeakerTime = SpeakerTime;
                StartQuestionTime = DateTime.Now.ToUniversalTime();
                Status = ESpeakerListStatus.Question;
            }
        }

        public void ResumeSpeaker()
        {
            if (CurrentSpeaker != null)
            {
                if (Status == ESpeakerListStatus.SpeakerPaused)
                    ContinueSpeaker();
                else if (Status == ESpeakerListStatus.AnswerPaused)
                    ContinueAnswer();
                else
                    StartSpeaker();

                // Fixes a small glitch in the Question time!
                StartQuestionTime = DateTime.Now.ToUniversalTime();

            }
            else
            {
                Status = ESpeakerListStatus.Stopped;
            }
        }

        private void StartSpeaker()
        {
            if (CurrentSpeaker != null)
            {
                PausedQuestionTime = QuestionTime;
                StartSpeakerTime = DateTime.Now.ToUniversalTime();
                Status = ESpeakerListStatus.Speaking;
            }
        }

        private void ContinueSpeaker()
        {
            StartSpeakerTime = DateTime.Now.ToUniversalTime().AddSeconds(RemainingSpeakerTime.TotalSeconds - SpeakerTime.TotalSeconds);
            Status = ESpeakerListStatus.Speaking;
        }

        private void ContinueAnswer()
        {
            StartSpeakerTime = DateTime.Now.ToUniversalTime().AddSeconds(RemainingSpeakerTime.TotalSeconds - QuestionTime.TotalSeconds);
            Status = ESpeakerListStatus.Answer;
        }

        public void ResetSpeakerTime()
        {
            StartSpeakerTime = DateTime.Now.ToUniversalTime();
        }

        public void ResetQuestionTime()
        {
            StartQuestionTime = DateTime.Now.ToUniversalTime();
        }
    }
}
