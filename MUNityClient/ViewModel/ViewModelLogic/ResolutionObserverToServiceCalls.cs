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
        }

        private void Observer_HeaderTopicChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderTopic(e).ConfigureAwait(false);

        private void Observer_HeaderFullNameChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderFullName(e).ConfigureAwait(false);

        private void Observer_HeaderNameChanged(object sender, HeaderStringPropChangedEventArgs e) =>
            _service.UpdateResolutionHeaderName(e).ConfigureAwait(false);
    }
}
