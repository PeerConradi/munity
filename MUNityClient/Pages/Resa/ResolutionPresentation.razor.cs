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
using MUNity.Models.Resolution;

namespace MUNityClient.Pages.Resa
{
    public partial class ResolutionPresentation
    {
        [Parameter]
        public string Id
        {
            get;
            set;
        }

        public Resolution Resolution
        {
            get;
            set;
        }

        public ViewModel.ResolutionViewModel ResolutionViewModel { get; set; }

        private enum SyncModes
        {
            Loading,
            FromStorage,
            FromServer
        }

        private SyncModes SyncMode = SyncModes.Loading;
        protected override async Task OnInitializedAsync()
        {
            var localResolution = await this.resolutionService.GetStoredResolution(this.Id);
            if (localResolution != null)
            {

            }
            else
            {
                var onlineResolution = await this.resolutionService.GetResolutionFromServer(this.Id);
                if (onlineResolution != null)
                {

                    this.ResolutionViewModel = await this.resolutionService.Subscribe(onlineResolution);
                }
            }
        }

        /// <summary>
        /// Reloads the Resolution and fires a State changed.
        /// </summary>
        private async void RefreshFromStorage()
        {
            if (Resolution != null)
            {
                this.Resolution = await this.resolutionService.GetStoredResolution(this.Resolution.ResolutionId);
                this.StateHasChanged();
            }
        }
    }
}