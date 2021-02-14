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

namespace MUNityClient.ViewModels
{

    /// <summary>
    /// The SimulationViewModel is holding data and information about the Simulation.
    /// For example the users, roles and petitions.
    /// You should use the Subscribe Method inside the SimulationService <see cref="Services.SimulationService.Subscribe(int)"/> Method to get an
    /// instance of this class.
    /// The instance is creating a SignalR Socket Listener that will update properties of this view model
    /// if necessary.
    /// </summary>
    public class SimulationViewModel
    {
        public delegate void OnRolesChanged(int sender, IEnumerable<MUNity.Schema.Simulation.SimulationRoleDto> roles);
        public event OnRolesChanged RolesChanged;

        public delegate void OnUserRoleChanged(int sender, int userId, int roleId);
        public event OnUserRoleChanged UserRoleChanged;

        public delegate void OnUserConnected(int sender, MUNity.Schema.Simulation.SimulationUserDefaultDto user);
        public event OnUserConnected UserConnected;

        public delegate void OnUserDisconnected(int sender, MUNity.Schema.Simulation.SimulationUserDefaultDto user);
        public event OnUserDisconnected UserDisconnected;

        public delegate void OnPhaseChanged(int sender, MUNity.Schema.Simulation.GamePhases phase);
        public event OnPhaseChanged PhaseChanged;

        public delegate void OnStatusChanged(int sender, string newStatus);
        public event OnStatusChanged StatusChanged;

        public delegate void OnLobbyModeChanged(int sender, MUNity.Schema.Simulation.LobbyModes mode);
        public event OnLobbyModeChanged LobbyModeChanged;

        public delegate void OnChatMessageRecieved(int simId, int userId, string msg);
        public event OnChatMessageRecieved ChatMessageRevieved;

        public delegate void OnUserPetitionAccepted(IPetition petition);
        public event OnUserPetitionAccepted UserPetitionAccpted;

        public delegate void OnUserPetitionDeleted(IPetition petition);
        public event OnUserPetitionDeleted UserPetitionDeleted;

        public event EventHandler<MUNity.Schema.Simulation.VotedEventArgs> UserVoted;

        public event EventHandler<MUNity.Schema.Simulation.CreatedVoteModel> VoteCreated;

        public event EventHandler<string> CurrentResolutionChanged;

        public event EventHandler<AgendaItemDto> AgendaItemAdded;

        public event EventHandler<PetitionDto> PetitionAdded;

        public HubConnection HubConnection { get; set; }

        private SimulationService _simulationService;

        public MUNity.Schema.Simulation.SimulationDto Simulation { get; private set; }

        public List<MUNity.Schema.Simulation.PetitionTypeSimulationDto> PetitionTypes { get; private set; }

        public List<MUNity.Schema.Simulation.AgendaItemDto> AgendaItems { get; private set; }

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

        /// <summary>
        /// Returns true if currently signed in user has a role which RoleType is Chairman.
        /// </summary>
        public bool IsChair
        {
            get
            {
                var role = this.MyRole?.RoleType;
                if (role == null) return false;
                return role == RoleTypes.Chairman;
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (MyAuth == null) return false;
                return MyAuth.CanCreateRole;
            }
        }

        public SimulationRoleDto MyRole
        {
            get
            {
                if (Me == null) return null;
                return Simulation?.Roles?.FirstOrDefault(n => n.SimulationRoleId == Me.RoleId);
            }
        }

        public SimulationAuthDto MyAuth { get; private set; }

        private SimulationViewModel(SimulationDto simulation, SimulationService service)
        {
            this.Simulation = simulation;
            this._simulationService = service;

            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/simsocket").Build();
            HubConnection.On<int, IEnumerable<SimulationRoleDto>>("RolesChanged", (id, roles) => RolesChanged?.Invoke(id, roles));
            HubConnection.On<int, int, int>("UserRoleChanged", (simId, userId, roleId) => UserRoleChanged?.Invoke(simId, userId, roleId));
            HubConnection.On<int, SimulationUserDefaultDto>("UserConnected", (id, user) => UserConnected?.Invoke(id, user));
            HubConnection.On<int, SimulationUserDefaultDto>("UserDisconnected", (id, user) => UserDisconnected?.Invoke(id, user));
            HubConnection.On<int, GamePhases>("PhaseChanged", (id, phase) => PhaseChanged?.Invoke(id, phase));
            HubConnection.On<int, string>("StatusChanged", (id, status) => StatusChanged?.Invoke(id, status));
            HubConnection.On<int, LobbyModes>("LobbyModeChanged", (id, mode) => LobbyModeChanged?.Invoke(id, mode));
            HubConnection.On<int, int, string>("ChatMessageRecieved", (simId, usrId, msg) => ChatMessageRevieved?.Invoke(simId, usrId, msg));
            HubConnection.On<IPetition>("UserPetitionAccepted", (petition) => UserPetitionAccpted?.Invoke(petition));
            HubConnection.On<IPetition>("UserPetitionDeleted", (petition) => UserPetitionDeleted?.Invoke(petition));
            HubConnection.On<VotedEventArgs>("Voted", (args) => UserVoted?.Invoke(this, args));
            HubConnection.On<CreatedVoteModel>("VoteCreated", (args) => VoteCreated?.Invoke(this, args));
            HubConnection.On<AgendaItemDto>(nameof(ITypedSimulationHub.AgendaItemAdded), (args) => this.AgendaItemAdded?.Invoke(this, args));
            HubConnection.On<PetitionDto>(nameof(ITypedSimulationHub.PetitionAdded), (args) => this.PetitionAdded?.Invoke(this, args));

            this.AgendaItemAdded += SimulationViewModel_AgendaItemAdded;
            this.PetitionAdded += SimulationViewModel_PetitionAdded;
            this.UserConnected += SimulationViewModel_UserConnected;
            this.UserDisconnected += SimulationViewModel_UserDisconnected;
            this.RolesChanged += SimulationViewModel_RolesChanged;
            this.UserRoleChanged += SimulationViewModel_UserRoleChanged;
        }

        private void SimulationViewModel_UserRoleChanged(int sender, int userId, int roleId)
        {
            var user = Simulation?.Users?.FirstOrDefault(n => n.SimulationUserId == userId);
            if (user != null)
            {
                user.RoleId = roleId;
            }
        }

        private void SimulationViewModel_UserDisconnected(int sender, SimulationUserDefaultDto user)
        {
            var userInSim = Simulation?.Users?.FirstOrDefault(n => n.SimulationUserId == user.SimulationUserId);
            if (user != null)
            {
                user.IsOnline = false;
            }
        }

        private void SimulationViewModel_RolesChanged(int sender, IEnumerable<SimulationRoleDto> roles)
        {
            this.Simulation.Roles = roles.ToList();
        }

        private void SimulationViewModel_UserConnected(int sender, SimulationUserDefaultDto user)
        {
            var userInSim = this.Simulation?.Users?.FirstOrDefault(n => n.SimulationUserId == user.SimulationUserId);
            if (userInSim != null)
            {
                user.IsOnline = true;
                if (!string.IsNullOrEmpty(user.DisplayName) && user.DisplayName != user.DisplayName)
                    user.DisplayName = user.DisplayName;
            }
        }

        private void SimulationViewModel_PetitionAdded(object sender, PetitionDto e)
        {
            var agendaItem = this.AgendaItems?.FirstOrDefault(n => n.AgendaItemId == e.TargetAgendaItemId);
            if (agendaItem != null)
            {
                if (agendaItem.Petitions.All(n => n.PetitionId != e.PetitionId))
                    agendaItem.Petitions.Add(e);
            }
        }

        private void SimulationViewModel_AgendaItemAdded(object sender, AgendaItemDto e)
        {
            if (this.AgendaItems == null)
                this.AgendaItems = new List<AgendaItemDto>();

            if (this.AgendaItems.All(n => n.AgendaItemId != e.AgendaItemId))
                this.AgendaItems.Add(e);
        }

        public static async Task<SimulationViewModel> CreateViewModel(SimulationDto simulation, SimulationService service)
        {
            var socket = new SimulationViewModel(simulation, service);
            await socket.LoadDataAsync();
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

        public async Task<HttpResponseMessage> MakePetition(PetitionTypeSimulationDto type, int agendaItemId)
        {
            if (type == null)
            {
                throw new ArgumentException("No type argument given!");
            }

            if (MyAuth == null)
            {
                Console.WriteLine("Not authenticated to make petitions!");
                return null;
            }
            var mdl = new MUNity.Schema.Simulation.CreatePetitionRequest()
            {
                PetitionDate = DateTime.Now,
                PetitionTypeId = type.PetitionTypeId,
                PetitionUserId = MyAuth.SimulationUserId,
                TargetAgendaItemId = agendaItemId,
                Text = "",
                SimulationId = Simulation.SimulationId
            };

            return await this._simulationService.MakePetition(mdl);
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

        /// <summary>
        /// Creates a new instance Agenda Item and will return a Success Method if the
        /// creation was successful.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> CreateAgendaItem(CreateAgendaItemDto model)
        {
            if (this.Simulation == null) return null;
            model.SimulationId = this.Simulation.SimulationId;
            return this._simulationService.CreateAgendaItem(model);
        }
    
        public async Task LoadDataAsync()
        {
            if (this.Simulation != null)
            {
                var loadRoles = this._simulationService.GetRoles(this.Simulation.SimulationId);
                var loadAuth = this._simulationService.GetMyAuth(this.Simulation.SimulationId);

                await Task.WhenAll(loadRoles, loadAuth);
                this.MyAuth = loadAuth.Result;
                this.Simulation.Roles = loadRoles.Result;

                _ = LoadUsersDependingAuth().ConfigureAwait(false);

                var petitionTypesTask = this._simulationService.PetitionTypes(this.Simulation.SimulationId);
                var agendaItemsTask = this._simulationService.AgendaItems(this.Simulation.SimulationId);

                await Task.WhenAll(petitionTypesTask, agendaItemsTask);
                this.PetitionTypes = petitionTypesTask.Result;
                this.AgendaItems = agendaItemsTask.Result;
            }
        }

        private async Task LoadUsersDependingAuth()
        {
            if (IsChair || IsAdmin)
            {
                var users = await this._simulationService.GetUserSetups(this.Simulation.SimulationId);
                this.Simulation.Users.Clear();
                this.Simulation.Users.AddRange(users);
            }
            else
            {
                var users = await this._simulationService.GetUsers(this.Simulation.SimulationId);
                this.Simulation.Users.Clear();
                this.Simulation.Users.AddRange(users);
            }

            if (Me != null)
            {
                Me.IsOnline = true;
            }
        }
    }
}
