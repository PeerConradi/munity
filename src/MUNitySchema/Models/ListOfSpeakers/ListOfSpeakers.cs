using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Text.Json.Serialization;
using MUNity.Converter;
using MUNityBase;

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
    public class ListOfSpeakers : INotifyPropertyChanged, IComparable<ListOfSpeakers>
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

        private EStatus _status;
        /// <summary>
        /// The Current Status of the list, is someone talking, paused or is the List reset to default.
        /// </summary>
        public EStatus Status 
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
        [JsonConverter(typeof(TimespanConverter))]
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
        [JsonConverter(typeof(TimespanConverter))]
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
        [JsonConverter(typeof(TimespanConverter))]
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
        [JsonConverter(typeof(TimespanConverter))]
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
                if (Status == EStatus.Stopped)
                    return SpeakerTime;
                if (Status == EStatus.Question || 
                    Status == EStatus.SpeakerPaused || 
                    Status == EStatus.QuestionPaused || 
                    Status == EStatus.AnswerPaused)
                {
                    return PausedSpeakerTime;
                }
                else if (Status == EStatus.Speaking)
                {
                    var finishTime = StartSpeakerTime.AddSeconds(SpeakerTime.TotalSeconds);
                    return finishTime - DateTime.Now.ToUniversalTime();
                    // Startzeitpunkt                 Startzeitpunkt + Speakertime
                    //       |---------------|<-------->|
                    //                          Verbleibende Zeit
                }
                else if (Status == EStatus.Answer)
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
                if (Status == EStatus.Stopped) return QuestionTime;
                if (Status != EStatus.Question) return PausedQuestionTime;

                var finishTime = StartQuestionTime.AddSeconds(QuestionTime.TotalSeconds);
                return finishTime - DateTime.Now.ToUniversalTime();
            }
        }

        /// <summary>
        /// List that holds all Speakers that are inside the Speakers or Questions List and also the Current Speaker/Question.
        /// </summary>
        public ObservableCollection<Speaker> AllSpeakers { get; set; }

        /// <summary>
        /// A list of speakers that are waiting to speak next.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Speaker> Speakers
        {
            get
            {
                return AllSpeakers.Where(n => n.Mode == Speaker.SpeakerModes.WaitToSpeak).OrderBy(n => n.OrdnerIndex);
            }
        }

        /// <summary>
        /// A list of people that want to ask a question.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Speaker> Questions
        {
            get
            {
                return AllSpeakers.Where(n => n.Mode == Speaker.SpeakerModes.WaitForQuesiton).OrderBy(n => n.OrdnerIndex);
            }
        }

        /// <summary>
        /// The person currently speaking or waiting to answer a question.
        /// </summary>
        [JsonIgnore]
        public Speaker CurrentSpeaker => AllSpeakers.FirstOrDefault(n => n.Mode == Speaker.SpeakerModes.CurrentlySpeaking);

        /// <summary>
        /// The person currently asking a question.
        /// </summary>
        [JsonIgnore]
        public Speaker CurrentQuestion => AllSpeakers.FirstOrDefault(n => n.Mode == Speaker.SpeakerModes.CurrentQuestion);


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
        public ListOfSpeakers()
        {
            this.ListOfSpeakersId = Guid.NewGuid().ToString();
            this.SpeakerTime = new TimeSpan(0, 3, 0);
            this.QuestionTime = new TimeSpan(0, 0, 30);
            this.PausedSpeakerTime = this.SpeakerTime;
            this.PausedQuestionTime = this.QuestionTime;
            AllSpeakers = new ObservableCollection<Speaker>();
            AllSpeakers.CollectionChanged += _allSpeakers_CollectionChanged;
        }

        private void _allSpeakers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems.OfType<Speaker>().Any(n => n.Mode == Speaker.SpeakerModes.WaitToSpeak))
                {
                    NotifyPropertyChanged(nameof(Speakers));
                }
                if (e.NewItems.OfType<Speaker>().Any(n => n.Mode == Speaker.SpeakerModes.WaitForQuesiton))
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
        public void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Compares this list of Speakers to another list of speakers by the given values.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ListOfSpeakers other)
        {
            if (this.ListOfSpeakersId != other.ListOfSpeakersId) return 1;
            if (this.CurrentQuestion == null && other.CurrentQuestion != null) return 1;
            if (this.CurrentQuestion != null && other.CurrentQuestion == null) return 1;
            if (this.CurrentQuestion != null && other.CurrentQuestion != null)
                if (this.CurrentQuestion.CompareTo(other.CurrentQuestion) != 0) return 1;

            if (this.CurrentSpeaker == null && other.CurrentSpeaker != null) return 1;
            if (this.CurrentSpeaker != null && other.CurrentSpeaker == null) return 1;
            if (this.CurrentSpeaker != null && other.CurrentSpeaker != null)
                if (this.CurrentSpeaker.CompareTo(other.CurrentSpeaker) != 0) return 1;

            if (this.ListClosed != other.ListClosed) return 1;
            if (this.Name != other.Name) return 1;
            if (this.PausedQuestionTime != other.PausedQuestionTime) return 1;
            if (this.PausedSpeakerTime != other.PausedSpeakerTime) return 1;
            if (this.PublicId != other.PublicId) return 1;
            if (this.QuestionsClosed != other.QuestionsClosed) return 1;
            if (this.QuestionTime != other.QuestionTime) return 1;
            if (this.SpeakerTime != other.SpeakerTime) return 1;
            if (this.StartQuestionTime != other.StartQuestionTime) return 1;
            if (this.StartSpeakerTime != other.StartSpeakerTime) return 1;
            if (this.Status != other.Status) return 1;
            if (this.AllSpeakers.Count != other.AllSpeakers.Count) return 1;
            if (this.AllSpeakers.Any() && other.AllSpeakers.Any())
            {
                for (int i = 0; i < this.AllSpeakers.Count; i++)
                {
                    if (this.AllSpeakers[i].CompareTo(other.AllSpeakers[i]) != 0) return 1;
                }
            }

            return 0;
            
        }
    }
}
