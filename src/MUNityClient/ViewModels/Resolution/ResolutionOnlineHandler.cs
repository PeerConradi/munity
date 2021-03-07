using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Hubs;
using MUNity.Models.Resolution;
using MUNity.Models.Resolution.EventArguments;
using MUNity.Schema.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MUNity.Extensions.ResolutionExtensions;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public class ResolutionOnlineHandler : IResolutionHandler
    {
        private string resolutionId => resolution.ResolutionId;

        private Resolution resolution;

        public HubConnection HubConnection { get; set; }

        public async Task SetCommitteeName(string committeeName)
        {
            var body = DefaultChangeHeaderRequest(committeeName);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/CommitteeName", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task SetSupporterNames(string supporterNames)
        {
            var body = DefaultChangeHeaderRequest(supporterNames);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/Supporters", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task SetFullName(string fullName)
        {
            var body = DefaultChangeHeaderRequest(fullName);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/FullName", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task SetName(string name)
        {
            var body = DefaultChangeHeaderRequest(name);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/Name", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task SetSubmitterName(string submitterName)
        {
            var body = DefaultChangeHeaderRequest(submitterName);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/SubmitterName", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task SetTopic(string topic)
        {
            var body = DefaultChangeHeaderRequest(topic);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/Topic", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task SetSession(string session)
        {
            var body = DefaultChangeHeaderRequest(session);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/Session", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task SetAgendaItem(string agendaItem)
        {
            var body = DefaultChangeHeaderRequest(agendaItem);
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Header/SesAgendaItemsion", body);
            if (!response.IsSuccessStatusCode)
                ErrorOccured.Invoke(this, $"An error occured while trying to send an update to the server {response.ReasonPhrase}");
        }

        public async Task AddPreambleParagraph()
        {
            var body = new AddPreambleParagraphRequest()
            {
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(Program.API_URL + "/api/Resa/Preamble/AddParagraph", body);
            if (response.IsSuccessStatusCode)
            {
                var newParagraph = await response.Content.ReadFromJsonAsync<PreambleParagraph>();
                if (newParagraph != null)
                {
                    // The signalR connection can be faster than this, in that case dont add
                    // the paragraph another time!
                    if (resolution.Preamble.Paragraphs.All(n => n.PreambleParagraphId != newParagraph.PreambleParagraphId))
                    {
                        this.resolution.Preamble.Paragraphs.Add(newParagraph);
                    }
                    
                }
            }
        }

        public async Task AddOperativeParagraph()
        {
            var body = new AddOperativeParagraphRequest() { ResolutionId = resolutionId };
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(Program.API_URL + "/api/Resa/Operative/AddParagraph", body);
            if (response.IsSuccessStatusCode)
            {
                var newParagraph = await response.Content.ReadFromJsonAsync<OperativeParagraph>();
                if (newParagraph != null)
                {
                    SafeAddOperativeParagraph(newParagraph);
                }
            }
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
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Preamble/Text", body);
            if (!response.IsSuccessStatusCode)
            {
                ErrorOccured.Invoke(this, "Preamble Paragraph wasnt changed!");
            }
        }

        public async Task SetOperativeParagraphText(string paragraphId, string text)
        {
            var body = new ChangeOperativeParagraphTextRequest()
            {
                NewText = text,
                OperativeParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Operative/Text", body);
            if (!response.IsSuccessStatusCode)
            {
                ErrorOccured.Invoke(this, "Operative Paragraph wasnt changed!");
            }
        }

        public async Task SetPreambleParagraphComment(string paragraphId, string comment)
        {
            var body = new ChangePreambleParagraphTextRequest()
            {
                NewText = comment,
                PreambleParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Preamble/Comment", body);
            if (!response.IsSuccessStatusCode)
            {
                ErrorOccured.Invoke(this, "Preamble paragraph wasnt changed!");
            }
        }

        public async Task DeletePreambleParagraph(string paragraphId)
        {
            var body = new RemovePreambleParagraphRequest()
            {
                PreambleParagraphId = paragraphId,
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Preamble/Remove", body);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error while deleting!");
                ErrorOccured.Invoke(this, "Preamble paragraph wasnt deleted!");
            }
            else
            {
                this.resolution.Preamble.Paragraphs.RemoveAll(n => n.PreambleParagraphId == paragraphId);
                this.ChangedFromExtern.Invoke(this, new EventArgs());
            }
        }

        public async Task ReorderPreambleParagraphs(IEnumerable<string> paragraphIdsInOrder)
        {
            var body = new ReorderPreambleRequest()
            {
                NewOrder = paragraphIdsInOrder.ToList(),
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Preamble/Reorder", body);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error while reordering!");
                ErrorOccured.Invoke(this, "Preamble paragraph was not reordered");
            }
            else
            {
                ChangedFromExtern.Invoke(this, new EventArgs());
            }
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
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Add/Create", body);
            if (response.IsSuccessStatusCode)
            {
                var args = await response.Content.ReadFromJsonAsync<AddAmendmentCreatedEventArgs>();
                if (args != null)
                {
                    this.AddAmendmentCreated?.Invoke(this, args);
                }
            }
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
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Change/Create", body);
            if (response.IsSuccessStatusCode)
            {
                var args = await response.Content.ReadFromJsonAsync<ChangeAmendment>();
                if (args != null)
                {
                    this.ChangeAmendmentCreated?.Invoke(this, args);
                }
            }
        }

        public async Task CreateDeleteAmendment(string submitter, string paragraphId)
        {
            var body = new CreateDeleteAmendmentRequest()
            {
                ParagraphId = paragraphId,
                ResolutionId = resolutionId,
                SubmitterName = submitter
            };
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Delete/Create", body);
            if (response.IsSuccessStatusCode)
            {
                var args = await response.Content.ReadFromJsonAsync<DeleteAmendment>();
                if (args != null)
                {
                    this.DeleteAmendmentCreated?.Invoke(this, args);
                }
            }
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
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Move/Create", body);
            if (response.IsSuccessStatusCode)
            {
                var args = await response.Content.ReadFromJsonAsync<MoveAmendmentCreatedEventArgs>();
                if (args != null)
                {
                    this.MoveAmendmentCreated?.Invoke(this, args);
                }
            }
        }

        public async Task ActivateAmendment(string amendmentId)
        {
            var body = new ActivateAmendmentRequest()
            {
                AmendmentId = amendmentId,
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Add/ActivateAmendment", body);
        }

        public async Task DeactivateAmendment(string amendmentId)
        {
            var body = new ActivateAmendmentRequest()
            {
                AmendmentId = amendmentId,
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Add/DeactivateAmendment", body);
        }

        public async Task SubmitAmendment(AbstractAmendment amendment)
        {
            var body = new AmendmentRequest()
            {
                AmendmentId = amendment.Id,
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            if (amendment is AddAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Add/Submit", body);
            else if (amendment is ChangeAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Change/Submit", body);
            else if (amendment is DeleteAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Delete/Submit", body);
            else if (amendment is MoveAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Move/Submit", body);
        }

        public async Task RemoveAmendment(AbstractAmendment amendment)
        {
            var body = new AmendmentRequest()
            {
                AmendmentId = amendment.Id,
                ResolutionId = resolutionId
            };
            var client = new HttpClient();
            if (amendment is AddAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Add/Remove", body);
            else if (amendment is ChangeAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Change/Remove", body);
            else if (amendment is DeleteAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Delete/Remove", body);
            else if (amendment is MoveAmendment)
                await client.PutAsJsonAsync(Program.API_URL + "/api/Resa/Amendment/Move/Remove", body);
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

        public event EventHandler<AddAmendmentCreatedEventArgs> AddAmendmentCreated;
        public event EventHandler<ChangeAmendment> ChangeAmendmentCreated;
        public event EventHandler<DeleteAmendment> DeleteAmendmentCreated;
        public event EventHandler<MoveAmendmentCreatedEventArgs> MoveAmendmentCreated;

        public event EventHandler<AmendmentActivatedChangedEventArgs> AmendmentActivatedChanged;

        public event EventHandler<string> AmendmentRemoved;
        public event EventHandler<string> AmendmentSubmitted;

        public event EventHandler ChangedFromExtern;

        private HeaderStringPropChangedEventArgs DefaultChangeHeaderRequest(string value)
        {
            return new HeaderStringPropChangedEventArgs(resolutionId, value)
            {
                Tan = new Random().Next(100000, 999999).ToString()
            };
        }

        public ResolutionOnlineHandler(Resolution resolution)
        {
            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/resasocket").Build();

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

            this.resolution = resolution;
        }

        private void ResolutionOnlineHandler_SupportersChanged(object sender, HeaderStringPropChangedEventArgs e)
        {
            if (resolution.Header.SupporterNames != e.Text)
            {
                resolution.Header.SupporterNames = e.Text;
                ChangedFromExtern?.Invoke(this, new EventArgs());
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
    }
}
