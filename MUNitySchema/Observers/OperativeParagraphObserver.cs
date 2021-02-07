using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;
using MUNity.Models.Resolution.EventArguments;

namespace MUNity.Observers
{
    /// <summary>
    /// This is a kind of Observer for an operative paragraph that will notify the user if something inside
    /// the Operative Paragraph has changed.
    /// </summary>
    public class OperativeParagraphObserver
    {
        public event EventHandler<OperativeParagraphChangedEventArgs> ParagraphChanged;

        private OperativeSectionObserver _sectionWorker;

        private OperativeParagraph _paragraph;

        public string ResolutionId => _sectionWorker?.ResolutionId;

        public OperativeParagraphObserver (OperativeSectionObserver sectionWorker, OperativeParagraph paragraph)
        {
            _sectionWorker = sectionWorker;
            _paragraph = paragraph;
            paragraph.Children.CollectionChanged += Children_CollectionChanged;
            paragraph.PropertyChanged += Paragraph_PropertyChanged;
        }

        private void Paragraph_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.ParagraphChanged?.Invoke(this, new OperativeParagraphChangedEventArgs(ResolutionId, _paragraph));
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var paragraph in e.NewItems.OfType<OperativeParagraph>())
                {
                    var subParagraphObserver = new OperativeParagraphObserver(_sectionWorker, paragraph);
                    subParagraphObserver.ParagraphChanged += (sdr, args) => this.ParagraphChanged?.Invoke(sdr, args);
                }
            }
        }
    }
}
