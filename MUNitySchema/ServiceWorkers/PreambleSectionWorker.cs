using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;

namespace MUNity.ServiceWorkers
{
    public class PreambleSectionWorker : IDisposable
    {
        private List<PreambleParagraphWorker> _subWorkers = new List<PreambleParagraphWorker>();

        private ResolutionWorker _resolutionWorker;

        private ResolutionPreamble _preamble;

        private PreambleSectionWorker(ResolutionWorker resolutionWorker, ResolutionPreamble preamble)
        {
            _resolutionWorker = resolutionWorker;
            _preamble = preamble;
            preamble.Paragraphs.CollectionChanged += Paragraphs_CollectionChanged;
            foreach(var paragraph in preamble.Paragraphs)
            {
                PreambleParagraphWorker.CreateWorker(this, paragraph);
            }
        }

        public static PreambleSectionWorker CreateWorker(ResolutionWorker resolutionWorker, ResolutionPreamble preamble)
        {
            return new PreambleSectionWorker(resolutionWorker, preamble);
        }

        private void Paragraphs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this._resolutionWorker.InvokeResolutionChanged();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var newItem in e.NewItems.OfType<PreambleParagraph>())
                {
                    PreambleParagraphWorker.CreateWorker(this, newItem);
                }
            }
        }

        public void InvokePreambleChanged()
        {
            _resolutionWorker.InvokePreambleChanged();
        }

        public void InvokePreambleParagraphTextChanged(PreambleParagraph paragraph)
        {
            _resolutionWorker.InvokePreambleParagraphTextChanged(paragraph);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void RegisterSubWorker(PreambleParagraphWorker preambleParagraphWorker)
        {
            _subWorkers.Add(preambleParagraphWorker);
        }
    }
}
