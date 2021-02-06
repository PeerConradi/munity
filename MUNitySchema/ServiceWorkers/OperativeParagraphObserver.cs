using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;

namespace MUNity.Observers
{
    /// <summary>
    /// This is a kind of Observer for an operative paragraph that will notify the user if something inside
    /// the Operative Paragraph has changed.
    /// </summary>
    public class OperativeParagraphObserver
    {
        private OperativeSectionObserver _sectionWorker;

        private OperativeParagraph _paragraph;

        private OperativeParagraphObserver (OperativeSectionObserver sectionWorker, OperativeParagraph paragraph)
        {
            _sectionWorker = sectionWorker;
            _paragraph = paragraph;
            paragraph.Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var paragraph in e.NewItems.OfType<OperativeParagraph>())
                {
                    OperativeParagraphObserver.CreateObserver(_sectionWorker, paragraph);
                }
            }
        }

        /// <summary>
        /// Create an instance of this Worker/Observer for a given operative Section
        /// </summary>
        /// <param name="sectionWorker"></param>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static OperativeParagraphObserver CreateObserver (OperativeSectionObserver sectionWorker, OperativeParagraph paragraph)
        {
            return new OperativeParagraphObserver(sectionWorker, paragraph);
        }
    }
}
