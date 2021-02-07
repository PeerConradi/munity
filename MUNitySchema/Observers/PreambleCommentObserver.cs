using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;

namespace MUNity.Observers
{
    public class PreambleCommentObserver
    {
        public event EventHandler<PreambleParagraphCommentTextChangedEventArgs> TextChanged;

        private Comment _comment;

        private PreambleParagraphObserver _paragraphObserver;

        public PreambleCommentObserver(PreambleParagraphObserver paragraphObserver, Comment comment)
        {
            this._comment = comment;
            this._paragraphObserver = paragraphObserver;
            comment.PropertyChanged += Comment_PropertyChanged;
        }

        private void Comment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Comment.Text))
            {
                var eventArgs = new PreambleParagraphCommentTextChangedEventArgs(_paragraphObserver.ResolutionId,
                    _paragraphObserver.ParagraphId, _comment.CommentId, _comment.Text);
                TextChanged?.Invoke(this, eventArgs);
            }
                
        }
    }
}
