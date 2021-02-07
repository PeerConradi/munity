using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;
using MUNity.Models.Resolution.EventArguments;

namespace MUNity.Observers
{
    /// <summary>
    /// An Observer for the OperativeSection.
    /// This will create more observers for every paragraph.
    /// </summary>
    public class OperativeSectionObserver : IDisposable
    {
        public event EventHandler<OperativeSectionChangedEventArgs> SectionChanged;

        public event EventHandler<OperativeParagraphChangedEventArgs> OperativeParagraphChanged;

        private ResolutionObserver _resolutionWorker;

        private OperativeSection _operativeSection;

        public string ResolutionId => _resolutionWorker?.Resolution?.ResolutionId;

        public OperativeSectionObserver(ResolutionObserver resoluionWorker, OperativeSection operativeSection)
        {
            this._resolutionWorker = resoluionWorker;
            this._operativeSection = operativeSection;
            _resolutionWorker.RegisterOperativeSectionWorker(this);
            _operativeSection.Paragraphs.CollectionChanged += Paragraphs_CollectionChanged;
            _operativeSection.AddAmendments.CollectionChanged += AddAmendments_CollectionChanged;
            _operativeSection.ChangeAmendments.CollectionChanged += ChangeAmendments_CollectionChanged;
            _operativeSection.DeleteAmendments.CollectionChanged += DeleteAmendments_CollectionChanged;
            _operativeSection.MoveAmendments.CollectionChanged += MoveAmendments_CollectionChanged;
            AppendOperativeSectionEvents(operativeSection.Paragraphs);
        }

        private void MoveAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.SectionChanged?.Invoke(this, new OperativeSectionChangedEventArgs(ResolutionId, _operativeSection));
        }

        private void DeleteAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.SectionChanged?.Invoke(this, new OperativeSectionChangedEventArgs(ResolutionId, _operativeSection));
        }

        private void ChangeAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.SectionChanged?.Invoke(this, new OperativeSectionChangedEventArgs(ResolutionId, _operativeSection));
        }

        private void AddAmendments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.SectionChanged?.Invoke(this, new OperativeSectionChangedEventArgs(ResolutionId, _operativeSection));
        }

        private void AppendOperativeSectionEvents(IEnumerable<OperativeParagraph> paragraphs)
        {
            foreach (var op in paragraphs)
            {
                var paragraphObserver = new OperativeParagraphObserver(this, op);
                paragraphObserver.ParagraphChanged += (sender, args) => this.OperativeParagraphChanged?.Invoke(sender, args);
            }
        }

        private void Paragraphs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                AppendOperativeSectionEvents(e.NewItems.OfType<OperativeParagraph>());
            }
            this.SectionChanged?.Invoke(this, new OperativeSectionChangedEventArgs(ResolutionId, _operativeSection));
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
