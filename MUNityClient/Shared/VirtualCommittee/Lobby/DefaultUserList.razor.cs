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
    public partial class DefaultUserList
    {
        [Parameter]
        public MUNityClient.ViewModel.SimulationViewModel SimulationContext
        {
            get;
            set;
        }

        public IEnumerable<MUNity.Schema.Simulation.SimulationUserDefaultDto> Users
        {
            get;
            set;
        }

        [Parameter]
        public IEnumerable<MUNity.Schema.Simulation.SimulationRoleDto> Roles
        {
            get;
            set;
        }

        protected override async Task OnInitializedAsync()
        {
            if (SimulationContext != null)
            {
                SimulationContext.UserConnected += OnUserConnected;
                SimulationContext.UserDisconnected += OnUserDisconnected;
                SimulationContext.UserRoleChanged += OnUserRoleChanged;
            }

            if (Roles == null)
            {
                this.Roles = await _simulationService.GetRoles(SimulationContext.Simulation.SimulationId);
            }

            this.Users = await _simulationService.GetUsers(SimulationContext.Simulation.SimulationId);
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
    }
}