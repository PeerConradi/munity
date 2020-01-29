using MUNityAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public interface ITypedResolutionHub
    {
        Task OperativeParagraphAdded(int position, HubObjects.HUBOperativeParagraph sectionModel);

        Task PreambleParagraphAdded(int position, string id, string text);

        Task OperativeParagraphChanged(string id, string newText);

        Task PreambleParagraphChanged(string id, string newText);

        Task PreambleSectionOrderChanged(IEnumerable<string> newOrder);

        Task OperativeSectionOrderChanged(IEnumerable<string> newOrder);

        Task TitleChanged(string newTitle);

        Task ResolutionSaved(DateTime time);

        Task DeleteAmendmentAdded(HubObjects.HUBDeleteAmendment amendment);

        Task AmendmentActivated(HubObjects.HUBAbstractAmendment amendment);

        Task AmendmentRemoved(HubObjects.HUBAbstractAmendment amendment);

        Task AmendmentDeactivated(HubObjects.HUBAbstractAmendment amendment);
        Task AmendmentSubmitted(HubObjects.HUBResolution resolution);
    }
}
