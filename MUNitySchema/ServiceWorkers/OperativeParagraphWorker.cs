using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;

namespace MUNity.ServiceWorkers
{
    public class OperativeParagraphWorker
    {
        private OperativeSectionWorker _sectionWorker;

        private OperativeParagraph _paragraph;

        private OperativeParagraphWorker (OperativeSectionWorker sectionWorker, OperativeParagraph paragraph)
        {
            _sectionWorker = sectionWorker;
            _paragraph = paragraph;
            paragraph.PropertyChanged += Paragraph_PropertyChanged;
            paragraph.Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _sectionWorker.InvokeParagraphChanged(this._paragraph);
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var paragraph in e.NewItems.OfType<OperativeParagraph>())
                {
                    OperativeParagraphWorker.CreateWorker(_sectionWorker, paragraph);
                }
            }
        }

        private void Paragraph_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _sectionWorker.InvokeParagraphChanged(this._paragraph);
        }

        public static OperativeParagraphWorker CreateWorker (OperativeSectionWorker sectionWorker, OperativeParagraph paragraph)
        {
            return new OperativeParagraphWorker(sectionWorker, paragraph);
        }
    }
}
