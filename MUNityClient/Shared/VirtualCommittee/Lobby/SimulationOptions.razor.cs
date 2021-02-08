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

namespace MUNityClient.Shared.VirtualCommittee.Lobby
{
    public partial class SimulationOptions
    {
        [Parameter]
        public MUNityClient.ViewModel.SimulationViewModel SimulationContext { get; set; } = null;
        private IEnumerable<Models.Simulation.SimulationPreset> _presets { get; set; }

        private Models.Simulation.SimulationPreset SelectedPreset { get; set; }

        private string SelectedPresetId { get => SelectedPreset?.Id ?? ""; set => SelectedPreset = _presets.FirstOrDefault(n => n.Id == value); }

        private async Task StartPhase()
        {
            var response = await this.simulationService.SetPhase(SimulationContext.Simulation.SimulationId, MUNity.Schema.Simulation.SimulationEnums.GamePhases.Online);
        }

        private async Task ActivatePreset()
        {
            if (SimulationContext.Simulation != null)
            {
                await this.simulationService.ApplyPreset(SimulationContext.Simulation.SimulationId, SelectedPresetId);
                SimulationContext.Simulation.Roles = await this.simulationService.GetRoles(SimulationContext.Simulation.SimulationId);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (this.SimulationContext != null)
            {
                this._presets = await this.simulationService.GetPresets();
            }
        }
    }
}