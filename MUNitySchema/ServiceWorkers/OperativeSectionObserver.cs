using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;

namespace MUNity.Observers
{
    /// <summary>
    /// An Observer for the OperativeSection.
    /// This will create more observers for every paragraph.
    /// </summary>
    public class OperativeSectionObserver : IDisposable
    {
        private ResolutionObserver _resolutionWorker;

        private OperativeSection _operativeSection;

        private OperativeSectionObserver(ResolutionObserver resoluionWorker, OperativeSection operativeSection)
        {
            this._resolutionWorker = resoluionWorker;
            this._operativeSection = operativeSection;
            _resolutionWorker.RegisterOperativeSectionWorker(this);
            AppendOperativeSectionEvents(operativeSection);
        }

        private void AppendOperativeSectionEvents(OperativeSection section)
        {
            _operativeSection.Paragraphs.CollectionChanged += Paragraphs_CollectionChanged;
            foreach (var op in _operativeSection.Paragraphs)
            {
                OperativeParagraphObserver.CreateObserver(this, op);
            }
        }

        private void Paragraphs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var newParagraph in e.NewItems.OfType<OperativeParagraph>())
                {
                    OperativeParagraphObserver.CreateObserver(this, newParagraph);
                }
            }
        }

        /// <summary>
        /// Creates an operative section observer
        /// </summary>
        /// <param name="resolutionWorker"></param>
        /// <param name="operativeSection"></param>
        /// <returns></returns>
        public static OperativeSectionObserver CreateWorker(ResolutionObserver resolutionWorker, OperativeSection operativeSection)
        {
            return new OperativeSectionObserver(resolutionWorker, operativeSection);
        }

        /// <summary>
        /// TODO: Disposes the object
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
