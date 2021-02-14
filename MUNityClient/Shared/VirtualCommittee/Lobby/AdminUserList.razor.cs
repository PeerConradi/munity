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
        public MUNityClient.ViewModel.SimulationViewModel SimulationContext
        {
            get;
            set;
        }

        private Boolean showPasswords = false;
        public List<MUNity.Schema.Simulation.SimulationUserAdminDto> Users
        {
            get;
            set;
        }

        [Parameter]
        public List<MUNity.Schema.Simulation.SimulationRoleDto> Roles
        {
            get;
            set;
        }

        protected override async Task OnInitializedAsync()
        {
            this.Users = await _simulationService.GetUserSetups(SimulationContext.Simulation.SimulationId);
            if (Roles == null)
            {
                this.Roles = await _simulationService.GetRoles(SimulationContext.Simulation.SimulationId);
            }

            if (SimulationContext != null)
            {
                SimulationContext.RolesChanged += OnRolesChanged;
                SimulationContext.UserConnected += OnUserConnected;
                SimulationContext.UserDisconnected += OnUserDisconnected;
                SimulationContext.UserRoleChanged += OnUserRoleChanged;
            }
        }

        private void OnUserRoleChanged(int sender, int userId, int roleId)
        {
            if (this.Users == null)
                return;
            var user = Users.FirstOrDefault(n => n.SimulationUserId == userId);
            if (user != null)
            {
                user.RoleId = roleId;
                this.StateHasChanged();
            }
        }

        private async Task<EventCallback> OnUserRemoved(int userId)
        {
            await this._simulationService.RemoveUser(this.SimulationContext.Simulation.SimulationId, userId);
            this.Users.RemoveAll(n => n.SimulationUserId == userId);
            return EventCallback.Empty;
        }

        private void OnRolesChanged(int sender, IEnumerable<MUNity.Schema.Simulation.SimulationRoleDto> roles)
        {
            this.Roles = roles.ToList();
            this.StateHasChanged();
        }

        private void OnUserConnected(int sender, MUNity.Schema.Simulation.SimulationUserDefaultDto usr)
        {
            var user = Users.FirstOrDefault(n => n.SimulationUserId == usr.SimulationUserId);
            if (user != null)
            {
                user.IsOnline = true;
                if (!string.IsNullOrEmpty(usr.DisplayName) && usr.DisplayName != user.DisplayName)
                    user.DisplayName = usr.DisplayName;
                StateHasChanged();
            }
        }

        private void OnUserDisconnected(int sender, MUNity.Schema.Simulation.SimulationUserDefaultDto usr)
        {
            var user = Users.FirstOrDefault(n => n.SimulationUserId == usr.SimulationUserId);
            if (user != null)
            {
                user.IsOnline = false;
                this.StateHasChanged();
            }
        }

        private async Task NewUser()
        {
            var user = await _simulationService.CreateUser(this.SimulationContext.Simulation.SimulationId);
            if (user != null)
            {
                Users.Add(user);
            }
        }

        private void CopyListToClipboard()
        {
            var zeilen = Users.Select(n => $"{n.DisplayName ?? "Freier Slot"}\t{n.PublicId}\t{n.Password}\t{Roles.FirstOrDefault(a => a.SimulationRoleId == n.RoleId)?.Name ?? "Keine Rolle"}");
            string ausgabe = "Benutzer\tUserId\tPasswort\tRolle\n";
            ausgabe += string.Join('\n', zeilen);
            JS.InvokeAsync<string>("navigator.clipboard.writeText", ausgabe);
        }
    }
}