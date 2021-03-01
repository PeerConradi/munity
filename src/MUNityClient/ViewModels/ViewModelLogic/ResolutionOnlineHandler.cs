using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.Resolution.EventArguments;
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
        private string resolutionId;

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

        public event EventHandler<string> ErrorOccured;
        public event EventHandler<HeaderStringPropChangedEventArgs> NameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> FullNameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> TopicChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> AgendaItemChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> SessionChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> SubmitterNameChanged;
        public event EventHandler<HeaderStringPropChangedEventArgs> CommitteeNameChanged;

        private HeaderStringPropChangedEventArgs DefaultChangeHeaderRequest(string value)
        {
            Console.WriteLine("ResolutionId: " + resolutionId);
            return new HeaderStringPropChangedEventArgs(resolutionId, value)
            {
                Tan = new Random().Next(100000, 999999).ToString()
            };
        }

        public ResolutionOnlineHandler(string resolutionId)
        {
            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/resasocket").Build();
            this.resolutionId = resolutionId;
        }
    }
}
