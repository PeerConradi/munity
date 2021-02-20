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
using MUNity.Extensions.LoSExtensions;
using System.Timers;
using System.ComponentModel.DataAnnotations;
using MUNityClient.Models.ListOfSpeaker;

namespace MUNityClient.Shared.Los
{
    public partial class LoSEditor
    {
        [Parameter]
        public string Id { get; set; }

        private MUNityClient.Shared.Bootstrap.Modal SpeakingTimeConfigModal
        {
            get;
            set;
        }



        [Parameter]
        public ViewModels.ListOfSpeakerViewModel ViewModel
        {
            get;
            set;
        }

        private SpeakerlistSettings Settings
        {
            get;
            set;
        }

        
        
        private Boolean invalidFormatSpeakerTime = false;
        private Boolean invalidFormatQuestionTime = false;
        private void SaveSettings()
        {
            var speakerTime = ViewModel.SourceList.SpeakerTime;
            var questionTime = ViewModel.SourceList.QuestionTime;
            if (TimeSpan.TryParseExact(Settings.Speakertime, @"mm\:ss", null, out speakerTime))
            {
                invalidFormatSpeakerTime = false;
                invalidFormatQuestionTime = false;
                this.ViewModel.SourceList.SpeakerTime = speakerTime;
            }
            else
            {
                // Die Eingabe der Redezeit ist ungültig!
                invalidFormatSpeakerTime = true;
            }

            if (TimeSpan.TryParseExact(Settings.Questiontime, @"mm\:ss", null, out questionTime))
            {
                this.ViewModel.SourceList.QuestionTime = questionTime;
            }
            else
            {
                // Die Eingegebene Zeit für Fragen und Kurzbemerkungen ist ungültig.
                invalidFormatQuestionTime = true;
            }
        }

        

        private void RemoveSpeaker(Speaker speaker)
        {
            this.ViewModel.Handler.Remove(speaker);
        }

        

        protected override async Task OnInitializedAsync()
        {

            // TODO: Get the ViewModel
            if (!string.IsNullOrEmpty(this.Id))
            {
                this.ViewModel = await ViewModels.ListOfSpeakerViewModel.CreateViewModel(listOfSpeakerService, this.Id);
            }
        }

        private async Task OpenReaderSpectatorView()
        {
            // Das her ist auch mal ein beispiel, wie man eine JavaScript Methode schnell aufrufen kann :)
            string url = "/los/read/" + this.Id;
            await jsRuntime.InvokeAsync<object>("open", url, "_blank");
        }
    }
}
