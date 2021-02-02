using System;
using System.Collections.Generic;
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
using MUNityClient.Models.Simulation;
using System.Linq;

namespace MUNityClient.Shared.VirtualCommittee.Lobby
{
    public partial class SimulationLobbyMain
    {
        [Parameter]
        public MUNityClient.ViewModel.SimulationViewModel SimulationContext { get; set; } = null;
        private int SelectedRole
        {
            get => SimulationContext.Simulation.Users.FirstOrDefault(n => n.SimulationUserId == SimulationContext.MyAuth.SimulationUserId).RoleId;
            set
            {
                SelectRole(value).ConfigureAwait(false);
            }
        }

        protected override void OnInitialized()
        {
            if (SimulationContext.Simulation != null)
            {
                AppendEvents(SimulationContext);
            }

            base.OnInitialized();
        }

        private void AppendEvents(MUNityClient.ViewModel.SimulationViewModel context)
        {
            context.PhaseChanged += PhaseChanged;
        }

        private void PhaseChanged(int sender, MUNity.Schema.Simulation.SimulationEnums.GamePhases phase)
        {
            if (phase == MUNity.Schema.Simulation.SimulationEnums.GamePhases.Online)
            {
                navigationManager.NavigateTo($"/sim/run/{SimulationContext.Simulation.SimulationId}");
            }
        }

        private async Task SelectRole(int roleId)
        {
            await this.simulationService.PickRole(SimulationContext.Simulation.SimulationId, roleId);
        }
    }
}