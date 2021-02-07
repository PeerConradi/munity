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

        /// <summary>
        /// The text of a preamble paragraph has changed.
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <param name="paragraphId"></param>
        /// <param name="text"></param>
        /// <param name="tan"></param>
        /// <returns></returns>
        Task PreambleParagraphTextChanged(PreambleParagraphTextChangedEventArgs args);

        Task PreambleParagraphCommentTextChanged(PreambleParagraphCommentTextChangedEventArgs args);

        Task PreambleParagraphRemoved(PreambleParagraphRemovedEventArgs args);

        /// <summary>
        /// The Text of an operative paragraph has changed.
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <param name="paragraphId"></param>
        /// <param name="text"></param>
        /// <param name="tan"></param>
        /// <returns></returns>
        Task OperativeParagraphTextChanged(OperativeParagraphTextChangedEventArgs args);

        /// <summary>
        /// Something within the given PreambleParagraph has changed. This could be the Text or the Comments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Obsolete("Use the differenz change Methods instead")]
        Task PreambleParagraphChanged(PreambleParagraphChangedArgs args);

        /// <summary>
        /// Something within the Operative Paragraph has changed this could be the text or the comments.
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <param name="para"></param>
        /// <param name="tan"></param>
        /// <returns></returns>
        Task OperativeParagraphChanged(OperativeParagraphChangedEventArgs args);

        Task OperativeSectionChanged(OperativeSectionChangedEventArgs args);

        /// <summary>
        /// The state if an amendment is activated or not activated has changed.
        /// </summary>
        /// <param name="resolutionId"></param>
        /// <param name="amendmentId"></param>
        /// <param name="value"></param>
        /// <param name="tan"></param>
        /// <returns></returns>
        Task AmendmentActivatedChanged(AmendmentActivatedChangedEventArgs args);



        


    }
}
