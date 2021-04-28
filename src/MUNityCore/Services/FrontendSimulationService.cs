using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using MUNity.Models.Simulation;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Voting;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Models.Simulation;
using MUNityCore.Models.Simulation.Presets;
using MUNity.Schema.Simulation.Resolution;

namespace MUNityCore.Services
{
    public class FrontendSimulationService
    {

        private MUNityCore.DataHandlers.EntityFramework.MunityContext _context;

        private Blazored.LocalStorage.ILocalStorageService _storageService;

        private Services.SpeakerlistService _speakerlistService;

        private Services.SimulationService _simulationService;

        private Services.SpeakerlistHubService _speakerlistHubService;

        private HubConnection _hubConnection;

        private NavigationManager _navigationManager;

        public event EventHandler<SimulationTabs> TabChanged;

        public event EventHandler<List<int>> ConnectedUsersChanged;

        public event EventHandler<MUNity.Schema.Simulation.Voting.SimulationVotingDto> VotingCreated;

        public event EventHandler<MUNity.Schema.Simulation.VotedEventArgs> Voted;

        public event EventHandler<MUNity.Schema.Simulation.AgendaItemDto> AgendaItemCreated;

        public event EventHandler<MUNity.Schema.Simulation.PetitionInfoDto> PetitionAdded;

        private int _currentSimulationId = -1;
        public int CurrentSimulationId
        {
            get => _currentSimulationId;
            set 
            {
                if (_currentSimulationId != value)
                {
                    _currentSimulationId = value;
                    CurrentSimulationIdChanged?.Invoke(this, value);
                }
            }
        }

        public bool IsChair { get; private set; }

        public enum SimulationTabs
        {
            Overview,
            Agenda,
            Voting,
            Resolutions
        }

        private SimulationTabs _currentTab;
        public SimulationTabs CurrentTab 
        { 
            get => _currentTab;
            set
            {
                if (this._currentTab != value)
                {
                    this._currentTab = value;
                    TabChanged?.Invoke(this, value);
                }
            }
        }

        public string CurrentUserToken { get; set; }

        public string CurrentDisplayName { get; set; } = "";

        public string CurrentRoleName { get; set; } = "";

        public string CurrentRoleIso { get; set; } = "un";

        public bool SpeakerlistOpened = false;

        public event EventHandler<int> CurrentSimulationIdChanged;

        public MUNity.Models.ListOfSpeakers.ListOfSpeakers CurrentSpeakerlist { get; set; }

        public void CurrentTabByString(string pageName)
        {
            switch (pageName.ToLower())
            {
                case "start":
                    this.CurrentTab = SimulationTabs.Overview;
                    break;
                case "agenda":
                    this.CurrentTab = SimulationTabs.Agenda;
                    break;
                case "voting":
                    this.CurrentTab = SimulationTabs.Voting;
                    break;
                case "resolution":
                    this.CurrentTab = SimulationTabs.Resolutions;
                    break;
                default:
                    break;
            }
        }

        public async Task<bool> Init(int simulationId)
        {
            var token = await GetUserTokenForSimulation(simulationId);
            if (token == null) return false;

            this.CurrentSimulationId = simulationId;
            this.CurrentUserToken = token;
            var infos = this._context.SimulationUser
                .Include(n => n.Role)
                .FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token);
            this.CurrentDisplayName = infos.DisplayName;
            this.CurrentRoleIso = infos.Role?.Iso ?? "un";
            this.CurrentRoleName = infos.Role?.Name ?? "";
            this.IsChair = infos.Role.RoleType == RoleTypes.Chairman;

            _hubConnection = new HubConnectionBuilder().WithUrl(_navigationManager.BaseUri + "simsocket").Build();

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

            await _hubConnection.StartAsync();

            await _hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.SignIn), this.CurrentSimulationId, infos.SimulationUserId);

            await InitSpeakerlist();
            return true;
        }

        private async Task InitSpeakerlist()
        {
            var listId = this._context.Simulations.Include(n => n.ListOfSpeakers).FirstOrDefault(n => n.SimulationId == this.CurrentSimulationId).ListOfSpeakers?.ListOfSpeakersId;
            if (listId != null)
            {
                this.CurrentSpeakerlist = _speakerlistService.GetSpeakerlist(listId);
            }
            else
            {
                this.CurrentSpeakerlist = this._simulationService.InitListOfSpeakers(this.CurrentSimulationId);
            }

            await this._speakerlistHubService.InitHub(this.CurrentSpeakerlist);
            await _speakerlistHubService.Subscribe(this.CurrentSpeakerlist.ListOfSpeakersId);
            //if (this.CurrentSpeakerlist != null)
            //{
            //    await speakerlistHubService.Subscribe(this.frontService.CurrentSpeakerlist.ListOfSpeakersId, _listOfSpeakersHub.ConnectionId);
            //}
        }

        public async Task AddMeToSpeakerlist()
        {
            if (this.CurrentSpeakerlist == null) return;
            await this._speakerlistHubService.AddSpeaker(this.CurrentSpeakerlist, this.CurrentRoleIso, this.CurrentRoleName);
        }

        private async Task<string> GetUserTokenForSimulation(int simulationId)
        {
            var knownSimulations = await this._storageService.GetItemAsync<List<MUNity.Schema.Simulation.SimulationTokenResponse>>("munity_simsims");
            var token = knownSimulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (token != null)
            {
                return token.Token;
            }
            else
            {
                return null;
            }
        }

        internal async Task CreateVoting(string displayName, bool allowAbstention)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.CreateVotingForDelegates), this.CurrentSimulationId, displayName, allowAbstention);
        }

        internal async Task Vote(string votingId, MUNity.Schema.Simulation.EVoteStates choice)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.Vote), votingId, choice);
        }

        internal async Task CreateAgendaItem(string name, string description)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.CreateAgendaItem), name, description);
        }

        internal async Task SubmitPetition(int agendaItemId, int petitionType)
        {
            await this._hubConnection.SendAsync(nameof(MUNityCore.Hubs.SimulationHub.MakePetition), agendaItemId, petitionType);
        }

        public FrontendSimulationService(MUNityCore.DataHandlers.EntityFramework.MunityContext munityContext,
            Blazored.LocalStorage.ILocalStorageService storageService,
            Services.SpeakerlistService speakerlistService, 
            Services.SimulationService simulationService,
            Services.SpeakerlistHubService speakerlistHubService,
            NavigationManager navigationManager)
        {
            this._context = munityContext;
            this._storageService = storageService;
            this._speakerlistService = speakerlistService;
            this._simulationService = simulationService;
            this._speakerlistHubService = speakerlistHubService;
            this._navigationManager = navigationManager;
        }

    }

}