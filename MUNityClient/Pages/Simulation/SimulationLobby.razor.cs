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

namespace MUNityClient.Pages.Simulation
{
    public partial class SimulationLobby
    {
        [Parameter]
        public string Id
        {
            get;
            set;
        }

        private MUNityClient.ViewModel.SimulationViewModel _context = null;

        protected override async Task OnInitializedAsync()
        {
            int id = 0;
            if (int.TryParse(Id, out id))
            {
                this._context = await simulationService.Subscribe(id);
                if (_context != null && _context.Simulation.Phase == MUNity.Schema.Simulation.GamePhases.Online)
                {
                    navigationManager.NavigateTo($"/sim/run/{id}");
                }
            }
        //return base.OnInitializedAsync();
        }
    }
}