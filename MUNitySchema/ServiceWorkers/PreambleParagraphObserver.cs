using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;
using MUNity.Models.Resolution.EventArguments;

namespace MUNity.Observers
{
    /// <summary>
    /// An observer for a preamble paragraph
    /// </summary>
    public class PreambleParagraphObserver
    {
        public event EventHandler<PreambleParagraphTextChangedEventArgs> PreambleTextChanged;

        private PreambleParagraph _preambleParagraph;

        private PreambleSectionObserver _sectionWorker;

        public string ResolutionId => _sectionWorker?.ResolutionId;

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
                    
                }
            }
            
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
            
        }

        private void Paragraph_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PreambleParagraph.Text))
            {
                var resolutionId = this._sectionWorker.ResolutionId;
                this.PreambleTextChanged?.Invoke(this, new PreambleParagraphTextChangedEventArgs(resolutionId, this._preambleParagraph.PreambleParagraphId, this._preambleParagraph.Text));
            }
            
        }
    }
}
