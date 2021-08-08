using MUNityBase;
using MUNityBase.Interfances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace MUNity.ViewModels.ListOfSpeakers
{

    /// <summary>
    /// A Speaker is someone who can be added to the Speakers or Questions inside a List of Speakers.
    /// You can give any time of name, so you could set it to a person a Country or Delegation.
    /// </summary>
    public class SpeakerViewModel : INotifyPropertyChanged, IComparable<SpeakerViewModel>, ISpeaker
    {

        /// <summary>
        /// The Id of the Speaker. This can and should change every time
        /// even if the same person is in one of the lists twice to be able to identify it exact.
        /// The Id has nothing to do with the Paricipant, Delegation, Country etc.
        /// </summary>
        public string Id { get; set; }

        private string _name;
        /// <summary>
        /// The Name that will be displayed.
        /// </summary>
        public string Name 
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _iso;
        /// <summary>
        /// An Iso code because mostly Counties will be used in this list. You could use the
        /// Iso to identify an icon.
        /// </summary>
        public string Iso 
        {
            get => _iso;
            set
            {
                if (value != _iso)
                {
                    _iso = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private SpeakerModes _mode;
        /// <summary>
        /// The Mode if the Speaker is on the List of Speakers or asking a question
        /// </summary>
        public SpeakerModes Mode 
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _orderIndex = 0;
        /// <summary>
        /// The Index of the Speaker.
        /// </summary>
        public int OrdnerIndex 
        {
            get => _orderIndex;
            set
            {
                if (_orderIndex != value)
                {
                    _orderIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The Parent SpeakerlistId
        /// </summary>
        [JsonIgnore]
        public ListOfSpeakersViewModel ListOfSpeakers { get; set; }

        /// <summary>
        /// Gets called when a proeprty has been changed. This does not inclide the SpeakerlistId or the Id.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Compares this Speaker to another Speaker by its values.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SpeakerViewModel other)
        {
            if (this.Id != other.Id) return 1;
            if (this.Iso != other.Iso) return 1;
            if (this.Name != other.Name) return 1;
            if (this.OrdnerIndex != other.OrdnerIndex) return 1;
            return 0;
        }

        public int CompareTo(ISpeaker other)
        {
            if (this.Id != other.Id) return 1;
            if (this.Iso != other.Iso) return 1;
            if (this.Name != other.Name) return 1;
            if (this.OrdnerIndex != other.OrdnerIndex) return 1;
            return 0;
        }
    }
}
