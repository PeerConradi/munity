using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;
using MUNity.Models.Resolution.EventArguments;

namespace MUNity.Observers
{

    /// <summary>
    /// Preamble section observer
    /// </summary>
    public class PreambleSectionObserver : IDisposable
    {
        public event EventHandler<PreambleParagraphAddedEventArgs> ParagraphAdded;

        public event EventHandler<PreambleParagraphTextChangedEventArgs> ParagraphTextChanged;

        public event EventHandler<PreambleParagraphCommentTextChangedEventArgs> ParagraphCommentTextChanged;

        public event EventHandler<PreambleParagraphRemovedEventArgs> ParagraphRemoved;

        private List<PreambleParagraphObserver> _subWorkers = new List<PreambleParagraphObserver>();

        private ResolutionObserver _resolutionWorker;

        private ResolutionPreamble _preamble;

        public string ResolutionId => this._resolutionWorker.Resolution.ResolutionId;

        public PreambleSectionObserver(ResolutionObserver resolutionWorker, ResolutionPreamble preamble)
        {
            _resolutionWorker = resolutionWorker;
            _preamble = preamble;
            preamble.Paragraphs.CollectionChanged += Paragraphs_CollectionChanged;
            foreach(var paragraph in preamble.Paragraphs)
            {
                var paragraphObserver = new PreambleParagraphObserver(this, paragraph);
                paragraphObserver.PreambleTextChanged += (sender, args) => this.ParagraphTextChanged?.Invoke(sender, args);
                paragraphObserver.CommentTextChanged += (sender, args) => this.ParagraphCommentTextChanged?.Invoke(sender, args);
            }
        }

        private void Paragraphs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var newItems = e.NewItems.OfType<PreambleParagraph>().ToList();
                if (newItems.Count == 1)
                {
                    this.ParagraphAdded?.Invoke(this, new PreambleParagraphAddedEventArgs(this._resolutionWorker.Resolution.ResolutionId, newItems[0]));
                }
                else
                {
                    throw new NotImplementedException("There is no way you added a range, how did you do that?");
                }
                foreach(var newItem in newItems)
                {
                    var paragraphObserver = new PreambleParagraphObserver(this, newItem);
                    paragraphObserver.PreambleTextChanged += (s, args) => this.ParagraphTextChanged?.Invoke(s, args);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var removedItem = e.OldItems.OfType<PreambleParagraph>().SingleOrDefault();
                if (removedItem != null)
                {
                    this._subWorkers.RemoveAll(n => n.ParagraphId == removedItem.PreambleParagraphId);
                    this.ParagraphRemoved?.Invoke(this, new PreambleParagraphRemovedEventArgs(ResolutionId, removedItem.PreambleParagraphId));
                }
            }
        }

        /// <summary>
        /// Disposes the Observer
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void RegisterSubWorker(PreambleParagraphObserver preambleParagraphWorker)
        {
            _subWorkers.Add(preambleParagraphWorker);
        }
    }
}
