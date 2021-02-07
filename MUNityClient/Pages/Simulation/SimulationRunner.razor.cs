using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MUNityClient;
using MUNityClient.Shared;
using System.ComponentModel;

namespace MUNityClient.Pages.Simulation
{
    public partial class SimulationRunner
    {
        [Parameter]
        public string Id
        {
            get;
            set;
        }

        private MUNityClient.ViewModel.SimulationViewModel _simulationContext;
        private Services.SocketHandlers.ListOfSpeakerSocketHandler _listOfSpeakerSocket;
        private MUNity.Models.ListOfSpeakers.ListOfSpeakers _listOfSpeakers;
        //private MUNity.Schema.Simulation.SimulationRoleItem _myRole;
        private string _listOfSpeakerId = "_loading_";
        private MUNityClient.Shared.VirtualCommittee.ActiveRoom.ActiveRoomLayoutWrapper _layoutWrapper;
        protected override async Task OnInitializedAsync()
        {
            _layoutWrapper = new Shared.VirtualCommittee.ActiveRoom.ActiveRoomLayoutWrapper();
            _layoutWrapper.PropertyChanged += LayoutChanged;
            int id = 0;
            if (int.TryParse(Id, out id))
            {
                _simulationContext = await simulationService.Subscribe(id);
                if (_simulationContext != null)
                {
                }

                AddHandlers(_simulationContext);
                this._listOfSpeakerId = await this.simulationService.GetListOfSpeakerId(id);
                if (!string.IsNullOrEmpty(_listOfSpeakerId))
                {
                    this._listOfSpeakers = await listOfSpeakerService.GetFromApi(_listOfSpeakerId);
                    if (_listOfSpeakers != null)
                    {
                        _listOfSpeakerSocket = await listOfSpeakerService.Subscribe(_listOfSpeakers);
                        if (_listOfSpeakerSocket != null)
                        {
                        //_listOfSpeakerSocket.SpeakerListChanged += OnSpeakerlistChanged;
                        }
                    }
                }

                this.StateHasChanged();
            }
        }

        private void LayoutChanged(object sender, PropertyChangedEventArgs args)
        {
            this.StateHasChanged();
        }

        private void AddHandlers(MUNityClient.ViewModel.SimulationViewModel context)
        {
            context.UserConnected += OnUserConnected;
            context.UserDisconnected += OnUserDisconnected;
            context.PhaseChanged += OnPhaseChanged;
            context.CurrentResolutionChanged += delegate { this.StateHasChanged(); };
            context.HubConnection.Closed += (ex) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} Verbindung beendet");
                if (ex != null)
                    Console.WriteLine(ex.Message);
                this.StateHasChanged();
                return null;
            };
            context.HubConnection.Reconnected += (ex) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} Reconnected");
                if (ex != null)
                    Console.WriteLine(ex);
                this.StateHasChanged();
                return null;
            };
            context.HubConnection.Reconnecting += (ex) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} Reconnecting");
                if (ex != null)
                    Console.WriteLine(ex.Message);
                this.StateHasChanged();
                return null;
            };
        }

        private void OnPhaseChanged(int sender, MUNity.Schema.Simulation.SimulationEnums.GamePhases phase)
        {
            if (phase == MUNity.Schema.Simulation.SimulationEnums.GamePhases.Lobby)
            {
                navigationManager.NavigateTo($"/sim/lobby/{Id}");
            }
        }

        private void OnUserConnected(int sender, MUNity.Schema.Simulation.SimulationUserItem user)
        {
            if (_simulationContext?.Simulation?.Users != null)
            {
                var tmpUser = _simulationContext.Simulation.Users.FirstOrDefault(n => n.SimulationUserId == user.SimulationUserId);
                if (tmpUser != null)
                {
                    tmpUser.IsOnline = true;
                    tmpUser.DisplayName = user.DisplayName;
                    this.StateHasChanged();
                }
            }
        }

        private void OnUserDisconnected(int sender, MUNity.Schema.Simulation.SimulationUserItem user)
        {
            if (_simulationContext?.Simulation?.Users != null)
            {
                var tmpUser = _simulationContext.Simulation.Users.FirstOrDefault(n => n.SimulationUserId == user.SimulationUserId);
                if (tmpUser != null)
                {
                    tmpUser.IsOnline = false;
                    this.StateHasChanged();
                }
            }
        }

        private async void InitListOfSpeakers()
        {
            if (this._simulationContext?.Simulation != null)
            {
                var list = await this.simulationService.InitListOfSpeakers(this._simulationContext.Simulation.SimulationId);
                if (list != null)
                {
                    this._listOfSpeakerId = list.ListOfSpeakersId;
                    this.StateHasChanged();
                }
            }
        }
    }
}