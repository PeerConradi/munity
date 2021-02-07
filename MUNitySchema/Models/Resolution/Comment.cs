using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MUNity.Models.Resolution
{
    /// <summary>
    /// A Comment for an operative paragraph or a preamble paragraph to allow a discussion about the paragraphs.
    /// </summary>
    public class Comment : INotifyPropertyChanged
    {
        /// <summary>
        /// The Id of the comment
        /// </summary>
        public string CommentId { get; set; }

        private string _authorName = "";
        /// <summary>
        /// The author name of the Comment
        /// </summary>
        public string AuthorName {
            get => _authorName; 
            set
            {
                _authorName = value;
                NotifyPropertyChanged(nameof(AuthorName));
            } 
        }

        private string _authorId = "";
        /// <summary>
        /// The Id of the author if you can use if the author is logged in.
        /// </summary>
        public string AuthorId {
            get => _authorId; 
            set
            {
                _authorId = value;
                NotifyPropertyChanged(nameof(AuthorId));
            }
        }

        private DateTime _creationDate;
        /// <summary>
        /// The date the comment has been created, you can also use this a last Changed date.
        /// </summary>
        public DateTime CreationDate {
            get => _creationDate; 
            set
            {
                _creationDate = value;
                NotifyPropertyChanged(nameof(CreationDate));
            } 
        }

        private string _title = "";
        /// <summary>
        /// The title of the comment. This can be set and will not be generated automaticaly.
        /// </summary>
        public string Title {
            get => _title; 
            set
            {
                _title = value;
                NotifyPropertyChanged(nameof(Title));
            } 
        }

        private string _text = "";
        /// <summary>
        /// The text of the comment.
        /// </summary>
        public string Text {
            get => _text; 
            set
            {
                _text = value;
                NotifyPropertyChanged(nameof(Text));
            } 
        }

        /// <summary>
        /// Tags that are added to the comment.
        /// </summary>
        public ObservableCollection<CommentTag> Tags { get; set; }

        /// <summary>
        /// List of names that have marked to comment as read.
        /// </summary>
        public ObservableCollection<string> ReadBy { get; set; }

        /// <summary>
        /// Creates a new Instance of a comment for operative paragraphs or preamble paragraphs and give them a new guid.
        /// </summary>
        public Comment()
        {
            CommentId = Guid.NewGuid().ToString();
            Tags = new ObservableCollection<CommentTag>();
            ReadBy = new ObservableCollection<string>();
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

        public void SetTextNoNotifyPropertyChanged(string text)
        {
            this._text = text;
        }
    }
}
