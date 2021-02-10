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
using System.Linq;

namespace MUNityClient.Shared.VirtualCommittee.Lobby
{
    public partial class SimulationLobbyMain
    {
        [Parameter]
        public MUNityClient.ViewModel.SimulationViewModel SimulationContext { get; set; } = null;

        public List<string> PetitionTemplates { get; set; }

        public string SelectedPetitionTemplate { get; set; }

        private bool SubmitFormSuccess { get; set; } = false;

        public enum Views
        {
            Users,
            Roles,
            Petitions
        }

        public Views CurrentView { get; set; } = Views.Users;

        private int SelectedRole
        {
            get => SimulationContext.Simulation.Users.FirstOrDefault(n => n.SimulationUserId == SimulationContext.MyAuth.SimulationUserId).RoleId;
            set
            {
                SelectRole(value).ConfigureAwait(false);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (SimulationContext.Simulation != null)
            {
                AppendEvents(SimulationContext);
                SimulationContext.RolesChanged += SimulationContext_RolesChanged;
                if (this.SimulationContext.PetitionTypes == null)
                    this.SimulationContext.PetitionTypes = await this.simulationService.PetitionTypes(this.SimulationContext.Simulation.SimulationId);
            }

            this.PetitionTemplates = await this.simulationService.GetPetitionPresetNames();
            
            //this.simulationService.GetPresets()
            if (PetitionTemplates.Any())
            {
                this.SelectedPetitionTemplate = PetitionTemplates.First();
            }
            await base.OnInitializedAsync();   
        }

        private void SimulationContext_RolesChanged(int sender, IEnumerable<MUNity.Schema.Simulation.SimulationRoleItem> roles)
        {
            this.StateHasChanged();
        }

        private async Task ApplyPetitionPreset()
        {
            if (this.SimulationContext == null) return;
            await this.simulationService.ApplyPetitionTemplate(this.SimulationContext.Simulation.SimulationId, SelectedPetitionTemplate);
            this.SimulationContext.PetitionTypes = await this.simulationService.PetitionTypes(this.SimulationContext.Simulation.SimulationId);
            this.StateHasChanged();
        }

        private void AppendEvents(MUNityClient.ViewModel.SimulationViewModel context)
        {
            context.PhaseChanged += PhaseChanged;
        }

        private void PhaseChanged(int sender, MUNity.Schema.Simulation.GamePhases phase)
        {
            if (phase == MUNity.Schema.Simulation.GamePhases.Online)
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