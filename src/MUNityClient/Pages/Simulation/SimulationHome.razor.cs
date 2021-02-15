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

namespace MUNityClient.Pages.Simulation
{
    public partial class SimulationHome
    {
        private enum ConnectionStates
        {
            Connecting,
            Connected,
            CantReachServer
        }

        private MUNityClient.Shared.Bootstrap.Modal _passwordModal;
        private ConnectionStates _connectionState
        {
            get;
            set;
        }

        = ConnectionStates.Connecting;
        private ICollection<MUNity.Schema.Simulation.SimulationTokenResponse> _tokens
        {
            get;
            set;
        }

        private ICollection<MUNity.Schema.Simulation.SimulationListItemDto> _simulations
        {
            get;
            set;
        }

        private MUNity.Schema.Simulation.SimulationListItemDto _selectedSimulation;
        private MUNity.Schema.Simulation.JoinAuthenticate _joinForm = new MUNity.Schema.Simulation.JoinAuthenticate();

        private void EnterSimulation(int id)
        {
            _joinForm.SimulationId = id;
            var fittingToken = _tokens?.FirstOrDefault(n => n.SimulationId == id);
            if (fittingToken == null)
            {
                this._selectedSimulation = this._simulations.FirstOrDefault(n => n.SimulationId == id);
                if (_selectedSimulation != null)
                {
                    // Draw the Modal
                    this.StateHasChanged();
                    _joinForm.SimulationId = id;
                    // Aks for password
                    _passwordModal.Open();
                }
            }
            else
            {
                // TODO: Zustand anfragen und dann entweder in die Lobby oder direkt ins Game!
                navigation.NavigateTo($"/sim/lobby/{id}");
            }
        }

        private async Task DeleteToken(int id)
        {
            await this.simulationService.RemoveToken(id);
            var token = _tokens.FirstOrDefault(n => n.SimulationId == id);
            if (token != null)
                _tokens.Remove(token);
        }

        private async Task JoinSimulation()
        {
            _passwordModal.Close();
            var pinToken = await this.simulationService.JoinSimulation(_joinForm);
            if (pinToken != null)
            {
                if (this._tokens == null)
                    this._tokens = new List<MUNity.Schema.Simulation.SimulationTokenResponse>();
                this._tokens.Add(pinToken);
                this.StateHasChanged();
            }
            else
            {
            // TODO: Beitritt verweigert Meldung!
            }
        //this.navigation.NavigateTo($"/sim/lobby/{_selectedSimulation.SimulationId}");
        }

        private void RootToSimulation()
        {
            this.navigation.NavigateTo($"/sim/lobby/{_selectedSimulation.SimulationId}");
        }

        protected async override Task OnInitializedAsync()
        {
            var serverOnline = await this.simulationService.IsOnline();
            this._connectionState = (serverOnline) ? ConnectionStates.Connected : ConnectionStates.CantReachServer;
            if (serverOnline)
            {
                _tokens = await simulationService.GetStoredTokens();
                _simulations = await simulationService.GetSimulationList();
            }
        }
    }
}