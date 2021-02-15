using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;
using MUNity.Models.Resolution.EventArguments;

namespace MUNity.Observers
{
    /// <summary>
    /// An observer for a preamble paragraph
    /// </summary>
    public class PreambleParagraphObserver
    {
        public event EventHandler<PreambleParagraphTextChangedEventArgs> PreambleTextChanged;

        public event EventHandler<PreambleParagraphCommentTextChangedEventArgs> CommentTextChanged;

        private PreambleParagraph _preambleParagraph;

        private PreambleSectionObserver _sectionWorker;

        public string ResolutionId => _sectionWorker?.ResolutionId;

        public string ParagraphId => _preambleParagraph?.PreambleParagraphId;

        public PreambleParagraphObserver(PreambleSectionObserver sectionWorker, PreambleParagraph paragraph)
        {
            _preambleParagraph = paragraph;
            _sectionWorker = sectionWorker;
            _sectionWorker.RegisterSubWorker(this);
            paragraph.PropertyChanged += Paragraph_PropertyChanged;
            //paragraph.Comments.CollectionChanged += Comments_CollectionChanged;
            //if (paragraph.Comments != null)
            //{
            //    foreach (var comment in paragraph.Comments)
            //    {
            //        var commentObserver = new PreambleCommentObserver(this, comment);
            //        commentObserver.TextChanged += (sndr, args) => this.CommentTextChanged?.Invoke(sndr, args);
            //    }
            //}
        }

        /// <summary>
        /// Creates a new instance of an observer for a preamble paragraph
        /// </summary>
        /// <param name="sectionWorker"></param>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        [Obsolete("Use the normal constructor!")]
        public static PreambleParagraphObserver CreateObserver(PreambleSectionObserver sectionWorker, PreambleParagraph paragraph)
        {
            return new PreambleParagraphObserver(sectionWorker, paragraph);
        }

        private void Paragraph_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PreambleParagraph.Text))
            {
                this.PreambleTextChanged?.Invoke(this, new PreambleParagraphTextChangedEventArgs(ResolutionId, this._preambleParagraph.PreambleParagraphId, this._preambleParagraph.Text));
            }
            // This is an implementation to allow to unify the comment for now. There is the idea to 
            // move to multiple comments for one paragraph in the future, when this takes place remember 
            // to remove this part!
            else if (e.PropertyName == nameof(PreambleParagraph.Comment))
            {
                this.CommentTextChanged?.Invoke(this, new PreambleParagraphCommentTextChangedEventArgs(ResolutionId, this._preambleParagraph.PreambleParagraphId, "", this._preambleParagraph.Comment));
            }
            
        }
    }
}
