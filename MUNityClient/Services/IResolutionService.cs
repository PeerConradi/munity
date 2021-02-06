using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MUNityClient.Services
{
    public interface IResolutionService
    {
        void SaveOfflineResolution(Resolution resolution);

        Task<HttpResponseMessage> UpdateResolutionHeaderName(HeaderStringPropChangedEventArgs args);

        Task<HttpResponseMessage> UpdateResolutionHeaderFullName(HeaderStringPropChangedEventArgs args);

        Task<HttpResponseMessage> UpdateResolutionHeaderTopic(HeaderStringPropChangedEventArgs args);

        Task<HttpResponseMessage> UpdateResolutionHeaderAgendaItem(HeaderStringPropChangedEventArgs args);

        Task<HttpResponseMessage> UpdateResolutionHeaderSession(HeaderStringPropChangedEventArgs args);

        Task<HttpResponseMessage> UpdateResolutionHeaderSubmitterName(HeaderStringPropChangedEventArgs args);

        Task<HttpResponseMessage> UpdateResolutionHeaderCommitteeName(HeaderStringPropChangedEventArgs args);

        Task<HttpResponseMessage> ResolutionAddPreambleParagraph(PreambleParagraphAddedEventArgs args);

        Task<HttpResponseMessage> ResolutionPreambleParagraphTextChanged(PreambleParagraphTextChangedEventArgs args);
    }
}
