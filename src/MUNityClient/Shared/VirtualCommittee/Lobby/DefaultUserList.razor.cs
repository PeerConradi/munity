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
using MUNity.Schema.Simulation;

namespace MUNityClient.Shared.VirtualCommittee.Lobby
{
    public partial class DefaultUserList
    {
        [Parameter]
        public MUNityClient.ViewModels.SimulationViewModel ViewModel
        {
            get;
            set;
        }

        public IEnumerable<MUNity.Schema.Simulation.SimulationUserDefaultDto> Users
        {
            get;
            set;
        }

        protected override async Task OnInitializedAsync()
        {
            if (ViewModel != null)
            {
                ViewModel.UserConnected += OnUserConnected;
                ViewModel.UserDisconnected += OnUserDisconnected;
                ViewModel.UserRoleChanged += OnUserRoleChanged;
            }

            this.Users = await _simulationService.GetUsers(ViewModel.Simulation.SimulationId);
        }

        private void OnUserRoleChanged(object sender, UserRoleChangedEventArgs args)
        {
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
    }
}