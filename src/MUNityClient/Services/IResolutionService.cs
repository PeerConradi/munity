using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using MUNityClient.Models.Resolution;
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

        Task<HttpResponseMessage> ResolutionPreambleParagraphCommentTextChanged(PreambleParagraphCommentTextChangedEventArgs args);

        Task<HttpResponseMessage> DeleteResolutionPreambleParagraph(PreambleParagraphRemovedEventArgs args);

        Task<HttpResponseMessage> UpdateOperativeParagraph(OperativeParagraphChangedEventArgs args);

        Task<HttpResponseMessage> UpdateOperativeSection(OperativeSectionChangedEventArgs args);

        Task<List<ResolutionInfo>> GetStoredResolutions();

        Task<Resolution> GetResolution(string resolutionId);

        Task<Resolution> GetStoredResolution(string id);

        Task<bool> IsOnline(bool forceRefresh = false);

        Task<ViewModels.ResolutionViewModel> Subscribe(Resolution resolution);

        Task<string> CreatePublicResolution();

        Task<Resolution> CreateResolution(string title = "");

        Task<bool> CanUserPostAmendments(string resolutionId);

        Task<HttpResponseMessage> PostDeleteAmendment(string resolutionId, DeleteAmendment amendment);

        Task<Resolution> GetResolutionFromServer(string resolutionId);

        Task<bool> ResolutionExistsServerside(string id);

        string GenerateTan();

        Task<MUNitySchema.Schema.Simulation.Resolution.ResolutionSmallInfo> GetInfo(string id);
    }
}
