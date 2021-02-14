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
    public partial class UserListItemAdmin
    {
        [Parameter]
        public MUNity.Schema.Simulation.SimulationUserAdminDto User
        {
            get;
            set;
        }

        [Parameter]
        public int SimulationId
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

        [Parameter]
        public Boolean PasswordShown
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback<int> UserRemoveClicked
        {
            get;
            set;
        }

        private int _roleId;
        public int RoleId
        {
            get => _roleId;
            set
            {
                if (_roleId != value)
                {
                    this.simulationService.SetUserRole(this.SimulationId, this.User.SimulationUserId, value).ConfigureAwait(false);
                    _roleId = value;
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            this._roleId = User.RoleId;
            if (Roles == null)
            {
                this.Roles = await this.simulationService.GetRoles(SimulationId);
            }
        }

        private void CopyPasswordToClippboard()
        {
            JS.InvokeAsync<string>("navigator.clipboard.writeText", @User.Password);
        }

        private void CopyUserIDToClippboard()
        {
            JS.InvokeAsync<string>("navigator.clipboard.writeText", @User.PublicId);
        }
    }
}