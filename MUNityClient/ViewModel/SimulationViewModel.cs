using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Hubs;
using MUNity.Schema.Simulation;
using System.Collections.ObjectModel;
using MUNity.Models.Simulation;
using MUNityClient.Services;
using System.Net.Http;

namespace MUNityClient.ViewModel
{

    /// <summary>
    /// The SocketHandler is turning more and more into some kind of ViewModel.
    /// It handles the Socket operations and holds the Simulation Object.
    /// </summary>
    public class SimulationViewModel
    {
        public delegate void OnRolesChanged(int sender, IEnumerable<MUNity.Schema.Simulation.SimulationRoleItem> roles);
        public event OnRolesChanged RolesChanged;

        public delegate void OnUserRoleChanged(int sender, int userId, int roleId);
        public event OnUserRoleChanged UserRoleChanged;

        public delegate void OnUserConnected(int sender, MUNity.Schema.Simulation.SimulationUserItem user);
        public event OnUserConnected UserConnected;

        public delegate void OnUserDisconnected(int sender, MUNity.Schema.Simulation.SimulationUserItem user);
        public event OnUserDisconnected UserDisconnected;

        public delegate void OnPhaseChanged(int sender, MUNity.Schema.Simulation.GamePhases phase);
        public event OnPhaseChanged PhaseChanged;

        public delegate void OnStatusChanged(int sender, string newStatus);
        public event OnStatusChanged StatusChanged;

        public delegate void OnLobbyModeChanged(int sender, MUNity.Schema.Simulation.LobbyModes mode);
        public event OnLobbyModeChanged LobbyModeChanged;

        public delegate void OnChatMessageRecieved(int simId, int userId, string msg);
        public event OnChatMessageRecieved ChatMessageRevieved;

        public delegate void OnUserPetition(IPetition petition);
        public event OnUserPetition UserPetition;

        public delegate void OnUserPetitionAccepted(IPetition petition);
        public event OnUserPetitionAccepted UserPetitionAccpted;

        public delegate void OnUserPetitionDeleted(IPetition petition);
        public event OnUserPetitionDeleted UserPetitionDeleted;

        public event EventHandler<MUNity.Schema.Simulation.VotedEventArgs> UserVoted;

        public event EventHandler<MUNity.Schema.Simulation.CreatedVoteModel> VoteCreated;

        public event EventHandler<string> CurrentResolutionChanged;

        public HubConnection HubConnection { get; set; }

        private SimulationService _simulationService;

        public MUNity.Schema.Simulation.SimulationResponse Simulation { get; private set; }

        public List<MUNity.Schema.Simulation.PetitionTypeSimulationDto> PetitionTypes { get; set; }

        public IUserItem Me => MyAuth != null ? Simulation.Users.FirstOrDefault(n => n.SimulationUserId == MyAuth.SimulationUserId) : null;

        /// <summary>
        /// The Id of the resolution that the user is currently reading. Not the one that the committee is currently working on!
        /// </summary>
        private string _currentResolutionId = null;
        public string CurrentResolutionId 
        {
            get => _currentResolutionId;
            set
            {
                if (_currentResolutionId == value) return;
                _currentResolutionId = value;
                this.CurrentResolutionChanged?.Invoke(this, value);
            }
        }

        public bool IsChair
        {
            get
            {
                var role = this.MyRole?.RoleType;
                if (role == null) return false;
                return role == RoleTypes.Chairman;
            }
        }

        public ObservableCollection<IPetition> Petitions { get; set; }

        public SimulationRoleItem MyRole
        {
            get
            {
                if (Me == null) return null;
                return Simulation?.Roles?.FirstOrDefault(n => n.SimulationRoleId == Me.RoleId);
            }
        }

        public SimulationAuthSchema MyAuth { get; private set; }

        private SimulationViewModel(SimulationResponse simulation, SimulationAuthSchema auth, SimulationService service)
        {
            this.Simulation = simulation;
            this.MyAuth = auth;
            this._simulationService = service;

            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/simsocket").Build();
            HubConnection.On<int, IEnumerable<SimulationRoleItem>>("RolesChanged", (id, roles) => RolesChanged?.Invoke(id, roles));
            HubConnection.On<int, int, int>("UserRoleChanged", (simId, userId, roleId) => UserRoleChanged?.Invoke(simId, userId, roleId));
            HubConnection.On<int, SimulationUserItem>("UserConnected", (id, user) => UserConnected?.Invoke(id, user));
            HubConnection.On<int, SimulationUserItem>("UserDisconnected", (id, user) => UserDisconnected?.Invoke(id, user));
            HubConnection.On<int, GamePhases>("PhaseChanged", (id, phase) => PhaseChanged?.Invoke(id, phase));
            HubConnection.On<int, string>("StatusChanged", (id, status) => StatusChanged?.Invoke(id, status));
            HubConnection.On<int, LobbyModes>("LobbyModeChanged", (id, mode) => LobbyModeChanged?.Invoke(id, mode));
            HubConnection.On<int, int, string>("ChatMessageRecieved", (simId, usrId, msg) => ChatMessageRevieved?.Invoke(simId, usrId, msg));
            HubConnection.On<IPetition>("UserPetition", (petition) => UserPetition?.Invoke(petition));
            HubConnection.On<IPetition>("UserPetitionAccepted", (petition) => UserPetitionAccpted?.Invoke(petition));
            HubConnection.On<IPetition>("UserPetitionDeleted", (petition) => UserPetitionDeleted?.Invoke(petition));
            HubConnection.On<VotedEventArgs>("Voted", (args) => UserVoted?.Invoke(this, args));
            HubConnection.On<CreatedVoteModel>("VoteCreated", (args) => VoteCreated?.Invoke(this, args));
        }

        public static async Task<SimulationViewModel> CreateHander(SimulationResponse simulation, SimulationAuthSchema auth, SimulationService service)
        {
            var socket = new SimulationViewModel(simulation, auth, service);
            await socket.HubConnection.StartAsync();
            return socket;
        }

        public async Task StartSimulation()
        {
            if (this.Simulation == null) return;
            await this._simulationService.SetPhase(this.Simulation.SimulationId, GamePhases.Online);
        }

        public async Task GoToLobby()
        {
            if (this.Simulation == null) return;
            await this._simulationService.SetPhase(this.Simulation.SimulationId, GamePhases.Lobby);
        }

        public bool CanMakeAnyPetition
        {
            get
            {
                if (this.PetitionTypes == null || !this.PetitionTypes.Any()) return false;

                if (this.MyRole == null) return false;

                if (MyRole.RoleType == RoleTypes.Chairman)
                    return this.PetitionTypes.Any(n => n.AllowChairs);
                else if (MyRole.RoleType == RoleTypes.Delegate)
                    return this.PetitionTypes.Any(n => n.AllowDelegates);
                else if (MyRole.RoleType == RoleTypes.Ngo)
                    return this.PetitionTypes.Any(n => n.AllowNgo);
                else if (MyRole.RoleType == RoleTypes.Spectator)
                    return this.PetitionTypes.Any(n => n.AllowSpectator);
                return false;
            }
        }

        public IEnumerable<PetitionTypeSimulationDto> MyPetitionTypes
        {
            get
            {
                if (this.PetitionTypes == null || !this.PetitionTypes.Any() || this.MyRole == null) 
                    return null;

                if (MyRole.RoleType == RoleTypes.Chairman)
                    return this.PetitionTypes.Where(n => n.AllowChairs);
                else if (MyRole.RoleType == RoleTypes.Delegate)
                    return this.PetitionTypes.Where(n => n.AllowDelegates);
                else if (MyRole.RoleType == RoleTypes.Ngo)
                    return this.PetitionTypes.Where(n => n.AllowNgo);
                else if (MyRole.RoleType == RoleTypes.Spectator)
                    return this.PetitionTypes.Where(n => n.AllowSpectator);
                return null;
            }
        }

        public Task<HttpResponseMessage> CreateAgendaItem(CreateAgendaItemDto model)
        {
            if (this.Simulation == null) return null;
            model.SimulationId = this.Simulation.SimulationId;
            return this._simulationService.CreateAgendaItem(model);
        }
    }
}
