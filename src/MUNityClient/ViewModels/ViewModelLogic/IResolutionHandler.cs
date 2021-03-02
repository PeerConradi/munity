﻿using MUNity.Models.Resolution.EventArguments;
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

        Task SetCommitteeName(string committeeName);

        Task SetSession(string session);

        Task SetAgendaItem(string agendaItem);

        Task AddPreambleParagraph();

        Task SetPreambleParagraphText(string paragraphId, string text);

        Task SetPreambleParagraphComment(string paragraphId, string comment);

        Task DeletePreambleParagraph(string paragraphId);

        Task ReorderPreambleParagraphs(IEnumerable<string> paragraphIdsInOrder);

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