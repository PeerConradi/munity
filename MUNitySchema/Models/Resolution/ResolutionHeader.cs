using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// The header and information of a resoltion.
    /// </summary>
    public class ResolutionHeader : INotifyPropertyChanged
    {
        private string _name = "";
        /// <summary>
        /// The name to find the document inside a register or list. This should not be used as the display name
        /// inside the resolution use the topic property for that case.
        /// The name could also be a number you give like: 1244
        /// </summary>
        public string Name {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private string _fullName = "";
        /// <summary>
        /// A longer version of the name for example: United Nations Security Council Resolution 1244
        /// </summary>
        public string FullName {
            get => _fullName;
            set
            {
                _fullName = value;
                NotifyPropertyChanged(nameof(FullName));
            }
        }

        private string _topic;
        /// <summary>
        /// The topic of the resolution. This should be displayed as the header of a resolution.
        /// </summary>
        public string Topic {
            get => _topic; 
            set
            {
                _topic = value;
                NotifyPropertyChanged(nameof(Topic));
            }
        }

        private string _agendaItem = "";
        /// <summary>
        /// The agenda item of the resolution.
        /// </summary>
        public string AgendaItem {
            get => _agendaItem;
            set
            {
                _agendaItem = value;
                NotifyPropertyChanged(nameof(AgendaItem));
            }
        }

        private string _session = "";
        /// <summary>
        /// The session for example: 4011th meeting
        /// or in case of your conference it could be: Day 2/Sess: 1
        /// </summary>
        public string Session {
            get => _session;
            set
            {
                _session = value;
                NotifyPropertyChanged(nameof(Session));
            }
        }

        private string _submitterName = "";
        /// <summary>
        /// The name of the Submitter of the resolution.
        /// </summary>
        public string SubmitterName {
            get => _submitterName; 
            set
            {
                _submitterName = value;
                NotifyPropertyChanged(nameof(SubmitterName));
            }
        }

        private string _committeeName = "";
        /// <summary>
        /// The Name of the committee, for example: The Security Council
        /// </summary>
        public string CommitteeName {
            get => _committeeName; 
            set
            {
                _committeeName = value;
                NotifyPropertyChanged(nameof(CommitteeName));
            }
        }

        /// <summary>
        /// List of the names of supporters for this document.
        /// </summary>
        public ObservableCollection<string> Supporters { get; set; }

        /// <summary>
        /// Creates a new resolution Header.
        /// </summary>
        public ResolutionHeader()
        {
            Supporters = new ObservableCollection<string>();
        }

        /// <summary>
        /// Event that is fired when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Internal Event to fire the Property Changed event.
        /// </summary>
        /// <param name="name"></param>
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
