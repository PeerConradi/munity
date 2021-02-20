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
        public string ListOfSpeakersId
        {
            get;
            set;
        }

        [Parameter]
        public bool IsOnline
        {
            get;
            set;
        }

        public ListOfSpeakers Speakerlist
        {
            get;
            set;
        }

        private Services.SocketHandlers.ListOfSpeakerViewModel _socket;
        // TODO: Implement different modes
        private enum SyncModes
        {
            FromLocalStorage,
            FromSocket
        }

        protected override async Task OnInitializedAsync()
        {
            if (IsOnline == false)
            {
                this.Speakerlist = await this.listOfSpeakerService.GetListOfSpeakersOffline(ListOfSpeakersId);
            }
            else
            {
                this.Speakerlist = await this.listOfSpeakerService.GetFromApi(ListOfSpeakersId);
                // Add Handlers
                _socket = await this.listOfSpeakerService.Subscribe(this.Speakerlist);
                if (_socket != null)
                {
                    _socket.SpeakerListChanged += OnSpeakerlistChanged;
                }
            }

            if (Speakerlist != null)
            {
                var timer = new System.Timers.Timer(1000);
                timer.Elapsed += TimerElapsed;
                timer.Start();
            }
        }

        private async void OnSpeakerlistChanged(object sender, ListOfSpeakers newList)
        {
            if (this.Speakerlist != null && this.Speakerlist.ListOfSpeakersId == newList.ListOfSpeakersId)
            {
                this.Speakerlist = newList;
                await this.listOfSpeakerService.StoreListOfSpeakers(newList);
                this.StateHasChanged();
            }
        }

        // Wir aktualisieren zunächst wieder jede Sekunden, da für den Countdown ohnehin jede Sekunde
        // gezählt werden muss.
        private async void TimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
        {
            if (!IsOnline)
            {
                var list = await this.listOfSpeakerService.GetListOfSpeakersOffline(ListOfSpeakersId);
                if (list != null)
                {
                    this.Speakerlist = list;
                    this.StateHasChanged();
                }
            }

            this.StateHasChanged();
        }
    }
}