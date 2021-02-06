using MUNity.Hubs;
using MUNity.Models.Resolution.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace MUNityClient.ViewModel.ViewModelLogic
{
    public class ResolutionSocketViewModelManipulator
    {
        private ResolutionViewModel viewModel;

        public ResolutionSocketViewModelManipulator(ResolutionViewModel viewModel)
        {
            this.viewModel = viewModel;
            if (viewModel != null)
            {
                AppendHeaderSocketEventHandlers();
                AppendPreambleSocketEventHandlers();
            }
        }

        private void AppendHeaderSocketEventHandlers()
        {
            AppendSocketChangedHeaderNameEventHandler();
            AppendSocketChangedHeaderFullNameEventHandler();
            AppendSocketChangedHeaderTopicEventHandler();
            AppendSocketChangedHeaderAgendaItemEventHandler();
            AppendSocketChangedHeaderSessionEventHandler();
            AppendSocketChangedHeaderSubmitterNameEventHandler();
            AppendSocketChangedCommitteeNameEventHandler();
        }

        private void AppendPreambleSocketEventHandlers()
        {
            AppendSocketPreambleParagraphAddedEventHandler();
            AppendSocketPreambleParagraphTextChangedEventHandler();
        }

        private void AppendSocketPreambleParagraphAddedEventHandler()
        {
            this.viewModel.HubConnection.On<PreambleParagraphAddedEventArgs>(nameof(ITypedResolutionHub.PreambleParagraphAdded), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Preamble.Paragraphs.All(n => n.PreambleParagraphId != args.Paragraph.PreambleParagraphId))
                {
                    viewModel.Resolution.Preamble.Paragraphs.Add(args.Paragraph);
                    viewModel.InvokePreambleChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketPreambleParagraphTextChangedEventHandler()
        {
            this.viewModel.HubConnection.On<PreambleParagraphTextChangedEventArgs>(nameof(ITypedResolutionHub.PreambleParagraphTextChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                var targetParagraph = viewModel.Resolution.Preamble.Paragraphs.FirstOrDefault(n => n.PreambleParagraphId == args.ParagraphId);
                if (targetParagraph != null)
                {
                    targetParagraph.SetTextNoNotifyPropertyChanged(args.Text);
                    viewModel.InvokePreambleChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketChangedHeaderNameEventHandler()
        {
            this.viewModel.HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderNameChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Header.Name != args.Text)
                {
                    viewModel.Resolution.Header.SetNameNoPropertyChanged(args.Text);
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketChangedHeaderFullNameEventHandler()
        {
            this.viewModel.HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderFullNameChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Header.FullName != args.Text)
                {
                    viewModel.Resolution.Header.SetFullNameNoPropertyChanged(args.Text);
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketChangedHeaderTopicEventHandler()
        {
            this.viewModel.HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderTopicChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Header.Topic != args.Text)
                {
                    viewModel.Resolution.Header.SetTopicNoPropertyChanged(args.Text);
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketChangedHeaderAgendaItemEventHandler()
        {
            this.viewModel.HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderAgendaItemChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Header.AgendaItem != args.Text)
                {
                    viewModel.Resolution.Header.SetAgendaItemNoPropertyChanged(args.Text);
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketChangedHeaderSessionEventHandler()
        {
            this.viewModel.HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderSessionChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Header.Session != args.Text)
                {
                    viewModel.Resolution.Header.SetSessionNoPropertyChanged(args.Text);
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketChangedHeaderSubmitterNameEventHandler()
        {
            this.viewModel.HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderSubmitterNameChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Header.SubmitterName != args.Text)
                {
                    viewModel.Resolution.Header.SetSubmitterNameNoPropertyChanged(args.Text);
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketChangedCommitteeNameEventHandler()
        {
            this.viewModel.HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderCommitteeNameChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                if (viewModel.Resolution.Header.CommitteeName != args.Text)
                {
                    viewModel.Resolution.Header.SetCommitteeNameNoPropertyChanged(args.Text);
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }
    }
}
