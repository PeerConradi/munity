using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;

namespace MUNity.Observers
{
    /// <summary>
    /// An observer for a preamble paragraph
    /// </summary>
    public class PreambleParagraphObserver
    {
        private PreambleParagraph _preambleParagraph;

        private PreambleSectionObserver _sectionWorker;

        private PreambleParagraphObserver(PreambleSectionObserver sectionWorker, PreambleParagraph paragraph)
        {
            _preambleParagraph = paragraph;
            _sectionWorker = sectionWorker;
            _sectionWorker.RegisterSubWorker(this);
            paragraph.PropertyChanged += Paragraph_PropertyChanged;
            paragraph.Comments.CollectionChanged += Comments_CollectionChanged;
            if (paragraph.Comments != null)
            {
                foreach (var comment in paragraph.Comments)
                {
                    comment.PropertyChanged += Comment_PropertyChanged;
                }
            }
            
        }

        private void Comment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this._sectionWorker.InvokePreambleChanged();
        }

        /// <summary>
        /// Creates a new instance of an observer for a preamble paragraph
        /// </summary>
        /// <param name="sectionWorker"></param>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public static PreambleParagraphObserver CreateObserver(PreambleSectionObserver sectionWorker, PreambleParagraph paragraph)
        {
            return new PreambleParagraphObserver(sectionWorker, paragraph);
        }

        private void Comments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(var paragraph in e.NewItems.OfType<PreambleParagraph>())
                {
                    var worker = new PreambleParagraphObserver(_sectionWorker, paragraph);
                    
                }
            }
            _sectionWorker.InvokePreambleChanged();
        }

        private void Paragraph_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PreambleParagraph.Text))
            {
                _sectionWorker.InvokePreambleParagraphTextChanged(this._preambleParagraph);
            }
            
        }
    }
}
