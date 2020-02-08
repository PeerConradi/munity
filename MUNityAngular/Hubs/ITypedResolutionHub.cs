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

        Task OperativeParagraphChanged(HubObjects.HUBOperativeParagraph paragraph);

        Task PreambleParagraphChanged(string id, string newText);

        /// <summary>
        /// This Change is very complex because also amendments could be effected
        /// thats why we update the whole thing
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        Task OperativeParagraphRemoved(HubObjects.HUBResolution resolution);

        Task PreambleParaghraphRemoved(IEnumerable<HubObjects.HUBPreambleParagraph> paragraphs);

        Task PreambleSectionOrderChanged(IEnumerable<HubObjects.HUBPreambleParagraph> paragraphs);

        Task OperativeSectionOrderChanged(IEnumerable<HubObjects.HUBOperativeParagraph> paragraphs);

        Task TitleChanged(string newTitle);

        Task CommitteeChanged(string newTitle);

        Task SubmitterChanged(string newSubmitter);

        Task ResolutionSaved(DateTime time);

        Task DeleteAmendmentAdded(HubObjects.HUBDeleteAmendment amendment);

        Task ChangeAmendmentAdded(HubObjects.HUBChangeAmendment amendment);

        Task MoveAmendmentAdded(HubObjects.HUBResolution resolution, HubObjects.HUBMoveAmendment amendment);

        Task AddAmendmentAdded(HubObjects.HUBResolution resolution, HubObjects.HUBAddAmendment amendment);

        Task AmendmentActivated(HubObjects.HUBResolution resolution, HubObjects.HUBAbstractAmendment amendment);

        Task AmendmentRemoved(HubObjects.HUBResolution resolution, HubObjects.HUBAbstractAmendment amendment);

        Task AmendmentDeactivated(HubObjects.HUBResolution resolution, HubObjects.HUBAbstractAmendment amendment);

        Task AmendmentSubmitted(HubObjects.HUBResolution resolution);

        Task AmendmentDenied(HubObjects.HUBResolution resolution, HubObjects.HUBAbstractAmendment amendment);

        Task HubContextIdChanged(string id);
    }
}
