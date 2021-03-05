using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public interface IResolutionHandler
    {
        Task SetName(string name);

        Task SetFullName(string fullName);

        Task SetTopic(string topic);

        Task SetSubmitterName(string submitterName);

        Task SetSupporterNames(string supporters);

        Task SetCommitteeName(string committeeName);

        Task SetSession(string session);

        Task SetAgendaItem(string agendaItem);

        Task AddPreambleParagraph();

        Task SetPreambleParagraphText(string paragraphId, string text);

        Task SetPreambleParagraphComment(string paragraphId, string comment);

        Task DeletePreambleParagraph(string paragraphId);

        Task ReorderPreambleParagraphs(IEnumerable<string> paragraphIdsInOrder);

        Task AddOperativeParagraph();

        Task SetOperativeParagraphText(string paragraphId, string text);

        Task CreateAddAmendment(string submitter, int index, string text);

        Task CreateChangeAmendment(string submitter, string paragraphId, string newText);

        Task CreateDeleteAmendment(string submitter, string paragraphId);

        Task CreateMoveAmendment(string submitter, string paragraphId, int newIndex);

        Task ActivateAmendment(string amendmentId);

        Task DeactivateAmendment(string amendmentId);

        Task SubmitAmendment(AbstractAmendment amendment);

        Task RemoveAmendment(AbstractAmendment amendment);

        event EventHandler<HeaderStringPropChangedEventArgs> NameChanged;
        event EventHandler<HeaderStringPropChangedEventArgs> FullNameChanged;
        event EventHandler<HeaderStringPropChangedEventArgs> TopicChanged;
        event EventHandler<HeaderStringPropChangedEventArgs> AgendaItemChanged;
        event EventHandler<HeaderStringPropChangedEventArgs> SessionChanged;
        event EventHandler<HeaderStringPropChangedEventArgs> SubmitterNameChanged;
        event EventHandler<HeaderStringPropChangedEventArgs> CommitteeNameChanged;

        event EventHandler<string> ErrorOccured;

        event EventHandler ChangedFromExtern;
    }
}
