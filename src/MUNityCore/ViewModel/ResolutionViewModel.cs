using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Hubs;
using MUNity.Models.Resolution.EventArguments;
using MUNity.Extensions.ResolutionExtensions;
using MUNity.Schema.Resolution;
using MUNity.Models.Resolution;

namespace MUNityCore.ViewModel
{
    public class ResolutionViewModel : IDisposable
    {
        private string resolutionId => resolution.ResolutionId;

        public Resolution Resolution => resolution;

        private Resolution resolution;

        public event EventHandler HeaderChanged;

        public event EventHandler PreambleChanged;

        public event EventHandler OperativeSectionChanged;

        public event EventHandler AmendmentsChanged;

        public HubConnection HubConnection { get; set; }

        public async Task SetCommitteeName(string committeeName)
        {
            var body = DefaultChangeHeaderRequest(committeeName);
            await HubConnection.SendAsync(nameof(MUNityCore.Hubs.ResolutionHub.SetCommitteeName), body);
        }

        public async Task SetSupporterNames(string supporterNames)
        {
            var body = DefaultChangeHeaderRequest(supporterNames);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetSupporterNames), body);
        }

        public async Task SetFullName(string fullName)
        {
            var body = DefaultChangeHeaderRequest(fullName);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetFullName), body);
        }

        public async Task SetName(string name)
        {
            var body = DefaultChangeHeaderRequest(name);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetName), body);
        }

        public async Task SetSubmitterName(string submitterName)
        {
            var body = DefaultChangeHeaderRequest(submitterName);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetSubmitterName), body);
        }

        public async Task SetTopic(string topic)
        {
            var body = DefaultChangeHeaderRequest(topic);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetTopic), body);
        }

        public async Task SetSession(string session)
        {
            var body = DefaultChangeHeaderRequest(session);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetSession), body);
        }

        public async Task SetAgendaItem(string agendaItem)
        {
            var body = DefaultChangeHeaderRequest(agendaItem);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetAgendaItem), body);
        }

        public async Task AddPreambleParagraph()
        {
            var body = new AddPreambleParagraphRequest(resolutionId);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.AddPreambleParagraph), body);
        }

        public async Task AddOperativeParagraph()
        {
            var body = new AddOperativeParagraphRequest(resolutionId);
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.AddOperativeParagraph), body);
        }

        /// <summary>
        /// Will add the operative paragraph into the resolution that is linked to this handler.
        /// It will return true if the operative paragraph has been added and false if not.
        /// A reason why its not added could be, that it already is inside the resolution.
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        private bool SafeAddOperativeParagraph(OperativeParagraph paragraph)
        {
            if (resolution.OperativeSection.FirstOrDefault(n => n.OperativeParagraphId == paragraph.OperativeParagraphId) == null)
            {
                resolution.OperativeSection.Paragraphs.Add(paragraph);
                return true;
            }
            return false;
        }

        public async Task SetPreambleParagraphText(string paragraphId, string text)
        {
            var body = new ChangePreambleParagraphTextRequest()
            {
                NewText = text,
                PreambleParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetPreambleParagraphText), body);
        }

        public async Task SetOperativeParagraphText(string paragraphId, string text)
        {
            var body = new ChangeOperativeParagraphTextRequest()
            {
                NewText = text,
                OperativeParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetOperativeParagraphText), body);
        }

        public async Task SetPreambleParagraphComment(string paragraphId, string comment)
        {
            var body = new ChangePreambleParagraphTextRequest()
            {
                NewText = comment,
                PreambleParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SetPreambleParagraphComment), body);
        }

        public async Task DeletePreambleParagraph(string paragraphId)
        {
            var body = new RemovePreambleParagraphRequest()
            {
                PreambleParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.DeletePreambleParagraph), body);
        }

        public async Task DeleteOperativeParagraph(string paragraphId)
        {
            var body = new RemoveOperativeParagraphRequest()
            {
                OperativeParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.DeleteOperativeParagraph), body);
        }

        public async Task ReorderPreambleParagraphs(IEnumerable<string> paragraphIdsInOrder)
        {
            var body = new ReorderPreambleRequest()
            {
                NewOrder = paragraphIdsInOrder.ToList(),
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.ReorderPreambleParagraphs), body);
        }

        public async Task CreateAddAmendment(string submitter, int index, string text)
        {
            var body = new CreateAddAmendmentRequest()
            {
                Index = index,
                ParentParagraphId = null,
                ResolutionId = resolutionId,
                SubmitterName = submitter,
                Text = text
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.CreateAddAmendment), body);
        }

        public async Task CreateChangeAmendment(string submitter, string paragraphId, string newText)
        {
            var body = new CreateChangeAmendmentRequest()
            {
                NewText = newText,
                ParagraphId = paragraphId,
                ResolutionId = resolutionId,
                SubmitterName = submitter
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.CreateChangeAmendment), body);
        }

        public async Task CreateDeleteAmendment(string submitter, string paragraphId)
        {
            var body = new CreateDeleteAmendmentRequest()
            {
                ParagraphId = paragraphId,
                ResolutionId = resolutionId,
                SubmitterName = submitter
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.CreateDeleteAmendment), body);
        }

        public async Task CreateMoveAmendment(string submitter, string paragraphId, int newIndex)
        {
            var body = new CreateMoveAmendmentRequest()
            {
                ParagraphId = paragraphId,
                ResolutionId = resolutionId,
                SubmitterName = submitter,
                NewIndex = newIndex
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.CreateMoveAmendment), body);
        }

        public async Task ActivateAmendment(string amendmentId)
        {
            var body = new ActivateAmendmentRequest()
            {
                AmendmentId = amendmentId,
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.ActivateAmendment), body);
        }

        public async Task DeactivateAmendment(string amendmentId)
        {
            var body = new ActivateAmendmentRequest()
            {
                AmendmentId = amendmentId,
                ResolutionId = resolutionId
            };
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.DeactivateAmendment), body);
        }

        public async Task SubmitAmendment(AbstractAmendment amendment)
        {
            var body = new AmendmentRequest()
            {
                AmendmentId = amendment.Id,
                ResolutionId = resolutionId
            };
            if (amendment is AddAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SubmitAddAmendment), body);
            else if (amendment is ChangeAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SubmitChangeAmendment), body);
            else if (amendment is DeleteAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SubmitDeleteAmendment), body);
            else if (amendment is MoveAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.SubmitMoveAmendment), body);
        }

        public async Task RemoveAmendment(AbstractAmendment amendment)
        {
            var body = new AmendmentRequest()
            {
                AmendmentId = amendment.Id,
                ResolutionId = resolutionId
            };
            if (amendment is AddAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.RemoveAddAmendment), body);
            else if (amendment is ChangeAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.RemoveChangeAmendment), body);
            else if (amendment is DeleteAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.RemoveDeleteAmendment), body);
            else if (amendment is MoveAmendment)
                await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.RemoveMoveAmendment), body);
        }

        public async Task SetPublicEditMode(bool allowPublicEdit)
        {
            await HubConnection.SendAsync(nameof(Hubs.ResolutionHub.ChangePublicMode), resolutionId, allowPublicEdit);
        }


        public event EventHandler<string> ErrorOccured;
        public event EventHandler<HeaderStringPropChangedEventArgs> NameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> FullNameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> TopicChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> AgendaItemChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> SessionChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> SubmitterNameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> CommitteeNameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> SupportersChanged;

        public event EventHandler<PreambleParagraphAddedEventArgs> PreambleParagraphAdded;
        public event EventHandler<PreambleParagraphTextChangedEventArgs> PreambleParagraphTextChanged;

        public event EventHandler<OperativeParagraphAddedEventArgs> OperativeParagraphAdded;
        public event EventHandler<OperativeParagraphTextChangedEventArgs> OperativeParagraphTextChanged;
        public event EventHandler<OperativeParagraphRemovedEventArgs> OperativeParagraphRemoved;

        public event EventHandler<AddAmendmentCreatedEventArgs> AddAmendmentCreated;
        public event EventHandler<ChangeAmendment> ChangeAmendmentCreated;
        public event EventHandler<DeleteAmendment> DeleteAmendmentCreated;
        public event EventHandler<MoveAmendmentCreatedEventArgs> MoveAmendmentCreated;

        public event EventHandler<AmendmentActivatedChangedEventArgs> AmendmentActivatedChanged;

        public event EventHandler<string> AmendmentRemoved;
        public event EventHandler<string> AmendmentSubmitted;

        public event EventHandler<PublicModeChangedEventArgs> PublicModeChanged;

        public event EventHandler ChangedFromExtern;

        private HeaderStringPropChangedEventArgs DefaultChangeHeaderRequest(string value)
        {
            return new HeaderStringPropChangedEventArgs(resolutionId, value)
            {
                Tan = new Random().Next(100000, 999999).ToString()
            };
        }

        

        private void ResolutionOnlineHandler_SupportersChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            if (resolution.Header.SupporterNames != e.Text)
            {
                resolution.Header.SupporterNames = e.Text;
                ChangedFromExtern?.Invoke(this, new EventArgs());
                HeaderChanged?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_AmendmentRemoved(object sender, string id)
        {
            var amendment = resolution.OperativeSection.GetAllAmendments().FirstOrDefault(n => n.Id == id);
            if (amendment != null)
            {
                resolution.OperativeSection.RemoveAmendment(amendment);
                ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_AmendmentSubmitted(object sender, string id)
        {
            var amendment = resolution.OperativeSection.GetAllAmendments().FirstOrDefault(n => n.Id == id);
            if (amendment != null)
            {
                amendment.Apply(resolution.OperativeSection);
                ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_AmendmentActivatedChanged(object sender, AmendmentActivatedChangedEventArgs e)
        {
            var inAddAmendments = resolution.OperativeSection.AddAmendments.FirstOrDefault(n => n.Id == e.AmendmentId);
            if (inAddAmendments != null)
            {
                inAddAmendments.Activated = e.Activated;
                var virtualParagraph = resolution.OperativeSection.FindOperativeParagraph(inAddAmendments.TargetSectionId);
                virtualParagraph.Visible = e.Activated;
            }

            var inMoveAmendments = resolution.OperativeSection.MoveAmendments.FirstOrDefault(n => n.Id == e.AmendmentId);
            if (inMoveAmendments != null)
            {
                inMoveAmendments.Activated = e.Activated;
                var virtualParagraph = resolution.OperativeSection.FindOperativeParagraph(inMoveAmendments.NewTargetSectionId);
                virtualParagraph.Visible = e.Activated;
            }

            var inChangeAmendments = resolution.OperativeSection.ChangeAmendments.FirstOrDefault(n => n.Id == e.AmendmentId);
            if (inChangeAmendments != null)
            {
                inChangeAmendments.Activated = e.Activated;
            }

            var inDeleteAmendments = resolution.OperativeSection.DeleteAmendments.FirstOrDefault(n => n.Id == e.AmendmentId);
            if (inDeleteAmendments != null)
            {
                inDeleteAmendments.Activated = e.Activated;
            }
            this.ChangedFromExtern?.Invoke(this, new EventArgs());
        }

        private void ResolutionOnlineHandler_MoveAmendmentCreated(object sender, MoveAmendmentCreatedEventArgs e)
        {
            if (this.resolution.OperativeSection.FirstOrDefault(n => n.OperativeParagraphId == e.VirtualParagraph.OperativeParagraphId) == null)
            {
                this.resolution.OperativeSection.Paragraphs.Insert(e.VirtualParagraphIndex, e.VirtualParagraph);
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }

            if (this.resolution.OperativeSection.MoveAmendments.All(n => n.Id != e.Amendment.Id))
            {
                this.resolution.OperativeSection.MoveAmendments.Add(e.Amendment);
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }

        }

        private void ResolutionOnlineHandler_DeleteAmendmentCreated(object sender, DeleteAmendment e)
        {
            if (this.resolution.OperativeSection.DeleteAmendments.All(n => n.Id != e.Id))
            {
                this.resolution.OperativeSection.DeleteAmendments.Add(e);
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_ChangeAmendmentCreated(object sender, ChangeAmendment e)
        {
            if (this.resolution.OperativeSection.ChangeAmendments.All(n => n.Id != e.Id))
            {
                this.resolution.OperativeSection.ChangeAmendments.Add(e);
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_AddAmendmentCreated(object sender, AddAmendmentCreatedEventArgs e)
        {
            Console.WriteLine("Amendment created!");
            // An dieser Stelle ist die Reihnfolge wichtig, erst der Virtuelle Absatz und dann der Änderungsantrag!
            if (resolution.OperativeSection.FirstOrDefault(n => n.OperativeParagraphId == e.VirtualParagraph.OperativeParagraphId) == null)
            {
                resolution.OperativeSection.Paragraphs.Insert(e.VirtualParagraphIndex, e.VirtualParagraph);
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }

            if (resolution.OperativeSection.AddAmendments.All(n => n.Id != e.Amendment.Id))
            {
                resolution.OperativeSection.AddAmendments.Add(e.Amendment);
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }


        }

        private void ResolutionOnlineHandler_OperativeParagraphTextChanged(object sender, OperativeParagraphTextChangedEventArgs e)
        {
            var paragraph = resolution.OperativeSection.FirstOrDefault(n => n.OperativeParagraphId == e.ParagraphId);
            if (paragraph != null && paragraph.Text != e.Text)
            {
                paragraph.Text = e.Text;
                ChangedFromExtern.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_PreambleParagraphTextChanged(object sender, PreambleParagraphTextChangedEventArgs e)
        {
            var paragraph = resolution.Preamble.Paragraphs.FirstOrDefault(n => n.PreambleParagraphId == e.ParagraphId);
            if (paragraph != null && paragraph.Text != e.Text)
            {
                paragraph.Text = e.Text;
                ChangedFromExtern.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_OperativeParagraphAdded(object sender, OperativeParagraphAddedEventArgs e)
        {
            var added = SafeAddOperativeParagraph(e.Paragraph);
            if (added) this.ChangedFromExtern.Invoke(this, new EventArgs());
        }

        private void ResolutionOnlineHandler_PreambleParagraphAdded(object sender, PreambleParagraphAddedEventArgs e)
        {
            if (resolution.Preamble.Paragraphs.All(n => n.PreambleParagraphId != e.Paragraph.PreambleParagraphId))
            {
                resolution.Preamble.Paragraphs.Add(e.Paragraph);
                this.ChangedFromExtern.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_SubmitterNameChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            if (this.resolution.Header.SubmitterName != e.Text)
            {
                this.resolution.Header.SubmitterName = e.Text;
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_CommitteeNameChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            if (this.resolution.Header.CommitteeName != e.Text)
            {
                this.resolution.Header.CommitteeName = e.Text;
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_TopicChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            if (this.resolution.Header.Topic != e.Text)
            {
                this.resolution.Header.Topic = e.Text;
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_FullNameChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            if (this.resolution.Header.FullName != e.Text)
            {
                this.resolution.Header.FullName = e.Text;
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionOnlineHandler_NameChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            if (this.resolution.Header.Name != e.Text)
            {
                this.resolution.Header.Name = e.Text;
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        private void ResolutionViewModel_OperativeParagraphRemoved(object sender, OperativeParagraphRemovedEventArgs e)
        {
            var paragraph = this.Resolution.OperativeSection.FindOperativeParagraph(e.OperativeParagraphId);
            if (paragraph != null)
            {
                this.Resolution.OperativeSection.RemoveOperativeParagraph(paragraph);
                this.ChangedFromExtern?.Invoke(this, new EventArgs());
            }
        }

        public static async Task<ResolutionViewModel> CreateViewModel(Resolution resolution, string socketPath)
        {
            var hubConnection = new HubConnectionBuilder().WithUrl(socketPath).Build();
            var viewModel = new ResolutionViewModel(hubConnection, resolution);
            await hubConnection.StartAsync();
            await hubConnection.SendAsync(nameof(Hubs.ResolutionHub.Subscribe), resolution.ResolutionId);
            return viewModel;
        }

        private ResolutionViewModel(HubConnection hubConnection, Resolution resolution)
        {
            HubConnection = hubConnection;

            NameChanged += ResolutionOnlineHandler_NameChanged;
            FullNameChanged += ResolutionOnlineHandler_FullNameChanged;
            TopicChanged += ResolutionOnlineHandler_TopicChanged;
            CommitteeNameChanged += ResolutionOnlineHandler_CommitteeNameChanged;
            SubmitterNameChanged += ResolutionOnlineHandler_SubmitterNameChanged;
            PreambleParagraphAdded += ResolutionOnlineHandler_PreambleParagraphAdded;
            PreambleParagraphTextChanged += ResolutionOnlineHandler_PreambleParagraphTextChanged;
            SupportersChanged += ResolutionOnlineHandler_SupportersChanged;

            OperativeParagraphAdded += ResolutionOnlineHandler_OperativeParagraphAdded;
            OperativeParagraphTextChanged += ResolutionOnlineHandler_OperativeParagraphTextChanged;
            OperativeParagraphRemoved += ResolutionViewModel_OperativeParagraphRemoved;

            AddAmendmentCreated += ResolutionOnlineHandler_AddAmendmentCreated;
            ChangeAmendmentCreated += ResolutionOnlineHandler_ChangeAmendmentCreated;
            DeleteAmendmentCreated += ResolutionOnlineHandler_DeleteAmendmentCreated;
            MoveAmendmentCreated += ResolutionOnlineHandler_MoveAmendmentCreated;

            AmendmentActivatedChanged += ResolutionOnlineHandler_AmendmentActivatedChanged;
            AmendmentSubmitted += ResolutionOnlineHandler_AmendmentSubmitted;
            AmendmentRemoved += ResolutionOnlineHandler_AmendmentRemoved;

            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderNameChanged), (args) => NameChanged?.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderFullNameChanged), (args) => FullNameChanged?.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderTopicChanged), (args) => TopicChanged?.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderCommitteeNameChanged), (args) => CommitteeNameChanged.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderSubmitterNameChanged), (args) => SubmitterNameChanged?.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderSupportersChanged), (args) => SupportersChanged?.Invoke(this, args));
            HubConnection.On<PreambleParagraphAddedEventArgs>(nameof(ITypedResolutionHub.PreambleParagraphAdded), (args) => PreambleParagraphAdded?.Invoke(this, args));
            HubConnection.On<OperativeParagraphAddedEventArgs>(nameof(ITypedResolutionHub.OperativeParagraphAdded), (args) => OperativeParagraphAdded?.Invoke(this, args));
            HubConnection.On<PreambleParagraphTextChangedEventArgs>(nameof(ITypedResolutionHub.PreambleParagraphTextChanged), (args) => PreambleParagraphTextChanged?.Invoke(this, args));
            HubConnection.On<OperativeParagraphTextChangedEventArgs>(nameof(ITypedResolutionHub.OperativeParagraphTextChanged), (args) => OperativeParagraphTextChanged?.Invoke(this, args));
            HubConnection.On<AddAmendmentCreatedEventArgs>(nameof(ITypedResolutionHub.AddAmendmentCreated), (args) => this.AddAmendmentCreated?.Invoke(this, args));
            HubConnection.On<ChangeAmendment>(nameof(ITypedResolutionHub.ChangeAmendmentCreated), (args) => ChangeAmendmentCreated?.Invoke(this, args));
            HubConnection.On<DeleteAmendment>(nameof(ITypedResolutionHub.DeleteAmendmentCreated), (args) => DeleteAmendmentCreated?.Invoke(this, args));
            HubConnection.On<MoveAmendmentCreatedEventArgs>(nameof(ITypedResolutionHub.MoveAmendmentCreated), (args) => MoveAmendmentCreated?.Invoke(this, args));
            HubConnection.On<AmendmentActivatedChangedEventArgs>(nameof(ITypedResolutionHub.AmendmentActivatedChanged), (args) => AmendmentActivatedChanged?.Invoke(this, args));
            HubConnection.On<string>(nameof(ITypedResolutionHub.AmendmentRemoved), (id) => AmendmentRemoved?.Invoke(this, id));
            HubConnection.On<string>(nameof(ITypedResolutionHub.AmendmentSubmitted), (id) => AmendmentSubmitted?.Invoke(this, id));
            HubConnection.On<PublicModeChangedEventArgs>(nameof(ITypedResolutionHub.PublicModeChanged), (args) => PublicModeChanged?.Invoke(this, args));
            HubConnection.On<OperativeParagraphRemovedEventArgs>(nameof(ITypedResolutionHub.OperativeParagraphRemoved), (args) => OperativeParagraphRemoved?.Invoke(this, args));

            this.resolution = resolution;
        }

        

        public void Dispose()
        {
            if (this.HubConnection != null)
                this.HubConnection.DisposeAsync();
        }
    }
}
