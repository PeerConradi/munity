using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Models.Resolution;
using System.Linq;

namespace MUNity.ServiceWorkers
{
    /// <summary>
    /// The Resolution worker tracks the resolution and invokes different events if something has changed.
    /// The worker is creating sub workers. Note that depending on the case the same action could call multiple 
    /// events. For example a text change of an operative paragraph will call the OperativeParagraphTextChanged event, then 
    /// then OperativeSectionChangedEvent and last the ResolutionChangedEvent.
    /// </summary>
    public class ResolutionWorker
    {
        private Resolution _resolution;

        private PreambleSectionWorker _preambleSectionWorker;

        private OperativeSectionWorker _operativeSectionWorker;

        /// <summary>
        /// A Delegate if the Resolution has changed.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void OnResolutionChanged(Resolution sender);

        /// <summary>
        /// Event gets fired when anything inside the Resolution has changed.
        /// Mutliple Sources: This event could be called multiple times for the same reason because when an
        /// amendment is accepted an amendment gets removed and something within an operative paragraph gets called.
        /// </summary>
        public event OnResolutionChanged ResolutionChanged;

        /// <summary>
        /// Delegate for changes inside the resolution header
        /// </summary>
        /// <param name="sender"></param>
        public delegate void OnResolutionHeaderChanged(Resolution sender);

        /// <summary>
        /// Gets called when the whole Header is replaced or something inside the header
        /// has been changed.
        /// </summary>
        public event OnResolutionHeaderChanged HeaderChanged;


        /// <summary>
        /// Delegate when the preamble or something inside the preamble has changed.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void OnPreambleChanged(Resolution sender);

        /// <summary>
        /// Somehting inside the Preamble has changed. This could be a new Paragraph, a paragraph has been removed or the text of a paragraph was changed.
        /// </summary>
        public event OnPreambleChanged PreambleChanged;

        public delegate void OnPreambleParagraphChanged(Resolution resolution, PreambleParagraph paragraph);

        public event OnPreambleParagraphChanged PreambleParagraphChanged;

        public delegate void OnPreambleParagraphCommentsChanged(Resolution resolution, PreambleParagraph paragraph);

        public event OnPreambleParagraphCommentsChanged PreambleParagraphCommentsChanged;

        /// <summary>
        /// Delegate if the Text proeprty of a preamble paragraph has changed.
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="paragraph"></param>
        public delegate void OnPreambleParagraphTextChanged(Resolution resolution, PreambleParagraph paragraph);

        /// <summary>
        /// Gets called when the Text Property of a preamble paragraph has changed.
        /// </summary>
        public event OnPreambleParagraphTextChanged PreambleParagraphTextChanged;

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

        public delegate void OnOperativeParagraphChanged(Resolution resolution, OperativeParagraph paragraph);

        public event OnOperativeParagraphChanged OperativeParagraphChanged;

        /// <summary>
        /// Creates a new Resolution Worker instance.
        /// </summary>
        /// <param name="resolution"></param>
        public ResolutionWorker(Resolution resolution)
        {
            _resolution = resolution;
            _resolution.PropertyChanged += _resolution_PropertyChanged;
            if (_resolution.Header != null)
            {
                _resolution.Header.PropertyChanged += Header_PropertyChanged;
            }
            if (_resolution.Preamble != null)
            {
                PreambleSectionWorker.CreateWorker(this, _resolution.Preamble);
            }
            if (_resolution.OperativeSection != null)
            {
                OperativeSectionWorker.CreateWorker(this, _resolution.OperativeSection);
            }
        }

        public void InvokePreambleParagraphTextChanged(PreambleParagraph paragraph)
        {
            PreambleParagraphTextChanged?.Invoke(this._resolution, paragraph);
            this.InvokePreambleChanged();
        }

        public void InvokePreambleCommentsChanged(PreambleParagraph paragraph)
        {
            PreambleParagraphCommentsChanged?.Invoke(this._resolution, paragraph);
            PreambleParagraphChanged?.Invoke(this._resolution, paragraph);
        }

        public void InvokeResolutionChanged()
        {
            ResolutionChanged?.Invoke(this._resolution);
        }

        public void InvokeOperativeParagraphChanged(OperativeParagraph paragraph)
        {
            OperativeParagraphChanged?.Invoke(this._resolution, paragraph);
            InvokeResolutionChanged();
        }

        public void InvokeOperativeSectionChanged()
        {
            OperativeSectionChanged?.Invoke(this._resolution);
            ResolutionChanged?.Invoke(this._resolution);
        }

        public void InvokePreambleChanged()
        {
            PreambleChanged?.Invoke(this._resolution);
            ResolutionChanged?.Invoke(this._resolution);
        }

        private void Header_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            HeaderChanged?.Invoke(this._resolution);
            ResolutionChanged?.Invoke(this._resolution);
        }

        /// <summary>
        /// Call when a property inside the resolution has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _resolution_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ResolutionChanged?.Invoke(this._resolution);
            if (e.PropertyName == nameof(Resolution.Header))
            {
                HeaderChanged?.Invoke(this._resolution);
            }
            else if (e.PropertyName == nameof(Resolution.Preamble))
            {
                PreambleChanged?.Invoke(this._resolution);
            }
        }

        public void RegisterPreambleWorker(PreambleSectionWorker worker)
        {
            if (_preambleSectionWorker != null)
            {
                _preambleSectionWorker.Dispose();
            }
            this._preambleSectionWorker = worker;
        }

        public void RegisterOperativeSectionWorker(OperativeSectionWorker worker)
        {
            if (_operativeSectionWorker != null)
                _operativeSectionWorker.Dispose();
            _operativeSectionWorker = worker;
        }
    }
}
