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
    }
}
