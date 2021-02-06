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

        private List<PreambleParagraphObserver> _subWorkers = new List<PreambleParagraphObserver>();

        private ResolutionObserver _resolutionWorker;

        private ResolutionPreamble _preamble;

        public string ResolutionId => this._resolutionWorker.Resolution.ResolutionId;

        private PreambleSectionObserver(ResolutionObserver resolutionWorker, ResolutionPreamble preamble)
        {
            _resolutionWorker = resolutionWorker;
            _preamble = preamble;
            preamble.Paragraphs.CollectionChanged += Paragraphs_CollectionChanged;
            foreach(var paragraph in preamble.Paragraphs)
            {
                var paragraphObserver = PreambleParagraphObserver.CreateObserver(this, paragraph);
                paragraphObserver.PreambleTextChanged += (sender, args) => this.ParagraphTextChanged?.Invoke(sender, args);
            }
        }

        /// <summary>
        /// Creates a preamble section observer
        /// </summary>
        /// <param name="resolutionWorker"></param>
        /// <param name="preamble"></param>
        /// <returns></returns>
        public static PreambleSectionObserver CreateWorker(ResolutionObserver resolutionWorker, ResolutionPreamble preamble)
        {
            return new PreambleSectionObserver(resolutionWorker, preamble);
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
                    var paragraphObserver = PreambleParagraphObserver.CreateObserver(this, newItem);
                    paragraphObserver.PreambleTextChanged += (s, args) => this.ParagraphTextChanged?.Invoke(s, args);
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
