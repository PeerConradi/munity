using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// A tag for a comment that could display: importand, question etc.
    /// </summary>
    public class CommentTag : INotifyPropertyChanged
    {
        /// <summary>
        /// To Identify the Tag later
        /// </summary>
        public string Id { get; set; }

        private string _type = "default";
        /// <summary>
        /// A Tag has a type, for example: success, danger, error etc.
        /// The types are inside a string for more felxibility. The Start Type (default type) is
        /// "default".
        /// </summary>
        public string Type {
            get => _type; 
            set
            {
                _type = value;
                NotifyPropertyChanged(nameof(Type));
            }
        }

        private string _text = "";
        /// <summary>
        /// The Text of the Tag, should be short but is not limited by default.
        /// </summary>
        public string Text {
            get => _type; 
            set
            {
                _text = value;
                NotifyPropertyChanged(nameof(Text));
            } 
        }

        /// <summary>
        /// Creates a new Comment Tag and will give it an id.
        /// </summary>
        public CommentTag()
        {
            this.Id = Guid.NewGuid().ToString();
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
