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

        protected override void OnInitialized()
        {
            if (ViewModel != null)
            {
                ViewModel.UserConnected += delegate { this.StateHasChanged(); };
                ViewModel.UserDisconnected += delegate { this.StateHasChanged(); };
                ViewModel.UserRoleChanged += delegate { this.StateHasChanged(); };
            }
            base.OnInitialized();
        }

    }
}