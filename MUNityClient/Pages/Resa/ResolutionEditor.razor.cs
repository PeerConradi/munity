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
using MUNity.Extensions.ResolutionExtensions;
using MUNityClient.Extensions;
using MUNityClient.Shared.Resa;

namespace MUNityClient.Pages.Resa
{
    public partial class ResolutionEditor
    {
        [Parameter]
        public string Id { get; set; }

        private Boolean fetchingResolutionErrored = false;
        private bool fullUpdate = false;
        private ViewModel.ResolutionViewModel _viewModel;

        protected override async Task OnInitializedAsync()
        {
            var localResolution = await this.resolutionService.GetStoredResolution(Id);
            if (localResolution != null)
            {
                this._viewModel = ViewModel.ResolutionViewModel.CreateViewModelOffline(localResolution, resolutionService);
                this._viewModel.SyncMode = ViewModel.ResolutionViewModel.SyncModes.Offline;
                this.StateHasChanged();
            }
            else
            {
                // Search online
                var onlineResolution = await this.resolutionService.GetResolutionFromServer(Id);
                if (onlineResolution != null)
                {
                    this._viewModel = await this.resolutionService.Subscribe(onlineResolution);
                    this._viewModel.SyncMode = ViewModel.ResolutionViewModel.SyncModes.OnlineButNotSyncing;
                }
                else
                {
                    this.fetchingResolutionErrored = true;
                }
                
                this.StateHasChanged();
            }
        }
    }
}