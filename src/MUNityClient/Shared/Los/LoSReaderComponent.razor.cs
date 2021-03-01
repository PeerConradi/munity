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
using MUNity.Models.ListOfSpeakers;

namespace MUNityClient.Shared.Los
{
    public partial class LoSReaderComponent
    {
        [Parameter]
        public string ListOfSpeakersId { get; set; }

        private ViewModels.ListOfSpeakerViewModel ViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.ViewModel = await ViewModels.ListOfSpeakerViewModel.CreateViewModel(listOfSpeakerService, ListOfSpeakersId);

            if (this.ViewModel != null)
            {
                this.ViewModel.Handler.Paused += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.QuestionSecondsAdded += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.SpeakerSecondsAdded += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.SpeakerAdded += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.SpeakerRemoved += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.ClearSpeaker += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.ClearQuestion += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.NextSpeakerPushed += delegate { this.StateHasChanged(); };
                this.ViewModel.Handler.NextQuestionPushed += delegate { this.StateHasChanged(); };
            }

            await base.OnInitializedAsync();
        }
    }
}