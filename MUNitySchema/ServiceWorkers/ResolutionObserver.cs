using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;
using MUNity.Models.Resolution.EventArguments;

namespace MUNity.Observers
{
    /// <summary>
    /// The Resolution worker tracks the resolution and invokes different events if something has changed.
    /// The worker is creating sub workers. Note that depending on the case the same action could call multiple 
    /// events. For example a text change of an operative paragraph will call the OperativeParagraphTextChanged event, then 
    /// then OperativeSectionChangedEvent and last the ResolutionChangedEvent.
    /// </summary>
    public class ResolutionObserver
    {
        public Resolution Resolution { get; private set; }

        public PreambleSectionObserver PreambleSectionObserver { get; private set; }

        public OperativeSectionObserver OperativeSectionObserver { get; private set; }


        public EventHandler<Resolution> ResolutionChanged;

        public event EventHandler<HeaderStringPropChangedEventArgs> HeaderNameChanged;

        public event EventHandler<HeaderStringPropChangedEventArgs> HeaderFullNameChanged;

        public event EventHandler<HeaderStringPropChangedEventArgs> HeaderTopicChanged;

        public event EventHandler<HeaderStringPropChangedEventArgs> HeaderAgendaItemChanged;

        public event EventHandler<HeaderStringPropChangedEventArgs> HeaderSessionChanged;

        public event EventHandler<HeaderStringPropChangedEventArgs> HeaderSubmitterChanged;

        public event EventHandler<HeaderStringPropChangedEventArgs> HeaderCommitteeChanged;

        /// <summary>
        /// Delegate for changes inside the resolution header
        /// </summary>
        /// <param name="sender"></param>
        public delegate void OnResolutionHeaderChanged(Resolution sender);


        /// <summary>
        /// Delegate when the preamble or something inside the preamble has changed.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void OnPreambleChanged(Resolution sender);

        /// <summary>
        /// Somehting inside the Preamble has changed. This could be a new Paragraph, a paragraph has been removed or the text of a paragraph was changed.
        /// </summary>
        public event OnPreambleChanged PreambleChanged;

        /// <summary>
        /// delegate when a preamble paragraph has changed
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="paragraph"></param>
        public delegate void OnPreambleParagraphChanged(Resolution resolution, PreambleParagraph paragraph);

        /// <summary>
        /// Event gets called when something inside the PreambleParagraph has changed. This could be the text or comment
        /// </summary>
        public event OnPreambleParagraphChanged PreambleParagraphChanged;

        /// <summary>
        /// Delegate for the Preamble Changed event
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="paragraph"></param>
        public delegate void OnPreambleParagraphCommentsChanged(Resolution resolution, PreambleParagraph paragraph);

        /// <summary>
        /// Gets called when the comments of the paragraph have changed.
        /// </summary>
        public event OnPreambleParagraphCommentsChanged PreambleParagraphCommentsChanged;

        public event EventHandler<PreambleParagraphTextChangedEventArgs> PreambleParagraphTextChanged;

        /// <summary>
        /// Delegate for changes inside the Operative Section. This could mean that a new paragraph was added, moved or removed,
        /// aswell as the text of a paragraph has changed, comments changed or an amendment has been added.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void OnOperativeSectionChanged(Resolution sender);

        /// <summary>
        /// Gets called when the operative section has changed. This could also be when a paragraph was added, moved, removed and something
        /// with the amendments has been done.
        /// </summary>
        public event OnOperativeSectionChanged OperativeSectionChanged;


        /// <summary>
        /// Event delagate when something insde the OperativeParagraph has changed.
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="paragraph"></param>
        public delegate void OnOperativeParagraphChanged(Resolution resolution, OperativeParagraph paragraph);

        /// <summary>
        /// Gets called when the operative paragraph has changed.
        /// </summary>
        public event OnOperativeParagraphChanged OperativeParagraphChanged;

        /// <summary>
        /// Creates a new Resolution Worker instance.
        /// </summary>
        /// <param name="resolution"></param>
        public ResolutionObserver(Resolution resolution)
        {
            Resolution = resolution;
            Resolution.PropertyChanged += _resolution_PropertyChanged;
            if (Resolution.Header != null)
            {
                Resolution.Header.PropertyChanged += Header_PropertyChanged;
            }
            if (Resolution.Preamble != null)
            {
                PreambleSectionObserver.CreateWorker(this, Resolution.Preamble);
            }
            if (Resolution.OperativeSection != null)
            {
                OperativeSectionObserver.CreateWorker(this, Resolution.OperativeSection);
            }
        }

        internal void InvokePreambleParagraphTextChanged(PreambleParagraph paragraph)
        {
            PreambleParagraphTextChanged?.Invoke(this, new PreambleParagraphTextChangedEventArgs(Resolution.ResolutionId, paragraph.PreambleParagraphId, paragraph.Text));
            this.InvokePreambleChanged();
        }

        internal void InvokePreambleCommentsChanged(PreambleParagraph paragraph)
        {
            PreambleParagraphCommentsChanged?.Invoke(this.Resolution, paragraph);
            PreambleParagraphChanged?.Invoke(this.Resolution, paragraph);
        }

        internal void InvokeResolutionChanged()
        {
            //ResolutionChanged?.Invoke(this.Resolution);
        }

        internal void InvokeOperativeParagraphChanged(OperativeParagraph paragraph)
        {
            OperativeParagraphChanged?.Invoke(this.Resolution, paragraph);
            InvokeResolutionChanged();
        }

        internal void InvokeOperativeSectionChanged()
        {
            OperativeSectionChanged?.Invoke(this.Resolution);
            //ResolutionChanged?.Invoke(this.Resolution);
        }

        internal void InvokePreambleChanged()
        {
            PreambleChanged?.Invoke(this.Resolution);
            //ResolutionChanged?.Invoke(this.Resolution);
        }

        private void Header_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Resolution.Header.Name))
                HeaderNameChanged?.Invoke(this, new HeaderStringPropChangedEventArgs(Resolution.ResolutionId, Resolution.Header.Name));

            if (e.PropertyName == nameof(Resolution.Header.FullName))
                HeaderFullNameChanged?.Invoke(this, new HeaderStringPropChangedEventArgs(Resolution.ResolutionId, Resolution.Header.FullName));

            if (e.PropertyName == nameof(Resolution.Header.Topic))
                HeaderTopicChanged?.Invoke(this, new HeaderStringPropChangedEventArgs(Resolution.ResolutionId, Resolution.Header.Topic));

            if (e.PropertyName == nameof(Resolution.Header.AgendaItem))
                HeaderAgendaItemChanged?.Invoke(this, new HeaderStringPropChangedEventArgs(Resolution.ResolutionId, Resolution.Header.AgendaItem));

            if (e.PropertyName == nameof(Resolution.Header.Session))
                HeaderSessionChanged?.Invoke(this, new HeaderStringPropChangedEventArgs(this.Resolution.ResolutionId, Resolution.Header.Session));

            if (e.PropertyName == nameof(Resolution.Header.SubmitterName))
                HeaderSubmitterChanged?.Invoke(this, new HeaderStringPropChangedEventArgs(this.Resolution.ResolutionId, Resolution.Header.SubmitterName));

            if (e.PropertyName == nameof(Resolution.Header.CommitteeName))
                HeaderCommitteeChanged?.Invoke(this, new HeaderStringPropChangedEventArgs(Resolution.ResolutionId, Resolution.Header.CommitteeName));
            //ResolutionChanged?.Invoke(this.Resolution);
        }

        /// <summary>
        /// Call when a property inside the resolution has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _resolution_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //ResolutionChanged?.Invoke(this.Resolution);
            if (e.PropertyName == nameof(Models.Resolution.Resolution.Header))
            {
                
            }
            else if (e.PropertyName == nameof(Models.Resolution.Resolution.Preamble))
            {
                PreambleChanged?.Invoke(this.Resolution);
            }
        }

        /// <summary>
        /// Registers a new Pramble Observer
        /// </summary>
        /// <param name="worker"></param>
        public void RegisterPreambleWorker(PreambleSectionObserver worker)
        {
            if (PreambleSectionObserver != null)
            {
                PreambleSectionObserver.Dispose();
            }
            this.PreambleSectionObserver = worker;
        }

        /// <summary>
        /// Registers a new Operative Section observer
        /// </summary>
        /// <param name="worker"></param>
        public void RegisterOperativeSectionWorker(OperativeSectionObserver worker)
        {
            if (OperativeSectionObserver != null)
                OperativeSectionObserver.Dispose();
            OperativeSectionObserver = worker;
        }
    }
}
