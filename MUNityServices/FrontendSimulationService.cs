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
using MUNityCore.ViewModel;

namespace MUNityCore.Services
{
    public class FrontendSimulationService : IDisposable
    {
        private MUNityCore.DataHandlers.EntityFramework.MunityContext _context;

        private Blazored.LocalStorage.ILocalStorageService _storageService;

        private Services.SpeakerlistService _speakerlistService;

        private Services.SimulationService _simulationService;

        private Services.SpeakerlistHubService _speakerlistHubService;

        private NavigationManager _navigationManager;

        public event EventHandler<SimulationTabs> TabChanged;

        private List<SimulationViewModel> _viewModels;

        public event EventHandler<SimulationViewModel> CurrentSimulationChanged;

        public ViewModel.ResolutionViewModel CurrentResolution;

        public enum SimulationTabs
        {
            Overview,
            Agenda,
            Voting,
            Resolutions,
            Protocol,
            Presents
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
                    this.StoreCurrentTab().ConfigureAwait(false);
                    TabChanged?.Invoke(this, value);
                }
            }
        }

        public SimulationViewModel CurrentSimulation => _viewModels?.FirstOrDefault();

        public bool SpeakerlistOpened = false;

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

        internal async Task<SimulationViewModel> GetOrInitViewModelWildcard(int simulationId)
        {
            var viewModel = this._viewModels.FirstOrDefault(n => n.SimulationId == simulationId);
            if (viewModel != null)
                return viewModel;

            viewModel = await SimulationViewModel.Init(_navigationManager.BaseUri + "simsocket");
            viewModel.SimulationId = simulationId;
            this._viewModels.Add(viewModel);
            string speakerlistId = _context.Simulations.Where(n => n.SimulationId == simulationId).Select(n => n.ListOfSpeakers.ListOfSpeakersId).FirstOrDefault();
            if (speakerlistId != null)
            {
                var speakerlistModel = await _speakerlistHubService.GetSpeakerlistViewModel(speakerlistId);
                viewModel.SpeakerlistViewModel = speakerlistModel;
            }
            return viewModel;
        }

        internal async Task EnsureUserContext(SimulationViewModel viewModel)
        {
            var token = await GetUserTokenForSimulation(viewModel.SimulationId);
            if (token == null)
            {
                return;
            }

            var infos = this._context.SimulationUser
                .Include(n => n.Role)
                .FirstOrDefault(n => n.Simulation.SimulationId == viewModel.SimulationId && n.Token == token);
            if (infos != null)
            {
                var userContext = new SimulationUserContext()
                {
                    DisplayName = infos.DisplayName,
                    RoleIso = infos.Role?.Iso ?? "un",
                    RoleName = infos.Role?.Name ?? "",
                    RoleType = infos.Role.RoleType,
                    UserId = infos.SimulationUserId,
                    Token = infos.Token
                };
                viewModel.UserContext = userContext;
                await viewModel.SignIn();
            }
        }

        internal async Task<SimulationViewModel> GetOrInitViewModel(int simulationId)
        {

            var viewModel = this._viewModels.FirstOrDefault(n => n.SimulationId == simulationId);
            if (viewModel != null)
                return viewModel;

            var token = await GetUserTokenForSimulation(simulationId);
            if (token == null) return null;

            var infos = this._context.SimulationUser
                .Include(n => n.Role)
                .FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token);
            if (infos != null)
            {
                viewModel = await SimulationViewModel.Init(_navigationManager.BaseUri + "simsocket");
                viewModel.SimulationId = simulationId;
                await EnsureUserContext(viewModel);
                
                this._viewModels.Add(viewModel);
                if (this._viewModels.Count == 1)
                    CurrentSimulationChanged?.Invoke(this, viewModel);

                string speakerlistId = _context.Simulations.Where(n => n.SimulationId == simulationId).Select(n => n.ListOfSpeakers.ListOfSpeakersId).FirstOrDefault();
                if (speakerlistId != null)
                {
                    var speakerlistModel = await _speakerlistHubService.GetSpeakerlistViewModel(speakerlistId);
                    viewModel.SpeakerlistViewModel = speakerlistModel;
                }
                
            }
            return viewModel;
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
        

        internal async Task<SimulationTokenResponse> CheckForToken(int simulationId)
        {
            var savedTokens = await this._storageService.GetItemAsync<List<SimulationTokenResponse>>("munity_simsims");
            if (savedTokens == null || savedTokens.Count == 0)
                return null;

            return savedTokens.FirstOrDefault(n => n.SimulationId == simulationId);
        }

        internal async Task StoreSimulationToken(SimulationTokenResponse simulationResponse)
        {
            var savedTokens = await GetStoredSimulations();
            bool modified = false;
            if (savedTokens.Any())
            {
                var existing = savedTokens.FirstOrDefault(n => n.SimulationId == simulationResponse.SimulationId);
                if (existing != null)
                {
                    if (existing.Name != simulationResponse.Name)
                    {
                        existing.Name = simulationResponse.Name;
                        modified = true;
                    }
                        
                    if (existing.Token != simulationResponse.Token)
                    {
                        existing.Token = simulationResponse.Token;
                        modified = true;
                    }
                }
                else
                {
                    savedTokens.Add(simulationResponse);
                    modified = true;
                }
            }
            else
            {
                savedTokens.Add(simulationResponse);
                modified = true;
            }
            if (modified)
            {
                await this._storageService.SetItemAsync("munity_simsims", savedTokens);
            }
        }

        internal async Task RemoveSimulationTokensForSimulation(int simulationId)
        {
            var tokens = await GetStoredSimulations();
            var toDelete = tokens.Where(n => n.SimulationId == simulationId);
            foreach(var del in toDelete)
            {
                tokens.Remove(del);
            }
            await this._storageService.SetItemAsync("munity_simsims", tokens);
        }

        internal async Task<List<SimulationTokenResponse>> GetStoredSimulations()
        {
            var list = await this._storageService.GetItemAsync<List<MUNity.Schema.Simulation.SimulationTokenResponse>>("munity_simsims");
            if (list == null)
                list = new List<SimulationTokenResponse>();
            return list;
        }

        internal async Task StoreCurrentTab()
        {
            await this._storageService.SetItemAsync<SimulationTabs>("munity_currentTab", this.CurrentTab);
        }

        internal async Task<SimulationTabs?> LoadLastTab()
        {
            return await this._storageService.GetItemAsync<SimulationTabs?>("munity_currentTab");
        }

        internal async Task<string> GetLastOpenedResolutionId()
        {
            return await this._storageService.GetItemAsync<string>("munity_lastResolution");
        }

        internal async Task SaveResolutionOpenedId(string resolutionId)
        {
            await this._storageService.SetItemAsync<string>("munity_lastResolution", resolutionId);
        }

        public void Dispose()
        {
            foreach(var viewModel in _viewModels)
            {
                viewModel.Dispose();
            }

            if (CurrentResolution != null)
            {
                CurrentResolution.Dispose();
            }
        }

        public FrontendSimulationService(MUNityCore.DataHandlers.EntityFramework.MunityContext context,
            Blazored.LocalStorage.ILocalStorageService storageService,
            Services.SpeakerlistService speakerlistService, 
            Services.SimulationService simulationService,
            Services.SpeakerlistHubService speakerlistHubService,
            NavigationManager navigationManager)
        {
            this._context = context;
            this._storageService = storageService;
            this._speakerlistService = speakerlistService;
            this._simulationService = simulationService;
            this._speakerlistHubService = speakerlistHubService;
            this._navigationManager = navigationManager;
            this._viewModels = new List<SimulationViewModel>();
        }

    }

}