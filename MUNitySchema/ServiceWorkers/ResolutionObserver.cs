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

        public event EventHandler<PreambleParagraphAddedEventArgs> PreambleParagraphAdded;

        public event EventHandler<PreambleParagraphTextChangedEventArgs> PreambleParagraphTextChanged;

        /// <summary>
        /// Creates a new Resolution Worker instance.
        /// </summary>
        /// <param name="resolution"></param>
        public ResolutionObserver(Resolution resolution)
        {
            Resolution = resolution;
            if (Resolution.Header != null)
            {
                Resolution.Header.PropertyChanged += Header_PropertyChanged;
            }
            if (Resolution.Preamble != null)
            {
                this.PreambleSectionObserver = PreambleSectionObserver.CreateWorker(this, Resolution.Preamble);
                this.PreambleSectionObserver.ParagraphAdded += (sender, args) => this.PreambleParagraphAdded?.Invoke(sender, args);
                this.PreambleSectionObserver.ParagraphTextChanged += (sender, args) => this.PreambleParagraphTextChanged?.Invoke(sender, args);
            }
            if (Resolution.OperativeSection != null)
            {
                OperativeSectionObserver.CreateWorker(this, Resolution.OperativeSection);
            }
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
