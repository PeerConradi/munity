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

        private enum SyncModes
        {
            Loading,
            FromStorage,
            FromServer
        }

        private SyncModes SyncMode = SyncModes.Loading;
        protected override async Task OnInitializedAsync()
        {
            var resolution = await this.resolutionService.GetResolution(this.Id);
            if (resolution != null)
            {
                this.Resolution = resolution;
                // Die Resolution wurde schon gefunden, wahrscheinlich im local Storage
                // Jetzt wird noch einmal geschaut ob die Resolution online existiert und
                // gelesen werden kann
                _ = this.resolutionService.IsOnline().ContinueWith(async (result) =>
                {
                    if (result.Result)
                    {
                        // Server ist online schaue nach ob dort eine Version existiert und gelesen werden kann
                        if (await this.resolutionService.CanReadResolution(Id))
                        {
                            this.Resolution = await this.resolutionService.GetResolutionFromServer(Id);
                            var socket = await this.resolutionService.Subscribe(resolution);
                            this.SyncMode = SyncModes.FromServer;
                        }
                    }
                    else
                    {
                        this.resolutionService.StorageChanged += RefreshFromStorage;
                        this.SyncMode = SyncModes.FromStorage;
                    }

                    this.StateHasChanged();
                });
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