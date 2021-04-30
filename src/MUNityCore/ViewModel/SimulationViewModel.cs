using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.ViewModel
{
    public class SimulationViewModel : IDisposable
    {
        private HubConnection _hubConnection;

        public event EventHandler<List<int>> ConnectedUsersChanged;

        public event EventHandler<MUNity.Schema.Simulation.Voting.SimulationVotingDto> VotingCreated;

        public event EventHandler<MUNity.Schema.Simulation.VotedEventArgs> Voted;

        public event EventHandler<MUNity.Schema.Simulation.AgendaItemDto> AgendaItemCreated;

        public event EventHandler<MUNity.Schema.Simulation.PetitionInfoDto> PetitionAdded;

        public event EventHandler<MUNity.Schema.Simulation.PetitionInteractedDto> PetitionRemoved;

        public event EventHandler<int> AgendaItemRemoved;

        public event EventHandler<string> PetitionActivated;

        public event EventHandler<SimulationStatusDto> StatusChanged;

        public SpeakerlistViewModel SpeakerlistViewModel;

        public int SimulationId { get; set; }

        public int UserId { get; set; }

        public bool IsChair { get; set; }

        public string Token { get; set; }

        public string DisplayName { get; set; }

        public string RoleIso { get; set; }

        public string RoleName { get; set; }


        private SimulationViewModel(HubConnection hubConnection)
        {
            this._hubConnection = hubConnection;

            _hubConnection.On<List<int>>(nameof(MUNity.Hubs.ITypedSimulationHub.ConnectedUsersChanged), (n) =>
            {
                this.ConnectedUsersChanged?.Invoke(this, n);
            });
            _hubConnection.On<MUNity.Schema.Simulation.Voting.SimulationVotingDto>(nameof(MUNity.Hubs.ITypedSimulationHub.VoteCreated), (n) =>
            {
                this.VotingCreated?.Invoke(this, n);
            });
            _hubConnection.On<MUNity.Schema.Simulation.VotedEventArgs>(nameof(MUNity.Hubs.ITypedSimulationHub.Voted), n => this.Voted?.Invoke(this, n));
            _hubConnection.On<MUNity.Schema.Simulation.AgendaItemDto>(nameof(MUNity.Hubs.ITypedSimulationHub.AgendaItemAdded), n => this.AgendaItemCreated?.Invoke(this, n));
            _hubConnection.On<MUNity.Schema.Simulation.PetitionInfoDto>(nameof(MUNity.Hubs.ITypedSimulationHub.PetitionAdded), n => this.PetitionAdded?.Invoke(this, n));
            _hubConnection.On<string>(nameof(MUNity.Hubs.ITypedSimulationHub.PetitionActivated), n => this.PetitionActivated?.Invoke(this, n));
            _hubConnection.On<MUNity.Schema.Simulation.PetitionInteractedDto>(nameof(MUNity.Hubs.ITypedSimulationHub.PetitionDeleted), n => this.PetitionRemoved?.Invoke(this, n));
            _hubConnection.On<int>(nameof(MUNity.Hubs.ITypedSimulationHub.AgendaItemRemoved), n => this.AgendaItemRemoved?.Invoke(this, n));
            _hubConnection.On<SimulationStatusDto>(nameof(MUNity.Hubs.ITypedSimulationHub.StatusChanged), n => StatusChanged?.Invoke(this, n));
        }

        public async Task SignIn()
        {
            await _hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.SignIn), this.SimulationId, UserId);
        }

        public async Task AddMeToSpeakerlist()
        {
            if (this.SpeakerlistViewModel == null) return;
            await this.SpeakerlistViewModel.AddSpeaker(this.RoleIso, this.RoleName);
        }

        public async Task AddMeToQuestions()
        {
            if (this.SpeakerlistViewModel == null) return;
            await this.SpeakerlistViewModel.AddQuestion(this.RoleIso, this.RoleName);
        }

        internal async Task CreateVoting(string displayName, bool allowAbstention)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.CreateVotingForDelegates), this.SimulationId, displayName, allowAbstention);
        }

        internal async Task Vote(string votingId, MUNity.Schema.Simulation.EVoteStates choice)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.Vote), votingId, choice);
        }

        internal async Task CreateAgendaItem(string name, string description)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.CreateAgendaItem), name, description);
        }

        internal async Task RemoveAgendaItem(int agendaItemId)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.RemoveAgendaItem), agendaItemId);
        }

        internal async Task SubmitPetition(int agendaItemId, int petitionType)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.MakePetition), agendaItemId, petitionType);
        }

        internal async Task ActivatePetition(string petitionId)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.ActivatePetition), petitionId);
        }

        internal async Task RemovePetition(string petitionId)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.RemovePetition), petitionId);
        }

        internal async Task ChangeStatus(string newStatusText)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.ChangeStatus), newStatusText);
        }

        public static async Task<SimulationViewModel> Init(string socketPath)
        {
            var hubConnection = new HubConnectionBuilder().WithUrl(socketPath).Build();
            var simViewModel = new SimulationViewModel(hubConnection);
            await hubConnection.StartAsync();
            return simViewModel;
        }

        public void Dispose()
        {
            if (this._hubConnection != null)
                this._hubConnection.DisposeAsync();
        }
    }
}
