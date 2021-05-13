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

        /// <summary>
        /// The context of the user that is currently connected to the simulation.
        /// If no user is given the user is either a spectator or moderator, in both cases the
        /// connected user can only view the simulation and not interact with anyone.
        /// </summary>
        public SimulationUserContext UserContext { get; set; }

        public string Name { get; private set; }

        public void FetchData(Services.SimulationService simulationService)
        {
            if (simulationService == null)
            {
                Console.WriteLine("Error: SimulationService was disposed!");
            }
            var sim = simulationService.GetSimulation(this.SimulationId);
            if (sim == null)
            {
                Console.WriteLine($"Error: the Simulation with the id: {this.SimulationId} was not found. No Data for the SimulationViewModel");
            }
            else
            {
                this.Name = sim.Name;
            }
            
        }


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
            await _hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.SignIn), this.SimulationId, UserContext.UserId);
        }

        public async Task SignInContext()
        {
            await _hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.SignInContext), this.SimulationId);
        }

        public async Task AddMeToSpeakerlist()
        {
            if (this.SpeakerlistViewModel == null) return;
            await this.SpeakerlistViewModel.AddSpeaker(this.UserContext.RoleIso, this.UserContext.RoleName);
        }

        public async Task AddMeToQuestions()
        {
            if (this.SpeakerlistViewModel == null) return;
            await this.SpeakerlistViewModel.AddQuestion(this.UserContext.RoleIso, this.UserContext.RoleName);
        }

        internal async Task CreateVoting(string displayName, bool allowAbstention, bool allowNgoVote, IEnumerable<int> exceptions)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.CreateVotingForDelegates), this.SimulationId, displayName, allowAbstention, allowNgoVote, exceptions);
        }

        internal async Task CreateVoting(MUNity.Schema.Simulation.Voting.NewVotingSocketDto model)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.CreateVoting), model);
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

        internal async Task CreateVotingForPresentDelegates(int presentsId, string text, bool allowAbstention, bool allowNgoVote, IEnumerable<int> exceptions)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.CreateVotingForPresentDelegates), presentsId, text, allowAbstention, allowNgoVote, exceptions);
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
