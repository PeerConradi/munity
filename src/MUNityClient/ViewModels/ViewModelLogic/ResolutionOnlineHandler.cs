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

        public event EventHandler<string> ErrorOccured;
        public event EventHandler<HeaderStringPropChangedEventArgs> NameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> FullNameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> TopicChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> AgendaItemChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> SessionChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> SubmitterNameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> CommitteeNameChanged;
        public event EventHandler<PreambleParagraphAddedEventArgs> PreambleParagraphAdded;
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

            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderNameChanged), (args) => NameChanged?.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderFullNameChanged), (args) => FullNameChanged?.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderTopicChanged), (args) => TopicChanged?.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderCommitteeNameChanged), (args) => CommitteeNameChanged.Invoke(this, args));
            HubConnection.On<HeaderStringPropChangedEventArgs>(nameof(ITypedResolutionHub.HeaderSubmitterNameChanged), (args) => SubmitterNameChanged?.Invoke(this, args));
            HubConnection.On<PreambleParagraphAddedEventArgs>(nameof(ITypedResolutionHub.PreambleParagraphAdded), (args) => PreambleParagraphAdded?.Invoke(this, args));

            this.resolution = resolution;
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
