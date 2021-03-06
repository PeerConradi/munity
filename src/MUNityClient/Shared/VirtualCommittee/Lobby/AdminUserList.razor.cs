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
    public partial class AdminUserList
    {
        [Parameter]
        public MUNityClient.ViewModels.SimulationViewModel ViewModel { get; set; }


        public List<MUNity.Schema.Simulation.SimulationUserAdminDto> Users
        {
            get;
            set;
        }

        protected override Task OnInitializedAsync()
        {
            if (ViewModel != null)
            {
                ViewModel.RolesChanged += delegate { this.StateHasChanged(); };
                ViewModel.UserConnected += delegate { this.StateHasChanged(); };
                ViewModel.UserDisconnected += delegate { this.StateHasChanged(); };
                ViewModel.UserRoleChanged += delegate { this.StateHasChanged(); };
                ViewModel.AdminUsers.CollectionChanged += delegate { this.StateHasChanged(); };
            }
            return base.OnInitializedAsync();
        }

        private async Task<EventCallback> OnUserRemoved(int userId)
        {
            await this._simulationService.RemoveUser(this.ViewModel.Simulation.SimulationId, userId);
            this.Users.RemoveAll(n => n.SimulationUserId == userId);
            return EventCallback.Empty;
        }

        private async Task NewUser()
        {
            await this.ViewModel.CreateUser();
            this.StateHasChanged();
        }

        private void CopyListToClipboard()
        {
            var zeilen = Users.Select(n => $"{n.DisplayName ?? "Freier Slot"}\t{n.PublicId}\t{n.Password}\t{ViewModel.Simulation.Roles.FirstOrDefault(a => a.SimulationRoleId == n.RoleId)?.Name ?? "Keine Rolle"}");
            string ausgabe = "Benutzer\tUserId\tPasswort\tRolle\n";
            ausgabe += string.Join('\n', zeilen);
            JS.InvokeAsync<string>("navigator.clipboard.writeText", ausgabe);
        }
    }
}