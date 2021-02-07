using MUNity.Observers;
using MUNityClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Models.Resolution.EventArguments;

namespace MUNityClient.ViewModel.ViewModelLogic
{
    /// <summary>
    /// Turns event thrown by an ResolutionObserver into calls for the MUNity API.
    /// 
    /// To use this pass the observer and an instance of the IResolutionService that should be used.
    /// </summary>
    public class ResolutionObserverToServiceCalls
    {
        private IResolutionService _service;

        private ResolutionViewModel _viewModel;

        public ResolutionObserverToServiceCalls(ResolutionViewModel viewModel, ResolutionObserver observer, IResolutionService service)
        {
            if (observer == null || service == null) throw new ArgumentNullException();
            this._viewModel = viewModel;
            this._service = service;
            // Header
            observer.HeaderNameChanged += Observer_HeaderNameChanged;
            observer.HeaderFullNameChanged += Observer_HeaderFullNameChanged;
            observer.HeaderTopicChanged += Observer_HeaderTopicChanged;
            observer.HeaderAgendaItemChanged += Observer_HeaderAgendaItemChanged;
            observer.HeaderSessionChanged += Observer_HeaderSessionChanged;
            observer.HeaderSubmitterChanged += Observer_HeaderSubmitterChanged;
            observer.HeaderCommitteeChanged += Observer_HeaderCommitteeChanged;
            // TODO: Handler for changes of supporters

            // Preamble
            observer.PreambleParagraphAdded += Observer_PreambleParagraphAdded;
            observer.PreambleParagraphTextChanged += Observer_PreambleParagraphTextChanged;
            observer.PreambleParagraphCommentTextChanged += Observer_PreambleParagraphCommentTextChanged;
            observer.PreambleParagraphRemoved += Observer_PreambleParagraphRemoved;

            // Operative Section
            observer.OperativeSectionChanged += Observer_OperativeSectionChanged;
            observer.OperativeParagraphChanged += Observer_OperativeParagraphChanged;
        }

        private void GenerateAndSaveTan(ResolutionEventArgs args)
        {
            args.Tan = _service.GenerateTan();
            this._viewModel.IgnoreTransactions.Add(args.Tan);
        }

        private void Observer_OperativeParagraphChanged(object sender, OperativeParagraphChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateOperativeParagraph(e).ConfigureAwait(false);
        }

        private void Observer_OperativeSectionChanged(object sender, OperativeSectionChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateOperativeSection(e).ConfigureAwait(false);
        }

        private void Observer_PreambleParagraphRemoved(object sender, PreambleParagraphRemovedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.DeleteResolutionPreambleParagraph(e).ConfigureAwait(false);
        }

        private void Observer_PreambleParagraphCommentTextChanged(object sender, PreambleParagraphCommentTextChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.ResolutionPreambleParagraphCommentTextChanged(e).ConfigureAwait(false);
        }

        private void Observer_PreambleParagraphTextChanged(object sender, PreambleParagraphTextChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.ResolutionPreambleParagraphTextChanged(e).ConfigureAwait(false);
        }

        private void Observer_PreambleParagraphAdded(object sender, PreambleParagraphAddedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.ResolutionAddPreambleParagraph(e).ConfigureAwait(false);
        }

        private void Observer_HeaderCommitteeChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateResolutionHeaderCommitteeName(e).ConfigureAwait(false);
        }
            

        private void Observer_HeaderSubmitterChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateResolutionHeaderSubmitterName(e).ConfigureAwait(false);
        }
            

        private void Observer_HeaderSessionChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateResolutionHeaderSession(e).ConfigureAwait(false);
        }
            
        
        private void Observer_HeaderAgendaItemChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateResolutionHeaderAgendaItem(e).ConfigureAwait(false);
        }
            

        private void Observer_HeaderTopicChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateResolutionHeaderTopic(e).ConfigureAwait(false);
        }
            

        private void Observer_HeaderFullNameChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateResolutionHeaderFullName(e).ConfigureAwait(false);
        }
            

        private void Observer_HeaderNameChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            GenerateAndSaveTan(e);
            _service.UpdateResolutionHeaderName(e).ConfigureAwait(false);
        }
            

    }
}
