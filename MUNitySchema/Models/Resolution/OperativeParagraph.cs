using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{

    /// <summary>
    /// An operative Paragraph is a paragraph inside the operative section. You can create amendments for this type of
    /// paragraph and they can also have child paragraphs.
    /// </summary>
    public class OperativeParagraph : INotifyPropertyChanged
    {

        /// <summary>
        /// The Id of the operativee Paragraph.
        /// </summary>
        public string OperativeParagraphId { get; set; }

        private string _name = "";
        /// <summary>
        /// The name of the paragraph if you want to identify it by a given name.
        /// </summary>
        public string Name {
            get => _name; 
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private bool _isLocked = false;
        /// <summary>
        /// Is the paragraph marked as locked. This will not effect the logic you can still submit amendments
        /// or apply amendments to it. This may change in future implementations!
        /// </summary>
        public bool IsLocked {
            get => _isLocked; 
            set
            {
                _isLocked = value;
                NotifyPropertyChanged(nameof(IsLocked));
            }
        }

        private bool _isVirtual = false;
        /// <summary>
        /// Virtual is true when the Operative Paragraph comes from an AddAmendment and doesn't really count as an
        /// paragraph or if it is from a move amendment and is the paragraph where the orignal should be moved to.
        /// </summary>
        public bool IsVirtual {
            get => _isVirtual; 
            set
            {
                _isVirtual = value;
                NotifyPropertyChanged(nameof(IsVirtual));
            } 
        }

        private string _text = "";

        
        /// <summary>
        /// The text of the operative Paragraph.
        /// </summary>
        public string Text
        {
            get => this._text;
            set
            {
                if (_text == value) return;
                var oldText = this._text;
                this._text = value;
                this.NotifyPropertyChanged(nameof(Text));
            }
        }

        private bool _visible = true;
        /// <summary>
        /// Is the operative Paragraph visible inside the views.
        /// </summary>
        public bool Visible {
            get => _visible; 
            set
            {
                _visible = value;
                NotifyPropertyChanged(nameof(Visible));
            }
        }

        private bool _corrected = false;
        /// <summary>
        /// Is the operative paragraph marked as corrected. Note that this
        /// does not interact with any form of logic, if the Text is changed it will
        /// still be marked as corrected.
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
        /// Child paragraphs of this operative paragraph.
        /// </summary>
        public ObservableCollection<OperativeParagraph> Children { get; set; }

        /// <summary>
        /// The comments of this operative paragraph.
        /// </summary>
        public ObservableCollection<Comment> Comments { get; set; }

        /// <summary>
        /// Has a property of the operative paragraph changed. This is not implemented complitly by now!
        /// </summary>

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates a new Operative Paragraph and will give it an id.
        /// </summary>
        /// <param name="text"></param>
        public OperativeParagraph(string text = "")
        {
            this.Text = text;
            this.OperativeParagraphId = Guid.NewGuid().ToString();
            this.Children = new ObservableCollection<OperativeParagraph>();
            this.Comments = new ObservableCollection<Comment>();
        }
    }
}
