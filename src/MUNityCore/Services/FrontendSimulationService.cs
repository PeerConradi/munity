using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using MUNity.Models.Simulation;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Voting;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Models.Simulation;
using MUNityCore.Models.Simulation.Presets;
using MUNitySchema.Schema.Simulation.Resolution;

namespace MUNityCore.Services
{
    public class FrontendSimulationService
    {
        private MUNityCore.DataHandlers.EntityFramework.MunityContext _context;

        private Blazored.LocalStorage.ILocalStorageService _storageService;

        private Services.SpeakerlistService _speakerlistService;

        private Services.SimulationService _simulationService;

        private Services.SpeakerlistHubService _speakerlistHubService;

        public event EventHandler<SimulationTabs> TabChanged;

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

        public FrontendSimulationService(MUNityCore.DataHandlers.EntityFramework.MunityContext munityContext,
            Blazored.LocalStorage.ILocalStorageService storageService,
            Services.SpeakerlistService speakerlistService, Services.SimulationService simulationService,
            Services.SpeakerlistHubService speakerlistHubService)
        {
            this._context = munityContext;
            this._storageService = storageService;
            this._speakerlistService = speakerlistService;
            this._simulationService = simulationService;
            this._speakerlistHubService = speakerlistHubService;
        }

    }

}