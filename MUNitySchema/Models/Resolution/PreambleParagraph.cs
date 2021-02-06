using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{

    /// <summary>
    /// A Preamble Paragraphs. This type of paragraph cannot have amendments or child paragraphs.
    /// </summary>
    public class PreambleParagraph : INotifyPropertyChanged
    {

        /// <summary>
        /// The Id of the Preamble Paragraph.
        /// </summary>
        public string PreambleParagraphId { get; set; }

        private string _text = "";
        /// <summary>
        /// The Text (content) of the paragraph.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyPropertyChanged(nameof(Text));
            }
        }

        public void SetTextNoNotifyPropertyChanged(string text)
        {
            this._text = text;
        }

        private bool _isLocked = false;
        /// <summary>
        /// is the paragraph marked as locked. This will not effect the Text Property you can still change the Text or comments
        /// event if this property is set to true (locked).
        /// </summary>
        public bool IsLocked {
            get => _isLocked;
            set
            {
                _isLocked = value;
                NotifyPropertyChanged(nameof(IsLocked));
            }
        }

        private bool _corrected = false;
        /// <summary>
        /// Marks the paragraph as corrected. Note that this property will still keep its value even if the text is changed.
        /// </summary>
        public bool Corrected {
            get => _corrected; 
            set
            {
                _corrected = value;
                NotifyPropertyChanged(nameof(Corrected));
            }
        }

        /// <summary>
        /// A List of comments for this paragraph. This will become an Observable collection in higher versions.
        /// </summary>
        public ObservableCollection<Comment> Comments { get; set; }

        /// <summary>
        /// Creates a new Preamble paragraph.
        /// </summary>
        public PreambleParagraph()
        {
            PreambleParagraphId = Guid.NewGuid().ToString();
            Comments = new ObservableCollection<Comment>();
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
