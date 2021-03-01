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
using MUNityClient.Extensions.Simulation;
using System.Net.Http.Json;
using MUNitySchema.Schema.Simulation.Resolution;
using Microsoft.AspNetCore.Components;

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

        internal async Task SetUserRole(int simulationUserId, int value)
        {
            var body = new SetUserSimulationRole()
            {
                RoleId = value,
                SimulationId = this.Simulation.SimulationId,
                UserId = simulationUserId,
                Token = this.Token
            };
            var client = new HttpClient();
            Console.WriteLine("Setze Rolle");
            await client.PutAsJsonAsync(Program.API_URL + "/api/Simulation/Roles/SetUserRole", body);
        }

        public event OnRolesChanged RolesChanged;

        public event EventHandler<UserRoleChangedEventArgs> UserRoleChanged;

        public delegate void OnUserConnected(int sender, MUNity.Schema.Simulation.SimulationUserDefaultDto user);
        public event OnUserConnected UserConnected;

        public delegate void OnUserDisconnected(int sender, MUNity.Schema.Simulation.SimulationUserDefaultDto user);
        public event OnUserDisconnected UserDisconnected;

        public delegate void OnPhaseChanged(int sender, MUNity.Schema.Simulation.GamePhases phase);
        public event OnPhaseChanged PhaseChanged;

        public event EventHandler<SimulationStatusDto> StatusChanged;

        public delegate void OnLobbyModeChanged(int sender, MUNity.Schema.Simulation.LobbyModes mode);
        public event OnLobbyModeChanged LobbyModeChanged;

        public delegate void OnChatMessageRecieved(int simId, int userId, string msg);
        public event OnChatMessageRecieved ChatMessageRevieved;

        public event EventHandler<MUNity.Schema.Simulation.VotedEventArgs> UserVoted;

        public event EventHandler<MUNity.Schema.Simulation.CreatedVoteModel> VoteCreated;

        public event EventHandler<string> CurrentResolutionChanged;

        

        public event EventHandler<AgendaItemDto> AgendaItemAdded;

        public event EventHandler<PetitionDto> PetitionAdded;

        public event EventHandler<PetitionInteractedDto> PetitionDeleted;

        public event EventHandler<NotificationViewModel> NotificationChanged;

        public int MyUserId { get; set; }

        public SimulationSlotDto MySlot => Slots.FirstOrDefault(n => n.SimulationUserId == MyUserId);

        public ObservableCollection<SimulationSlotDto> Slots { get; set; } = new ObservableCollection<SimulationSlotDto>();

        public async Task CreateResolution()
        {
            var client = new HttpClient();
            var body = new SimulationRequest()
            {
                SimulationId = this.Simulation.SimulationId,
                Token = this.Token
            };
            var result = await client.PostAsJsonAsync(Program.API_URL + "/api/Simulation/CreateResolution", body);
            if (result.IsSuccessStatusCode)
            {
                var newResolution = await result.Content.ReadFromJsonAsync<ResolutionSmallInfo>();
                if (newResolution != null)
                {
                    this.Resolutions.Add(newResolution);
                }
                else
                {
                    this.ShowError("Cast Fehler", "Resolution wurde nicht erstellt, bei dem Erzeugen des Elements ist ein Fehler aufgetreten");
                }
            }
            else
            {
                this.ShowError("Fehler", $"Resolution wurde nicht erstellt {result.StatusCode}");
            }
        }

        internal async Task SetStatus(string statusText)
        {
            var client = new HttpClient();
            var body = new SetSimulationStatusDto()
            {
                SimulationId = this.Simulation.SimulationId,
                StatusText = statusText,
                Token = Token
            };
            var result = await client.PutAsJsonAsync(Program.API_URL + "/api/Simulation/Status/CurrentStatus", body);
            //if (!result.IsSuccessStatusCode)
            //{
                
            //}
            this.ShowError("Fehler", "Status konnte nicht geändert werden!");
        }

        internal async Task UpdatePetitionTypes()
        {
            await this._simulationService.SecurePetitionTypes(this);
        }

        public string Token { get; set; }

        private NotificationViewModel _currentNotification;
        public NotificationViewModel CurrentNotification
        {
            get => _currentNotification;
            set
            {
                if (this._currentNotification == value) return;
                _currentNotification = value;
                NotificationChanged?.Invoke(this, value);
            }
        }

        public HubConnection HubConnection { get; set; }

        private SimulationService _simulationService;

        public MUNity.Schema.Simulation.SimulationDto Simulation { get; private set; }

        public List<MUNity.Schema.Simulation.PetitionTypeSimulationDto> PetitionTypes { get; set; }

        public List<MUNity.Schema.Simulation.AgendaItemDto> AgendaItems { get; set; }

        public ObservableCollection<SimulationUserAdminDto> AdminUsers { get; set; } = new ObservableCollection<SimulationUserAdminDto>();

        public IUserItem Me => MyAuth != null ? Simulation.Users.FirstOrDefault(n => n.SimulationUserId == MyAuth.SimulationUserId) : null;

        public SimulationStatusDto CurrentStatus { get; set; }

        public MUNity.Schema.Simulation.AgendaItemDto SelectedAgendaItem { get; set; }

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

        public ObservableCollection<ResolutionSmallInfo> Resolutions { get; set; } = new ObservableCollection<ResolutionSmallInfo>();

        public async Task UpdateResolutions()
        {
            this.Resolutions.Clear();
            var resolutions = await _simulationService.GetSimulationResolutionInfos(this.Simulation.SimulationId);
            foreach(var item in resolutions)
            {
                this.Resolutions.Add(item);
            }
        }

        public SimulationCurrentVoting CurrentVoting { get; private set; }

        /// <summary>
        /// Returns true if currently signed in user has a role which RoleType is Chairman.
        /// </summary>
        public bool IsChair
        {
            get
            {
                if (MySlot == null) return false;
                return MySlot.RoleType == RoleTypes.Chairman;
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (MySlot == null) return false;
                return MySlot.CanCreateRole;
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

        public SimulationAuthDto MyAuth { get; set; }

        private SimulationViewModel(SimulationDto simulation, SimulationService service)
        {
            this.Simulation = simulation;
            this._simulationService = service;

            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/simsocket").Build();
            HubConnection.On<int, IEnumerable<SimulationRoleDto>>("RolesChanged", (id, roles) => RolesChanged?.Invoke(id, roles));
            HubConnection.On<UserRoleChangedEventArgs>(nameof(ITypedSimulationHub.UserRoleChanged), (args) => UserRoleChanged?.Invoke(this, args));
            HubConnection.On<int, SimulationUserDefaultDto>("UserConnected", (id, user) => UserConnected?.Invoke(id, user));
            HubConnection.On<int, SimulationUserDefaultDto>(nameof(ITypedSimulationHub.UserDisconnected), (id, user) => UserDisconnected?.Invoke(id, user));
            HubConnection.On<int, GamePhases>("PhaseChanged", (id, phase) => PhaseChanged?.Invoke(id, phase));
            HubConnection.On<SimulationStatusDto>(nameof(ITypedSimulationHub.StatusChanged), (body) => StatusChanged?.Invoke(this, body));
            HubConnection.On<int, LobbyModes>("LobbyModeChanged", (id, mode) => LobbyModeChanged?.Invoke(id, mode));
            HubConnection.On<int, int, string>("ChatMessageRecieved", (simId, usrId, msg) => ChatMessageRevieved?.Invoke(simId, usrId, msg));
            HubConnection.On<VotedEventArgs>("Voted", (args) => UserVoted?.Invoke(this, args));
            HubConnection.On<CreatedVoteModel>("VoteCreated", (args) => VoteCreated?.Invoke(this, args));
            HubConnection.On<AgendaItemDto>(nameof(ITypedSimulationHub.AgendaItemAdded), (args) => this.AgendaItemAdded?.Invoke(this, args));
            HubConnection.On<PetitionDto>(nameof(ITypedSimulationHub.PetitionAdded), (args) => this.PetitionAdded?.Invoke(this, args));
            HubConnection.On<PetitionInteractedDto>(nameof(ITypedSimulationHub.PetitionDeleted), (args) => this.PetitionDeleted?.Invoke(this, args));

            this.AgendaItemAdded += SimulationViewModel_AgendaItemAdded;
            this.PetitionAdded += SimulationViewModel_PetitionAdded;
            this.PetitionDeleted += SimulationViewModel_PetitionDeleted;

            this.UserConnected += SimulationViewModel_UserConnected;
            this.UserDisconnected += SimulationViewModel_UserDisconnected;
            this.RolesChanged += SimulationViewModel_RolesChanged;
            this.UserRoleChanged += SimulationViewModel_UserRoleChanged;

            this.VoteCreated += SimulationViewModel_VoteCreated;
            this.UserVoted += SimulationViewModel_UserVoted;

            this.StatusChanged += SimulationViewModel_StatusChanged;
        }

        private void SimulationViewModel_StatusChanged(object sender, SimulationStatusDto e)
        {
            Console.WriteLine("Status has changed!");
            this.CurrentStatus = e;
        }

        private void SimulationViewModel_UserVoted(object sender, VotedEventArgs e)
        {
            if (this.CurrentVoting != null)
                this.CurrentVoting.Vote(e);
        }

        private void SimulationViewModel_VoteCreated(object sender, CreatedVoteModel e)
        {
            this.CurrentVoting = new SimulationCurrentVoting(e);
        }

        private void SimulationViewModel_PetitionDeleted(object sender, PetitionInteractedDto e)
        {
            var agendaItem = this.AgendaItems.FirstOrDefault(n => n.AgendaItemId == e.AgendaItemId);
            if (agendaItem != null)
            {
                agendaItem.Petitions.RemoveAll(n => n.PetitionId == e.PetitionId);
            }
        }

        private void SimulationViewModel_UserRoleChanged(object sender, UserRoleChangedEventArgs args)
        {
            var user = Slots.FirstOrDefault(n => n.SimulationUserId == args.UserId);
            if (user != null)
            {
                user.RoleId = args.RoleId;
                user.RoleName = this.Simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == args.RoleId)?.Name ?? "";
            }
        }

        private void SimulationViewModel_UserDisconnected(int sender, SimulationUserDefaultDto user)
        {
            Console.WriteLine($"User diconnected {user.SimulationUserId}");
            var userInSim = Slots.FirstOrDefault(n => n.SimulationUserId == user.SimulationUserId);
            if (userInSim != null)
            {
                userInSim.IsOnline = false;
            }
        }

        private void SimulationViewModel_RolesChanged(int sender, IEnumerable<SimulationRoleDto> roles)
        {
            this.Simulation.Roles = roles.ToList();
        }

        public Task<HttpResponseMessage> DeletePetition(PetitionDto petition)
        {
            return this._simulationService.DeletePetition(this.Simulation.SimulationId, petition);
        }

        

        private void SimulationViewModel_UserConnected(int sender, SimulationUserDefaultDto user)
        {
            var userInSim = this.Slots.FirstOrDefault(n => n.SimulationUserId == user.SimulationUserId);
            if (userInSim != null)
            {
                userInSim.IsOnline = true;
                if (!string.IsNullOrEmpty(user.DisplayName) && userInSim.DisplayName != user.DisplayName)
                    userInSim.DisplayName = user.DisplayName;
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

        public void NotifyRolesChanged()
        {
            this.RolesChanged?.Invoke(0, this.Simulation.Roles);
        }

        private void SimulationViewModel_AgendaItemAdded(object sender, AgendaItemDto e)
        {
            if (this.AgendaItems == null)
                this.AgendaItems = new List<AgendaItemDto>();

            if (this.AgendaItems.All(n => n.AgendaItemId != e.AgendaItemId))
                this.AgendaItems.Add(e);
        }

        /// <summary>
        /// Creates a new ViewModel instance. Note that this will load a token from the stored tokens so you need to have
        /// a valid token for the simulation created before calling this method, otherwise it will return null.
        /// </summary>
        /// <param name="simulation"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static async Task<SimulationViewModel> CreateViewModel(SimulationDto simulation, SimulationService service)
        {
            var token = await service.GetSimulationToken(simulation.SimulationId);
            if (token == null) return null;

            var viewModel = new SimulationViewModel(simulation, service);
            viewModel.Token = token.Token;

            await LoadMe(viewModel, token.Token);
            await LoadSlots(viewModel, token.Token);
            await LoadStatus(viewModel);

            await viewModel.LoadDataAsync();
            await viewModel.HubConnection.StartAsync();
            return viewModel;
        }

        private static async Task LoadSlots(SimulationViewModel viewModel, string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("simsimtoken", token);
            var result = await httpClient.GetAsync(Program.API_URL + $"/api/Simulation/Slots?simulationId={viewModel.Simulation.SimulationId}");
            if (result.IsSuccessStatusCode)
            {
                var slots = await result.Content.ReadFromJsonAsync<List<SimulationSlotDto>>();
                if (slots != null && slots.Any())
                {
                    slots.ForEach(n => viewModel.Slots.Add(n));
                }
            }
        }

        private static async Task LoadMe(SimulationViewModel viewModel, string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("simsimtoken", token);
            var result = await httpClient.GetAsync(Program.API_URL + $"/api/Simulation/User/MyUserId?simulationId={viewModel.Simulation.SimulationId}");
            if (result.IsSuccessStatusCode)
            {
                var userId = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(userId))
                {
                    viewModel.MyUserId = int.Parse(userId);
                }
            }
        }

        private static async Task LoadStatus(SimulationViewModel viewModel)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(Program.API_URL + $"/api/Simulation/Status/Current?simulationId={viewModel.Simulation.SimulationId}");
            if (result.IsSuccessStatusCode)
            {
                viewModel.CurrentStatus = await result.Content.ReadFromJsonAsync<SimulationStatusDto>();
            }
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

        public async Task CloseAllConnections()
        {
            await this._simulationService.CloseAllConnections(this.Simulation.SimulationId);
        }

        public async Task<string> GetDirectJoinLink(int userId)
        {
            var client = new HttpClient();
            var body = new SimulationUserTokenRequest()
            {
                SimulationId = this.Simulation.SimulationId,
                Token = this.Token,
                UserId = userId
            };
            var response = await client.PutAsJsonAsync(Program.API_URL + "/api/Simulation/User/UserToken", body);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return $"Sim/DirectJoin/{this.Simulation.SimulationId}/{token}";
            }
            else
            {
                this.ShowError("Fehler", "Link konnte nicht geholt werden!");
                return null;
            }

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

                var loadRoles = this._simulationService.SecureGetRoles(this);
                var loadAuth = this._simulationService.SecureGetMyAuth(this);

                await Task.WhenAll(loadRoles, loadAuth);

                _ = LoadUsersDependingAuth().ConfigureAwait(false);

                var petitionTypesTask = this._simulationService.SecurePetitionTypes(this);
                var agendaItemsTask = this._simulationService.SecureAgendaItems(this);

                await Task.WhenAll(petitionTypesTask, agendaItemsTask);
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

        public async Task CreateUser()
        {
            var client = new HttpClient();
            var body = new SimulationRequest()
            {
                SimulationId = this.Simulation.SimulationId,
                Token = this.Token
            };
            var response = await client.PostAsJsonAsync(Program.API_URL + "/api/Simulation/User/CreateUser", body);
            if (!response.IsSuccessStatusCode)
            {
                this.ShowError("Fehler", $"Benutzer konnte nicht erstellt werden {response.StatusCode}");
            }
            else
            {
                var newUser = await response.Content.ReadFromJsonAsync<SimulationUserAdminDto>();
                if (newUser != null)
                {
                    var slot = new SimulationSlotDto()
                    {
                        CanCreateRole = false,
                        CanEditListOfSpeakers = false,
                        CanEditResolution = false,
                        CanSelectRole = false,
                        DisplayName = newUser.DisplayName,
                        IsOnline = newUser.IsOnline,
                        RoleId = newUser.RoleId,
                        RoleIso = "un",
                        RoleName = "",
                        RoleType = RoleTypes.None,
                        SimulationUserId = newUser.SimulationUserId
                    };
                    this.Slots.Add(slot);
                    //AdminUsers.Add(newUser);
                }
            }
        }
    }
}
