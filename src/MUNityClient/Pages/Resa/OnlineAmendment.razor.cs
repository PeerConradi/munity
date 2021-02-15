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
    public partial class OnlineAmendment
    {
        [Parameter]
        public string Id
        {
            get;
            set;
        }

        private MUNityClient.Shared.Bootstrap.Modal AddAmendmentModal
        {
            get;
            set;
        }

        private MUNityClient.Shared.Resa.NewAmendmentForm NewAmendmentForm
        {
            get;
            set;
        }

        private enum EStatus
        {
            Loading,
            CantReachServer,
            NotAllowed,
            Allowed
        }

        private Resolution _resolution
        {
            get;
            set;
        }

        private EStatus Status
        {
            get;
            set;
        }

        = EStatus.Loading;
        protected override async Task OnInitializedAsync()
        {
            var canEdit = await this.resolutionService.CanUserPostAmendments(Id);
            if (!canEdit)
            {
                this.Status = EStatus.NotAllowed;
            }
            else
            {
                _resolution = await this.resolutionService.GetResolutionFromServer(Id);
                this.Status = EStatus.Allowed;
            }

            StateHasChanged();
        }

        private void ShowNewAmendmentModal()
        {
            this.AddAmendmentModal.Open();
        }

        private async void NewAmendment()
        {
            var amendment = this.NewAmendmentForm.GetAmendment();
            if (amendment == null)
            {
            // TODO: Meldung zeigen Resolution konnte nicht erstellt werden!
            }
            else if (amendment is DeleteAmendment deleteAmendment)
            {
                await this.resolutionService.PostDeleteAmendment(this._resolution.ResolutionId, deleteAmendment);
            }

            this.AddAmendmentModal.Close();
        }
    }
}