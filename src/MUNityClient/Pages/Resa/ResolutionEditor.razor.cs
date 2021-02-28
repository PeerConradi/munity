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

        public ViewModels.ResolutionViewModel ViewModel { get; set; }

        private MUNityClient.Shared.Bootstrap.Modal AddAmendmentModal { get; set; }

        private MUNityClient.Shared.Resa.NewAmendmentForm NewAmendmentForm { get; set; }


        private void NewAmendment()
        {
            var amendment = this.NewAmendmentForm.GetAmendment();
            if (amendment == null)
            {
                // TODO: Meldung zeigen Resolution konnte nicht erstellt werden!
            }
            this.AddAmendmentModal.Close();
        }

        public void AmendmentInteracted()
        {
            this.StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            var localResolution = await this.resolutionService.GetStoredResolution(Id);
            if (localResolution != null)
            {
                this.ViewModel = ViewModels.ResolutionViewModel.CreateViewModelOffline(localResolution, resolutionService);
                this.ViewModel.SyncMode = ViewModels.ResolutionViewModel.SyncModes.Offline;
                this.StateHasChanged();
            }
            else
            {
                // Search online
                var onlineResolution = await this.resolutionService.GetResolutionFromServer(Id);
                if (onlineResolution != null)
                {
                    this.ViewModel = await this.resolutionService.Subscribe(onlineResolution);
                    this.ViewModel.SyncMode = ViewModels.ResolutionViewModel.SyncModes.OnlineButNotSyncing;
                    this.ViewModel.PreambleChangedFromExtern += delegate { this.StateHasChanged(); };
                    this.ViewModel.OperativeSeciontChangedFromExtern += delegate { this.StateHasChanged(); };
                    //onlineResolution.Preamble.Paragraphs.CollectionChanged += delegate { this.StateHasChanged(); };

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