using MUNity.Hubs;
using MUNity.Models.Resolution.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNityClient.ViewModels.ViewModelLogic
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
                AppendOperativeSectionEventHandlers();
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
            AppendSocketPreambleParagraphCommentTextChangedEventHandler();
            AppendSocketPreambleParagraphRemovedEventHandler();
        }

        private void AppendOperativeSectionEventHandlers()
        {
            AppendSockerOperativeParagraphChangedEventHandler();
            AppendSockerOperativeSectionChangedEventHandler();
        }

        private void AppendSockerOperativeParagraphChangedEventHandler()
        {
            this.viewModel.HubConnection.On<OperativeParagraphChangedEventArgs>(nameof(ITypedResolutionHub.OperativeParagraphChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                var paragraph = viewModel?.Resolution?.OperativeSection?.FirstOrDefault(n => n.OperativeParagraphId == args.Paragraph.OperativeParagraphId);
                if (paragraph != null)
                {
                    viewModel.Resolution.OperativeSection.Paragraphs.Add(args.Paragraph);
                }
            });
        }

        private void AppendSockerOperativeSectionChangedEventHandler()
        {
            this.viewModel.HubConnection.On<OperativeSectionChangedEventArgs>(nameof(ITypedResolutionHub.OperativeSectionChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                this.viewModel.Resolution.OperativeSection = args.Section;
                viewModel.InvokeOperativeSectionChangedFromExtern(this, args);
            });
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
                    viewModel.InvokePreambleChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketPreambleParagraphCommentTextChangedEventHandler()
        {
            this.viewModel.HubConnection.On<PreambleParagraphCommentTextChangedEventArgs>(nameof(ITypedResolutionHub.PreambleParagraphCommentTextChanged), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                var targetParagraph = viewModel.Resolution.Preamble.Paragraphs.FirstOrDefault(n => n.PreambleParagraphId == args.PreambleParagraphId);
                if (targetParagraph != null)
                {
                    viewModel.InvokePreambleChangedFromExtern(this, args);
                }
            });
        }

        private void AppendSocketPreambleParagraphRemovedEventHandler()
        {
            this.viewModel.HubConnection.On<PreambleParagraphRemovedEventArgs>(nameof(ITypedResolutionHub.PreambleParagraphRemoved), (args) =>
            {
                if (viewModel.VerifyTanAndRemoveItIfExisting(args)) return;
                var targetParagraph = viewModel.Resolution.Preamble.Paragraphs.FirstOrDefault(n => n.PreambleParagraphId == args.PreambleParagraphId);
                if (targetParagraph != null)
                {
                    viewModel.Resolution.Preamble.Paragraphs.Remove(targetParagraph);
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
                    //viewModel.Resolution.Header.SetTopicNoPropertyChanged(args.Text);
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
                    viewModel.InvokeHeaderChangedFromExtern(this, args);
                }
            });
        }
    }
}
