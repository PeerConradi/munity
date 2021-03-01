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

namespace MUNityClient.Pages.Resa
{
    public partial class ResolutionHome
    {
        private bool loadingLocalsDone = false;
        private bool backendReady = false;
        private List<MUNityClient.Models.Resolution.ResolutionInfo> localResolutions;
        private async Task CreateLocalResolution()
        {
            var resolution = await this.resolutionService.CreateResolution("Neue lokale Resolution");
            if (resolution != null)
            {
                navigationManager.NavigateTo($"resa/edit/{resolution.ResolutionId}");
            }
        }

        private async Task CreatePublicResolution()
        {
            var resolutionId = await this.resolutionService.CreatePublicResolution();
            if (resolutionId != null)
            {
                navigationManager.NavigateTo($"resa/edit/{resolutionId}");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            this.localResolutions = await resolutionService.GetStoredResolutions();
            this.loadingLocalsDone = true;
            this.backendReady = await resolutionService.IsOnline(true);
        }
    }
}