using MUNity.Observers;
using MUNityClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Models.Resolution.EventArguments;

namespace MUNityClient.ViewModel.ViewModelLogic
{
    public class ResolutionObserverToServiceCalls
    {
        private IResolutionService _service;

        public ResolutionObserverToServiceCalls(ResolutionObserver observer, IResolutionService service)
        {
            if (observer == null || service == null) throw new ArgumentNullException();

            this._service = service;
            observer.HeaderNameChanged += Observer_HeaderNameChanged;
            observer.HeaderFullNameChanged += Observer_HeaderFullNameChanged;
            observer.HeaderTopicChanged += Observer_HeaderTopicChanged;
            observer.HeaderAgendaItemChanged += Observer_HeaderAgendaItemChanged;
            observer.HeaderSessionChanged += Observer_HeaderSessionChanged;
            observer.HeaderSubmitterChanged += Observer_HeaderSubmitterChanged;
            observer.HeaderCommitteeChanged += Observer_HeaderCommitteeChanged;
            // TODO: Handler for changes of supporters
            observer.PreambleParagraphAdded += Observer_PreambleParagraphAdded;
            observer.PreambleParagraphTextChanged += Observer_PreambleParagraphTextChanged;
        }

        private void Observer_PreambleParagraphTextChanged(object sender, PreambleParagraphTextChangedEventArgs e)
        {
            _service.ResolutionPreambleParagraphTextChanged(e).ConfigureAwait(false);
        }

        private void Observer_PreambleParagraphAdded(object sender, PreambleParagraphAddedEventArgs e)
        {
            _service.ResolutionAddPreambleParagraph(e).ConfigureAwait(false);
        }

        private void Observer_HeaderCommitteeChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderCommitteeName(e).ConfigureAwait(false);

        private void Observer_HeaderSubmitterChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderSubmitterName(e).ConfigureAwait(false);

        private void Observer_HeaderSessionChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderSession(e).ConfigureAwait(false);
        
        private void Observer_HeaderAgendaItemChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderAgendaItem(e).ConfigureAwait(false);

        private void Observer_HeaderTopicChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderTopic(e).ConfigureAwait(false);

        private void Observer_HeaderFullNameChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderFullName(e).ConfigureAwait(false);

        private void Observer_HeaderNameChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderName(e).ConfigureAwait(false);

    }
}
