using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;

namespace MUNity.Observers
{

    /// <summary>
    /// Preamble section observer
    /// </summary>
    public class PreambleSectionObserver : IDisposable
    {
        private List<PreambleParagraphObserver> _subWorkers = new List<PreambleParagraphObserver>();

        private ResolutionObserver _resolutionWorker;

        private ResolutionPreamble _preamble;

        private PreambleSectionObserver(ResolutionObserver resolutionWorker, ResolutionPreamble preamble)
        {
            _resolutionWorker = resolutionWorker;
            _preamble = preamble;
            preamble.Paragraphs.CollectionChanged += Paragraphs_CollectionChanged;
            foreach(var paragraph in preamble.Paragraphs)
            {
                PreambleParagraphObserver.CreateObserver(this, paragraph);
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
            this._resolutionWorker.InvokeResolutionChanged();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var newItem in e.NewItems.OfType<PreambleParagraph>())
                {
                    PreambleParagraphObserver.CreateObserver(this, newItem);
                }
            }
        }

        internal void InvokePreambleChanged()
        {
            _resolutionWorker.InvokePreambleChanged();
        }

        internal void InvokePreambleParagraphTextChanged(PreambleParagraph paragraph)
        {
            _resolutionWorker.InvokePreambleParagraphTextChanged(paragraph);
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
