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
            _operativeSection.AddAmendments.CollectionChanged += AddAmendments_CollectionChanged;
            _operativeSection.ChangeAmendments.CollectionChanged += ChangeAmendments_CollectionChanged;
            _operativeSection.DeleteAmendments.CollectionChanged += DeleteAmendments_CollectionChanged;
            _operativeSection.MoveAmendments.CollectionChanged += MoveAmendments_CollectionChanged;
            foreach (var op in _operativeSection.Paragraphs)
            {
                OperativeParagraphObserver.CreateObserver(this, op);
            }
        }

        private void MoveAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _resolutionWorker.InvokeOperativeSectionChanged();
        }

        private void DeleteAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _resolutionWorker.InvokeOperativeSectionChanged();
        }

        private void ChangeAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _resolutionWorker.InvokeOperativeSectionChanged();
        }

        private void AddAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _resolutionWorker.InvokeOperativeSectionChanged();
        }

        private void AddAmendmentPropertyChanged(AbstractAmendment amendment)
        {
            amendment.PropertyChanged += Amendment_PropertyChanged;
        }

        private void Amendment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _resolutionWorker.InvokeOperativeSectionChanged();
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

            this._resolutionWorker.InvokeOperativeSectionChanged();
        }


        internal void InvokeParagraphChanged(OperativeParagraph paragraph)
        {
            this._resolutionWorker.InvokeOperativeParagraphChanged(paragraph);
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
