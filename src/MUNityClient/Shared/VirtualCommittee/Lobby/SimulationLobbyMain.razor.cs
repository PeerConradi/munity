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
        public MUNityClient.ViewModels.SimulationViewModel ViewModel { get; set; } = null;

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
            get => ViewModel.Simulation.Users.FirstOrDefault(n => n.SimulationUserId == ViewModel.MyAuth.SimulationUserId).RoleId;
            set
            {
                SelectRole(value).ConfigureAwait(false);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (ViewModel.Simulation != null)
            {
                AppendEvents(ViewModel);
                ViewModel.RolesChanged += SimulationContext_RolesChanged;
            }

            this.PetitionTemplates = await this.simulationService.GetPetitionPresetNames();
            
            //this.simulationService.GetPresets()
            if (PetitionTemplates.Any())
            {
                this.SelectedPetitionTemplate = PetitionTemplates.First();
            }
            await base.OnInitializedAsync();   
        }

        private void SimulationContext_RolesChanged(int sender, IEnumerable<MUNity.Schema.Simulation.SimulationRoleDto> roles)
        {
            this.StateHasChanged();
        }

        private async Task ApplyPetitionPreset()
        {
            if (this.ViewModel == null) return;
            await this.simulationService.ApplyPetitionTemplate(this.ViewModel.Simulation.SimulationId, SelectedPetitionTemplate);
            this.StateHasChanged();
        }

        private void AppendEvents(MUNityClient.ViewModels.SimulationViewModel context)
        {
            context.PhaseChanged += PhaseChanged;
        }

        private void PhaseChanged(int sender, MUNity.Schema.Simulation.GamePhases phase)
        {
            if (phase == MUNity.Schema.Simulation.GamePhases.Online)
            {
                navigationManager.NavigateTo($"/sim/run/{ViewModel.Simulation.SimulationId}");
            }
        }

        private async Task SelectRole(int roleId)
        {
            await this.simulationService.PickRole(ViewModel.Simulation.SimulationId, roleId);
        }
    }
}