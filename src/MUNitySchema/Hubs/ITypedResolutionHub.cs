using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Hubs
{

    /// <summary>
    /// The HUB used by the API for SignalR communication when talking to the clients.
    /// </summary>
    public interface ITypedResolutionHub
    {
        /// <summary>
        /// Something within the Resolution has changed and the client should refresh the views.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Obsolete("Use singled out events instead")]
        Task ResolutionChanged(ResolutionChangedArgs args);

        Task HeaderNameChanged(HeaderStringPropChangedEventArgs args);

        Task HeaderFullNameChanged(HeaderStringPropChangedEventArgs args);

        Task HeaderTopicChanged(HeaderStringPropChangedEventArgs args);

        Task HeaderAgendaItemChanged(HeaderStringPropChangedEventArgs args);

        Task HeaderSessionChanged(HeaderStringPropChangedEventArgs args);

        Task HeaderSubmitterNameChanged(HeaderStringPropChangedEventArgs args);

        Task HeaderCommitteeNameChanged(HeaderStringPropChangedEventArgs args);

        Task PreambleParagraphAdded(PreambleParagraphAddedEventArgs args);

        Task PreambleParagraphTextChanged(PreambleParagraphTextChangedEventArgs args);

        Task PreambleParagraphCommentTextChanged(PreambleParagraphCommentTextChangedEventArgs args);

        Task PreambleParagraphRemoved(PreambleParagraphRemovedEventArgs args);

        Task OperativeParagraphAdded(OperativeParagraphAddedEventArgs args);
        Task DeleteAmendmentCreated(DeleteAmendment dto);
        Task OperativeParagraphTextChanged(OperativeParagraphTextChangedEventArgs args);
        Task ChangeAmendmentCreated(ChangeAmendment dto);
        Task OperativeParagraphChanged(OperativeParagraphChangedEventArgs args);

        Task OperativeSectionChanged(OperativeSectionChangedEventArgs args);

        Task AmendmentActivatedChanged(AmendmentActivatedChangedEventArgs args);
        Task AddAmendmentCreated(AddAmendmentCreatedEventArgs args);
        Task AmendmentRemoved(string amendmentId);
        Task MoveAmendmentCreated(MoveAmendmentCreatedEventArgs dto);
        Task AmendmentSubmitted(string amendmentId);
    }
}
